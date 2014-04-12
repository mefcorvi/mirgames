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
        var ProjectWorkItemPage = (function (_super) {
            __extends(ProjectWorkItemPage, _super);
            function ProjectWorkItemPage($scope, eventBus, apiService) {
                _super.call(this, $scope, eventBus);
                this.apiService = apiService;
                this.$scope.items = this.convertItemsToScope(this.pageData.workItems);
                this.$scope.dataLoaded = true;
            }
            /** Loads the list of work items */
            ProjectWorkItemPage.prototype.loadWorkItems = function () {
                var _this = this;
                var query = {
                    ProjectAlias: this.pageData.projectAlias
                };

                this.apiService.getAll("GetProjectWorkItemsQuery", query, 0, 20, function (result) {
                    _this.$scope.$apply(function () {
                        _this.$scope.items = result;
                    });
                });
                this.$scope.dataLoaded = true;
            };

            /** Converts DTO to the scope object */
            ProjectWorkItemPage.prototype.convertItemsToScope = function (items) {
                var _this = this;
                return Enumerable.from(items).select(function (item) {
                    return _this.convertItemToScope(item);
                }).toArray();
            };

            /** Converts DTO to the scope object */
            ProjectWorkItemPage.prototype.convertItemToScope = function (item) {
                return {
                    type: WorkItemType[item.ItemType],
                    internalId: item.InternalId,
                    state: WorkItemState[item.State],
                    title: item.Title,
                    canBeEdited: item.CanBeEdited,
                    canBeDeleted: item.CanBeDeleted,
                    url: Router.action("Projects", "WorkItem", { projectAlias: this.pageData.projectAlias, workItemId: item.InternalId })
                };
            };
            ProjectWorkItemPage.$inject = ['$scope', 'eventBus', 'apiService'];
            return ProjectWorkItemPage;
        })(MirGames.BasePage);
        Wip.ProjectWorkItemPage = ProjectWorkItemPage;

        var WorkItemState;
        (function (WorkItemState) {
            WorkItemState[WorkItemState["Undefined"] = 0] = "Undefined";
            WorkItemState[WorkItemState["Open"] = 1] = "Open";
            WorkItemState[WorkItemState["Closed"] = 2] = "Closed";
            WorkItemState[WorkItemState["Active"] = 3] = "Active";
            WorkItemState[WorkItemState["Queued"] = 4] = "Queued";
        })(WorkItemState || (WorkItemState = {}));

        var WorkItemType;
        (function (WorkItemType) {
            WorkItemType[WorkItemType["Undefined"] = 0] = "Undefined";
            WorkItemType[WorkItemType["Bug"] = 1] = "Bug";
            WorkItemType[WorkItemType["Task"] = 2] = "Task";
            WorkItemType[WorkItemType["Feature"] = 3] = "Feature";
        })(WorkItemType || (WorkItemType = {}));
    })(MirGames.Wip || (MirGames.Wip = {}));
    var Wip = MirGames.Wip;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=ProjectWorkItemPage.js.map
