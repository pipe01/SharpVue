import { createRouter, createWebHashHistory } from 'vue-router'

const routes = [
    {
        path: '/view',
        name: 'View',
        component: () => import('../views/View.vue')
    }
]

const router = createRouter({
    history: createWebHashHistory(),
    routes
})

export default router
