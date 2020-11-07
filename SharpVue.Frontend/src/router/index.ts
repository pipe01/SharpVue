import { createRouter, createWebHashHistory } from 'vue-router'

const routes = [
    {
        path: '/ref/:item?',
        name: 'ViewReference',
        component: () => import('../views/ViewReference.vue')
    }
]

const router = createRouter({
    history: createWebHashHistory(),
    routes
})

export default router
