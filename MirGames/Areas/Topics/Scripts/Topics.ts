/// <reference path="_references.ts" />
module MirGames.Topics {
    export class TopicsPage extends MirGames.BasePage<ITopicsPageData, ITopicsScope> {
        static $inject = ['$scope', 'eventBus', 'apiService'];

        constructor($scope: ITopicsScope, eventBus: Core.IEventBus, private apiService: Core.IApiService) {
            super($scope, eventBus);
        }
    }

    export interface ITopicsScope extends ng.IScope {
    }

    export interface ITopicsPageData {
    }
}