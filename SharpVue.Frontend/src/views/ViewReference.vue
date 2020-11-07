<template lang="pug">
main.row
    .col-sm-3.bg-dark
        .sidebar
            Sidebar
    .col-sm-9.main
        TypeReference(v-if="item" :type="item")
</template>

<script lang="ts">
import { computed, defineComponent } from 'vue';
import { useRoute } from 'vue-router';

import Sidebar from "@/components/Sidebar.vue";
import TypeReference from "@/components/TypeReference.vue";
import { Namespace, allTypes } from '@/data';

export default defineComponent({
    components: {
        Sidebar,
        TypeReference
    },

    setup() {
        const route = useRoute();
        
        const item = computed(() => {
            const fullName = route.params["item"] as string;

            if (!fullName) {
                return null;
            }

            return allTypes[fullName];
        });

        return { item }
    }
})
</script>

<style scoped>
main {
    margin-left: auto;
    margin-right: auto;
    width: 100%;
    max-width: 1600px;
}

.sidebar {
    position: sticky;
    top: 0;
}

ul {
    list-style: none;
    padding-left: 1rem;
}
</style>