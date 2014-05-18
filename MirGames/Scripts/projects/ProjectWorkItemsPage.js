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
                this.$scope.view = MirGames.Wip.ProjectWorkItemList;
                this.$scope.viewMode = 0 /* List */;
                this.$scope.typeNames = ['Неизвестный', 'Ошибка', 'Задача', 'Фича'];
                this.$scope.statusNames = ['Неизестный', 'Открытая', 'Закрытая', 'Активная', 'В очереди', 'Удаленная'];
                this.$scope.newItem = this.getEmptyNewItem();

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
            ProjectWorkItemsPage.prototype.getEmptyNewItem = function () {
                var _this = this;
                return {
                    attachments: [],
                    focus: false,
                    post: function () {
                        return _this.postNewItem();
                    },
                    tags: '',
                    text: '',
                    title: '',
                    type: 1 /* Bug */,
                    availableItemTypes: this.pageData.availableItemTypes
                };
            };

            /** Shows work items as a blocks */
            ProjectWorkItemsPage.prototype.showBlocks = function () {
                var _this = this;
                this.$scope.view = MirGames.Wip.ProjectWorkItemBlocks;
                this.$scope.viewMode = 1 /* Blocks */;
                setTimeout(function () {
                    _this.$scope.$parent.$digest();
                }, 0);
            };

            /** Show work items as a list */
            ProjectWorkItemsPage.prototype.showList = function () {
                this.$scope.view = MirGames.Wip.ProjectWorkItemList;
                this.$scope.viewMode = 0 /* List */;
                this.$scope.$digest();
            };

            /** Posts the new item */
            ProjectWorkItemsPage.prototype.postNewItem = function () {
                var _this = this;
                var command = {
                    ProjectAlias: this.pageData.projectAlias,
                    Title: this.$scope.newItem.title,
                    Tags: this.$scope.newItem.tags,
                    Type: this.$scope.newItem.type,
                    Attachments: this.$scope.newItem.attachments,
                    Description: this.$scope.newItem.text
                };

                this.apiService.executeCommand('CreateNewProjectWorkItemCommand', command, function (internalId) {
                    _this.eventBus.emit(_this.pageData.projectAlias + '.workitems.new', internalId);
                    _this.$scope.$apply(function () {
                        _this.$scope.newItem = _this.getEmptyNewItem();
                        _this.$scope.newItem.focus = true;
                    });
                });
            };

            /** Sets filter by type  */
            ProjectWorkItemsPage.prototype.setFilterByType = function (itemType) {
                this.$scope.filterByType = itemType;
                this.loadWorkItems();
            };

            /** Sets filter by status  */
            ProjectWorkItemsPage.prototype.setFilterByStatus = function (itemStatus) {
                this.$scope.filterByStatus = itemStatus;
                this.loadWorkItems();
            };

            /** Loads work items */
            ProjectWorkItemsPage.prototype.loadWorkItems = function () {
            };
            ProjectWorkItemsPage.$inject = ['$scope', 'eventBus', 'apiService'];
            return ProjectWorkItemsPage;
        })(MirGames.BasePage);
        Wip.ProjectWorkItemsPage = ProjectWorkItemsPage;

        (function (WorkItemState) {
            WorkItemState[WorkItemState["Undefined"] = 0] = "Undefined";
            WorkItemState[WorkItemState["Open"] = 1] = "Open";
            WorkItemState[WorkItemState["Closed"] = 2] = "Closed";
            WorkItemState[WorkItemState["Active"] = 3] = "Active";
            WorkItemState[WorkItemState["Queued"] = 4] = "Queued";
            WorkItemState[WorkItemState["Removed"] = 5] = "Removed";
        })(Wip.WorkItemState || (Wip.WorkItemState = {}));
        var WorkItemState = Wip.WorkItemState;

        (function (WorkItemType) {
            WorkItemType[WorkItemType["Undefined"] = 0] = "Undefined";
            WorkItemType[WorkItemType["Bug"] = 1] = "Bug";
            WorkItemType[WorkItemType["Task"] = 2] = "Task";
            WorkItemType[WorkItemType["Feature"] = 3] = "Feature";
        })(Wip.WorkItemType || (Wip.WorkItemType = {}));
        var WorkItemType = Wip.WorkItemType;

        (function (ViewMode) {
            ViewMode[ViewMode["List"] = 0] = "List";
            ViewMode[ViewMode["Blocks"] = 1] = "Blocks";
        })(Wip.ViewMode || (Wip.ViewMode = {}));
        var ViewMode = Wip.ViewMode;
    })(MirGames.Wip || (MirGames.Wip = {}));
    var Wip = MirGames.Wip;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=ProjectWorkItemsPage.js.map
