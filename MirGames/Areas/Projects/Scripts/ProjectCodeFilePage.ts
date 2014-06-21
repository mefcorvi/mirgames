/// <reference path="_references.ts" />
module MirGames.Wip {
    export class ProjectCodeFilePage extends MirGames.BasePage<IProjectCodeFilePageData, IProjectCodeFilePageScope> {
        static $inject = ['$scope', 'commandBus', 'eventBus'];

        constructor($scope: IProjectCodeFilePageScope, private commandBus: Core.ICommandBus, eventBus: Core.IEventBus) {
            super($scope, eventBus);
        }
    }

    export interface IProjectCodeFilePageScope extends IPageScope {
    }

    export interface IProjectCodeFilePageData {
    }
}