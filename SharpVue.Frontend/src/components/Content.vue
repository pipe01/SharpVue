<template lang="pug">
template(v-for="ins in modelValue.insertions")
    //- Plain text
    template(v-if="ins.type == 0")
        | {{ins.text}}

    //- Site link
    template(v-else-if="ins.type == 1")
        router-link.code-word(:to="ins.data") {{ins.text}}

    //- Lang keyword
    template(v-else-if="ins.type == 2")
        span.code-word {{ins.text}}
</template>

<script lang="ts">
import { Content, InsertionType } from '@/data'
import { computed, defineComponent, PropType } from 'vue'

export default defineComponent({
    props: {
        modelValue: Object as PropType<Content>
    },
})
</script>
