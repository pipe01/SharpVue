import { createRouter, createWebHashHistory } from 'vue-router'

import ViewReference from "@/views/ViewReference.vue";

const routes = [
    {
        path: '/ref/:item?/:member?',
        name: 'ViewReference',
        component: ViewReference
    }
]

const router = createRouter({
    history: createWebHashHistory(),
    routes
})

export default router
