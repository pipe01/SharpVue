<template lang="pug">
ul
    li(v-for="item in data" :key="item.fullName")
        NamespaceItem(:namespace="item")
</template>

<script lang="ts">
import { defineComponent } from 'vue'
import data from "@/gen/data.json";

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