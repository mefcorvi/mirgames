/// <reference path="../_references.ts" />
var Core;
(function (Core) {
    var QueryBus = (function () {
        function QueryBus(config, service) {
            this.config = config;
            this.service = service;
        }
        QueryBus.prototype.getAll = function (query, callback) {
            this.service.callMethod('api/all', command.data, callback);
        };

        QueryBus.prototype.createCommandFromScope = function (commandType, scope) {
            var command = new commandType();
            this.fillCommandFromScope(command, scope);

            return command;
        };

        QueryBus.prototype.expandArray = function (obj) {
            for (var key in obj) {
                if (obj.hasOwnProperty(key) && Utils.isArray(obj[key])) {
                    var arr = obj[key];
                    delete obj[key];

                    for (var i = 0; i < arr.length; i++) {
                        obj[key + "[" + i + "]"] = arr[i];
                    }
                }
            }
        };

        QueryBus.prototype.fillCommandFromScope = function (command, scope) {
            for (var property in command) {
                if (property != 'data' && command[property] != 'constructor') {
                    if (Utils.isUndefined(scope[property])) {
                        throw new Error("Property " + property + " have not been found in the specified scope");
                    }

                    command[property] = scope[property];
                }
            }

            if (Utils.isDefined(scope.captcha) && Utils.isDefined(scope.captcha.response) && Utils.isDefined(scope.captcha.challenge)) {
                command.data['recaptcha_challenge_field'] = scope.captcha.challenge;
                command.data['recaptcha_response_field'] = scope.captcha.response;
            }
        };
        return QueryBus;
    })();

    angular.module('core.commandBus', ['core.service', 'core.config']).factory('commandBus', ['config', 'service', function (config, service) {
            return new CommandBus(config, service);
        }]);
})(Core || (Core = {}));
//# sourceMappingURL=QueryBus.js.map
