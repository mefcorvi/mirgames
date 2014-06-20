/// <reference path="_references.ts" />
module MirGames.Topics {
    export class EditCommentDialogController {
        static $inject = ['$scope', 'dialog', 'apiService', 'dialogOptions'];

        constructor(private $scope: IEditCommentDialogControllerScope, private dialog: UI.IDialog, private apiService: Core.IApiService, private options: any) {
            var commentId: number = options['comment-id'];
            $scope.attachments = [];
            $scope.commentId = commentId;
            $scope.width = "60%";

            var query: MirGames.Domain.Topics.Queries.GetCommentForEditQuery = { CommentId: commentId };
            apiService.getOne('GetCommentForEditQuery', query, (result: MirGames.Domain.Topics.ViewModels.CommentForEditViewModel) => {
                $scope.$apply(() => {
                    $scope.text = result.SourceText;
                    $scope.isFocused = true;
                });
            });

            $scope.saveComment = () => {
                var command: MirGames.Domain.Topics.Commands.EditCommentCommand = {
                    Attachments: $scope.attachments,
                    Text: $scope.text,
                    CommentId: $scope.commentId
                };

                apiService.executeCommand('EditCommentCommand', command, result => {
                    $scope.close();
                });
            };

            $scope.close = () => {
                dialog.close(true);
            }
        }
    }

    export interface IEditCommentDialogControllerScope extends ng.IScope {
        editCommentForm: ng.IFormController;
        commentId: number;
        text: string;
        width: string;
        isFocused: boolean;
        attachments: number[];
        saveComment(): void;
        close(): void;
    }
}