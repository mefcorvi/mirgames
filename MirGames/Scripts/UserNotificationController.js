/// <reference path="_references.ts" />
var MirGames;
(function (MirGames) {
    var UserNotificationController = (function () {
        function UserNotificationController($scope, $element, eventBus, $timeout) {
            var _this = this;
            this.$scope = $scope;
            this.$element = $element;
            this.eventBus = eventBus;
            this.$timeout = $timeout;
            this.$scope.notifications = [];

            eventBus.addListener('user.notification', function (arg) {
                return _this.onUserNotification(arg);
            });
        }
        UserNotificationController.prototype.onUserNotification = function (notification) {
            var _this = this;
            this.$scope.notifications.push({ isHiding: false, text: notification });
            this.$scope.$apply();

            $('body').one('mousemove', null, null, function (ev) {
                _this.hideNotification(notification);
            });
        };

        UserNotificationController.prototype.hideNotification = function (notification) {
            var _this = this;
            this.$timeout(function () {
                Enumerable.from(_this.$scope.notifications).where(function (n) {
                    return n.text == notification;
                }).forEach(function (item) {
                    return item.isHiding = true;
                });

                _this.$timeout(function () {
                    _this.$scope.notifications = Enumerable.from(_this.$scope.notifications).where(function (n) {
                        return n.text != notification;
                    }).toArray();
                }, 1000);
            }, 1000);
        };
        UserNotificationController.$inject = ['$scope', '$element', 'eventBus', '$timeout'];
        return UserNotificationController;
    })();
    MirGames.UserNotificationController = UserNotificationController;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=UserNotificationController.js.map
