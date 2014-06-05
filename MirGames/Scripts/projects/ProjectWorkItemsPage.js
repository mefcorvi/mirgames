var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var MirGames;
(function (MirGames) {
    /// <reference path="../_references.ts" />
    (function (Wip) {
        var ProjectWorkItemsPage = (function (_super) {
            __extends(ProjectWorkItemsPage, _super);
            function ProjectWorkItemsPage($scope, eventBus, apiService) {
                var _this = this;
                _super.call(this, $scope, eventBus);
                this.apiService = apiService;
                this.$scope.viewMode = 0 /* List */;
                this.$scope.typeNames = ['Неизвестный', 'Ошибка', 'Задача', 'Фича'];
                this.$scope.statusNames = ['Неизестный', 'Открытая', 'Закрытая', 'Активная', 'В очереди', 'Удаленная'];

                this.$scope.filterByType = this.pageData.filterByType;
                this.$scope.filterByStatus = null;

                this.$scope.setFilterByType = function (filter) {
                    return _this.setFilterByType(filter);
                };
                this.$scope.setFilterByStatus = function (filter) {
                    return _this.setFilterByStatus(filter);
                };

                this.$scope.showBlocks = function () {
                    return _this.showBlocks();
                };
                this.$scope.showList = function () {
                    return _this.showList();
                };
            }
            /** Shows work items as a blocks */
            ProjectWorkItemsPage.prototype.showBlocks = function () {
                this.$scope.viewMode = 1 /* Blocks */;
            };

            /** Show work items as a list */
            ProjectWorkItemsPage.prototype.showList = function () {
                this.$scope.viewMode = 0 /* List */;
            };

            /** Sets filter by type  */
            ProjectWorkItemsPage.prototype.setFilterByType = function (itemType) {
                this.$scope.filterByType = itemType;
            };

            /** Sets filter by status  */
            ProjectWorkItemsPage.prototype.setFilterByStatus = function (itemStatus) {
                this.$scope.filterByStatus = itemStatus;
            };
            ProjectWorkItemsPage.$inject = ['$scope', 'eventBus', 'apiService'];
            return ProjectWorkItemsPage;
        })(MirGames.BasePage);
        Wip.ProjectWorkItemsPage = ProjectWorkItemsPage;

        (function (ViewMode) {
            ViewMode[ViewMode["List"] = 0] = "List";
            ViewMode[ViewMode["Blocks"] = 1] = "Blocks";
        })(Wip.ViewMode || (Wip.ViewMode = {}));
        var ViewMode = Wip.ViewMode;

        angular.module('ng').directive('workItemsList', function () {
            return {
                restrict: 'E',
                replace: true,
                scope: {
                    'filterByType': '=',
                    'filterByStatus': '='
                },
                controller: MirGames.Wip.ProjectWorkItemList,
                transclude: false,
                templateUrl: '/content/projects/work-items-list.html'
            };
        });

        angular.module('ng').directive('workItemBlocks', function () {
            return {
                restrict: 'E',
                replace: true,
                scope: {
                    'filterByType': '=',
                    'filterByStatus': '='
                },
                controller: MirGames.Wip.ProjectWorkItemBlocks,
                transclude: false,
                templateUrl: '/content/projects/work-item-blocks.html'
            };
        });
    })(MirGames.Wip || (MirGames.Wip = {}));
    var Wip = MirGames.Wip;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=ProjectWorkItemsPage.js.map
