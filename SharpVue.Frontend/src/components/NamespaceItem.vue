<template lang="pug">
a.name(href="#" @click.prevent="collapsed = !collapsed") {{namespace.fullName}}
ul(v-if="!collapsed")
    li(v-for="item in namespace.types" :key="item.fullName")
        TypeItem(:type="item")
</template>

<script lang="ts">
import { defineComponent, PropType, ref, watch } from 'vue';
import { Namespace } from "@/data";

import TypeItem from "./TypeItem.vue";
import store from '@/store';

export default defineComponent({
    name: "NamespaceItem",
    components: {
        TypeItem
    },
    props: {
        namespace: Object as PropType<Namespace>
    },
    setup(props) {
        const collapsed = ref(true);
        
        watch(() => store.currentType, (newType, oldValue) => {
            if (!props.namespace?.types || !newType)
                return;
            
            if (props.namespace.types.includes(newType)) {
                collapsed.value = false;
            }
        })

        return {
            collapsed
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