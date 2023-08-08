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
              <td>{{ reservation.movieTitle}}</td>
              <td>{{ reservation.startDate }}</td>
              <td>{{ reservation.endDate }}</td>
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
      reservations: [],
      id: null,
      movieTitle: "",
      startDate: "",
      endDate: ""
    };
  },
  created() {
    this.getReservations();
  },
  methods: {
    getReservations() {
      axios.get(`${process.env.VUE_APP_BACKEND_URL_RESERVATIONS}/api/reservations`)
        .then(response => {
          this.reservations = response.data;
        })
        .catch(error => {
          console.error('Error fetching movies:', error);
        });
    }
  }
};
</script>