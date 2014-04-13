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

                this.$scope.newItem = this.getEmptyNewItem();
            }
            ProjectWorkItemPage.prototype.getEmptyNewItem = function () {
                var _this = this;
                return {
                    attachments: [],
                    focus: false,
                    post: function () {
                        return _this.postNewItem();
                    },
                    tags: '',
                    text: '',
                    title: ''
                };
            };

            /** Loads the list of work items */
            ProjectWorkItemPage.prototype.loadWorkItems = function () {
                var _this = this;
                var query = {
                    ProjectAlias: this.pageData.projectAlias
                };

                this.apiService.getAll("GetProjectWorkItemsQuery", query, 0, 20, function (result) {
                    _this.$scope.$apply(function () {
                        _this.$scope.items = result;
                        _this.$scope.dataLoaded = true;
                    });
                });
            };

            ProjectWorkItemPage.prototype.loadWorkItem = function (internalId) {
                var _this = this;
                var query = {
                    ProjectAlias: this.pageData.projectAlias,
                    InternalId: internalId
                };

                this.$scope.dataLoaded = false;

                this.apiService.getOne("GetProjectWorkItemQuery", query, function (result) {
                    _this.$scope.$apply(function () {
                        _this.$scope.items.push(_this.convertItemToScope(result));
                        _this.$scope.dataLoaded = true;
                    });
                });
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
                    tags: this.convertTagsToScope(item.TagsList),
                    url: Router.action("Projects", "WorkItem", { projectAlias: this.pageData.projectAlias, workItemId: item.InternalId })
                };
            };

            /** Converts tag to the scope item */
            ProjectWorkItemPage.prototype.convertTagsToScope = function (item) {
                var _this = this;
                return Enumerable.from((item || '').split(',')).where(function (t) {
                    return t != null && t != '';
                }).select(function (tag) {
                    return _this.convertTagToScope(tag);
                }).toArray();
            };

            /** Converts tag to the scope item */
            ProjectWorkItemPage.prototype.convertTagToScope = function (item) {
                return {
                    text: item.trim(),
                    url: Router.action("Projects", "WorkItems", { projectAlias: this.pageData.projectAlias, tag: item.trim() })
                };
            };

            /** Posts the new item */
            ProjectWorkItemPage.prototype.postNewItem = function () {
                var _this = this;
                var command = {
                    ProjectAlias: this.pageData.projectAlias,
                    Title: this.$scope.newItem.title,
                    Tags: this.$scope.newItem.tags,
                    Type: 2 /* Task */,
                    Attachments: this.$scope.newItem.attachments,
                    Description: this.$scope.newItem.text
                };

                this.apiService.executeCommand('CreateNewProjectWorkItemCommand', command, function (internalId) {
                    _this.loadWorkItem(internalId);
                    _this.$scope.$apply(function () {
                        _this.$scope.newItem = _this.getEmptyNewItem();
                        _this.$scope.newItem.focus = true;
                    });
                });
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
            WorkItemState[WorkItemState["Removed"] = 5] = "Removed";
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
