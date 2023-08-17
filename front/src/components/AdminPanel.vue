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

  <body style="display: flex">
    <aside>
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
    </aside>
    
    <main>
      <div class="table-responsive">
        <h2>Reservations</h2>
        <table class="table table-striped table-hover">
          <thead class="thead-dark">
            <tr>
              <th>Id</th>
              <th>Title</th>
              <th>Start Date</th>
              <th>End Date</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="reservation in reservations" :key="reservation.id">
              <td>{{ reservation.id }}</td>
              <td>{{ reservation.movieTitle }}</td>
              <td>{{ formatDate(reservation.startDate) }}</td>
              <td>{{ formatDate(reservation.endDate) }}</td>
              <td><button class="btn btn-success" @click="showEditForm(reservation)">Edit</button></td>
              <td><button class="btn btn-danger" @click="cancellReservation(reservation.id)">Remove</button></td>
            </tr>

            <tr v-if="editingReservation">
              <td colspan="6">
                <form @submit.prevent="updateReservation" class="reservation-form">
                  <div class="form-row">
                    <div class="form-group">
                      <label for="startDate">Start Date:</label>
                      <input type="date" v-model="editedReservation.startDate" id="startDate" class="form-control">
                    </div>

                    <div class="form-group">
                      <label for="endDate">End Date:</label>
                      <input type="date" v-model="editedReservation.endDate" id="endDate" class="form-control">
                    </div>
                  </div>

                  <div class="form-row button-row">
                    <div class="form-group">
                      <button class="btn btn-success" @click="editReservation()">Edit</button>
                    </div>
                    <div class="form-group">
                      <button class="btn btn-secondary" @click="cancellReservation(id)">Cancel</button>
                    </div>
                  </div>
                </form>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </main>
  </body>
</template>
  
<style>
@import "@/assets/adminStyles.css";
@import "@/assets/styles.css";
</style>

<script>
import axios from 'axios';
import moment from 'moment';

export default {
  data() {
    return {
      movieId: 0,
      title: "",
      movieCategory: "",
      releaseDate: 0,
      rating: 0,
      reservations: [],
      id: null,
      movieTitle: "",
      startDate: "",
      endDate: "",
      editingReservation: null,
      editedReservation: {
        id: null,
        startDate: null,
        endDate: null
      }
    };
  },
  created() {
    this.getReservations();
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

      axios.post(`${process.env.VUE_APP_BACKEND_URL}/api/movies/addMovie`, newMovieBody).then((response) => {
        console.log(`${process.env.VUE_APP_BACKEND_URL}/movies`
          + "\nMovie added successfully:", response.data);
      });
    },
    updateMovie() {
      const updatedMovieBody = {
        movieId: this.movieId,
        title: this.title,
        movieCategory: this.movieCategory,
        releaseDate: this.releaseDate,
        rating: this.rating
      }
      axios.put(`${process.env.VUE_APP_BACKEND_URL}/api/movies/updateMovie`, updatedMovieBody).then((response) => {
        console.log("Movie updated successfully:", response.data);
      });

    },
    deleteMovie() {
      var id = this.movieId;
      axios.delete(`${process.env.VUE_APP_BACKEND_URL}/api/movies/${id}`).then((response) => {
        console.log(`${process.env.VUE_APP_BACKEND_URL}/delete/${id}`
          + "\nMovie deleted successfully:", response.data);
      });
    },
    getReservations() {
      axios.get(`${process.env.VUE_APP_BACKEND_URL_RESERVATIONS}/api/reservations`)
        .then(response => {
          this.reservations = response.data;
        })
        .catch(error => {
          console.error('Error fetching movies:', error);
        });
    },
    editReservation() {
      const updatedReservationBody = {
        id: this.editedReservation.id,
        startDate: this.editedReservation.startDate,
        endDate: this.editedReservation.endDate,
        movieTitle: this.editedReservation.movieTitle
      }

      axios.put(`${process.env.VUE_APP_BACKEND_URL_RESERVATIONS}/api/reservations/updateReservation`, updatedReservationBody)
        .then(() => {
          this.getReservations();
        })
        .catch(error => {
          console.error('Error updating reservation:', error);
        });
    },
    cancellReservation(id) {
      axios.delete(`${process.env.VUE_APP_BACKEND_URL_RESERVATIONS}/delete/${id}`)
        .then(() => {
          this.getReservations();
        })
    },
    showEditForm(reservation) {
      this.editingReservation = reservation.id;
      this.editedReservation.id = reservation.id;
      this.editedReservation.startDate = reservation.startDate;
      this.editedReservation.endDate = reservation.endDate;
      this.editedReservation.movieTitle = reservation.movieTitle;
    },
    cancelEdit() {
      this.editingReservation = null;
      this.editedReservation.id = null;
      this.editedReservation.startDate = null;
      this.editedReservation.endDate = null;
    },
    formatDate(date) {
      return moment(date).format('DD-MM-YYYY');
    }
  }
};
</script>