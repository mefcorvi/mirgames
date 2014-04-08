/// <reference path="_references.ts" />
var MirGames;
(function (MirGames) {
    var RequestNotificationController = (function () {
        function RequestNotificationController($scope, $element, eventBus) {
            this.$scope = $scope;
            this.$element = $element;
            this.eventBus = eventBus;
            this.$scope.requestsExecutingCount = 0;

            eventBus.addListener('ajax-request.executing', this.onRequestExecuting.bind(this));
            eventBus.addListener('ajax-request.executed', this.onRequestExecuted.bind(this));
            eventBus.addListener('ajax-request.failed', this.onRequestExecuted.bind(this));
        }
        RequestNotificationController.prototype.onRequestExecuting = function () {
            this.$scope.requestsExecutingCount++;
            this.$scope.safeApply();
        };

        RequestNotificationController.prototype.onRequestExecuted = function () {
            this.$scope.requestsExecutingCount--;

            if (this.$scope.requestsExecutingCount < 0) {
                this.$scope.requestsExecutingCount = 0;
            }

            this.$scope.safeApply();
        };
        RequestNotificationController.$inject = ['$scope', '$element', 'eventBus'];
        return RequestNotificationController;
    })();
    MirGames.RequestNotificationController = RequestNotificationController;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=RequestNotificationController.js.map
