declare global {
    interface Window {
        data: Data;
    }
}

if (process.env.NODE_ENV == "development") {
    require("@/gen/data.js");
}

const data = window.data;
export default data;

interface Descriptable {
    name: string;
    summary?: Content | null;
    remarks?: Content | null;
}

interface Member extends Descriptable {
    returns?: Content | null;
    returnType: Content;
    inheritedFrom: string;
}

export interface Data {
    namespaces: Namespace[];
    articles: Article[];
    config: Configuration;
}

export interface Configuration {
    appName: string;
    dark: boolean;
}

export interface Article {
    title: string;
    content?: string;
    children: Article[];
}

export interface Namespace {
    fullName: string;
    types: Type[];
}

export interface Type extends Descriptable {
    fullName: string;
    kind: "class" | "enum" | "struct" | "interface" | "type";
    namespace: string;
    assembly: string;

    inherits: string[];
    implements: string[];
    properties: Property[];
    methods: Method[];
}

export interface Property extends Member {
    getter: boolean;
    setter: boolean;
}

export interface Method extends Member {
    prettyName: Content;
    parameters: Parameter[];
}

export interface Parameter {
    name: string;
    type: Content;
    description?: Content | null;
}

export interface Field extends Member {
    readOnly: boolean;
}

export interface Content {
    insertions: ContentInsertion[];
}

export function contentText(c: Content) {
    return c.insertions.map(o => o.text).join("");
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

    for (const type of data.namespaces.flatMap(o => o.types)) {
        types[type.fullName] = type;
    }

    return types;
}();

export function findArticle(path: string[]): Article | null {
    return findArticleIn(path, data.articles);
}

function findArticleIn(path: string[], haystack: Article[]): Article | null {
    for (const item of haystack) {
        if (path[0] == item.title) {
            path.shift();

            if (path.length == 0) {
                return item;
            }

            return findArticleIn(path, item.children);
        }
    }

    return null;
}
