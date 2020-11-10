<template lang="pug">
.d-flex.flex-column.h-100
    #top
    Navbar

    router-view.flex-grow-1
</template>

<script lang="ts">
import { defineComponent } from 'vue'

import Navbar from "@/components/Navbar.vue";

export default defineComponent({
    components: { Navbar }
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