/// <reference path="../_references.ts" />
module Core {
    export interface IConfig {
        rootUrl: string;
        antiForgery: string;
    }

    class Config {
        public rootUrl: string;
        public antiForgery: string;

        constructor(config: IConfig) {
            if (!config.hasOwnProperty('rootUrl')) {
                throw new Error('Root URL is not configured');
            }

            if (!config.hasOwnProperty('antiForgery')) {
                throw new Error('Anti Forgery is not configured');
            }

            this.rootUrl = config.rootUrl;
            this.antiForgery = config.antiForgery;
        }
    }

    angular
        .module('core.config', [])
        .factory('config', () => new Config(window.config));
}

interface Window {
    config: Core.IConfig;
}