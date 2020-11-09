<template lang="pug">
mixin common
    //- Type
    dl
        dt Value type:
        dd
            Content(v-model="member.returnType" element="span")

    //- Inheritance
    dl(v-if="member.inheritedFrom")
        dt Inherited from:
        dd
            reference(:to="member.inheritedFrom")

    //- Summary
    Content(v-model="member.summary")


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
template.mb-4(v-if="type.properties.length > 0 && type.kind != 'enum'")
    h2 Properties

    template(v-for="member in type.properties")
        div(v-if="!member.inheritedFrom || showInherited" :id="member.name" :key="type.fullName + member.name")
            //- Name
            h4
                router-link.code-word(:to="'/ref/' + type.fullName + '/' + member.name")
                    | {{member.name}}
                small.text-muted(v-if="member.getter && !member.setter") &nbsp;(read-only)
                small.text-muted(v-if="!member.getter && member.setter") &nbsp;(write-only)
                small.text-muted(v-if="member.inheritedFrom") &nbsp;(inherited)
            
            +common
    hr

//- Methods
template.mb-4(v-if="type.methods.length > 0 && type.kind != 'enum'")
    h2 Methods
    
    template(v-for="member in type.methods")
        div(v-if="!member.inheritedFrom || showInherited" :id="methodId(member)" :key="type.fullName + member.name")
            //- Name
            h4
                router-link.code-word(:to="'/ref/' + type.fullName + '/' + methodId(member)")
                    Content(v-model="member.prettyName" element="span")
                small.text-muted(v-if="member.inheritedFrom") &nbsp;(inherited)
            
            +common
            
            //- Parameters
            template(v-if="member.parameters.length > 0")
                span.font-weight-bold Parameters:
                table.table
                    tbody
                        tr.param(v-for="param in member.parameters")
                            td
                                Content(v-model="param.type")
                            td {{param.name}}
                            td
                                Content(v-model="param.description")

    hr

//- Fields
template.mb-4(v-if="type.fields.length > 0")
    h2 Fields
    
    template(v-for="member in type.fields")
        div(v-if="!member.inheritedFrom || showInherited" :id="member.name" :key="type.fullName + member.name")
            //- Name
            h4
                router-link.code-word(:to="'/ref/' + type.fullName + '/' + member.name")
                    | {{member.name}}
                small.text-muted(v-if="member.inheritedFrom") &nbsp;(inherited)

            +common
</template>

<script lang="ts">
import { computed, defineComponent, inject, nextTick, onMounted, onUpdated, PropType, ref, watch } from 'vue'
import { useRoute } from 'vue-router';

import { contentText, Method, Type } from '@/data'
import Content from "@/components/Content.vue";
import store from "@/store"

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

        watch(() => props.type, () => {
            document.title = `${props.type?.fullName} - ${appName} documentation`;

            // Use nextTick because we can't use immediate mode here and on the sidebar items at the same time
            nextTick(() => store.currentType = props.type);
        },
        { immediate: true });

        watch(() => route.params["member"], (newValue, oldValue) => {
            Array.from(document.getElementsByClassName("highlight")).forEach(el => el.classList.remove("highlight"));
            
            if (newValue) {
                nextTick(() => {
                    const el = document.getElementById(newValue as string);

                    el?.scrollIntoView({ behavior: "smooth" });
                    el?.classList.add("highlight");
                })
            }
        },
        { immediate: true })

        function methodId(method: Method) {
            return method.name + "(" + method.parameters.map(p => contentText(p.type)).join(", ") + ")";
        }

        return { brokenName, showInherited, methodId }
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
    text-decoration: none;
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
    animation: glow 1s 2;
}

@keyframes glow {
    0% { background: none; }
    50% { background: rgba(yellow, .1); }
    100% { background: none; }
}
</style>