<template lang="pug">
div
    .row.h-100
        .col-sm-2.bg-light.sidebar
            ListItem(:namespace="data")
        .col-sm-10.main
            - var n = 0;
            ul
                while n < 10
                    li= n++
</template>

<script lang="ts">
import { defineComponent } from 'vue';

import ListItem from "@/components/SidebarListItem.vue";

import data from "@/data.json";
import { Namespace } from "@/data-type";

export default defineComponent({
    components: {
        ListItem
    },
    
    setup() {
        const typed = data as unknown as Namespace
        
        for (const item in typed.children) {
            console.log(item);
            
        }

        return { data: typed, k: Object.keys(data.children) }
    }
})
</script>

<style scoped>
.sidebar, .main {
    height: 100%;
    overflow-y: auto;
}
</style>