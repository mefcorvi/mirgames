var MirGames;
(function (MirGames) {
    /// <reference path="../_references.ts" />
    (function (Forum) {
        var DeleteTopicDialogController = (function () {
            function DeleteTopicDialogController($scope, commandBus, dialog, apiService, options) {
                this.$scope = $scope;
                this.commandBus = commandBus;
                this.dialog = dialog;
                this.apiService = apiService;
                this.options = options;
                var topicId = options['topic-id'];

                $scope.deleteTopic = function () {
                    var command = {
                        TopicId: topicId
                    };

                    apiService.executeCommand('DeleteForumTopicCommand', command, function (result) {
                        dialog.close(true);
                    });
                };

                $scope.close = function () {
                    dialog.cancel();
                };
            }
            DeleteTopicDialogController.$inject = ['$scope', 'commandBus', 'dialog', 'apiService', 'dialogOptions'];
            return DeleteTopicDialogController;
        })();
        Forum.DeleteTopicDialogController = DeleteTopicDialogController;
    })(MirGames.Forum || (MirGames.Forum = {}));
    var Forum = MirGames.Forum;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=DeleteTopicDialogController.js.map
