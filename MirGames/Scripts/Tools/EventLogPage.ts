/// <reference path="../_references.ts" />
module MirGames.Tools {

    export class EventLogPage {
        static $inject = ['$scope', '$element', 'apiService', 'eventBus'];

        constructor(private $scope: IEventLogPageScope, private $element: JQuery, private apiService: Core.IApiService, private eventBus: Core.IEventBus) {
            this.$scope.username = '';
            this.$scope.filter = this.filter.bind(this);
            this.$scope.logType = 0;
            this.$scope.from = moment().add('d', -7).toDate();
            this.$scope.to = null;
            this.$scope.source = '';
            this.$scope.message = '';

            this.$scope.logTypes = [
                { key: 'Error', value: 0 },
                { key: 'Warning', value: 1 },
                { key: 'Information', value: 2 },
                { key: 'Verbose', value: 3 }
            ];

            this.filter();
        }

        private filter() {
            var query: MirGames.Domain.Tools.Queries.GetEventLogQuery = {
                LogType: this.$scope.logType,
                UserName: this.$scope.username,
                From: this.$scope.from,
                To: this.$scope.to,
                Message: this.$scope.message,
                Source: this.$scope.source
            };

            this.apiService.getAll('GetEventLogQuery', query, 0, 50, (result: MirGames.Domain.Tools.ViewModels.EventLogViewModel[]) => {
                this.$scope.$apply(() => {
                    this.$scope.items = Enumerable.from(result).select(item => {
                        var details: {
                            IP: string;
                            Url: string;
                            Exception: ExceptionViewModel;
                            Referrer: string;
                            Browser: string;
                        } = JSON.parse(item.Details) || {};

                        var exceptions: IException[] = [];
                        var exception = details.Exception;

                        while (exception != null) {
                            exceptions.push({
                                className: exception.ClassName,
                                message: exception.Message,
                                stackTrace: exception.StackTraceString,
                            });

                            exception = exception.InnerException;
                        }

                        return <IEventLogItemScope>{
                            id: item.Id,
                            date: item.Date,
                            details: {
                                ip: details.IP,
                                url: details.Url,
                                exceptions: exceptions,
                                collapsed: true,
                                browser: details.Browser,
                                referrer: details.Referrer
                            },
                            eventLogType: item.EventLogType,
                            login: item.Login,
                            message: item.Message,
                            source: item.Source
                        };
                    }).toArray();
                });
            });
        }
    }

    export interface IEventLogPageScope extends IPageScope {
        items: IEventLogItemScope[];
        logType: MirGames.Infrastructure.Logging.EventLogType;
        username: string;
        source: string;
        message: string;
        from: Date;
        to?: Date;
        logTypes: LogTypeScope[];
        filter(): void;
    }

    export interface IEventLogItemScope {
        id: number;
        eventLogType: MirGames.Infrastructure.Logging.EventLogType;
        login: string;
        message: string;
        source: string;
        details: {
            exceptions: IException[];
            url: string;
            ip: string;
            collapsed: boolean;
            browser: string;
            referrer: string;
        };
        date: Date;
    }

    export interface IException {
        className: string;
        message: string;
        stackTrace: string;
    }

    export interface LogTypeScope {
        key: string;
        value: number;
    }

    interface ExceptionViewModel {
        ClassName: string;
        StackTraceString: string;
        Message: string;
        InnerException: ExceptionViewModel;
    }
}