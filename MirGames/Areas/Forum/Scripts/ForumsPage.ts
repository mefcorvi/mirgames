/// <reference path="_references.ts" />
module MirGames.Forum {
    export class ForumsPage extends MirGames.BasePage<IPageData, IForumsPageScope> {
        static $inject = ['$scope', 'commandBus', 'eventBus'];

        constructor($scope: IForumsPageScope, private commandBus: Core.ICommandBus, eventBus: Core.IEventBus) {
            super($scope, eventBus);
        }
    }

    export interface IForumsPageScope extends IPageScope {
    }
}