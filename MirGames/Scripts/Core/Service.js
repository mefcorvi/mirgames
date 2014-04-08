/// <reference path="../_references.ts" />
var Core;
(function (Core) {
    var Service = (function () {
        function Service(config, eventBus) {
            this.config = config;
            this.eventBus = eventBus;
        }
        Service.prototype.callMethod = function (url, data, callback) {
            var _this = this;
            if (data == null) {
                data = {};
            }

            data['__RequestVerificationToken'] = this.config.antiForgery;
            this.eventBus.emit('ajax-request.executing');

            $.ajax({
                url: url,
                data: data,
                type: 'POST'
            }).done(function (result) {
                _this.eventBus.emit('ajax-request.executed');

                if (callback != null) {
                    callback(result);
                }
            }).fail(function (context, failType, failText) {
                _this.eventBus.emit('ajax-request.failed', failType, failText);
            });
        };

        Service.prototype.callAction = function (controller, action, data, callback) {
            this.callMethod(Router.action(controller, action), data, callback);
        };
        return Service;
    })();

    angular.module('core.service', ['core.config', 'core.eventBus']).factory('service', ['config', 'eventBus', function (config, eventBus) {
            return new Service(config, eventBus);
        }]);
})(Core || (Core = {}));
//# sourceMappingURL=Service.js.map
