/// <reference path="_references.ts" />
module MirGames.Wip {
    export class ProjectCodeFilePage extends MirGames.BasePage<IProjectCodeFilePageData, IProjectCodeFilePageScope> {
        static $inject = ['$scope', 'eventBus'];

        constructor($scope: IProjectCodeFilePageScope, eventBus: Core.IEventBus) {
            super($scope, eventBus);
        }
    }

    export interface IProjectCodeFilePageScope extends IPageScope {
    }

    export interface IProjectCodeFilePageData {
    }
}