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
                    this.$scope.items = result;
                });
            });
        }
    }

    export interface IEventLogPageScope extends IPageScope {
        items: MirGames.Domain.Tools.ViewModels.EventLogViewModel[];
        logType: MirGames.Infrastructure.Logging.EventLogType;
        username: string;
        source: string;
        message: string;
        from: Date;
        to?: Date;
        logTypes: LogTypeScope[];
        filter(): void;
    }

    export interface LogTypeScope {
        key: string;
        value: number;
    }
}