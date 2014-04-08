/// <reference path="_references.ts" />
var MirGames;
(function (MirGames) {
    var ActivationNotificationController = (function () {
        function ActivationNotificationController($scope, $element, apiService) {
            var _this = this;
            this.$scope = $scope;
            this.$element = $element;
            this.apiService = apiService;
            this.$scope.sendNotification = function () {
                return _this.sendNotification();
            };
            this.$scope.notificationSent = false;
        }
        ActivationNotificationController.prototype.sendNotification = function () {
            var _this = this;
            this.$scope.notificationSent = false;

            var command = {};
            this.apiService.executeCommand('ResendActivationCommand', command, function () {
                _this.$scope.$apply(function () {
                    _this.$scope.notificationSent = true;
                });
            });
        };
        ActivationNotificationController.$inject = ['$scope', '$element', 'apiService'];
        return ActivationNotificationController;
    })();
    MirGames.ActivationNotificationController = ActivationNotificationController;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=ActivationNotificationController.js.map
