/// <reference path="_references.ts" />
module MirGames.Wip {
    export class ProjectWorkItemsPage extends MirGames.BasePage<IProjectWorkItemsPageData, IProjectWorkItemsPageScope> {
        static $inject = ['$scope', 'eventBus', 'apiService'];

        constructor($scope: IProjectWorkItemsPageScope, eventBus: Core.IEventBus, private apiService: Core.IApiService) {
            super($scope, eventBus);
            this.$scope.viewMode = ViewMode.List;
            this.$scope.typeNames = ['Неизвестный', 'Ошибка', 'Задача', 'Фича'];
            this.$scope.statusNames = ['Неизестный', 'Открытая', 'Закрытая', 'Активная', 'В очереди', 'Удаленная'];

            this.$scope.filterByType = this.pageData.filterByType;
            this.$scope.filterByStatus = null;

            this.$scope.setFilterByType = filter => this.setFilterByType(filter);
            this.$scope.setFilterByStatus = filter => this.setFilterByStatus(filter);

            this.$scope.showBlocks = () => this.showBlocks();
            this.$scope.showList = () => this.showList();
        }

        /** Shows work items as a blocks */
        private showBlocks() {
            this.$scope.viewMode = ViewMode.Blocks;
        }

        /** Show work items as a list */
        private showList() {
            this.$scope.viewMode = ViewMode.List;
        }

        /** Sets filter by type  */
        private setFilterByType(itemType?: Domain.Wip.ViewModels.WorkItemType) {
            this.$scope.filterByType = itemType;
        }

        /** Sets filter by status  */
        private setFilterByStatus(itemStatus?: Domain.Wip.ViewModels.WorkItemState) {
            this.$scope.filterByStatus = itemStatus;
        }
    }

    export interface IWorkItemUserScope {
        login: string;
        avatar: string;
        id: number;
    }

    export interface IProjectWorkItemScope {
        workItemId: number;
        type: string;
        state: string;
        title: string;
        description: string;
        internalId: number;
        canBeEdited: boolean;
        canBeDeleted: boolean;
        priority: number;
        url: string;
        tags: IProjectWorkItemTagScope[];
        author: IWorkItemUserScope;
        assignedTo: IWorkItemUserScope;
        changeState: () => void;
    }

    export interface IProjectWorkItemTagScope {
        text: string;
        url: string;
    }

    export interface IProjectWorkItemsPageScope extends IPageScope {
        typeNames: string[];
        statusNames: string[];

        viewMode: ViewMode;

        showList: () => void;
        showBlocks: () => void;

        filterByType?: Domain.Wip.ViewModels.WorkItemType;
        filterByStatus?: Domain.Wip.ViewModels.WorkItemState;

        setFilterByType: (filterByType?: Domain.Wip.ViewModels.WorkItemType) => void;
        setFilterByStatus: (filterByStatus?: Domain.Wip.ViewModels.WorkItemState) => void;
    }

    export interface IProjectWorkItemsPageData {
        projectAlias: string;
        workItems: Domain.Wip.ViewModels.ProjectWorkItemViewModel[];
        filterByType?: Domain.Wip.ViewModels.WorkItemType;
        availableItemTypes: number[];
    }

    export enum ViewMode {
        List = 0,
        Blocks = 1
    }

    angular
        .module('ng')
        .directive('workItemsList', () => {
            return {
                restrict: 'E',
                replace: true,
                scope: {
                    'filterByType': '=',
                    'filterByStatus': '='
                },
                controller: ProjectWorkItemList,
                transclude: false,
                templateUrl: '/areas/projects/content/work-items-list.html'
            };
        });

    angular
        .module('ng')
        .directive('workItemBlocks', () => {
            return {
                restrict: 'E',
                replace: true,
                scope: {
                    'filterByType': '=',
                    'filterByStatus': '='
                },
                controller: ProjectWorkItemBlocks,
                transclude: false,
                templateUrl: '/areas/projects/content/work-item-blocks.html'
            };
        });
}