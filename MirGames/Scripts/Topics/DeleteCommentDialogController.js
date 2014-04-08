var MirGames;
(function (MirGames) {
    /// <reference path="../_references.ts" />
    (function (Topics) {
        var DeleteCommentDialogController = (function () {
            function DeleteCommentDialogController($scope, dialog, apiService, options) {
                this.$scope = $scope;
                this.dialog = dialog;
                this.apiService = apiService;
                this.options = options;
                var commentId = options['comment-id'];

                $scope.deleteComment = function () {
                    var command = {
                        CommentId: commentId
                    };

                    apiService.executeCommand('DeleteCommentCommand', command, function (result) {
                        dialog.close(true);
                    });
                };

                $scope.close = function () {
                    dialog.cancel();
                };
            }
            DeleteCommentDialogController.$inject = ['$scope', 'dialog', 'apiService', 'dialogOptions'];
            return DeleteCommentDialogController;
        })();
        Topics.DeleteCommentDialogController = DeleteCommentDialogController;
    })(MirGames.Topics || (MirGames.Topics = {}));
    var Topics = MirGames.Topics;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=DeleteCommentDialogController.js.map
