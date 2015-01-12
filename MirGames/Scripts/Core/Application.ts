/// <reference path="../_references.ts" />
module Core {
    export class Application {
        private static instance: Application;

        public static getInstance(): Application {
            if (Application.instance == null) {
                Application.instance = angular
                    .injector(['core.application'])
                    .get('application');
            }
            
            return Application.instance;
        }

        constructor(private config: IConfig, private service: IService) {
        }

        public navigateToUrl(url: string, newWindow?: boolean) {
            if (newWindow) {
                window.open(url, '_blank');
            } else {
                document.location.assign(url);
            }
        }

        /**
        * Переходит на новую страницу.
        */
        public navigateTo(controller: string, action: string, params: any = null) {
            document.location.assign(Router.action(controller, action, params));
        }
    }

    angular
        .module('core.application', ['core.config', 'core.service'])
        .factory('application', ['config', 'service', (config: IConfig, service: IService) => new Application(config, service)]);
}