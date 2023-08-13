import { createApp } from 'vue';
import Toast from "vue-toastification";
import "vue-toastification/dist/index.css";
import App from './App.vue';
import router from './router/router';
import 'bootstrap/dist/css/bootstrap.min.css'
import 'bootstrap/dist/js/bootstrap.bundle.min.js' 

const options = { };

const app = createApp(App);
app.use(router);
app.mount('#app');
app.use(Toast, options);