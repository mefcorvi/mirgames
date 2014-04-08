interface IEventEmitter {
    addListener(event: string, listener: Function): void;
    on(event: string, listener: Function): void;
    once(event: string, listener: Function): void;
    removeListener(event: string, listener: Function): void;
    removeAllListeners(event?: string): void;
    setMaxListeners(n: number): void;
    listeners(event: string): Function[];
    emit(event: string, arg1?: any, arg2?: any): void;
}

declare class EventEmitter implements IEventEmitter {
    addListener(event: string, listener: Function): void;
    on(event: string, listener: Function): void;
    once(event: string, listener: Function): void;
    removeListener(event: string, listener: Function): void;
    removeAllListeners(event?: string): void;
    setMaxListeners(n: number): void;
    listeners(event: string): Function[];
    emit(event: string, arg1?: any, arg2?: any): void;
}