import { createRouter, createWebHashHistory } from 'vue-router'

import ViewReference from "@/views/ViewReference.vue";
import Home from "@/views/Home.vue";

const routes = [
    { path: "/", name: "Home", component: Home },
    { path: '/ref/:item?/:member?', name: 'ViewReference', component: ViewReference }
]

const router = createRouter({
    history: createWebHashHistory(),
    routes
})

export default router
