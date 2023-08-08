<template>
  <header class="header">
    <h1 class="title">Movie library</h1>
    <nav class="navbar">
      <ul class="nav_buttons">
        <router-link to="/">Home</router-link>
        <router-link to="/reservations">Reservations</router-link>
        <a href="#">Contact</a>
        <router-link to="/admin-panel">Admin</router-link>
      </ul>
    </nav>
  </header>

  <body class="body">
    <aside class="filter-panel">
        <div class="filter-content">
          <h2>Filter Movies</h2>
          <label>Title: </label>
          <input type="text" class="filter-input" placeholder="Enter title...">
          <label>Category: </label>
          <select id="genreFilter" class="filter-select" v-model="selectedGenre">
            <option value="All">All</option>
            <option value="Horror">Horror</option>
            <option value="Comedy">Comedy</option>
          </select>
        </div>
      </aside>

    <main>
      <div class="table-responsive">
        <h2>Movies</h2>
        <table class="table table-striped table-hover">
          <thead class="thead-dark">
            <tr>
              <th scope="col" @click="sortBy('movieId')" :class="{ 'active-column': sortByColumn === 'movieId' }">Id</th>
              <th scope="col" @click="sortBy('title')" :class="{ 'active-column': sortByColumn === 'title' }">Title</th>
              <th scope="col" @click="sortBy('movieCategory')" :class="{ 'active-column': sortByColumn === 'movieCategory' }">Category</th>
              <th scope="col" @click="sortBy('releaseDate')" :class="{ 'active-column': sortByColumn === 'releaseDate' }">
                Release Date</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="movie in sortedMovies" :key="movie.movieId">
              <td>{{ movie.movieId }}</td>
              <td>{{ movie.title }}</td>
              <td>{{ movie.movieCategory }}</td>
              <td>{{ movie.releaseDate }}</td>
              <td><button class="btn btn-primary" @click="addReservation(movie)">Reserve</button></td>
            </tr>
          </tbody>
        </table>
      </div>
    </main>
  </body>
</template>
  
<style>
@import "@/assets/styles.css";
</style>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      movies: [],
      sortByColumn: 'movieId',
      sortDirection: 'asc',
      selectedGenre: 'All',
      movieId: null,
      newMovieName: '',
      newMovieRating: '',
      selectedAction: null
    };
  },
  created() {
    this.getMovies();
  },
  computed: {
    filteredMovies() {
      return this.selectedGenre === 'All' ? this.movies : this.movies.filter(movie => movie.movieCategory === this.selectedGenre);
    },
    sortedMovies() {
      const sorted = [...this.filteredMovies];
      sorted.sort((a, b) => {
        const modifier = this.sortDirection === 'asc' ? 1 : -1;
        const column = this.sortByColumn;
        if (a[column] < b[column]) return -1 * modifier;
        if (a[column] > b[column]) return 1 * modifier;
        return 0;
      });
      return sorted;
    },
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
    addReservation(movie){

      var today = new Date();
      var dd = String(today.getDate()).padStart(2, '0');
      var mm = String(today.getMonth() + 1).padStart(2, '0');
      var yyyy = today.getFullYear();

      today = yyyy + '-' + dd + '-' + mm;

      const reservationBody = {
        id: movie.movieId,
        startDate: today,
        endDate: today,
        movieTitle: movie.title
      };

      axios.post(`${process.env.VUE_APP_BACKEND_URL_RESERVATIONS}/api/reservations`, reservationBody).then((response) => {
        console.log(`${process.env.VUE_APP_BACKEND_URL_RESERVATIONS}/addMovies`
          +"\nMovie added successfully:", response.data);
      })
    }
  }
};
</script>