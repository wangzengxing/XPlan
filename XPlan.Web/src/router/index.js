import Vue from 'vue';
import VueRouter from 'vue-router';
import Plan from '../components/Plan';
import Schedule from '../components/Schedule';
import Project from '../components/Project';

Vue.use(VueRouter);

const routes = [
  { path: '/', component: Plan },
  { path: '/1', component: Plan },
  { path: '/2', component: Schedule },
  { path: '/3', component: Project }
]

export default new VueRouter({
  routes
})
