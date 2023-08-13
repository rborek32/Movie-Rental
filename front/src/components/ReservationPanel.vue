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
    <div class="reservationHeader">
      <h2>Reservation Details</h2>
    </div>

    <div class="reservationPanel">

      <div class="reservationPanelImgContainer">
        <img src="@/assets/movie.jpg" class="img-thumbnail" alt="Cinque Terre">
      </div>

      <aside class="reservationDetails">
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

    <div class="reservedMovies">
      <div class="table-responsive">
        <h2>Reservations</h2>
        <table class="table table-striped table-hover">
          <thead class="thead-dark">
            <tr>
              <th>Title</th>
              <th>Start Date</th>
              <th>End Date</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="reservation in reservations" :key="reservation.id">
              <td>{{ reservation.movieTitle }}</td>
              <td>{{ formatDate(reservation.StartDate) }}</td>
              <td>{{ formatDate(reservation.EndDate) }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </body>
</template>
  
<style>
@import "@/assets/styles.css";
</style>

<script>
import axios from 'axios';
import { useToast } from "vue-toastification";
import moment from 'moment';

export default {
  setup() {
    const toast = useToast();
    return { toast }
  },
  data() {
    return {
      startDate: "",
      endDate: "",
      movieId: null,
      title: "",
      movieCategory: null,
      rating: null,
      releaseDate: null,
      popupMessage: '',
      reservations: []
    };
  },
  beforeRouteEnter(to, from, next) {
    next(vm => {
      vm.movieId = to.query.movieId;
      vm.title = to.query.title;
      vm.movieCategory = to.query.movieCategory;
      vm.rating = to.query.rating;
      vm.releaseDate = to.query.releaseDate;

      vm.getReservations();
    });
  },
  methods: {
    getReservations() {
      axios.get(`${process.env.VUE_APP_BACKEND_URL_RESERVATIONS}/api/reservations/movie`,
        { params: { title: this.title } })
        .then((response) => {
          this.reservations = response.data;
        })
        .catch(error => {
          console.error('Error fetching reservations:', error);
        });
    },
    async addReservation() {
      const reservationBody = {
        id: this.movieId,
        startDate: this.startDate,
        endDate: this.endDate,
        movieTitle: this.title
      };

      if (this.startDate != null && this.endDate != null && this.startDate != "" && this.endDate != "") {
        try {
          const isReservedResponse = await axios.post(`${process.env.VUE_APP_BACKEND_URL_RESERVATIONS}/api/reservations/isReserved`, reservationBody);

          if (isReservedResponse.data != "Movie can be reserved.") {
            this.toast.error("The movie is already reserved!");
          } else {
            const reservationResponse = await axios.post(`${process.env.VUE_APP_BACKEND_URL_RESERVATIONS}/api/reservations/add`, reservationBody);
            console.log('Movie added successfully:', reservationResponse.data);
            this.toast.success("Success!");
          }
        } catch (error) {
          console.error('Error checking reservation:', error);
        }
      } else {
        console.log('Start date or end date is empty!');
      }
      this.getReservations();
    },
    formatDate(date) {
      return moment(date).format('DD-MM-YYYY');
    }
  }
};
</script>