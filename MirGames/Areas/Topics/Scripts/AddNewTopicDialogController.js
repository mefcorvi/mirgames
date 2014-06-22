var MirGames;
(function (MirGames) {
    /// <reference path="_references.ts" />
    (function (Topics) {
        var AddNewTopicDialogController = (function () {
            function AddNewTopicDialogController($scope, dialog, apiService, options) {
                var _this = this;
                this.$scope = $scope;
                this.dialog = dialog;
                this.apiService = apiService;
                this.options = options;
                this.$scope.blogId = options['blog-id'] || null;
                this.$scope.attachments = [];
                this.$scope.isTitleFocused = true;
                this.$scope.width = '60%';

                this.$scope.save = function () {
                    return _this.submit();
                };
                this.$scope.close = function () {
                    return dialog.cancel();
                };
            }
            AddNewTopicDialogController.prototype.submit = function () {
                var command = {
                    Attachments: this.$scope.attachments,
                    BlogId: this.$scope.blogId,
                    Tags: this.$scope.tags,
                    Text: this.$scope.text,
                    Title: this.$scope.title
                };

                this.apiService.executeCommand('AddNewTopicCommand', command, function (result) {
                    Core.Application.getInstance().navigateToUrl(Router.action("Topics", "Topic", { topicId: result, area: 'Topics' }));
                });
            };
            AddNewTopicDialogController.$inject = ['$scope', 'dialog', 'apiService', 'dialogOptions'];
            return AddNewTopicDialogController;
        })();
        Topics.AddNewTopicDialogController = AddNewTopicDialogController;
    })(MirGames.Topics || (MirGames.Topics = {}));
    var Topics = MirGames.Topics;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=AddNewTopicDialogController.js.map
