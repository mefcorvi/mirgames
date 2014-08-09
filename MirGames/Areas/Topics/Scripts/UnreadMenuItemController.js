var MirGames;
(function (MirGames) {
    /// <reference path="_references.ts" />
    (function (Topics) {
        var UnreadMenuItemController = (function () {
            function UnreadMenuItemController($scope, $element, pageData, socketService) {
                var _this = this;
                this.$scope = $scope;
                this.pageData = pageData;
                this.$scope.goToUnread = function (url, newWindow) {
                    return _this.goToUnread(url, newWindow);
                };
                this.$scope.unreadCount = pageData.blogTopicsUnreadCount;

                socketService.addHandler('eventsHub', 'NewNotification', function (data) {
                    if (data.NotificationType == 'Topics.NewBlogTopic' || data.NotificationType == 'Topics.NewTopicComment') {
                        $scope.$apply(function () {
                            $scope.unreadCount++;
                        });
                    }
                });

                socketService.addHandler('eventsHub', 'RemoveNotification', function (data) {
                    if (data.NotificationType == 'Topics.NewBlogTopic' || data.NotificationType == 'Topics.NewTopicComment') {
                        $scope.$apply(function () {
                            $scope.unreadCount--;
                        });
                    }
                });
            }
            UnreadMenuItemController.prototype.goToUnread = function (url, newWindow) {
                Core.Application.getInstance().navigateToUrl(url, newWindow);
            };
            UnreadMenuItemController.$inject = ['$scope', '$element', 'pageData', 'socketService'];
            return UnreadMenuItemController;
        })();
        Topics.UnreadMenuItemController = UnreadMenuItemController;
    })(MirGames.Topics || (MirGames.Topics = {}));
    var Topics = MirGames.Topics;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=UnreadMenuItemController.js.map
