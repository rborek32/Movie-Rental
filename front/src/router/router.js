import { createRouter, createWebHistory } from 'vue-router';
import MainPage from '../components/MainPage.vue';
import AdminPanel from '../components/AdminPanel.vue';
import ReservationPanel from '../components/ReservationPanel.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/', component: MainPage },
    { path: '/admin-panel', component: AdminPanel },
    { path: '/reservations', component: ReservationPanel },
  ]
});

export default router;