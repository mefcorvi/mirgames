/// <reference path="../_references.ts" />
module MirGames.Wip {
    export class ProjectWorkItemPage extends MirGames.BasePage<IProjectWorkItemPageData, IProjectWorkItemPageScope> {
        static $inject = ['$scope', 'eventBus', 'apiService'];

        constructor($scope: IProjectWorkItemPageScope, eventBus: Core.IEventBus, private apiService: Core.IApiService) {
            super($scope, eventBus);
            this.$scope.items = this.convertItemsToScope(this.pageData.workItems);
            this.$scope.dataLoaded = true;
        }

        /** Loads the list of work items */
        private loadWorkItems() {
            var query: Domain.Wip.Queries.GetProjectWorkItemsQuery = {
                ProjectAlias: this.pageData.projectAlias
            };

            this.apiService.getAll("GetProjectWorkItemsQuery", query, 0, 20, (result) => {
                this.$scope.$apply(() => {
                    this.$scope.items = result;
                });
            });
            this.$scope.dataLoaded = true;
        }

        /** Converts DTO to the scope object */
        private convertItemsToScope(items: Domain.Wip.ViewModels.ProjectWorkItemViewModel[]): IProjectWorkItemScope[] {
            return Enumerable.from(items).select(item => this.convertItemToScope(item)).toArray();
        }

        /** Converts DTO to the scope object */
        private convertItemToScope(item: Domain.Wip.ViewModels.ProjectWorkItemViewModel): IProjectWorkItemScope {
            return {
                type: WorkItemType[item.ItemType],
                internalId: item.InternalId,
                state: WorkItemState[item.State],
                title: item.Title,
                canBeEdited: item.CanBeEdited,
                canBeDeleted: item.CanBeDeleted,
                url: Router.action("Projects", "WorkItem", { projectAlias: this.pageData.projectAlias, workItemId: item.InternalId })
            };
        }
    }

    export interface IProjectWorkItemScope {
        type: string;
        state: string;
        title: string;
        internalId: number;
        canBeEdited: boolean;
        canBeDeleted: boolean;
        url: string;
    }

    export interface IProjectWorkItemPageScope extends IPageScope {
        items: IProjectWorkItemScope[];
        dataLoaded: boolean;
    }

    export interface IProjectWorkItemPageData {
        projectAlias: string;
        workItems: Domain.Wip.ViewModels.ProjectWorkItemViewModel[];
    }

    enum WorkItemState {
        Undefined = 0,
        Open = 1,
        Closed = 2,
        Active = 3,
        Queued = 4
    }

    enum WorkItemType {
        Undefined = 0,
        Bug = 1,
        Task = 2,
        Feature = 3
    }
}