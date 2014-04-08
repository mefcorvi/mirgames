/// <reference path="../_references.ts" />
module Core {
    export interface IApiService {
        getAll(queryType: string, query: any, pageNum: number, pageSize: number, callback: (result: any[]) => void, blockInput?: boolean): void;
        getOne(queryType: string, query: any, callback: (result: any) => void, blockInput?: boolean): void;
        executeCommand(commandType: string, command: any, callback?: (result: any) => void, blockInput?: boolean): void;
    }

    class ApiService implements IApiService {
        constructor(private config: IConfig, private eventBus: IEventBus) {
        }

        public getAll(queryType: string, query: any, pageNum: number, pageSize: number, callback: (result: any[]) => void, blockInput: boolean = true): void {
            query.type = queryType;
            this.invokeQuery('all', query, { pageNum: pageNum, pageSize: pageSize }, (res: any) => callback(res), blockInput);
        }

        public getOne(queryType: string, query: any, callback: (result: any) => void, blockInput: boolean = true): void {
            query.type = queryType;
            this.invokeQuery('one', query, {}, (res: any) => callback(res), blockInput);
        }

        public executeCommand(commandType: string, command: any, callback?: (result: any) => void, blockInput: boolean = true): void {
            command.type = commandType;

            if (blockInput) {
                this.eventBus.emit('ajax-request.executing');
            }

            $.ajax({
                url: this.config.rootUrl + 'api',
                data: JSON.stringify({ Command: JSON.stringify(command), SessionId: $.cookie('key') }),
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8'
            }).done((result: any) => {
                    if (blockInput) {
                        this.eventBus.emit('ajax-request.executed');
                    }

                    if (callback != null) {
                        callback(result);
                    }
                }).fail((context: any, failType: string, failText: string) => {
                    this.eventBus.emit('ajax-request.failed', failType, failText);
                });
        }

        private invokeQuery(type: string, query: any, data: any, callback: (result: any) => void, blockInput: boolean = true) {
            if (blockInput) {
                this.eventBus.emit('ajax-request.executing');
            }

            data['query'] = JSON.stringify(query);
            data['sessionId'] = $.cookie('key');

            $.ajax({
                url: this.config.rootUrl + 'api/' + type,
                data: data,
                type: 'GET'
            }).done((result: any) => {
                    if (blockInput) {
                        this.eventBus.emit('ajax-request.executed');
                    }

                    if (callback != null) {
                        callback(result);
                    }
                }).fail((context: any, failType: string, failText: string) => {
                    this.eventBus.emit('ajax-request.failed', failType, failText);
                });
        }
    }

    angular
        .module('core.apiService', ['core.config', 'core.eventBus'])
        .factory('apiService', ['config', 'eventBus', (config: IConfig, eventBus: IEventBus) => new ApiService(config, eventBus)]);
}