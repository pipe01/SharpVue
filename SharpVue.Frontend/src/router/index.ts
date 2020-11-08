import { createRouter, createWebHashHistory } from 'vue-router'

import ViewReference from "@/views/ViewReference.vue";
import ViewArticle from "@/views/ViewArticles.vue";
import Home from "@/views/Home.vue";

const routes = [
    { path: "/",                    name: "Home",           component: Home },
    { path: '/ref/:item?/:member?', name: 'ViewReference',  component: ViewReference },
    { path: '/articles/:item*',      name: 'ViewArticle',    component: ViewArticle },
]

const router = createRouter({
    history: createWebHashHistory(),
    routes
})

export default router
