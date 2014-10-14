/// <reference path="_references.ts" />
module MirGames.Forum {
    export class DeleteTopicDialogController {
        static $inject = ['$scope', 'dialog', 'apiService', 'dialogOptions'];

        constructor(private $scope: IDeleteTopicDialogControllerScope, private dialog: UI.IDialog, private apiService: Core.IApiService, private options: any) {
            var topicId: number = options['topic-id'];

            $scope.deleteTopic = () => {
                var command: MirGames.Domain.Forum.Commands.DeleteForumTopicCommand = {
                    TopicId: topicId
                };

                apiService.executeCommand('DeleteForumTopicCommand', command, result => {
                    dialog.close(true);
                });
            };

            $scope.close = () => {
                dialog.cancel();
            }
        }
    }

    export interface IDeleteTopicDialogControllerScope extends ng.IScope {
        deleteTopic(): void;
        close(): void;
    }
}