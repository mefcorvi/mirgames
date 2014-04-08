// Type definitions for linq.js, ver 3.0.3-Beta4
// Project: http://linqjs.codeplex.com/
// Definitions by: neuecc (http://www.codeplex.com/site/users/view/neuecc)

declare module linqjs {
    interface IEnumerator {
        current(): any;
        moveNext(): boolean;
        dispose(): void;
    }

    interface EnumerableStatic {
        Utils: {
            createLambda(expression: any): (...params: any[]) => any;
            createEnumerable(getEnumerator: () => IEnumerator): GenericEnumerable<any>;
            createEnumerator(initialize: () => void , tryGetNext: () => boolean, dispose: () => void ): IEnumerator;
            extendTo(type: any): void;
        };
        choice(...params: any[]): GenericEnumerable<any>;
        cycle(...params: any[]): GenericEnumerable<any>;
        empty(): GenericEnumerable<any>;
        from(): GenericEnumerable<any>;
        from(obj: GenericEnumerable<any>): GenericEnumerable<any>;
        from(obj: string): GenericEnumerable<any>;
        from(obj: number): GenericEnumerable<any>;
        from<T>(obj: T[]): GenericEnumerable<T>;
        make(element: any): GenericEnumerable<any>;
        matches(input: string, pattern: RegExp): GenericEnumerable<any>;
        matches(input: string, pattern: string, flags?: string): GenericEnumerable<any>;
        range(start: number, count: number, step?: number): GenericEnumerable<any>;
        rangeDown(start: number, count: number, step?: number): GenericEnumerable<any>;
        rangeTo(start: number, to: number, step?: number): GenericEnumerable<any>;
        repeat(element: any, count?: number): GenericEnumerable<any>;
        repeatWithFinalize(initializer: () => any, finalizer: (element: any) => void): GenericEnumerable<any>;
        generate(func: () => any, count?: number): GenericEnumerable<any>;
        toInfinity(start?: number, step?: number): GenericEnumerable<any>;
        toNegativeInfinity(start?: number, step?: number): GenericEnumerable<any>;
        unfold(seed: any, func: (value: any) => any): GenericEnumerable<any>;
        defer(enumerableFactory: () => GenericEnumerable<any>): GenericEnumerable<any>;
    }

    interface GenericEnumerable<T> {
        constructor(getEnumerator: () => IEnumerator): any;
        getEnumerator(): IEnumerator;

