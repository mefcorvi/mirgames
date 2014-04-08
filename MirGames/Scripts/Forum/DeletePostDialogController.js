var MirGames;
(function (MirGames) {
    /// <reference path="../_references.ts" />
    (function (Forum) {
        var DeletePostDialogController = (function () {
            function DeletePostDialogController($scope, commandBus, dialog, apiService, options) {
                this.$scope = $scope;
                this.commandBus = commandBus;
                this.dialog = dialog;
                this.apiService = apiService;
                this.options = options;
                var postId = options['post-id'];

                $scope.deletePost = function () {
                    var command = {
                        PostId: postId
                    };

                    apiService.executeCommand('DeleteForumPostCommand', command, function (result) {
                        dialog.close(true);
                    });
                };

                $scope.close = function () {
                    dialog.cancel();
                };
            }
            DeletePostDialogController.$inject = ['$scope', 'commandBus', 'dialog', 'apiService', 'dialogOptions'];
            return DeletePostDialogController;
        })();
        Forum.DeletePostDialogController = DeletePostDialogController;
    })(MirGames.Forum || (MirGames.Forum = {}));
    var Forum = MirGames.Forum;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=DeletePostDialogController.js.map
