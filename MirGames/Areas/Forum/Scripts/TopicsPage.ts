/// <reference path="_references.ts" />
module MirGames.Forum {
    export class TopicsPage extends MirGames.BasePage<IPageData, ITopicsPageScope> {
        static $inject = ['$scope', '$location', 'apiService', 'eventBus'];

        constructor($scope: ITopicsPageScope, private $location: ng.ILocationService, private apiService: Core.IApiService, eventBus: Core.IEventBus) {
            super($scope, eventBus);
            this.$scope.searchString = (<any>this.pageData).searchString;
            this.$scope.markAllAsRead = this.markAllAsRead.bind(this);
            this.$scope.search = () => this.search();
        }

        private search(): void {
            var address = Router.action('Forum', 'Topics', {
                area: 'Forum',
                searchString: this.$scope.searchString,
                forumAlias: (<any>this.pageData).forumAlias
            });

            this.$location.url(address);
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
        searchString: string;
        search(): void;
    }
}