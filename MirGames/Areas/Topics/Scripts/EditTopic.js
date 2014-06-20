var MirGames;
(function (MirGames) {
    /// <reference path="_references.ts" />
    (function (Topics) {
        var EditTopicPage = (function () {
            function EditTopicPage($scope, $element, commandBus, eventBus, pageData) {
                var _this = this;
                this.$scope = $scope;
                this.$element = $element;
                this.commandBus = commandBus;
                this.eventBus = eventBus;
                this.pageData = pageData;
                this.$scope.topicId = pageData.topicId;
                this.$scope.title = pageData.title;
                this.$scope.text = pageData.text;
                this.$scope.tags = pageData.tags;
                this.$scope.save = this.submit.bind(this);
                this.$scope.attachments = [];

                this.$scope.switchPreviewMode = function () {
                    _this.$scope.showPreview = !_this.$scope.showPreview;
                };
            }
            EditTopicPage.prototype.submit = function () {
                var _this = this;
                var command = this.commandBus.createCommandFromScope(MirGames.Domain.SaveTopicCommand, this.$scope);

                this.commandBus.executeCommand(Router.action("Topics", "SaveTopic"), command, function (result) {
                    Core.Application.getInstance().navigateToUrl(Router.action("Topics", "Topic", { topicId: _this.$scope.topicId }));
                });
            };
            EditTopicPage.$inject = ['$scope', '$element', 'commandBus', 'eventBus', 'pageData'];
            return EditTopicPage;
        })();
        Topics.EditTopicPage = EditTopicPage;
    })(MirGames.Topics || (MirGames.Topics = {}));
    var Topics = MirGames.Topics;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=EditTopic.js.map
