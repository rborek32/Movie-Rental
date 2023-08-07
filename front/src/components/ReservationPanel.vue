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


  <body>
        <div class="movie-editor">
          <h2>Add, Update, or Delete Movies</h2>
          <form class="form-horizontal">
            <div class="form-group">
              <label class="control-label">Id</label>
              <input type="number" class="form-control" v-model="movieId">
              <label class="control-label">Movie Name</label>
              <input type="text" class="form-control" v-model="newMovieName">
              <label class="control-label">Rating</label>
              <input type="number" class="form-control" v-model="newMovieRating">
            </div>
          </form>
          <div class="btn-group" role="group" style="margin-top: 15px;">
            <button class="btn btn-primary" @click="addMovie">Add Movie</button>
            <button class="btn btn-success" @click="updateMovie">Update Movie</button>
            <button class="btn btn-danger" @click="deleteMovie">Delete Movie</button>
          </div>
        </div>
  </body>
</template>
  
<style>
* {
  margin: 0;
  padding: 10;
  box-sizing: border-box;
  font-family: "Poppins", sans-serif;
}

body {
  padding-top: 60px;
}

.header {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  padding: 20px;
  background: rgb(28, 34, 34);
  display: flex;
  justify-content: space-between;
  align-items: center;
  z-index: 100;
}

.title {
  left: 10%;
  font-size: 32px;
  color: #fff;
  text-decoration: none;
  font-weight: 700;
}

.navbar {
  display: flex;
  align-items: center;
  right: 15%;
}

.navbar a {
  position: relative;
  font-size: 18px;
  color: #fff;
  font-weight: 500;
  text-decoration: none;
  margin-left: 40px;
}

.navbar a::before {
  content: "";
  position: absolute;
  top: 100%;
  left: 0;
  width: 0;
  height: 1px;
  background: #fff;
  transition: 0.3s;
}

.navbar a:hover::before {
  width: 100%
}

.filter-panel {
  padding-left: 10%;
}
.movie-editor{
  width: 15%;
    margin: 0 auto;

}
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
    addMovie() {
      const newMovieBody = [{
        movie: this.newMovieName,
        rating: this.newMovieRating,
      }];

      axios.post(`${process.env.VUE_APP_BACKEND_URL}/addMovies`, newMovieBody).then((response) => {
        console.log(`${process.env.VUE_APP_BACKEND_URL}/addMovies`
          +"\nMovie added successfully:", response.data);
          this.fetchMovies();
      });
    },
    updateMovie() {
      const updatedMovieBody = {
        movie: this.newMovieName,
        rating: this.newMovieRating
      }
      var id = this.movieId;
      axios.put(`${process.env.VUE_APP_BACKEND_URL}/update/${id}`, updatedMovieBody).then((response) => {
        console.log(`${process.env.VUE_APP_BACKEND_URL}/update/${id}`
          +"\nMovie updated successfully:", response.data);
          this.fetchMovies();
      });

    },
    deleteMovie() {
      var id = this.movieId;
      axios.delete(`${process.env.VUE_APP_BACKEND_URL}/delete/${id}`).then((response) => {
        console.log(`${process.env.VUE_APP_BACKEND_URL}/delete/${id}`
          +"\nMovie deleted successfully:", response.data);
          this.fetchMovies();
      });
    },
  }
};
</script>