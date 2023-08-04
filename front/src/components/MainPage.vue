<template>
  <div class="container mt-5">
    <h1 class="mb-4">Movies</h1>
    <div class="table-responsive">
      <table class="table table-striped table-hover">
        <thead class="thead-dark">
          <tr>
            <th scope="col" @click="sortBy('movieId')" :class="{ 'active-column': sortByColumn === 'movieId' }">Id</th>
            <th scope="col" @click="sortBy('title')" :class="{ 'active-column': sortByColumn === 'title' }">Title</th>
            <th scope="col" @click="sortBy('movieCategory')" :class="{ 'active-column': sortByColumn === 'movieCategory' }">Category</th>
            <th scope="col" @click="sortBy('releaseDate')" :class="{ 'active-column': sortByColumn === 'releaseDate' }">Release Date</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="movie in sortedMovies" :key="movie.movieId">
            <td>{{ movie.movieId }}</td>
            <td>{{ movie.title }}</td>
            <td>{{ movie.movieCategory }}</td>
            <td>{{ movie.releaseDate }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>
  
<script>
import axios from 'axios';

export default {
  data() {
    return {
      movies: [],
      sortByColumn: 'movieId',
      sortDirection: 'asc'
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
  }
};
</script>

<style>
.active-column:hover {
  cursor: pointer;
  text-decoration: underline;
}
</style>