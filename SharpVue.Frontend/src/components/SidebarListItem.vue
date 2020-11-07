<template lang="pug">
ul
    li(v-for="(val, name) in namespace.children")
        strong {{name}}
        SideBarListItem(:namespace="val")
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { Namespace } from "@/data-type";

export default defineComponent({
    name: "SideBarListItem",
    props: {
        namespace: Namespace
    }
})
</script>

<style scoped>
ul {
    list-style: none;
    padding-left: 1rem;
}
</style>