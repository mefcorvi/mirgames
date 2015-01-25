/// <reference path="_references.ts" />
module MirGames.Forum {
    export class TopicsPage extends MirGames.BasePage<ITopicsPageData, ITopicsPageScope> {
        static $inject = ['$scope', 'apiService', 'eventBus'];

        constructor($scope: ITopicsPageScope, private apiService: Core.IApiService, eventBus: Core.IEventBus) {
            super($scope, eventBus);
        }

        private search(): void {
        }
    }

    export interface ITopicsPageScope extends IPageScope {
    }

    export interface ITopicsPageData extends IPageScope {
    }
}