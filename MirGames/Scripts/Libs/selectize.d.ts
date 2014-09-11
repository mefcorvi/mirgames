/// <reference path="jquery.d.ts" />

interface JQuery {
    selectize(options: ISelectizeOptions): void;
}

interface ISelectizeOptions {
    delimeter?: string;
    diacritics?: boolean;
    persist?: boolean;
    create?: (input: string, callback: Function) => any;
    createOnBlur?: boolean;
    createFilter?: RegExp;
    highlight?: boolean;
    openOnFocus?: boolean;
    maxOptions?: number;
    maxItems?: number;
    hideSelected?: boolean;
    allowEmptyOption?: boolean;
    scrollDuration?: number;
    loadThrottle?: number;
    preload?: any;
    dropdownParent?: string;
    addPrecedence?: boolean;
    selectOnTab?: boolean;
    options?: any[];
    dataAttr?: string;
    valueField?: string;
    optgroupValueField?: string;
    labelField?: string;
    optgroupLabelField?: string;
    optgroupField?: string;
    sortField?: any;
    searchField?: string[];
    searchConjunction?: string;
    optgroupOrder?: any;
    render?: any;

    load?: (query: string, callback: Function) => void;
    score?: (search: ISelectizeSearch) => number;
    onInitialize?: () => void;
    onChange?: (value: any) => void;
    onItemAdd?: (value: any, $item: any) => void;
    onItemRemove?: (value: any) => void;
    onClear?: () => void;
    onDelete?: (values: any[]) => void;
    onOptionAdd?: (value: any, data: any) => void;
    onOptionRemove?: (value: any) => void;
    onDropdownOpen?: ($dropdown: JQuery) => void;
    onDropdownClose?: ($dropdown: JQuery) => void;
    onType?: (str: string) => void;
    onLoad?: (data: any) => void;
}

interface ISelectizeSearch {
    options: any;
    query: string;
    tokens: { 'string': string; regex: RegExp }[];
    total: number;
    items: { score: number; id: string; }[];
}