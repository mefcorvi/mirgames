/// <reference path="../_references.ts" />
module Utils {
    export function isUndefined(obj: any): boolean {
        return typeof (obj) == 'undefined';
    }

    export function isArray(obj: any): boolean {
        return obj instanceof Array;
    }

    export function isDefined(obj: any): boolean {
        return !isUndefined(obj);
    }

    export function isFunction(obj: any): boolean {
        return typeof (obj) == 'function';
    }

    export function isNullOrEmpty(obj: any): boolean {
        return obj == null || obj == '';
    }
}

interface Type<T> {
    new (): T;
}

interface IIndexable<T> {
    [key: string]: T;
}