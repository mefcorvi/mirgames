/// <reference path="_references.ts" />
module MirGames.Projects {
    export class WorkItemDialogController {
        static $inject = ['$scope', 'dialog', 'apiService', 'dialogOptions', 'eventBus'];

        constructor(private $scope: IWorkItemDialogControllerScope, private dialog: UI.IDialog, private apiService: Core.IApiService, private options: any, private eventBus: Core.IEventBus) {
            $scope.internalId = options['work-item-id'];
            $scope.projectAlias = options['project-alias'];

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

                this.apiService.getAll('GetProjectWorkItemCommentsQuery', commentsQuery, 0, 100,
                (comments: MirGames.Domain.Wip.ViewModels.ProjectWorkItemCommentViewModel[]) => {
                    $scope.comments = comments;
                    $scope.commentsLoading = false;
                    $scope.$apply();
                });
            });

            $scope.comment = {
                post: () => this.postComment(),
                attachments: [],
                focus: false,
                text: ''
            };

            $scope.close = () => {
                dialog.cancel();
            }
        }

        private getTagUrl(tag: string): string {
            return Router.action("Projects", "WorkItems", { projectAlias: this.$scope.projectAlias, tag: tag.trim(), area: 'Projects', itemType: null });
        }

        private getCommentForm(): IProjectNewWorkItemCommentScope {
            return {
                attachments: [],
                focus: false,
                post: () => this.postComment(),
                text: ''
            };
        }

        private postComment(): void {
            var command: Domain.Wip.Commands.PostWorkItemCommentCommand = {
                Attachments: this.$scope.comment.attachments,
                Text: this.$scope.comment.text,
                WorkItemId: this.$scope.id
            };

            this.apiService.executeCommand('PostWorkItemCommentCommand', command, (commentId: number) => {
                this.$scope.$apply(() => {
                    this.loadComment(commentId);
                    this.$scope.comment = this.getCommentForm();
                });
            });
        }

        private loadComment(commentId: number): void {
            var query: Domain.Wip.Queries.GetProjectWorkItemCommentQuery = {
                CommentId: commentId
            };

            this.$scope.commentsLoading = true;

            this.apiService.getOne('GetProjectWorkItemCommentQuery', query, (comment: Domain.Wip.ViewModels.ProjectWorkItemCommentViewModel) => {
                this.$scope.comments.push(comment);
                this.$scope.commentsLoading = false;
                this.$scope.$apply();
            });
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
        postComment(): void;
        projectAlias: string;
        assignedTo: MirGames.Domain.Users.ViewModels.AuthorViewModel;
        comments: MirGames.Domain.Wip.ViewModels.ProjectWorkItemCommentViewModel[];
        comment: IProjectNewWorkItemCommentScope;
        width: string;
    }

    export interface IWorkItemCommentScope extends ng.IScope {
        
    }

    export interface IProjectNewWorkItemCommentScope {
        text: string;
        focus: boolean;
        attachments: number[];
        post: () => void;
    }

    export interface IProjectWorkItemPageData {
        projectAlias: string;
        workItemId: number;
    }
} 