export interface Namespace {
    fullName: string;
    children: { [name: string]: Namespace };
    types: Type[];
}

export interface Type {
    fullName: string;
    name: string;
    namespace: string;
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