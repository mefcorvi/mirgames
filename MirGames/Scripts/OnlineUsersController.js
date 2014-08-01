/// <reference path="_references.ts" />
var MirGames;
(function (MirGames) {
    var OnlineUsersController = (function () {
        function OnlineUsersController($scope, $element, socketService) {
            var _this = this;
            this.$scope = $scope;
            this.$element = $element;
            this.socketService = socketService;
            var eventsHub = $.connection.eventsHub;
            var pageData = window['pageData'];
            this.$scope.users = [];

            for (var i = 0; i < pageData.onlineUsers.length; i++) {
                var onlineUser = pageData.onlineUsers[i];
                var tags = pageData.onlineUserTags[onlineUser.Id];

                this.addUser(onlineUser, tags);
            }

            socketService.addHandler('eventsHub', 'newNotification', function (data) {
                console.log('Notification', data);
            });

            socketService.addHandler('eventsHub', 'userOnlineTagAdded', function (userId, tag) {
                _this.$scope.$apply(function () {
                    var user = _this.getUser(userId);

                    if (user != null) {
                        user.tags.push(tag);
                    }
                });
            });

            socketService.addHandler('eventsHub', 'userOnlineTagRemoved', function (userId, tag) {
                _this.$scope.$apply(function () {
                    var user = _this.getUser(userId);

                    if (user == null) {
                        return;
                    }

                    user.tags = Enumerable.from(user.tags).where(function (item) {
                        return item != tag;
                    }).toArray();
                });
            });

            socketService.addHandler('eventsHub', 'userOnline', function (userJson) {
                var user = JSON.parse(userJson);
                _this.$scope.$apply(function () {
                    _this.addUser(user);
                });
            });

            socketService.addHandler('eventsHub', 'userOffline', function (userJson) {
                var user = JSON.parse(userJson);
                _this.$scope.$apply(function () {
                    _this.removeUser(user.Id);
                });
            });
        }
        OnlineUsersController.prototype.getUser = function (userId) {
            return Enumerable.from(this.$scope.users).firstOrDefault(function (x) {
                return x.id == userId;
            });
        };

        OnlineUsersController.prototype.convertUser = function (user) {
            return {
                avatarUrl: user.AvatarUrl,
                id: user.Id,
                login: user.Login,
                userUrl: Router.action('Users', 'Profile', { userId: user.Id }),
                tags: []
            };
        };

        OnlineUsersController.prototype.addUser = function (user, tags) {
            if (typeof tags === "undefined") { tags = []; }
            this.removeUser(user.Id);
            var scopeUser = this.convertUser(user);
            scopeUser.tags = tags;
            this.$scope.users.splice(0, 0, scopeUser);
        };

        OnlineUsersController.prototype.removeUser = function (userId) {
            for (var i = this.$scope.users.length - 1; i >= 0; i--) {
                var user = this.$scope.users[i];

                if (user.id == userId) {
                    this.$scope.users.splice(i, 1);
                }
            }
        };
        OnlineUsersController.$inject = ['$scope', '$element', 'socketService'];
        return OnlineUsersController;
    })();
    MirGames.OnlineUsersController = OnlineUsersController;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=OnlineUsersController.js.map
