var MirGames;
(function (MirGames) {
    /// <reference path="_references.ts" />
    (function (Forum) {
        var UnreadMenuItemController = (function () {
            function UnreadMenuItemController($scope, $element, pageData, socketService) {
                var _this = this;
                this.$scope = $scope;
                this.pageData = pageData;
                this.$scope.goToUnread = function (url, newWindow) {
                    return _this.goToUnread(url, newWindow);
                };
                this.$scope.unreadCount = pageData.forumTopicsUnreadCount;

                socketService.addHandler('eventsHub', 'NewNotification', function (data) {
                    console.log(data);
                    if (data.NotificationType == 'Forum.NewAnswer' || data.NotificationType == 'Forum.NewTopic') {
                        $scope.$apply(function () {
                            $scope.unreadCount++;
                        });
                    }
                });

                socketService.addHandler('eventsHub', 'RemoveNotification', function (data) {
                    console.log(data);
                    if (data.NotificationType == 'Forum.NewAnswer' || data.NotificationType == 'Forum.NewTopic') {
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
        Forum.UnreadMenuItemController = UnreadMenuItemController;
    })(MirGames.Forum || (MirGames.Forum = {}));
    var Forum = MirGames.Forum;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=UnreadMenuItemController.js.map
