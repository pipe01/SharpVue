import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

import { plugin } from "./config"

import Reference from "@/components/Reference.vue";

createApp(App)
    .component("reference", Reference)
    .use(router)
    .use(plugin)
    .mount('#app')
