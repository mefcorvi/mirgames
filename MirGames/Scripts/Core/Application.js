/// <reference path="../_references.ts" />
var Core;
(function (Core) {
    var Application = (function () {
        function Application(config, service) {
            this.config = config;
            this.service = service;
        }
        Application.getInstance = function () {
            if (Application.instance == null) {
                Application.instance = angular.injector(['core.application']).get('application');
            }

            return Application.instance;
        };

        Application.prototype.navigateToUrl = function (url, newWindow) {
            if (newWindow) {
                window.open(url, '_blank');
            } else {
                document.location.assign(url);
            }
        };

        /**
        * Переходит на новую страницу.
        */
        Application.prototype.navigateTo = function (controller, action, params) {
            if (typeof params === "undefined") { params = null; }
            document.location.assign(Router.action(controller, action, params));
        };
        return Application;
    })();
    Core.Application = Application;

    angular.module('core.application', ['core.config', 'core.service']).factory('application', ['config', 'service', function (config, service) {
            return new Application(config, service);
        }]).factory('pageData', [function () {
            return window['pageData'];
        }]);
})(Core || (Core = {}));
//# sourceMappingURL=Application.js.map
