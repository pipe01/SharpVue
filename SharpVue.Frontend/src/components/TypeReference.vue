<template lang="pug">
h1.full-name {{item.kind}} {{brokenName}}
//- Full namespace name
dl
    dt Namespace:
    dd {{item.namespace}}

//- Assembly name
dl
    dt Assembly:
    dd {{item.assembly}}

//- Inheritance
dl(v-if="item.inherits.length > 0")
    dt Inheritance:
    dd
        template(v-for="(cls, i) in item.inherits")
            reference(:to="cls")
            span(v-if="i < item.inherits.length - 1") &nbsp;&rarr;&nbsp;

    //- TODO Hide if there are no inherited members
    .form-check
        input.form-check-input#showInherited(type="checkbox" v-model="showInherited")
        label.form-check-label(for="showInherited") Show inherited members


//- Summary
Content(v-model="item.summary")

hr

//- Properties
template.mb-4(v-if="item.properties.length > 0")
    h2 Properties

    template(v-for="prop in item.properties")
        div(v-if="!prop.inheritedFrom || showInherited")
            //- Name
            router-link.unlink(:to="'/ref/' + item.fullName + '/' + prop.name")
                h4
                    | {{item.name}}.{{prop.name}}
                    span.text-muted(v-if="prop.getter && !prop.setter") &nbsp;(read-only)
                    span.text-muted(v-if="!prop.getter && prop.setter") &nbsp;(write-only)
                    span.text-muted(v-if="prop.inheritedFrom") &nbsp;(inherited)
            
            //- Type
            dl
                dt Value type:
                dd
                    Content(v-model="prop.returnType" element="span")

            //- Inheritance
            dl(v-if="prop.inheritedFrom")
                dt Inherited from:
                dd
                    reference(:to="prop.inheritedFrom")

            //- Summary
            Content(v-model="prop.summary")
</template>

<script lang="ts">
import { computed, defineComponent, PropType, ref } from 'vue'

import { Type } from '@/data'
import Content from "@/components/Content.vue";

export default defineComponent({
    components: {
        Content
    },
    props: {
        item: Object as PropType<Type>
    },

    setup(props) {
        // Break full name by words and join them with zero-width spaces to improve word-wrap
        const brokenName = computed(() => props.item && Array.from(props.item.name.matchAll(/[A-Z][^A-Z]*/g)).join("\u200b"));

        const showInherited = ref(true);

        return { brokenName, showInherited }
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

.unlink {
    color: unset;
}
</style>