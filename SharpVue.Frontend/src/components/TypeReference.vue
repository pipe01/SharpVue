<template lang="pug">
h1.full-name {{item.kind}} {{brokenName}}
dl
    dt Namespace:
    dd {{item.namespace}}
dl
    dt Assembly:
    dd {{item.assembly}}

dl(v-if="item.inherits.length > 0")
    dt Inheritance:
    dd
        template(v-for="(cls, i) in item.inherits")
            reference(:to="cls")
            span(v-if="i < item.inherits.length - 1") &nbsp;&rarr;&nbsp;
</template>

<script lang="ts">
import { Type } from '@/data'
import { computed, defineComponent, PropType } from 'vue'

export default defineComponent({
    props: {
        item: Object as PropType<Type>
    },

    setup(props) {
        // Break full name by words and join them with zero-width spaces to improve word-wrap
        const brokenName = computed(() => props.item && Array.from(props.item.fullName.matchAll(/[A-Z][^A-Z]*/g)).join("\u200b"));

        return { brokenName }
    }
})
</script>

<style lang="scss" scoped>
dt, dd {
    display: inline;
}

dt {
    margin-right: .25rem;
}

.full-name {
    word-break: break-word;
}
</style>