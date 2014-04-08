/// <reference path="../_references.ts" />
var Core;
(function (Core) {
    var Routing = (function () {
        function Routing(config) {
            this.config = config;
            this.routingManager = $["routeManager"];
            this.map("Topics/?searchString={searchQuery}", { controller: "Topics", action: "Search" });
            this.map("Topics/{topicId}", { controller: "Topics", action: "Topic" });
            this.map("Accounts/{userId}", { controller: "Account", action: "Profile" });
            this.map("{controller}/{action}/{id}", { controller: "Home", action: "Index" });
        }
        Routing.prototype.map = function (pattern, data) {
            this.routingManager.mapRoute(pattern, pattern, data);
        };

        Routing.prototype.url = function (controller, action, params) {
            if (typeof params === "undefined") { params = null; }
            var routeData = {
                controller: controller,
                action: action
            };

            if (params != null) {
                $.extend(routeData, params);
            }

            var routeUrl = this.routingManager.action(routeData).toUrl();

            return this.config.rootUrl + routeUrl;
        };
        return Routing;
    })();

    angular.module('core.routing', ['core.config']).factory('routing', ['config', function (config) {
            return new Routing(config);
        }]);
})(Core || (Core = {}));
//# sourceMappingURL=Routing.js.map
