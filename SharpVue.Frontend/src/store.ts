import { reactive } from 'vue';
import { Type } from './data';

export default reactive({
    currentType: <Type | null | undefined>null
})
