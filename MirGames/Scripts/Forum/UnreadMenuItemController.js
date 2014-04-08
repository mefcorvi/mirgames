var MirGames;
(function (MirGames) {
    /// <reference path="../_references.ts" />
    (function (Forum) {
        var UnreadMenuItemController = (function () {
            function UnreadMenuItemController($scope, $element, pageData, socketService) {
                var _this = this;
                this.$scope = $scope;
                this.pageData = pageData;
                this.$scope.goToUnread = this.goToUnread.bind(this);
                this.$scope.unreadCount = pageData.forumTopicsUnreadCount;

                socketService.addHandler('eventsHub', 'NewTopicUnread', function () {
                    $scope.$apply(function () {
                        $scope.unreadCount++;
                    });
                });

                socketService.addHandler('eventsHub', 'NewTopic', function (ev) {
                    if (_this.pageData.currentUser && _this.pageData.currentUser.Id != ev.AuthorId) {
                        $scope.$apply(function () {
                            $scope.unreadCount++;
                        });
                    }
                });

                socketService.addHandler('eventsHub', 'ForumTopicRead', function () {
                    $scope.$apply(function () {
                        $scope.unreadCount--;
                    });
                });
            }
            UnreadMenuItemController.prototype.goToUnread = function (newWindow) {
                Core.Application.getInstance().navigateToUrl(Router.action("Forum", "Unread"), newWindow);
            };
            UnreadMenuItemController.$inject = ['$scope', '$element', 'pageData', 'socketService'];
            return UnreadMenuItemController;
        })();
        Forum.UnreadMenuItemController = UnreadMenuItemController;
    })(MirGames.Forum || (MirGames.Forum = {}));
    var Forum = MirGames.Forum;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=UnreadMenuItemController.js.map
