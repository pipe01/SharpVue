<template lang="pug">
router-link(:to="'/ref/' + type.fullName") {{type.name}}
</template>

<script lang="ts">
import { Type } from '@/data'
import { defineComponent, PropType } from 'vue'

export default defineComponent({
    props: {
        type: Object as PropType<Type>
    }
})
</script>

<style lang="scss" scoped>
a {
    color: unset;
}
</style>