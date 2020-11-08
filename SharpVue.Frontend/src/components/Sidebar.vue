<template lang="pug">
ul
    li(v-for="item in namespaces" :key="item.fullName")
        NamespaceItem(:namespace="item")
</template>

<script lang="ts">
import { defineComponent } from 'vue'
import { namespaces } from "@/gen/data.json";

import NamespaceItem from "@/components/NamespaceItem.vue";

export default defineComponent({
    components: {
        NamespaceItem
    },
    data() {
        return { namespaces }
    }
})
</script>