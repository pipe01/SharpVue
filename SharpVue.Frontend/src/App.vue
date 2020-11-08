<template lang="pug">
.d-flex.flex-column.h-100
    nav.navbar.navbar-dark.bg-dark.justify-content-start
        router-link.navbar-brand(to="/") {{appName}} documentation
        ul.navbar-nav.flex-row
            li.nav-item
                router-link.nav-link(to="/ref") API reference
            li.nav-item
                router-link.nav-link(to="/articles") Articles

    router-view.flex-grow-1
</template>

<script lang="ts">
import { defineComponent, inject } from 'vue'

export default defineComponent({
    setup() {
        return { appName: inject("appName") }
    }
})
</script>

<style lang="scss">
html, body, #app {
    height: 100%;
    overflow-x: hidden;
}

.code-word, .code-word:hover {
    color: #4dc5ff;
}

main {
    margin-left: auto !important;
    margin-right: auto !important;
    width: 100%;
    max-width: 1600px;
}

.sidebar {
    position: sticky;
    top: 0;
    max-height: 90vh;
    overflow-y: auto;
    word-break: break-word;
}

@media (max-width: 992px) {
    .sidebar {
        max-height: 50vh;
    }
}
</style>