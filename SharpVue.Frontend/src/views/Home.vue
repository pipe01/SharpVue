<template lang="pug">
.container
    h1 {{appName}} documentation

    p
        | This is the main page for the {{appName}} documentation, you can go to #[router-link(:to="{name:'ViewReference'}") the API reference page] to browse the API.
</template>


<script lang="ts" setup>
import { inject } from 'vue';

export const appName = inject("appName");
</script>
