/// <reference path="../_references.ts" />
module Core {
    export interface IService {
        callMethod(url: string, data: any, callback: (result: any) => void): void;
        callAction(controller: string, action: string, data: any, callback?: (result: any) => void): void;
    }

    class Service implements IService {
        constructor(private config: IConfig, private eventBus: IEventBus) {

        }

        public callMethod(url: string, data: any, callback: (result: any) => void) {
            if (data == null) {
                data = {}
            }

            data['__RequestVerificationToken'] = this.config.antiForgery;
            this.eventBus.emit('ajax-request.executing');

            $.ajax({
                url: url,
                data: data,
                type: 'POST'
            }).done((result: any) => {
                this.eventBus.emit('ajax-request.executed');

                if (callback != null) {
                    callback(result);
                }
            }).fail((context: any, failType: string, failText: string) => {
                this.eventBus.emit('ajax-request.failed', failType, failText);
            });
        }

        public callAction(controller: string, action: string, data: any, callback?: (result: any) => void) {
            this.callMethod(Router.action(controller, action), data, callback);
        }
    }

    angular
        .module('core.service', ['core.config', 'core.eventBus'])
        .factory('service', ['config', 'eventBus', (config: IConfig, eventBus: IEventBus) => new Service(config, eventBus)]);
}