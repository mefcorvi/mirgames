/// <reference path="../_references.ts" />
module MirGames.Forum {
    export class EditPostDialogController {
        static $inject = ['$scope', 'commandBus', 'dialog', 'apiService', 'dialogOptions'];

        constructor(private $scope: IEditPostDialogControllerScope, private commandBus: Core.ICommandBus, private dialog: UI.IDialog, private apiService: Core.IApiService, private options: any) {
            var postId: number = options['post-id'];
            $scope.attachments = [];
            $scope.postId = postId;
            $scope.width = "60%";

            var query: MirGames.Domain.Forum.Queries.GetForumPostForEditQuery = { PostId: postId };
            apiService.getOne('GetForumPostForEditQuery', query, (result: MirGames.Domain.Forum.ViewModels.ForumPostForEditViewModel) => {
                $scope.$apply(() => {
                    $scope.title = result.TopicTitle;
                    $scope.text = result.SourceText;
                    $scope.tags = result.TopicTags;
                    $scope.canChangeTags = result.CanChangeTags;
                    $scope.canChangeTitle = result.CanChangeTitle;
                    $scope.isFocused = true;
                });
            });

            $scope.savePost = () => {
                var command: MirGames.Domain.Forum.Commands.UpdateForumPostCommand = {
                    Attachments: $scope.attachments,
                    PostId: $scope.postId,
                    Text: $scope.text,
                    TopicsTags: $scope.tags,
                    TopicTitle: $scope.title
                };

                apiService.executeCommand('UpdateForumPostCommand', command, result => {
                    $scope.close();
                });
            };

            $scope.close = () => {
                dialog.close(true);
            }
        }
    }

    export interface IEditPostDialogControllerScope extends ng.IScope {
        editPostForm: ng.IFormController;
        postId: number;
        title: string;
        text: string;
        tags: string;
        width: string;
        isFocused: boolean;
        canChangeTitle: boolean;
        canChangeTags: boolean;
        attachments: number[];
        savePost(): void;
        close(): void;
    }
}