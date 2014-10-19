/// <reference path="_references.ts" />
module MirGames.Forum {
    export class TopicsPage extends MirGames.BasePage<IPageData, ITopicsPageScope> {
        static $inject = ['$scope', 'apiService', 'eventBus'];

        constructor($scope: ITopicsPageScope, private apiService: Core.IApiService, eventBus: Core.IEventBus) {
            super($scope, eventBus);
            this.$scope.markAllAsRead = this.markAllAsRead.bind(this);
        }

        private markAllAsRead(): void {
            var command: MirGames.Domain.Forum.Commands.MarkAllTopicsAsReadCommand = {
            };

            this.apiService.executeCommand('MarkAllTopicsAsReadCommand', command, () => {
                window.location.reload();
            });
        }
    }

    export interface ITopicsPageScope extends IPageScope {
        markAllAsRead: () => void;
    }
}