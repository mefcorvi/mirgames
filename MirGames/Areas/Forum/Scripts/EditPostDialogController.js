var MirGames;
(function (MirGames) {
    /// <reference path="_references.ts" />
    (function (Forum) {
        var EditPostDialogController = (function () {
            function EditPostDialogController($scope, commandBus, dialog, apiService, options) {
                this.$scope = $scope;
                this.commandBus = commandBus;
                this.dialog = dialog;
                this.apiService = apiService;
                this.options = options;
                var postId = options['post-id'];
                $scope.attachments = [];
                $scope.postId = postId;
                $scope.width = "60%";

                var query = { PostId: postId };
                apiService.getOne('GetForumPostForEditQuery', query, function (result) {
                    $scope.$apply(function () {
                        $scope.title = result.TopicTitle;
                        $scope.text = result.SourceText;
                        $scope.tags = result.TopicTags;
                        $scope.canChangeTags = result.CanChangeTags;
                        $scope.canChangeTitle = result.CanChangeTitle;
                        $scope.isFocused = true;
                    });
                });

                $scope.savePost = function () {
                    var command = {
                        Attachments: $scope.attachments,
                        PostId: $scope.postId,
                        Text: $scope.text,
                        TopicsTags: $scope.tags,
                        TopicTitle: $scope.title
                    };

                    apiService.executeCommand('UpdateForumPostCommand', command, function (result) {
                        $scope.close();
                    });
                };

                $scope.close = function () {
                    dialog.close(true);
                };
            }
            EditPostDialogController.$inject = ['$scope', 'commandBus', 'dialog', 'apiService', 'dialogOptions'];
            return EditPostDialogController;
        })();
        Forum.EditPostDialogController = EditPostDialogController;
    })(MirGames.Forum || (MirGames.Forum = {}));
    var Forum = MirGames.Forum;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=EditPostDialogController.js.map
