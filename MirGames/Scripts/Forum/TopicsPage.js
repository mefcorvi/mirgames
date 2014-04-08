var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var MirGames;
(function (MirGames) {
    /// <reference path="../_references.ts" />
    (function (Forum) {
        var TopicsPage = (function (_super) {
            __extends(TopicsPage, _super);
            function TopicsPage($scope, commandBus, eventBus) {
                _super.call(this, $scope, eventBus);
                this.commandBus = commandBus;
                this.$scope.markAllAsRead = this.markAllAsRead.bind(this);
            }
            TopicsPage.prototype.markAllAsRead = function () {
                var command = this.commandBus.createCommandFromScope(MirGames.Domain.MarkAllTopicsAsReadCommand, this.$scope);
                this.commandBus.executeCommand(Router.action("Forum", "MarkAllTopicsAsRead"), command, function (response) {
                    if (response.result) {
                        window.location.reload();
                    }
                });
            };
            TopicsPage.$inject = ['$scope', 'commandBus', 'eventBus'];
            return TopicsPage;
        })(MirGames.BasePage);
        Forum.TopicsPage = TopicsPage;
    })(MirGames.Forum || (MirGames.Forum = {}));
    var Forum = MirGames.Forum;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=TopicsPage.js.map
