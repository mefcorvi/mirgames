/// <reference path="_references.ts" />
var MirGames;
(function (MirGames) {
    var SocketNotificationController = (function () {
        function SocketNotificationController($scope, $element, eventBus) {
            var _this = this;
            this.$scope = $scope;
            this.$element = $element;
            this.eventBus = eventBus;
            eventBus.addListener('socket.connecting', function () {
                return _this.setState('connecting');
            });
            eventBus.addListener('socket.reconnecting', function () {
                return _this.setState('reconnecting');
            });
            eventBus.addListener('socket.connected', function () {
                return _this.setState('connected');
            });
            eventBus.addListener('socket.disconnected', function () {
                return _this.setState('disconnected');
            });
            eventBus.addListener('socket.connection-slow', function () {
                return _this.setState('connection-slow');
            });
        }
        SocketNotificationController.prototype.setState = function (state) {
            this.$scope.state = state;
            this.$scope.safeApply();
        };
        SocketNotificationController.$inject = ['$scope', '$element', 'eventBus'];
        return SocketNotificationController;
    })();
    MirGames.SocketNotificationController = SocketNotificationController;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=SocketNotificationController.js.map