        traverseBreadthFirst(func: (element: T) => GenericEnumerable<any>, resultSelector?: (element: T, nestLevel: number) => any): GenericEnumerable<T>;
        traverseDepthFirst(func: (element: T) => GenericEnumerable<any>, resultSelector?: (element: T, nestLevel: number) => any): GenericEnumerable<T>;
        flatten(): GenericEnumerable<T>;
        pairwise(selector: (prev: any, current: any) => any): GenericEnumerable<T>;
        scan(func: (prev: any, current: any) => any): GenericEnumerable<T>;
        scan(seed: any, func: (prev: any, current: any) => any): GenericEnumerable<T>;
        select<TResult>(selector: (element: T, index: number) => TResult): GenericEnumerable<TResult>;
        selectMany(collectionSelector: (element: T, index: number) => any[], resultSelector?: (outer: any, inner: any) => any): GenericEnumerable<T>;
        selectMany(collectionSelector: (element: T, index: number) => GenericEnumerable<any>, resultSelector?: (outer: any, inner: any) => any): GenericEnumerable<T>;
        selectMany(collectionSelector: (element: T, index: number) => { length: number;[x: number]: any; }, resultSelector?: (outer: any, inner: any) => any): GenericEnumerable<T>;
        where(predicate: (element: T, index: number) => boolean): GenericEnumerable<T>;
        choose(selector: (element: T, index: number) => any): GenericEnumerable<T>;
        ofType(type: any): GenericEnumerable<T>;
        zip(second: any[], resultSelector: (first: any, second: any, index: number) => any): GenericEnumerable<T>;
        zip(second: GenericEnumerable<T>, resultSelector: (first: any, second: any, index: number) => any): GenericEnumerable<T>;
        zip(second: { length: number;[x: number]: any; }, resultSelector: (first: any, second: any, index: number) => any): GenericEnumerable<T>;
        zip(...params: any[]): GenericEnumerable<T>; // last one is selector
        merge(second: any[], resultSelector: (first: any, second: any, index: number) => any): GenericEnumerable<T>;
        merge(second: GenericEnumerable<T>, resultSelector: (first: any, second: any, index: number) => any): GenericEnumerable<T>;
        merge(second: { length: number;[x: number]: any; }, resultSelector: (first: any, second: any, index: number) => any): GenericEnumerable<T>;
        merge(...params: any[]): GenericEnumerable<T>; // last one is selector
        join(inner: GenericEnumerable<T>, outerKeySelector: (outer: any) => any, innerKeySelector: (inner: any) => any, resultSelector: (outer: any, inner: any) => any, compareSelector?: (obj: any) => any): GenericEnumerable<T>;
        groupJoin(inner: GenericEnumerable<T>, outerKeySelector: (outer: any) => any, innerKeySelector: (inner: any) => any, resultSelector: (outer: any, inner: any) => any, compareSelector?: (obj: any) => any): GenericEnumerable<T>;
        all(predicate: (element: T) => boolean): boolean;
        any(predicate?: (element: T) => boolean): boolean;
        isEmpty(): boolean;
        concat(sequence: T[]): GenericEnumerable<T>;
        insert(index: number, second: any[]): GenericEnumerable<T>;
        insert(index: number, second: GenericEnumerable<T>): GenericEnumerable<T>;
        insert(index: number, second: { length: number;[x: number]: any; }): GenericEnumerable<T>;
        alternate(alternateValue: any): GenericEnumerable<T>;
        alternate(alternateSequence: any[]): GenericEnumerable<T>;
        alternate(alternateSequence: GenericEnumerable<T>): GenericEnumerable<T>;
        contains(value: any, compareSelector: (element: T) => any): GenericEnumerable<T>;
        contains(value: any): GenericEnumerable<T>;
        defaultIfEmpty(defaultValue?: any): GenericEnumerable<T>;
        distinct(compareSelector?: (element: T) => any): GenericEnumerable<T>;
        distinctUntilChanged(compareSelector: (element: T) => any): GenericEnumerable<T>;
        except(second: any[], compareSelector?: (element: T) => any): GenericEnumerable<T>;
        except(second: { length: number;[x: number]: any; }, compareSelector?: (element: T) => any): GenericEnumerable<T>;
        except(second: GenericEnumerable<T>, compareSelector?: (element: T) => any): GenericEnumerable<T>;
        intersect(second: any[], compareSelector?: (element: T) => any): GenericEnumerable<T>;
        intersect(second: { length: number;[x: number]: any; }, compareSelector?: (element: T) => any): GenericEnumerable<T>;
        intersect(second: GenericEnumerable<T>, compareSelector?: (element: T) => any): GenericEnumerable<T>;
        sequenceEqual(second: any[], compareSelector?: (element: T) => any): GenericEnumerable<T>;
        sequenceEqual(second: { length: number;[x: number]: any; }, compareSelector?: (element: T) => any): GenericEnumerable<T>;
        sequenceEqual(second: GenericEnumerable<T>, compareSelector?: (element: T) => any): GenericEnumerable<T>;
        union(second: any[], compareSelector?: (element: T) => any): GenericEnumerable<T>;
        union(second: { length: number;[x: number]: any; }, compareSelector?: (element: T) => any): GenericEnumerable<T>;
        union(second: GenericEnumerable<T>, compareSelector?: (element: T) => any): GenericEnumerable<T>;
        orderBy(keySelector: (element: T) => any): OrderedEnumerable<T>;
        orderByDescending(keySelector: (element: T) => any): OrderedEnumerable<T>;
        reverse(): GenericEnumerable<T>;
        shuffle(): GenericEnumerable<T>;
        weightedSample(weightSelector: (element: T) => any): GenericEnumerable<T>;
        groupBy(keySelector: (element: T) => any, elementSelector?: (element: T) => any, resultSelector?: (key: any, element: T) => any, compareSelector?: (element: T) => any): GenericEnumerable<T>;
        partitionBy(keySelector: (element: T) => any, elementSelector?: (element: T) => any, resultSelector?: (key: any, element: T) => any, compareSelector?: (element: T) => any): GenericEnumerable<T>;
        buffer(count: number): GenericEnumerable<T>;
        aggregate<TResult>(func: (prev: T, current: TResult) => TResult): TResult;
        aggregate(seed: any, func: (prev: any, current: any) => any, resultSelector?: (last: any) => any): any;
        average(selector?: (element: T) => any): number;
        count(predicate?: (element: T, index: number) => boolean): number;
        max(selector?: (element: T) => any): number;
        min(selector?: (element: T) => any): number;
        maxBy(keySelector: (element: T) => any): any;
        minBy(keySelector: (element: T) => any): any;
        sum(selector?: (element: T) => any): number;
        elementAt(index: number): T;
        elementAtOrDefault(index: number, defaultValue?: any): T;
        first(predicate?: (element: T, index: number) => boolean): T;
        firstOrDefault(predicate?: (element: T, index: number) => boolean, defaultValue?: T): T;
        last(predicate?: (element: T, index: number) => boolean): T;
        lastOrDefault(predicate?: (element: T, index: number) => boolean, defaultValue?: any): T;
        single(predicate?: (element: T, index: number) => boolean): T;
        singleOrDefault(predicate?: (element: T, index: number) => boolean, defaultValue?: T): T;
        skip(count: number): GenericEnumerable<T>;
        skipWhile(predicate: (element: T, index: number) => boolean): GenericEnumerable<T>;
        take(count: number): GenericEnumerable<T>;
        takeWhile(predicate: (element: T, index: number) => boolean): GenericEnumerable<T>;
        takeExceptLast(count?: number): GenericEnumerable<T>;
        takeFromLast(count: number): GenericEnumerable<T>;
        indexOf(item: any): number;
        indexOf(predicate: (element: T, index: number) => boolean): number;
        lastIndexOf(item: any): number;
        lastIndexOf(predicate: (element: T, index: number) => boolean): number;
        asEnumerable(): GenericEnumerable<T>;
        toArray(): T[];
        toLookup(keySelector: (element: T) => any, elementSelector?: (element: T) => any, compareSelector?: (element: T) => any): Lookup;
        toObject(keySelector: (element: T) => any, elementSelector?: (element: T) => any): Object;
        toDictionary(keySelector: (element: T) => any, elementSelector?: (element: T) => any, compareSelector?: (element: T) => any): Dictionary;
        toJSONString(replacer: (key: string, value: any) => any): string;
        toJSONString(replacer: any[]): string;
        toJSONString(replacer: (key: string, value: any) => any, space: any): string;
        toJSONString(replacer: any[], space: any): string;
        toJoinedString(separator?: string, selector?: (element: T, index: number) => any): string;
        doAction(action: (element: T, index: number) => void): GenericEnumerable<T>;
        doAction(action: (element: T, index: number) => boolean): GenericEnumerable<T>;
        forEach(action: (element: T, index: number) => void): void;
        forEach(action: (element: T, index: number) => boolean): void;
        write(separator?: string, selector?: (element: T) => any): void;
        writeLine(selector?: (element: T) => any): void;
        force(): void;
        letBind(func: (source: GenericEnumerable<T>) => any[]): GenericEnumerable<T>;
        letBind(func: (source: GenericEnumerable<T>) => { length: number;[x: number]: any; }): GenericEnumerable<T>;
        letBind(func: (source: GenericEnumerable<T>) => GenericEnumerable<T>): GenericEnumerable<T>;
        share(): DisposableEnumerable<T>;
        memoize(): DisposableEnumerable<T>;
        catchError(handler: (exception: any) => void): GenericEnumerable<T>;
        finallyAction(finallyAction: () => void): GenericEnumerable<T>;
        log(selector?: (element: T) => void): GenericEnumerable<T>;
        trace(message?: string, selector?: (element: T) => void): GenericEnumerable<T>;
    }

    interface OrderedEnumerable<T> extends GenericEnumerable<T> {
        createOrderedEnumerable(keySelector: (element: any) => any, descending: boolean): OrderedEnumerable<T>;
        thenBy(keySelector: (element: any) => any): OrderedEnumerable<T>;
        thenByDescending(keySelector: (element: any) => any): OrderedEnumerable<T>;
    }

    interface DisposableEnumerable<T> extends GenericEnumerable<T> {
        dispose(): void;
    }

    interface Dictionary {
        add(key: any, value: any): void;
        get(key: any): any;
        set(key: any, value: any): boolean;
        contains(key: any): boolean;
        clear(): void;
        remove(key: any): void;
        count(): number;
        toEnumerable(): GenericEnumerable<any>; // Enumerable<KeyValuePair>
    }

    interface Lookup {
        count(): number;
        get(key: any): GenericEnumerable<any>;
        contains(key: any): boolean;
        toEnumerable(): GenericEnumerable<any>; // Enumerable<Groping>
    }

    interface Grouping extends GenericEnumerable<any> {
        key(): any;
    }
}

// export definition
declare var Enumerable: linqjs.EnumerableStatic;