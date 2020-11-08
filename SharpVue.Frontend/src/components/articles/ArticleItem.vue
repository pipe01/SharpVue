<template lang="pug">
li
    router-link(v-if="article.content" :to="'/articles/' + path") {{article.title}}
    span(v-else) {{article.title}}
    ul(v-if="article.children && article.children.length > 0")
        ArticleItem(v-for="item in article.children" :article="item" :path="!path ? item.title : (path + '/' + item.title)")
</template>

<script lang="ts">
import { Article } from '@/data'
import { defineComponent, PropType } from 'vue'

export default defineComponent({
    name: "ArticleItem",
    props: {
        article: {
            type: Object as PropType<Article>,
            required: true
        },
        path: {
            type: String,
            default: ""
        }
    },

    setup(props) {
        
    }
})
</script>