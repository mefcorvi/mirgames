interface ITinycon {
    setBubble(label: any, color?: string): void;
    reset(): void;
    setImage(url: string): void;
    setOptions(options: ITinyconOptions): void;
}

interface ITinyconOptions {
    width?: number;
    height?: number;
    font?: string;
    colour?: string;
    background?: string;
    fallback?: any;
}

declare var Tinycon: ITinycon;