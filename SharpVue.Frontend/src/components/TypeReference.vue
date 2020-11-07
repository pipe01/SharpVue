<template lang="pug">
h1.full-name {{type.kind}} {{brokenName}}
//- Full namespace name
dl
    dt Namespace:
    dd {{type.namespace}}

//- Assembly name
dl
    dt Assembly:
    dd {{type.assembly}}

//- Inheritance
dl(v-if="type.inherits.length > 0")
    dt Inheritance:
    dd
        template(v-for="(cls, i) in type.inherits")
            reference(:to="cls")
            span(v-if="i < type.inherits.length - 1") &nbsp;&rarr;&nbsp;

//- Interfaces
dl(v-if="type.implements && type.implements.length > 0")
    dt Implemented interfaces:
    dd 
        template(v-for="(t, i) in type.implements")
            reference(:to="t")
            span(v-if="i < type.implements.length - 1") ,

//- Summary
Content(v-model="type.summary")

hr

//- Show inherited members checkbox
//- TODO Hide if there are no inherited members
.form-check(v-if="type.inherits.length > 0")
    input.form-check-input#showInherited(type="checkbox" v-model="showInherited")
    label.form-check-label.text-muted(for="showInherited") Show inherited members

//- Properties
template.mb-4(v-if="type.properties.length > 0")
    h2 Properties

    template(v-for="prop in type.properties")
        div(v-if="!prop.inheritedFrom || showInherited" :id="prop.name")
            //- Name
            router-link.unlink(:to="'/ref/' + type.fullName + '/' + prop.name")
                h4
                    | {{type.name}}.{{prop.name}}
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
    
    hr

//- Methods
template.mb-4(v-if="type.methods.length > 0")
    h2 Methods
    
    template(v-for="method in type.methods")
        div(v-if="!method.inheritedFrom || showInherited" :id="method.name + '!' + method.parameters.length")
            //- Name
            router-link.unlink(:to="'/ref/' + type.fullName + '/' + method.name + '!' + method.parameters.length")
                h4
                    | {{type.name}}.
                    Content(v-model="method.prettyName" element="span")
                    span.text-muted(v-if="method.inheritedFrom") &nbsp;(inherited)
            
            //- Type
            dl
                dt Return type:
                dd
                    Content(v-model="method.returnType" element="span")

            //- Inheritance
            dl(v-if="method.inheritedFrom")
                dt Inherited from:
                dd
                    reference(:to="method.inheritedFrom")

            //- Summary
            Content(v-model="method.summary")

            //- Parameters
            template(v-if="method.parameters.length > 0")
                span.font-weight-bold Parameters:
                table.table
                    tbody
                        tr.param(v-for="param in method.parameters")
                            td
                                Content(v-model="param.type")
                            td {{param.name}}
                            td
                                Content(v-model="param.description")

    hr
</template>

<script lang="ts">
import { computed, defineComponent, inject, onMounted, PropType, ref, watch } from 'vue'
import { useRoute } from 'vue-router';

import { Type } from '@/data'
import Content from "@/components/Content.vue";

export default defineComponent({
    components: {
        Content
    },
    props: {
        type: Object as PropType<Type>
    },

    setup(props) {
        // Break full name by words and join them with zero-width spaces to improve word-wrap
        const brokenName = computed(() => props.type && Array.from(props.type.name.matchAll(/[A-Z][^A-Z]*/g)).join("\u200b"));

        const showInherited = ref(true);

        const route = useRoute();
        const appName = inject("appName");

        watch(() => props.type, () => document.title = `${props.type?.fullName} - ${appName} documentation`, { immediate: true });

        onMounted(() => {
            const member = route.params["member"];

            if (member) {
                const el = document.getElementById(member as string);
                el?.scrollIntoView();
                el?.classList.add("highlight");
            }
        })

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

tr.param {
    & > :nth-child(1) {
        width: 25%;
    }
    & > :nth-child(2) {
        width: 20%;
    }
    & > :nth-child(3) {
        width: 55%;
    }
}

.highlight {
    animation: glow 4s;
}

@keyframes glow {
    0% { background: rgba(yellow, .2); }
    25% { background: rgba(yellow, .1); }
    100% { background: none; }
}
</style>