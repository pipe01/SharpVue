<template lang="pug">
main.row
    .col-lg-3(:class="[dark ? 'bg-dark' : 'bg-light']")
        .sidebar
            Sidebar
    .col-lg-9.main
        TypeReference(v-if="item" :type="item")
</template>

<script lang="ts">
import { computed, defineComponent, inject } from 'vue';
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

        return { item, dark: inject<any>("config").dark }
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
    max-height: 90vh;
    overflow-y: auto;
    word-break: break-word;
}

@media (max-width: 992px) {
    .sidebar {
        max-height: 50vh;
    }
}

ul {
    list-style: none;
    padding-left: 1rem;
}
</style>