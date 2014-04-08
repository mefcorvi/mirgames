/// <reference path="jquery.d.ts" />

interface JQueryStatic {
    ionSound: {
        (options: IonSoundOptions): void;
        play(soundName: string): void;
        stop(soundName: string): void;
        kill(soundName: string): void;
        destroy(): void;
    }
}

interface IonSoundOptions {
    sounds: string[];
    path: string;
    multiPlay: boolean;
    volume: number;
}