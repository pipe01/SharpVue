<template lang="pug">
.d-flex.flex-column.h-100
    nav.navbar.navbar-dark.bg-dark
        ul.navbar-nav
            li.nav-item
                router-link.nav-link(to="/ref") API reference

    router-view.flex-grow-1
</template>

<style>
html, body, #app {
    height: 100%;
    overflow-x: hidden;
}

.code-word, .code-word:hover {
    color: #4dc5ff;
}
</style>