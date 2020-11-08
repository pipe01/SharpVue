<template lang="pug">
ul
    ArticleItem(v-for="item in articles" :article="item" :path="item.title")
</template>

<script lang="ts">
import { defineComponent } from 'vue'

import ArticleItem from "@/components/articles/ArticleItem.vue";
import { articles } from "@/gen/data.json";

export default defineComponent({
    components: {
        ArticleItem
    },
    
    setup() {
        return { articles }
    }
})
</script>