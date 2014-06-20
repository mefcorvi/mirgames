var MirGames;
(function (MirGames) {
    /// <reference path="_references.ts" />
    (function (Topics) {
        var NewTopicPage = (function () {
            function NewTopicPage($scope, $element, commandBus, eventBus) {
                var _this = this;
                this.$scope = $scope;
                this.$element = $element;
                this.commandBus = commandBus;
                this.eventBus = eventBus;
                this.$scope.save = this.submit.bind(this);
                this.$scope.attachments = [];

                this.$scope.switchPreviewMode = function () {
                    _this.$scope.showPreview = !_this.$scope.showPreview;
                };
                this.$scope.isTitleFocused = true;
            }
            NewTopicPage.prototype.submit = function () {
                var command = this.commandBus.createCommandFromScope(MirGames.Domain.AddNewTopicCommand, this.$scope);

                this.commandBus.executeCommand(Router.action("Topics", "AddTopic"), command, function (result) {
                    Core.Application.getInstance().navigateToUrl(Router.action("Topics", "Topic", { topicId: result.topicId }));
                });
            };
            NewTopicPage.$inject = ['$scope', '$element', 'commandBus', 'eventBus'];
            return NewTopicPage;
        })();
        Topics.NewTopicPage = NewTopicPage;
    })(MirGames.Topics || (MirGames.Topics = {}));
    var Topics = MirGames.Topics;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=NewTopic.js.map
