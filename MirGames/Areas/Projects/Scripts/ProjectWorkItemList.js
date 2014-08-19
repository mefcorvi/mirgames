/// <reference path="_references.ts" />
/// <reference path="ProjectWorkItemsPage.ts" />
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var MirGames;
(function (MirGames) {
    (function (Wip) {
        var ProjectWorkItemList = (function (_super) {
            __extends(ProjectWorkItemList, _super);
            function ProjectWorkItemList($scope, eventBus, apiService) {
                var _this = this;
                _super.call(this, $scope, eventBus);
                this.apiService = apiService;
                this.$scope.items = this.convertItemsToScope(this.pageData.workItems);
                this.$scope.dataLoaded = true;

                $scope.$watch('filterByType', function () {
                    return _this.loadWorkItems();
                });
                $scope.$watch('filterByStatus', function () {
                    return _this.loadWorkItems();
                });

                this.eventBus.on(this.pageData.projectAlias + '.workitems.new', function (internalId) {
                    _this.loadWorkItem(internalId);
                });
            }
            /** Loads the list of work items */
            ProjectWorkItemList.prototype.loadWorkItems = function () {
                var _this = this;
                this.$scope.dataLoaded = false;
                this.$scope.items = [];

                var query = {
                    ProjectAlias: this.pageData.projectAlias,
                    Tag: null,
                    WorkItemType: this.$scope.filterByType,
                    WorkItemState: this.$scope.filterByStatus,
                    OrderBy: 1 /* Priority */
                };

                this.apiService.getAll("GetProjectWorkItemsQuery", query, 0, 20, function (result) {
                    _this.$scope.$apply(function () {
                        _this.$scope.items = _this.convertItemsToScope(result);
                        _this.$scope.dataLoaded = true;
                    });
                }, false);
            };

            ProjectWorkItemList.prototype.loadWorkItem = function (internalId) {
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
            ProjectWorkItemList.prototype.convertItemsToScope = function (items) {
                var _this = this;
                return Enumerable.from(items).select(function (item) {
                    return _this.convertItemToScope(item);
                }).toArray();
            };

            /** Converts DTO to the scope object */
            ProjectWorkItemList.prototype.convertItemToScope = function (item) {
                var _this = this;
                var workItem = {
                    workItemId: item.WorkItemId,
                    type: MirGames.Domain.Wip.ViewModels.WorkItemType[item.ItemType],
                    internalId: item.InternalId,
                    state: MirGames.Domain.Wip.ViewModels.WorkItemState[item.State],
                    title: item.Title,
                    canBeEdited: item.CanBeEdited,
                    canBeDeleted: item.CanBeDeleted,
                    tags: this.convertTagsToScope(item.TagsList),
                    description: item.ShortDescription,
                    url: Router.action("Projects", "WorkItem", { projectAlias: this.pageData.projectAlias, workItemId: item.InternalId }),
                    priority: Math.round(Math.max(0, item.Priority) / 25),
                    author: {
                        avatar: item.Author.AvatarUrl,
                        id: item.Author.Id,
                        login: item.Author.Login
                    },
                    assignedTo: {
                        avatar: item.AssignedTo.AvatarUrl,
                        id: item.AssignedTo.Id,
                        login: item.AssignedTo.Login
                    },
                    changeState: function () {
                        return _this.changeWorkItemState(workItem, item);
                    }
                };

                return workItem;
            };

            /** Converts tag to the scope item */
            ProjectWorkItemList.prototype.convertTagsToScope = function (item) {
                var _this = this;
                return Enumerable.from((item || '').split(',')).where(function (t) {
                    return t != null && t != '';
                }).select(function (tag) {
                    return _this.convertTagToScope(tag);
                }).toArray();
            };

            /** Converts tag to the scope item */
            ProjectWorkItemList.prototype.convertTagToScope = function (item) {
                return {
                    text: item.trim(),
                    url: Router.action("Projects", "WorkItems", { projectAlias: this.pageData.projectAlias, tag: item.trim() })
                };
            };

            /** Changes state of the work item */
            ProjectWorkItemList.prototype.changeWorkItemState = function (workItem, viewModel) {
                var _this = this;
                if (!workItem.canBeEdited) {
                    return;
                }

                var command = {
                    WorkItemId: viewModel.WorkItemId,
                    State: null
                };

                this.apiService.executeCommand('ChangeWorkItemStateCommand', command, function (newState) {
                    viewModel.State = newState;

                    _this.$scope.$apply(function () {
                        workItem.state = MirGames.Domain.Wip.ViewModels.WorkItemState[viewModel.State];
                    });
                });
            };
            ProjectWorkItemList.$inject = ['$scope', 'eventBus', 'apiService'];
            return ProjectWorkItemList;
        })(MirGames.BasePage);
        Wip.ProjectWorkItemList = ProjectWorkItemList;
    })(MirGames.Wip || (MirGames.Wip = {}));
    var Wip = MirGames.Wip;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=ProjectWorkItemList.js.map
