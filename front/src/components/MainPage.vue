<template>
  <header>
    <nav>
      <div class="nav__title">
        <h1 class="title">Movies</h1>
        <ul class="nav__links">
          <li><a href="#">Home</a></li>
          <li><a href="#">Reservations</a></li>
        </ul>
      </div>
    </nav>
    <a class="cta" href="#"><button></button>Contact</a>
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

* {
  box-sizing: border-box;
  margin: 0;
  padding: 0;
}

header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 30px;
  background-color: #24252a;
}

.nav__title {
  display: flex;
  align-items: center;
  padding: 0px 15%;
}

.nav__links {
  list-style: none;
  display: flex;
  justify-content: center;
  margin-right: auto;
}

.nav__links a,
.cta,
.title {
  font-family: "Lucida Console", sans-serif;
  font-weight: 500;
  color: #edf0f1;
  text-decoration: none;
}

.nav__links li {
  padding: 10px 25px;
}

.nav__links li a {
  font-family: "Lucida Console", sans-serif;
  font-weight: 500;
  color: black;
  text-decoration: none;
  background-color: white; /* Add white background */
  padding: 8px 10px; 
  border-radius: 10px;
  transition: color 0.3s ease 0s;
}

.nav__links li a:hover {
  color: #0088a9;
}

.cta {
  padding: 9px 25px;
  background-color: rgba(0, 136, 169, 1);
  border: none;
  border-radius: 50px;
  cursor: pointer;
  transition: background-color 0.3s ease 0s;
}

.cta:hover {
  background-color: rgba(0, 136, 169, 0.8);
}
</style>