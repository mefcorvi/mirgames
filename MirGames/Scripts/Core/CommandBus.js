/// <reference path="../_references.ts" />
var Core;
(function (Core) {
    var CommandBus = (function () {
        function CommandBus(config, service) {
            this.config = config;
            this.service = service;
        }
        CommandBus.prototype.executeCommand = function (url, command, callback) {
            this.expandArray(command.data);
            console.log(command);
            this.service.callMethod(url, command.data, callback);
        };

        CommandBus.prototype.createCommandFromScope = function (commandType, scope) {
            var command = new commandType();
            this.fillCommandFromScope(command, scope);

            return command;
        };

        CommandBus.prototype.expandArray = function (obj) {
            for (var key in obj) {
                var item = obj[key];
                if (obj.hasOwnProperty(key) && Utils.isArray(item)) {
                    for (var i = 0; i < obj[key].length; i++) {
                        obj[key + "[" + i + "]"] = item[i];
                    }

                    delete obj[key];
                }
            }
        };

        CommandBus.prototype.fillCommandFromScope = function (command, scope) {
            var map = command;

            for (var property in map) {
                if (property != 'data' && map[property] != 'constructor') {
                    if (Utils.isUndefined(scope[property])) {
                        throw new Error("Property " + property + " have not been found in the specified scope");
                    }

                    map[property] = scope[property];
                }
            }

            if (Utils.isDefined(scope.captcha) && Utils.isDefined(scope.captcha.response) && Utils.isDefined(scope.captcha.challenge)) {
                command.data['recaptcha_challenge_field'] = scope.captcha.challenge;
                command.data['recaptcha_response_field'] = scope.captcha.response;
            }
        };
        return CommandBus;
    })();

    angular.module('core.commandBus', ['core.service', 'core.config']).factory('commandBus', ['config', 'service', function (config, service) {
            return new CommandBus(config, service);
        }]);
})(Core || (Core = {}));
//# sourceMappingURL=CommandBus.js.map
