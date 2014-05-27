/// <reference path="../_references.ts" />
/// <reference path="ProjectWorkItemsPage.ts" />

module MirGames.Wip {
    export class ProjectWorkItemBlocks extends MirGames.BasePage<IProjectWorkItemsPageData, IProjectWorkItemBlocksScope> {
        static $inject = ['$scope', 'eventBus', 'apiService'];

        constructor($scope: IProjectWorkItemBlocksScope, eventBus: Core.IEventBus, private apiService: Core.IApiService) {
            super($scope, eventBus);
            this.$scope.dataLoaded = false;
            this.$scope.onDrop = ($event, $data, array) => this.onDrop($event, $data, array);
            this.$scope.dropSuccessHandler = ($event, $index, array) => this.dropSuccessHandler($event, $index, array);

            $scope.$watch('filterByType', () => this.loadWorkItems());
            $scope.$watch('filterByStatus', () => this.loadWorkItems());
            this.loadWorkItems();

            this.eventBus.on(this.pageData.projectAlias + '.workitems.new', (internalId: number) => {
                this.loadWorkItem(internalId);
            });
        }

        /** Loads the list of work items */
        private loadWorkItems() {
            this.loadWorkItemsByStatus(Domain.Wip.ViewModels.WorkItemState.Open, items => this.$scope.openedItems = items);
            this.loadWorkItemsByStatus(Domain.Wip.ViewModels.WorkItemState.Active, items => this.$scope.activeItems = items);
            this.loadWorkItemsByStatus(Domain.Wip.ViewModels.WorkItemState.Closed, items => this.$scope.closedItems = items);
        }

        private loadWorkItemsByStatus(status: Domain.Wip.ViewModels.WorkItemState, callback: (items: IProjectWorkItemScope[]) => void) {
            var query: Domain.Wip.Queries.GetProjectWorkItemsQuery = {
                ProjectAlias: this.pageData.projectAlias,
                Tag: null,
                WorkItemType: this.$scope.filterByType,
                WorkItemState: status
            };

            this.apiService.getAll("GetProjectWorkItemsQuery", query, 0, 20, (result) => {
                this.$scope.$apply(() => {
                    callback(this.convertItemsToScope(result));
                    this.$scope.dataLoaded = true;
                });
            }, false);
        }

        private loadWorkItem(internalId: number) {
            var query: Domain.Wip.Queries.GetProjectWorkItemQuery = {
                ProjectAlias: this.pageData.projectAlias,
                InternalId: internalId
            };

            this.$scope.dataLoaded = false;

            this.apiService.getOne("GetProjectWorkItemQuery", query, (result) => {
                this.$scope.$apply(() => {
                    var item = this.convertItemToScope(result);
                    this.$scope.dataLoaded = true;
                });
            }, false);
        }

        /** Converts DTO to the scope object */
        private convertItemsToScope(items: Domain.Wip.ViewModels.ProjectWorkItemViewModel[]): IProjectWorkItemScope[] {
            return Enumerable.from(items).select(item => this.convertItemToScope(item)).toArray();
        }

        /** Converts DTO to the scope object */
        private convertItemToScope(item: Domain.Wip.ViewModels.ProjectWorkItemViewModel): IProjectWorkItemScope {
            var workItem: IProjectWorkItemScope = {
                type: WorkItemType[item.ItemType],
                internalId: item.InternalId,
                state: WorkItemState[item.State],
                title: item.Title,
                canBeEdited: item.CanBeEdited,
                canBeDeleted: item.CanBeDeleted,
                tags: this.convertTagsToScope(item.TagsList),
                description: item.ShortDescription,
                url: Router.action("Projects", "WorkItem", { projectAlias: this.pageData.projectAlias, workItemId: item.InternalId }),
                priority: Math.round(Math.max(0, item.Priority) / 25),
                author: {
                    avatar: item.Author.AvatarUrl,
                    id: item.Author.Id,
                    login: item.Author.Login
                },
                changeState: () => this.changeWorkItemState(workItem, item)
            };

            return workItem;
        }

        /** Converts tag to the scope item */
        private convertTagsToScope(item: string): IProjectWorkItemTagScope[] {
            return Enumerable
                .from((item || '').split(','))
                .where(t => t != null && t != '')
                .select(tag => this.convertTagToScope(tag))
                .toArray();
        }

        /** Converts tag to the scope item */
        private convertTagToScope(item: string): IProjectWorkItemTagScope {
            return {
                text: item.trim(),
                url: Router.action("Projects", "WorkItems", { projectAlias: this.pageData.projectAlias, tag: item.trim() })
            };
        }

        /** Changes state of the work item */
        private changeWorkItemState(workItem: IProjectWorkItemScope, viewModel: Domain.Wip.ViewModels.ProjectWorkItemViewModel) {
            if (!workItem.canBeEdited) {
                return;
            }

            var command: Domain.Wip.Commands.ChangeWorkItemStateCommand = {
                WorkItemId: viewModel.WorkItemId
            };

            this.apiService.executeCommand('ChangeWorkItemStateCommand', command, (newState: Domain.Wip.ViewModels.WorkItemState) => {
                viewModel.State = newState;

                this.$scope.$apply(() => {
                    workItem.state = WorkItemState[viewModel.State];
                });
            });
        }

        /** Sets filter by type  */
        private setFilterByType(itemType?: Domain.Wip.ViewModels.WorkItemType) {
            this.$scope.filterByType = itemType;
            this.loadWorkItems();
        }

        /** Handles drop */
        private onDrop($event: any, $data: any, array: IProjectWorkItemScope[]) {
            array.push($data);
        }

        /** Handles successfull drops */
        private dropSuccessHandler($event: any, $index: any, array: IProjectWorkItemScope[]) {
            array.splice($index, 1);
        }
    }

    export interface IProjectWorkItemBlocksScope extends ng.IScope {
        openedItems: IProjectWorkItemScope[];
        activeItems: IProjectWorkItemScope[];
        closedItems: IProjectWorkItemScope[];
        dataLoaded: boolean;
        dropSuccessHandler: ($event: any, $index: any, array: IProjectWorkItemScope[]) => void;
        onDrop: ($event: any, $data: any, array: IProjectWorkItemScope[]) => void;
        filterByType?: Domain.Wip.ViewModels.WorkItemType;
    }
} 