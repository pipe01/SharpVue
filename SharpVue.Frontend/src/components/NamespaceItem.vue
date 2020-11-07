<template lang="pug">
a.name(href="#" @click.prevent="collapsed = !collapsed") {{namespace.fullName}}
ul(v-if="!collapsed")
    li(v-for="item in namespace.types" :key="item.fullName")
        TypeItem(:type="item")
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue';
import { Namespace } from "@/data";

import TypeItem from "./TypeItem.vue";

export default defineComponent({
    name: "NamespaceItem",
    components: {
        TypeItem
    },
    props: {
        namespace: Object as PropType<Namespace>
    },
    data() {
        return {
            collapsed: true
        }
    }
})
</script>

<style scoped>
.name {
    color: unset;
    font-weight: bold;
    font-size: 1.1rem;
}
</style>