<template lang="pug">
ul
    ArticleItem(v-for="item in articles" :article="item" :path="item.title")
</template>

<script lang="ts">
import { defineComponent } from 'vue'

import ArticleItem from "@/components/articles/ArticleItem.vue";
import data from "@/data";

export default defineComponent({
    components: {
        ArticleItem
    },
    
    setup() {
        return { articles: data.articles }
    }
})
</script>