<template lang="pug">
div(v-html="article.content")
</template>

<script lang="ts">
import { Article } from '@/data'
import { defineComponent, PropType } from 'vue'

export default defineComponent({
    props: {
        article: Object as PropType<Article>
    },
})
</script>