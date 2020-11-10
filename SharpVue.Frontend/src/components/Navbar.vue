<template lang="pug">
nav.navbar.navbar-dark.bg-dark.justify-content-start

    router-link.navbar-brand(to="/")
        img.d-inline-block(v-if="config.logoImage" :src="config.logoImage")
        span(v-if="config.showAppName" :class="{'ml-2': config.logoImage}")
            | {{config.appName}} documentation
    ul.navbar-nav.flex-row
        li.nav-item
            router-link.nav-link(to="/ref") API reference
        li.nav-item
            router-link.nav-link(to="/articles") Articles
</template>

<script lang="ts">
import { defineComponent, inject } from 'vue'

export default defineComponent({
    setup() {
        return { config: inject("config") }
    }
})
</script>
