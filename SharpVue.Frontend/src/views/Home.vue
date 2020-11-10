<template lang="pug">
.container
    h1 {{appName}} documentation

    p Welcome to the main page for the {{appName}} documentation, you can go to #[router-link(:to="{name:'ViewReference'}") the API reference page] to browse the API.
</template>

<script lang="ts">
import { defineComponent, inject, onMounted } from 'vue';

export default defineComponent({
    setup() {
        const appName = inject<string>("appName");

        onMounted(() => document.title = appName + " documentation");

        return { appName }
    }
})
</script>
