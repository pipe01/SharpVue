<template lang="pug">
main.row
    .col-lg-3(:class="[dark ? 'bg-dark' : 'bg-light']")
        .sidebar
            NamespaceSidebar
    .col-lg-9.main
        TypeReference(v-if="item" :type="item")
</template>

<script lang="ts">
import { computed, defineComponent, inject } from 'vue';
import { useRoute } from 'vue-router';

import NamespaceSidebar from "@/components/NamespaceSidebar.vue";
import TypeReference from "@/components/TypeReference.vue";
import { Namespace, allTypes } from '@/data';

export default defineComponent({
    components: {
        NamespaceSidebar,
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
ul {
    list-style: none;
    padding-left: 1rem;
}
</style>