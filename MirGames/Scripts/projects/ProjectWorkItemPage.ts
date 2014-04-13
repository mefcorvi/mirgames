/// <reference path="../_references.ts" />
module MirGames.Wip {
    export class ProjectWorkItemPage extends MirGames.BasePage<IProjectWorkItemPageData, IProjectWorkItemPageScope> {
        static $inject = ['$scope', 'eventBus', 'apiService'];

        constructor($scope: IProjectWorkItemPageScope, eventBus: Core.IEventBus, private apiService: Core.IApiService) {
            super($scope, eventBus);
            this.$scope.comment = this.getCommentForm();
            this.$scope.comments = [];
            this.$scope.commentsLoading = false;
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
                WorkItemId: this.pageData.workItemId
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

    export interface IProjectWorkItemPageScope extends IPageScope {
        comment: IProjectNewWorkItemCommentScope;
        comments: MirGames.Domain.Wip.ViewModels.ProjectWorkItemCommentViewModel[];
        commentsLoading: boolean;
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