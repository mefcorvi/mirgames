/// <reference path="../_references.ts" />
module MirGames.Topics {
    export class DeleteCommentDialogController {
        static $inject = ['$scope', 'dialog', 'apiService', 'dialogOptions'];

        constructor(private $scope: IDeleteCommentDialogControllerScope, private dialog: UI.IDialog, private apiService: Core.IApiService, private options: any) {
            var commentId: number = options['comment-id'];

            $scope.deleteComment = () => {
                var command: MirGames.Domain.Topics.Commands.DeleteCommentCommand = {
                    CommentId: commentId
                };

                apiService.executeCommand('DeleteCommentCommand', command, result => {
                    dialog.close(true);
                });
            };

            $scope.close = () => {
                dialog.cancel();
            }
        }
    }

    export interface IDeleteCommentDialogControllerScope extends ng.IScope {
        deleteComment(): void;
        close(): void;
    }
}