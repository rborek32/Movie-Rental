<template>
  <header class="header">
    <h1 class="title">Movie library</h1>
    <nav class="navbar">
        <ul class="nav_buttons">
          <a href="#">Home</a>
          <a href="#">Reservations</a>
          <a href="#">Contact</a>
        </ul>
    </nav>
  </header>

  <body>
    <div class="container mt-5">
      <div class="table-responsive">
        <table class="table table-striped table-hover">
          <thead class="thead-dark">
            <tr>
              <th scope="col" @click="sortBy('movieId')" :class="{ 'active-column': sortByColumn === 'movieId' }">Id</th>
              <th scope="col" @click="sortBy('title')" :class="{ 'active-column': sortByColumn === 'title' }">Title</th>
              <th scope="col" @click="sortBy('movieCategory')"
                :class="{ 'active-column': sortByColumn === 'movieCategory' }">Category</th>
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
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </body>
</template>
  
<style>
*{
  margin: 0;
  padding: 10;
  box-sizing: border-box;
  font-family: "Poppins", sans-serif;
}

body {
  padding-top: 40px;
}

.header{
  position:fixed;
  top:0;
  left:0;
  width:100%;
  padding: 20px ;
  background: rgb(28, 34, 34);
  display:flex;
  justify-content: space-between;
  align-items: center;
  z-index: 100;
}

.title{
  font-size: 32px;
  color: #fff;
  text-decoration: none;
  font-weight: 700;
}

.navbar {
  display: flex;
  align-items: center; 
}

.navbar a{
  position: relative;
  font-size: 18px;
  color: #fff;
  font-weight: 500;
  text-decoration: none;
  margin-left: 40px;
}

.navbar a::before{
  content: "";
  position:absolute;
  top:100%;
  left:0;
  width:0;
  height:1px;
  background: #fff;
  transition: 0.3s;
}

.navbar a:hover::before{
  width:100%
}

</style>

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

