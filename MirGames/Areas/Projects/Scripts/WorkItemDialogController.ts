/// <reference path="_references.ts" />
module MirGames.Projects {
    export class WorkItemDialogController {
        static $inject = ['$scope', 'commandBus', 'dialog', 'apiService', 'dialogOptions', 'eventBus'];

        constructor(private $scope: IWorkItemDialogControllerScope, private commandBus: Core.ICommandBus, private dialog: UI.IDialog, private apiService: Core.IApiService, private options: any, private eventBus: Core.IEventBus) {
            $scope.internalId = options['work-item-id'];
            $scope.projectAlias = options['project-alias'];
            $scope.width = '90%';

            if (!$scope.internalId || !$scope.projectAlias) {
                throw new Error('One or more required parameters have not been specified.');
            }

            var query: MirGames.Domain.Wip.Queries.GetProjectWorkItemQuery = {
                InternalId: $scope.internalId,
                ProjectAlias: $scope.projectAlias
            };

            this.apiService.getOne('GetProjectWorkItemQuery', query, (data: MirGames.Domain.Wip.ViewModels.ProjectWorkItemViewModel) => {
                $scope.id = data.WorkItemId;
                $scope.title = data.Title;
                $scope.description = data.Description;
                $scope.createdDate = data.CreatedDate;
                $scope.author = data.Author;
                $scope.assignedTo = data.AssignedTo;
                $scope.canBeCommented = data.CanBeCommented;
                $scope.tags = Enumerable.from(data.Tags).select(tag => {
                    return {
                        text: tag,
                        url: this.getTagUrl(tag)
                    }
                }).toArray();
                $scope.commentsLoading = true;
                $scope.$apply();

                var commentsQuery: MirGames.Domain.Wip.Queries.GetProjectWorkItemCommentsQuery = {
                    WorkItemId: data.WorkItemId
                };

                this.apiService.getAll('GetProjectWorkItemCommentsQuery', commentsQuery, 1, 100,
                (comments: MirGames.Domain.Wip.ViewModels.ProjectWorkItemCommentViewModel[]) => {
                    $scope.comments = comments;
                    $scope.commentsLoading = false;
                    $scope.$apply();
                });
            });

            $scope.close = () => {
                dialog.cancel();
            }
        }

        getTagUrl(tag: string): string {
            return Router.action("Projects", "WorkItems", { projectAlias: this.$scope.projectAlias, tag: tag.trim(), area: 'Projects', itemType: null });
        }
    }

    export interface IWorkItemDialogControllerScope extends ng.IScope {
        internalId: number;
        title: string;
        description: string;
        createdDate: Date;
        tags: { url: string; text: string; }[];
        author: MirGames.Domain.Users.ViewModels.AuthorViewModel;
        id: number;
        canBeCommented: boolean;
        commentsLoading: boolean;
        close(): void;
        projectAlias: string;
        assignedTo: MirGames.Domain.Users.ViewModels.AuthorViewModel;
        comments: MirGames.Domain.Wip.ViewModels.ProjectWorkItemCommentViewModel[];
        width: string;
    }

    export interface IWorkItemCommentScope extends ng.IScope {
        
    }
} 