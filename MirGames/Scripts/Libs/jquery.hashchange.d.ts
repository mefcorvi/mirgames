/// <reference path="jquery.d.ts" />

interface JQuery {
    hashchange(options: IHashChangeOptions): void;
}

interface IHashChangeOptions {
    hash: string;
    onSet?: () => void;
    onRemove?: () => void;
}
