/// <reference path="jquery.d.ts" />

interface JQueryStatic {
    cookie(key: string): string;
    cookie(key: string, value: string): void;
    cookie(key: string, value: string, options: ICookieOptions): void;
}

interface ICookieOptions {
    expires?: number;
    path?: string;
    domain?: string;
    secure?: boolean;
}
