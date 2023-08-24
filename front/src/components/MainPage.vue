<template>
  <header class="header">
    <h1 class="title">Movie library</h1>
    <nav class="navbar">
      <ul class="nav_buttons">
        <router-link to="/">Home</router-link>
        <router-link to="/admin-panel">Admin</router-link>
      </ul>
    </nav>
  </header>

  <body class="mainBody">
    <aside>
      <div class="filter-panel">
        <div class="filter-content">
          <h2>Filter Movies</h2>

          <div class="filter-input-container">
            <label for="titleInput">Title:</label>
            <input type="text" id="titleInput" class="filter-input" placeholder="Enter title..." v-model="titleFilter"
              v-on:keyup.enter="filterByTitle">
          </div>

          <div class="filter-input-container">
            <label for="genreFilter">Category:</label>
            <select id="genreFilter" class="filter-select" v-model="selectedGenre">
              <option value="All">All</option>
              <option value="Horror">Horror</option>
              <option value="Comedy">Comedy</option>
            </select>
          </div>

          <div class="filter-input-container">
            <label for="fromYearInput">Date range:</label>
            <input type="number" id="fromYearInput" class="filter-input" placeholder="From year..." v-model="startYear"
              v-on:keyup.enter="filterByReleaseDates">
            <span class="date-range-separator">to</span>
            <input type="number" class="filter-input" placeholder="To year..." v-model="endYear"
              v-on:keyup.enter="filterByReleaseDates">
          </div>

          <div class="filter-input-container">
            <label for="ratingRangeInput">Rating range:</label>
            <input type="number" id="minRatingInput" class="filter-input" placeholder="Min rating..." v-model="minRating"
              v-on:keyup.enter="filterByRating">
            <span class="rating-range-separator">to</span>
            <input type="number" class="filter-input" placeholder="Max rating..." v-model="maxRating"
              v-on:keyup.enter="filterByRating">
          </div>

          <button class="btn btn-success" @click="filterMovies()">Filter!</button>
          <button class="btn btn-danger" style="margin: 10px" @click="clearFilters()">Clear!</button>

        </div>
      </div>
    </aside>

    <main>
      <div class="table-responsive">
        <h2>Movies</h2>
        <table class="table table-striped table-hover">
          <thead class="thead-dark">
            <tr>
              <th scope="col" @click="sortBy('id')" :class="{ 'active-column': sortByColumn === 'id' }">Id</th>
              <th scope="col" @click="sortBy('title')" :class="{ 'active-column': sortByColumn === 'title' }">Title</th>
              <th scope="col" @click="sortBy('movieCategory')"
                :class="{ 'active-column': sortByColumn === 'movieCategory' }">Category</th>
              <th scope="col" @click="sortBy('rating')" :class="{ 'active-column': sortByColumn === 'rating' }">Rating
              </th>
              <th scope="col" @click="sortBy('releaseDate')" :class="{ 'active-column': sortByColumn === 'releaseDate' }">
                Release Date</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="movie in sortedMovies" :key="movie.id">
              <td>{{ movie.id }}</td>
              <td>{{ movie.title }}</td>
              <td>{{ movie.movieCategory }}</td>
              <td>{{ movie.rating }}</td>
              <td>{{ movie.releaseDate }}</td>
              <td>
                <button class="btn btn-primary btn-sm" @click="redirectToReservations(movie)">Reserve</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </main>
  </body>
</template>
  
<style>
@import "@/assets/styles.css";
@import "@/assets/filterPanelStyles.css";
</style>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      movies: [],
      sortByColumn: 'id',
      sortDirection: 'asc',
      selectedGenre: 'All',
      id: null,
      newMovieName: '',
      newMovieRating: '',
      selectedAction: null,
      titleFilter: "",
      startYear: null,
      endYear: null,
      minRating: null,
      maxRating: null
    };
  },
  created() {
    this.getMovies();
  },
  computed: {
    sortedMovies() {
      const sorted = [...this.movies];
      sorted.sort((a, b) => {
        const modifier = this.sortDirection === 'asc' ? 1 : -1;
        const column = this.sortByColumn;
        if (a[column] < b[column]) return -1 * modifier;
        if (a[column] > b[column]) return 1 * modifier;
        return 0;
      });
      return sorted;
    }
  },
  methods: {
    getMovies() {
      axios.get(`${process.env.VUE_APP_BACKEND_URL}/api/movies`)
        .then(response => {
          this.movies = response.data;
        })
        .catch(error => {
          console.error('Error fetching movies:', error);
        });
    },
    sortBy(column) {
      if (this.sortByColumn === column) {
        this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
      } else {
        this.sortByColumn = column;
        this.sortDirection = 'asc';
      }
    },
    openForm(movie) {
      const formId = 'myForm_' + movie.id;
      document.getElementById(formId).style.display = 'block';
    },
    closeForm(movie) {
      const formId = 'myForm_' + movie.id;
      document.getElementById(formId).style.display = 'none';
    },
    filterByTitle() {
      if (this.titleFilter == null || this.titleFilter == "") {
        this.getMovies();
      }
      else {
        axios.get(`${process.env.VUE_APP_BACKEND_URL}/api/movies/titleContains`, {
          params: { title: this.titleFilter }
        })
          .then((response) => {
            this.movies = response.data;
          })
          .catch((error) => {
            console.error('Error filtering movies by title:', error);
          });
      }
    },
    filterByReleaseDates() {
      axios.get(`${process.env.VUE_APP_BACKEND_URL}/api/movies/getMoviesBetweenYears`, {
        params: {
          startYear: this.startYear,
          endYear: this.endYear
        }
      })
        .then((response) => {
          this.movies = response.data;
        })
        .catch((error) => {
          console.error('Error filtering movies by dates:', error);
        });
    },
    filterByRating() {
      axios.get(`${process.env.VUE_APP_BACKEND_URL}/api/movies/getMoviesWithRating`, {
        params: {
          minRating: this.minRating,
          maxRating: this.maxRating
        }
      })
        .then((response) => {
          this.movies = response.data;
        })
        .catch((error) => {
          console.error('Error filtering movies by dates:', error);
        });
    },
    async filterMovies() {
      const params = {
        title: this.titleFilter,
        category: this.selectedGenre,
        minRating: this.minRating,
        maxRating: this.maxRating,
        startYear: this.startYear,
        endYear: this.endYear
      };

      try {
        const response = await axios.get(`${process.env.VUE_APP_BACKEND_URL}/api/movies/filterMovies`, { params });
        this.movies = response.data;
      } catch (error) {
        console.error('Error filtering movies:', error);
      }
    },
    async clearFilters() {
      this.titleFilter = "";
      this.selectedGenre = "All";
      this.startYear = null;
      this.endYear = null;
      this.minRating = null;
      this.maxRating = null;
      
      const params = {
        title: null,
        category: null,
        minRating: null,
        maxRating: null,
        startYear: null,
        endYear: null
      };

      try {
        const response = await axios.get(`${process.env.VUE_APP_BACKEND_URL}/api/movies/filterMovies`, { params });
        this.movies = response.data;
      } catch (error) {
        console.error('Error filtering movies:', error);
      }
    },
    redirectToReservations(movie) {
      this.$router.push({
        name: 'reservations',
        query: {
          id: movie.id,
          title: movie.title,
          movieCategory: movie.movieCategory,
          rating: movie.rating,
          releaseDate: movie.releaseDate
        }
      });
    }
  }
};
</script>