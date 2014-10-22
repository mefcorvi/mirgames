/// <reference path="_references.ts" />
module MirGames.Topics {
    export class TopicsPage extends MirGames.BasePage<ITopicsPageData, ITopicsScope> {
        static $inject = ['$scope', 'eventBus', 'apiService'];

        constructor($scope: ITopicsScope, eventBus: Core.IEventBus, private apiService: Core.IApiService) {
            super($scope, eventBus);
            this.$scope.markAllTopicsAsRead = () => this.markAllTopicsAsRead();
        }

        private markAllTopicsAsRead() {
            var command: MirGames.Domain.Topics.Commands.MarkAllBlogTopicsAsReadCommand = {};
            this.apiService.executeCommand('MarkAllBlogTopicsAsReadCommand', command, () => {
                window.location.reload();
            });
        }
    }

    export interface ITopicsScope extends ng.IScope {
        markAllTopicsAsRead(): void;
    }

    export interface ITopicsPageData {
        
    }
}