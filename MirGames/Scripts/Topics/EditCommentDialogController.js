var MirGames;
(function (MirGames) {
    /// <reference path="../_references.ts" />
    (function (Topics) {
        var EditCommentDialogController = (function () {
            function EditCommentDialogController($scope, dialog, apiService, options) {
                this.$scope = $scope;
                this.dialog = dialog;
                this.apiService = apiService;
                this.options = options;
                var commentId = options['comment-id'];
                $scope.attachments = [];
                $scope.commentId = commentId;
                $scope.width = "60%";

                var query = { CommentId: commentId };
                apiService.getOne('GetCommentForEditQuery', query, function (result) {
                    $scope.$apply(function () {
                        $scope.text = result.SourceText;
                        $scope.isFocused = true;
                    });
                });

                $scope.saveComment = function () {
                    var command = {
                        Attachments: $scope.attachments,
                        Text: $scope.text,
                        CommentId: $scope.commentId
                    };

                    apiService.executeCommand('EditCommentCommand', command, function (result) {
                        $scope.close();
                    });
                };

                $scope.close = function () {
                    dialog.close(true);
                };
            }
            EditCommentDialogController.$inject = ['$scope', 'dialog', 'apiService', 'dialogOptions'];
            return EditCommentDialogController;
        })();
        Topics.EditCommentDialogController = EditCommentDialogController;
    })(MirGames.Topics || (MirGames.Topics = {}));
    var Topics = MirGames.Topics;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=EditCommentDialogController.js.map
