/// <reference path="../_references.ts" />
module MirGames.Wip {
    export class ProjectWorkItemsPage extends MirGames.BasePage<IProjectWorkItemsPageData, IProjectWorkItemsPageScope> {
        static $inject = ['$scope', 'eventBus', 'apiService'];

        constructor($scope: IProjectWorkItemsPageScope, eventBus: Core.IEventBus, private apiService: Core.IApiService) {
            super($scope, eventBus);
            this.$scope.items = this.convertItemsToScope(this.pageData.workItems);
            this.$scope.dataLoaded = true;

            this.$scope.typeNames = ['Неизвестный', 'Ошибка', 'Задача', 'Фича'];
            this.$scope.newItem = this.getEmptyNewItem();
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

        /** Loads the list of work items */
        private loadWorkItems() {
            var query: Domain.Wip.Queries.GetProjectWorkItemsQuery = {
                ProjectAlias: this.pageData.projectAlias,
                Tag: null
            };

            this.apiService.getAll("GetProjectWorkItemsQuery", query, 0, 20, (result) => {
                this.$scope.$apply(() => {
                    this.$scope.items = result;
                    this.$scope.dataLoaded = true;
                });
            });
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
            return {
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
                }
            };
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
                this.loadWorkItem(internalId);
                this.$scope.$apply(() => {
                    this.$scope.newItem = this.getEmptyNewItem();
                    this.$scope.newItem.focus = true;
                });
            });
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
    }

    export interface IProjectWorkItemTagScope {
        text: string;
        url: string;
    }

    export interface IProjectWorkItemsPageScope extends IPageScope {
        items: IProjectWorkItemScope[];
        dataLoaded: boolean;
        newItem: IProjectNewWorkItemScope;
        typeNames: string[];
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
        availableItemTypes: number[];
    }

    enum WorkItemState {
        Undefined = 0,
        Open = 1,
        Closed = 2,
        Active = 3,
        Queued = 4,
        Removed = 5
    }

    enum WorkItemType {
        Undefined = 0,
        Bug = 1,
        Task = 2,
        Feature = 3
    }
}