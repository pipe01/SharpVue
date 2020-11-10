<template lang="pug">
router-link(:to="'/ref/' + type.fullName" :class="{'font-weight-bold': active}" @click="goToTop") {{type.name}}
</template>

<script lang="ts">
import { computed, defineComponent, PropType } from 'vue'

import { Type } from '@/data'
import store from "@/store"

export default defineComponent({
    props: {
        type: Object as PropType<Type>
    },

    setup(props) {
        const active = computed(() => store.currentType && props.type && (store.currentType == props.type));

        function goToTop() {
            document.getElementById("top")?.scrollIntoView();
        }

        return { active, goToTop }
    }
})
</script>

<style lang="scss" scoped>
a {
    color: unset;
}
</style>