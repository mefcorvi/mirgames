/// <reference path="_references.ts" />
module MirGames.Wip {
    export class ProjectPage extends MirGames.BasePage<IProjectPageData, IProjectPageScope> {
        static $inject = ['$scope', 'apiService', 'eventBus'];

        constructor($scope: IProjectGalleryPageScope, private apiService: Core.IApiService, eventBus: Core.IEventBus) {
            super($scope, eventBus);
        }
    }

    export interface IProjectPageScope extends IPageScope {
    }

    export interface IProjectPageData {
        projectAlias: string;
    }
}