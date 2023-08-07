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
          <input type="number" class="form-control" v-model="movieId" required>
          <label class="control-label">Title</label>
          <input type="text" class="form-control" v-model="title" required>
          <label class="text">Category</label>
          <input type="text" class="form-control" v-model="movieCategory" required>
          <label class="control-label">Release year</label>
          <input type="number" class="form-control" v-model="releaseDate" required>
          <label class="control-label">Rating</label>
          <input type="number" class="form-control" v-model="rating" required>
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
@import "@/assets/styles.css";
</style>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      movieId: 0,
      title: "",
      movieCategory: "",
      releaseDate: 0,
      rating: 0
    };
  },
  methods: {
  addMovie() {
    const newMovieBody = {
      movieId: this.movieId,
      title: this.title,
      movieCategory: this.movieCategory,
      releaseDate: this.releaseDate,
      rating: this.rating
    };

    axios.post(`${process.env.VUE_APP_BACKEND_URL}/api/movies`, newMovieBody).then((response) => {
      console.log(`${process.env.VUE_APP_BACKEND_URL}/movies`
        + "\nMovie added successfully:", response.data);
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
        + "\nMovie updated successfully:", response.data);
    });

  },
  deleteMovie() {
    var id = this.movieId;
    axios.delete(`${process.env.VUE_APP_BACKEND_URL}/api/movies/${id}`).then((response) => {
      console.log(`${process.env.VUE_APP_BACKEND_URL}/delete/${id}`
        + "\nMovie deleted successfully:", response.data);
    });
  },
}
};
</script>