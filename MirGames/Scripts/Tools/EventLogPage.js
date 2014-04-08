var MirGames;
(function (MirGames) {
    /// <reference path="../_references.ts" />
    (function (Tools) {
        var EventLogPage = (function () {
            function EventLogPage($scope, $element, apiService, eventBus) {
                this.$scope = $scope;
                this.$element = $element;
                this.apiService = apiService;
                this.eventBus = eventBus;
                this.$scope.username = '';
                this.$scope.filter = this.filter.bind(this);
                this.$scope.logType = 0;
                this.$scope.from = moment().add('d', -7).toDate();
                this.$scope.to = null;
                this.$scope.source = '';
                this.$scope.message = '';

                this.$scope.logTypes = [
                    { key: 'Error', value: 0 },
                    { key: 'Warning', value: 1 },
                    { key: 'Information', value: 2 },
                    { key: 'Verbose', value: 3 }
                ];

                this.filter();
            }
            EventLogPage.prototype.filter = function () {
                var _this = this;
                var query = {
                    LogType: this.$scope.logType,
                    UserName: this.$scope.username,
                    From: this.$scope.from,
                    To: this.$scope.to,
                    Message: this.$scope.message,
                    Source: this.$scope.source
                };

                this.apiService.getAll('GetEventLogQuery', query, 0, 50, function (result) {
                    _this.$scope.$apply(function () {
                        _this.$scope.items = result;
                    });
                });
            };
            EventLogPage.$inject = ['$scope', '$element', 'apiService', 'eventBus'];
            return EventLogPage;
        })();
        Tools.EventLogPage = EventLogPage;
    })(MirGames.Tools || (MirGames.Tools = {}));
    var Tools = MirGames.Tools;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=EventLogPage.js.map
