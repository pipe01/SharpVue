<template lang="pug">
router-link(v-if="isKnownType" :to="'/ref/' + to") {{name}}
a(v-else :href="'https://docs.microsoft.com/dotnet/api/' + to" target="_blank") {{to}}
</template>

<script lang="ts">
import { computed, defineComponent } from 'vue'
import { allTypes } from '@/data';

export default defineComponent({
    props: {
        to: String
    },

    setup(props) {
        const isKnownType = computed(() => props.to! in allTypes);
        const name = computed(() => {
            if (!props.to)
                return null;
            
            var parts = props.to.split(".");
            return parts[parts.length - 1];
        })

        return { isKnownType, name }
    }
})
</script>