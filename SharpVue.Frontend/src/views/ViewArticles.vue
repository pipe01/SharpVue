<template lang="pug">
main.row
    .col-lg-3(:class="[dark ? 'bg-dark' : 'bg-light']")
        .sidebar
            ArticleSidebar
    .col-lg-9.main
        ViewArticle(v-if="item" :article="item")
</template>

<script lang="ts">
import { computed, defineComponent, inject } from 'vue'
import { useRoute } from 'vue-router';

import ArticleSidebar from "@/components/articles/ArticleSidebar.vue";
import ViewArticle from "@/components/articles/ViewArticle.vue";
import { findArticle } from '@/data';

export default defineComponent({
    components: {
        ArticleSidebar,
        ViewArticle
    },

    setup() {
        const route = useRoute();

        const item = computed(() => {
            const fullName = route.params["item"] as string[];
            if (!fullName)
                return null;

            return findArticle(fullName);
        })
        
        return { dark: inject<any>("config").dark, item }
    }
})
</script>