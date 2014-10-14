/// <reference path="_references.ts" />
module MirGames.Forum {
    export class ForumsPage extends MirGames.BasePage<IPageData, IForumsPageScope> {
        static $inject = ['$scope', 'eventBus'];

        constructor($scope: IForumsPageScope, eventBus: Core.IEventBus) {
            super($scope, eventBus);
        }
    }

    export interface IForumsPageScope extends IPageScope {
    }
}