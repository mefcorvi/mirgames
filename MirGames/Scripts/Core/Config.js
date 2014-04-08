/// <reference path="../_references.ts" />
var Core;
(function (Core) {
    var Config = (function () {
        function Config(config) {
            if (!config.hasOwnProperty('rootUrl')) {
                throw new Error('Root URL is not configured');
            }

            if (!config.hasOwnProperty('antiForgery')) {
                throw new Error('Anti Forgery is not configured');
            }

            this.rootUrl = config.rootUrl;
            this.antiForgery = config.antiForgery;
        }
        return Config;
    })();

    angular.module('core.config', []).factory('config', function () {
        return new Config(window.config);
    });
})(Core || (Core = {}));
//# sourceMappingURL=Config.js.map
