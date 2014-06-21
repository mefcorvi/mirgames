/// <reference path="_references.ts" />
module MirGames.Forum {
    export class DeletePostDialogController {
        static $inject = ['$scope', 'commandBus', 'dialog', 'apiService', 'dialogOptions'];

        constructor(private $scope: IDeletePostDialogControllerScope, private commandBus: Core.ICommandBus, private dialog: UI.IDialog, private apiService: Core.IApiService, private options: any) {
            var postId: number = options['post-id'];

            $scope.deletePost = () => {
                var command: MirGames.Domain.Forum.Commands.DeleteForumPostCommand = {
                    PostId: postId
                };

                apiService.executeCommand('DeleteForumPostCommand', command, result => {
                    dialog.close(true);
                });
            };

            $scope.close = () => {
                dialog.cancel();
            }
        }
    }

    export interface IDeletePostDialogControllerScope extends ng.IScope {
        deletePost(): void;
        close(): void;
    }
}