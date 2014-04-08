/// <reference path="../_references.ts" />
module Core {
    export interface ICommandBus {
        createCommandFromScope<T extends MirGames.Domain.Command>(commandType: Type<T>, scope: any): T;
        executeCommand(url: string, command: MirGames.Domain.Command, callback: (data: any) => void): void;
    }

    class CommandBus implements ICommandBus {
        constructor(private config: IConfig, private service: IService) {
        }

        public executeCommand(url: string, command: MirGames.Domain.Command, callback: (data: any) => void): void {
            this.expandArray(command.data);
            console.log(command);
            this.service.callMethod(url, command.data, callback);
        }

        public createCommandFromScope<T extends MirGames.Domain.Command>(commandType: Type<T>, scope: any): T {
            var command = <T>new commandType();
            this.fillCommandFromScope(command, scope);

            return command;
        }

        private expandArray(obj: IIndexable<any[]>) {
            for (var key in obj) {
                var item = obj[key];
                if (obj.hasOwnProperty(key) && Utils.isArray(item)) {
                    delete obj[key];

                    for (var i = 0; i < obj[key].length; i++) {
                        obj[key + "[" + i + "]"] = item[i];
                    }
                }
            }
        }

        private fillCommandFromScope(command: MirGames.Domain.Command, scope: any): void {
            var map: IIndexable<any> = <any>command;

            for (var property in map) {
                if (property != 'data' && map[property] != 'constructor') {
                    if (Utils.isUndefined(scope[property])) {
                        throw new Error("Property " + property + " have not been found in the specified scope");
                    }

                    map[property] = scope[property];
                }
            }

            if (Utils.isDefined(scope.captcha)
                && Utils.isDefined(scope.captcha.response)
                && Utils.isDefined(scope.captcha.challenge)) {
                    command.data['recaptcha_challenge_field'] = scope.captcha.challenge;
                    command.data['recaptcha_response_field'] = scope.captcha.response;
            }
        }
    }

    angular
        .module('core.commandBus', ['core.service', 'core.config'])
        .factory('commandBus', ['config', 'service', (config: IConfig, service: IService) => new CommandBus(config, service)]);
}