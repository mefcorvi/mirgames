﻿/// <reference path="_references.ts" />
/// <reference path="ProjectWorkItemsPage.ts" />

module MirGames.Wip {
    export class ProjectWorkItemList extends MirGames.BasePage<IProjectWorkItemsPageData, IProjectWorkItemListScope> {
        static $inject = ['$scope', 'eventBus', 'apiService'];

        constructor($scope: IProjectWorkItemListScope, eventBus: Core.IEventBus, private apiService: Core.IApiService) {
            super($scope, eventBus);
            this.$scope.items = this.convertItemsToScope(this.pageData.workItems);
            this.$scope.dataLoaded = true;

            $scope.$watch('filterByType', () => this.loadWorkItems());
            $scope.$watch('filterByStatus', () => this.loadWorkItems());

            this.eventBus.on(this.pageData.projectAlias + '.workitems.new', (internalId: number) => {
                this.loadWorkItem(internalId);
            });
        }

        /** Loads the list of work items */
        private loadWorkItems() {
            this.$scope.dataLoaded = false;
            this.$scope.items = [];

            var query: Domain.Wip.Queries.GetProjectWorkItemsQuery = {
                ProjectAlias: this.pageData.projectAlias,
                Tag: null,
                WorkItemType: this.$scope.filterByType,
                WorkItemState: this.$scope.filterByStatus,
                OrderBy: Domain.Wip.ViewModels.WorkItemsOrderType.Priority
            };

            this.apiService.getAll("GetProjectWorkItemsQuery", query, 0, 20, (result) => {
                this.$scope.$apply(() => {
                    this.$scope.items = this.convertItemsToScope(result);
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
                    this.$scope.items.push(this.convertItemToScope(result));
                    this.$scope.dataLoaded = true;
                });
            });
        }

        /** Converts DTO to the scope object */
        private convertItemsToScope(items: Domain.Wip.ViewModels.ProjectWorkItemViewModel[]): IProjectWorkItemScope[] {
            return Enumerable.from(items).select(item => this.convertItemToScope(item)).toArray();
        }

        /** Converts DTO to the scope object */
        private convertItemToScope(item: Domain.Wip.ViewModels.ProjectWorkItemViewModel): IProjectWorkItemScope {
            var workItem: IProjectWorkItemScope = {
                workItemId: item.WorkItemId,
                projectAlias: this.pageData.projectAlias,
                type: Domain.Wip.ViewModels.WorkItemType[item.ItemType],
                internalId: item.InternalId,
                state: Domain.Wip.ViewModels.WorkItemState[item.State],
                title: item.Title,
                canBeEdited: item.CanBeEdited,
                canBeDeleted: item.CanBeDeleted,
                tags: this.convertTagsToScope(item.TagsList),
                description: item.ShortDescription,
                dialogUrl: Router.action("Projects", "WorkItemDialog"),
                priority: Math.round(Math.max(0, item.Priority) / 25),
                author: {
                    avatar: item.Author.AvatarUrl,
                    id: item.Author.Id,
                    login: item.Author.Login
                },
                assignedTo: {
                    avatar: item.AssignedTo.AvatarUrl,
                    id: item.AssignedTo.Id,
                    login: item.AssignedTo.Login
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
                WorkItemId: viewModel.WorkItemId,
                State: null
            };

            this.apiService.executeCommand('ChangeWorkItemStateCommand', command, (newState: Domain.Wip.ViewModels.WorkItemState) => {
                viewModel.State = newState;

                this.$scope.$apply(() => {
                    workItem.state = Domain.Wip.ViewModels.WorkItemState[viewModel.State];
                });
            });
        }
    }

    export interface IProjectWorkItemListScope extends ng.IScope {
        items: IProjectWorkItemScope[];
        dataLoaded: boolean;

        filterByType?: Domain.Wip.ViewModels.WorkItemType;
        filterByStatus?: Domain.Wip.ViewModels.WorkItemState;
    }
} 