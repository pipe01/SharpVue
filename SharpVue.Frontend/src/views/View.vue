<template lang="pug">
div
    .row.h-100
        .col-sm-2.bg-light.sidebar
            ul
                li(v-for="item in all")
                    | {{item.name}}
        .col-sm-10.main
            - var n = 0;
            ul
                while n < 10
                    li= n++
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue';

import * as data from "@/data.json";

export default defineComponent({
    setup() {
        const current = ref(data.reference[0]);

        const namespaces = data.reference.map(o => o.namespace);

        return { current, all: data.reference }
    }
})
</script>

<style scoped>
.sidebar, .main {
    height: 100%;
    overflow-y: auto;
}
</style>