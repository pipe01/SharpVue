import { createRouter, createWebHashHistory } from 'vue-router'

const routes = [
    {
        path: '/ref/:item?/:member?',
        name: 'ViewReference',
        component: () => import('../views/ViewReference.vue')
    }
]

const router = createRouter({
    history: createWebHashHistory(),
    routes
})

export default router
