interface JQuery {
    autosize(settings ?: IAutoSizeSettings): void;
}

interface IAutoSizeSettings {
    className?: string;
    append?: string;
    callback?: () => void;
}
