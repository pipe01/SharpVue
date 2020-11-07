import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

import "bootswatch/dist/slate/bootstrap.min.css";

import Reference from "@/components/Reference.vue";

createApp(App)
    .component("reference", Reference)
    .use(router)
    .mount('#app')
