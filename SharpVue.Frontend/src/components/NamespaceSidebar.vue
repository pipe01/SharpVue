<template lang="pug">
ul
    li.position-relative(v-for="item in data.namespaces" :key="item.fullName")
        NamespaceItem(:namespace="item")
</template>

<script lang="ts">
import { defineComponent } from 'vue'
import data from "@/data";

import NamespaceItem from "@/components/NamespaceItem.vue";

export default defineComponent({
    components: {
        NamespaceItem
    },
    data() {
        return { data }
    }
})
</script>