<template>
  <header class="header">
    <h1 class="title">Movie library</h1>
    <nav class="navbar">
      <ul class="nav_buttons">
        <router-link to="/">Home</router-link>
        <a href="#">Contact</a>
        <router-link to="/admin-panel">Admin</router-link>
      </ul>
    </nav>
  </header>


  <body>
    <div class="reservationPanel">
    <div class="reservationPanelImgContainer">
      <img src="@/assets/movie.jpg" class="img-thumbnail" alt="Cinque Terre">
    </div>

    <aside class="reservationDetails">
              <h2>Reservation Details</h2>

        <p><strong>Movie ID:</strong> {{ movieId }}</p>
        <p><strong>Title:</strong> {{ title }}</p>
        <p><strong>Category:</strong> {{ movieCategory }}</p>
        <p><strong>Rating:</strong> {{ rating }}</p>
        <p><strong>Release Date:</strong> {{ releaseDate }}</p>
      </aside>

      <form class="form-container" onsubmit="return false;">
        <label for="date"><b>Start Date</b></label>
        <input type="date" placeholder="Start..." v-model="startDate" required>

        <label for="date"><b>End date</b></label>
        <input type="date" placeholder="End..." v-model="endDate" required>

        <button class="btn btn-primary btn-sm" @click="addReservation()">Reserve!</button>
      </form>
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
      startDate: "",
      endDate: "",
      movieId: null,
      title: null,
      movieCategory: null,
      rating: null,
      releaseDate: null
    };
  },
  beforeRouteEnter(to, from, next) {
    next(vm => {
      vm.movieId = to.query.movieId;
      vm.title = to.query.title;
      vm.movieCategory = to.query.movieCategory;
      vm.rating = to.query.rating;
      vm.releaseDate = to.query.releaseDate;
    });
  },
  methods: {
    addReservation() {
      var today = new Date();
      var dd = String(today.getDate()).padStart(2, '0');
      var mm = String(today.getMonth() + 1).padStart(2, '0');
      var yyyy = today.getFullYear();

      today = yyyy + '-' + dd + '-' + mm;

      const reservationBody = {
        id: this.movieId,
        startDate: this.startDate,
        endDate: this.endDate,
        movieTitle: this.title
      };

      if (this.startDate != null && this.endDate != null && this.startDate != "" && this.endDate != "") {
        axios.post(`${process.env.VUE_APP_BACKEND_URL_RESERVATIONS}/api/reservations`, reservationBody).then((response) => {
          console.log(`${process.env.VUE_APP_BACKEND_URL_RESERVATIONS}/api/reservations`
            + "\nMovie added successfully:", response.data);
        })
      }
      else {
        console.log("Startdate or enddate is empty!");
      }
    }
  }
};
</script>