/// <reference path="../_references.ts" />
module MirGames.Wip {
    export class ProjectWorkItemsPage extends MirGames.BasePage<IProjectWorkItemsPageData, IProjectWorkItemsPageScope> {
        static $inject = ['$scope', 'eventBus', 'apiService'];

        constructor($scope: IProjectWorkItemsPageScope, eventBus: Core.IEventBus, private apiService: Core.IApiService) {
            super($scope, eventBus);
            this.$scope.view = ProjectWorkItemList;
            this.$scope.viewMode = ViewMode.List;
            this.$scope.typeNames = ['Неизвестный', 'Ошибка', 'Задача', 'Фича'];
            this.$scope.statusNames = ['Неизестный', 'Открытая', 'Закрытая', 'Активная', 'В очереди', 'Удаленная'];
            this.$scope.newItem = this.getEmptyNewItem();

            this.$scope.filterByType = this.pageData.filterByType;
            this.$scope.filterByStatus = null;

            this.$scope.setFilterByType = filter => this.setFilterByType(filter);
            this.$scope.setFilterByStatus = filter => this.setFilterByStatus(filter);

            this.$scope.showBlocks = () => this.showBlocks();
            this.$scope.showList = () => this.showList();
        }

        private getEmptyNewItem(): IProjectNewWorkItemScope {
            return {
                attachments: [],
                focus: false,
                post: () => this.postNewItem(),
                tags: '',
                text: '',
                title: '',
                type: Domain.Wip.ViewModels.WorkItemType.Bug,
                availableItemTypes: this.pageData.availableItemTypes
            };
        }

        /** Shows work items as a blocks */
        private showBlocks() {
            this.$scope.view = ProjectWorkItemBlocks;
            this.$scope.viewMode = ViewMode.Blocks;
            setTimeout(() => {
                this.$scope.$parent.$digest();
            }, 0);
        }

        /** Show work items as a list */
        private showList() {
            this.$scope.view = ProjectWorkItemList;
            this.$scope.viewMode = ViewMode.List;
            this.$scope.$digest();
        }

        /** Posts the new item */
        private postNewItem(): void {
            var command: Domain.Wip.Commands.CreateNewProjectWorkItemCommand = {
                ProjectAlias: this.pageData.projectAlias,
                Title: this.$scope.newItem.title,
                Tags: this.$scope.newItem.tags,
                Type: this.$scope.newItem.type,
                Attachments: this.$scope.newItem.attachments,
                Description: this.$scope.newItem.text
            };

            this.apiService.executeCommand('CreateNewProjectWorkItemCommand', command, (internalId: number) => {
                this.eventBus.emit(this.pageData.projectAlias + '.workitems.new', internalId);
                this.$scope.$apply(() => {
                    this.$scope.newItem = this.getEmptyNewItem();
                    this.$scope.newItem.focus = true;
                });
            });
        }

        /** Sets filter by type  */
        private setFilterByType(itemType?: Domain.Wip.ViewModels.WorkItemType) {
            this.$scope.filterByType = itemType;
            this.loadWorkItems();
        }

        /** Sets filter by status  */
        private setFilterByStatus(itemStatus?: Domain.Wip.ViewModels.WorkItemState) {
            this.$scope.filterByStatus = itemStatus;
            this.loadWorkItems();
        }

        /** Loads work items */
        private loadWorkItems() {
        }
    }

    export interface IWorkItemAuthorScope {
        login: string;
        avatar: string;
        id: number;
    }

    export interface IProjectWorkItemScope {
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
        author: IWorkItemAuthorScope;
        changeState: () => void;
    }

    export interface IProjectWorkItemTagScope {
        text: string;
        url: string;
    }

    export interface IProjectWorkItemsPageScope extends IPageScope {
        newItem: IProjectNewWorkItemScope;
        typeNames: string[];
        statusNames: string[];

        view: any;
        viewMode: ViewMode;

        showList: () => void;
        showBlocks: () => void;

        filterByType?: Domain.Wip.ViewModels.WorkItemType;
        filterByStatus?: Domain.Wip.ViewModels.WorkItemState;

        setFilterByType: (filterByType?: Domain.Wip.ViewModels.WorkItemType) => void;
        setFilterByStatus: (filterByStatus?: Domain.Wip.ViewModels.WorkItemState) => void;
    }

    export interface IProjectNewWorkItemScope {
        title: string;
        text: string;
        tags: string;
        post: () => void ;
        attachments: number[];
        type: Domain.Wip.ViewModels.WorkItemType;
        availableItemTypes: number[];
        focus: boolean;
    }

    export interface IProjectWorkItemsPageData {
        projectAlias: string;
        workItems: Domain.Wip.ViewModels.ProjectWorkItemViewModel[];
        filterByType?: Domain.Wip.ViewModels.WorkItemType;
        availableItemTypes: number[];
    }

    export enum WorkItemState {
        Undefined = 0,
        Open = 1,
        Closed = 2,
        Active = 3,
        Queued = 4,
        Removed = 5
    }

    export enum WorkItemType {
        Undefined = 0,
        Bug = 1,
        Task = 2,
        Feature = 3
    }

    export enum ViewMode {
        List = 0,
        Blocks = 1
    }
}