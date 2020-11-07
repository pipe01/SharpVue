import data from "@/data.json";

export interface Namespace {
    fullName: string;
    types: Type[];
}

export interface Type {
    fullName: string;
    name: string;
    namespace: string;
    assembly: string;
    summary: Content;

    inherits: string[];
    implements: string[];
    properties: Property[];
}

export interface Property {
    name: string;
    summary: Content;
    remarks: Content;
    returns: Content;
    
    returnType: string;
    getter: boolean;
    setter: boolean;
    inheritedFrom: string;
}

export interface Content {
    insertions: ContentInsertion[];
}

export interface ContentInsertion {
    type: InsertionType;
    text: string;
    data?: string;
}

export enum InsertionType {
    PlainText = 0,
    SiteLink,
    LangKeyword,
}

export const allTypes = function() {
    var types: { [fullName: string]: Type } = {};

    for (const type of (data as Namespace[]).flatMap(o => o.types)) {
        types[type.fullName] = type;
    }

    return types;
}();
