import data from "@/data.json";

export interface Namespace {
    fullName: string;
    types: Type[];
}

export interface Type {
    fullName: string;
    name: string;
    namespace: string;
    inherits: string[];
    implements: string[];
    properties: Property[];
}

export interface Property {
    name: string;
    summary: string;
    remarks: string;
    returns: string;
    returnType: string;
    getter: boolean;
    setter: boolean;
}

export const allTypes = typeDictionary();

function typeDictionary() {
    var types: { [fullName: string]: Type } = {};

    for (const type of (data as Namespace[]).flatMap(o => o.types)) {
        types[type.fullName] = type;
    }

    return types;
}