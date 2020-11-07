<template lang="pug">
router-link.code-word(v-if="isDocsType" :to="'/ref/' + to") {{name}}
a.code-word(v-else-if="to.startsWith('Unity')" :href="'https://docs.unity3d.com/ScriptReference/' + unityName(to) + '.html'" target="_blank") {{to}}
a.code-word(v-else :href="'https://docs.microsoft.com/dotnet/api/' + to.replace('`', '-')" target="_blank") {{keywordType ?? to}}
</template>

<script lang="ts">
import { computed, defineComponent } from 'vue'
import { allTypes } from '@/data';

const keywordTypes: { [type: string]: string } = {
    "System.String": "string",
    "System.Boolean": "bool",
    "System.Char": "char",
    "System.Decimal": "decimal",
    "System.Double": "double",
    "System.Single": "float",
    "System.Byte": "byte",
    "System.SByte": "sbyte",
    "System.Int16": "short",
    "System.Int32": "int",
    "System.Int64": "long",
    "System.UInt16": "ushort",
    "System.UInt32": "uint",
    "System.UInt64": "ulong",
}

export default defineComponent({
    props: {
        to: String
    },

    setup(props) {
        const isDocsType = computed(() => props.to! in allTypes);
        const name = computed(() => {
            if (!props.to)
                return null;
            
            var parts = props.to.split(".");
            var name = parts[parts.length - 1];

            return name;
        })

        const keywordType = computed(() => {
            if (!props.to)
                return null;

            return keywordTypes[props.to];
        })

        function unityName(type: string) {
            var parts = type.split(".");
            parts[parts.length - 1] = parts[parts.length - 1].replace("`", "_");
            parts.shift();

            return parts.join(".");
        }

        return { isDocsType, name, keywordType, unityName }
    }
})
</script>