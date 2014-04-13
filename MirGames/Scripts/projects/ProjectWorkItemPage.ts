/// <reference path="../_references.ts" />
module MirGames.Wip {
    export class ProjectWorkItemPage extends MirGames.BasePage<IProjectWorkItemPageData, IProjectWorkItemPageScope> {
        static $inject = ['$scope', 'eventBus', 'apiService'];

        constructor($scope: IProjectWorkItemPageScope, eventBus: Core.IEventBus, private apiService: Core.IApiService) {
            super($scope, eventBus);
            this.$scope.comment = this.getCommentForm();
        }

        getCommentForm(): IProjectNewWorkItemCommentScope {
            return {
                attachments: [],
                focus: false,
                post: () => this.postComment(),
                text: ''
            };
        }

        postComment(): void {
            var command: Domain.Wip.Commands.PostWorkItemCommentCommand = {
                Attachments: this.$scope.comment.attachments,
                Text: this.$scope.comment.text,
                WorkItemId: this.pageData.workItemId
            };

            this.apiService.executeCommand('PostWorkItemCommentCommand', command, (commentId: number) => {
                this.$scope.$apply(() => {
                    this.$scope.comment = this.getCommentForm();
                });
            });
        }
    }

    export interface IProjectWorkItemPageScope extends IPageScope {
        comment: IProjectNewWorkItemCommentScope;
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