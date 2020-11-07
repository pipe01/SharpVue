import data from "@/data.json";

interface Descriptable {
    name: string;
    summary?: Content | null;
    remarks?: Content | null;
}

interface Member {
    returns?: Content | null;
    returnType: Content;
    inheritedFrom: string;
}

export interface Namespace {
    fullName: string;
    types: Type[];
}

export interface Type extends Descriptable {
    fullName: string;
    namespace: string;
    assembly: string;

    inherits: string[];
    implements: string[];
    properties: Property[];
    methods: Method[];
}

export interface Property extends Descriptable, Member {
    getter: boolean;
    setter: boolean;
}

export interface Method extends Descriptable, Member {
    prettyName: Content;
    parameters: Parameter[];
}

export interface Parameter {
    name: string;
    type: Content;
    description?: Content | null;
}

export interface Content {
    insertions: ContentInsertion[];
}

export interface ContentInsertion {
    type: InsertionType;
    text: string;
    data?: string | null;
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
