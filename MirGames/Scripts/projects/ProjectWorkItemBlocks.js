/// <reference path="../_references.ts" />
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
        var ProjectWorkItemBlocks = (function (_super) {
            __extends(ProjectWorkItemBlocks, _super);
            function ProjectWorkItemBlocks($scope, eventBus, apiService) {
                var _this = this;
                _super.call(this, $scope, eventBus);
                this.apiService = apiService;
                this.$scope.dataLoaded = false;
                this.$scope.onDrop = function ($event, $data, array) {
                    return _this.onDrop($event, $data, array);
                };
                this.$scope.dropSuccessHandler = function ($event, $index, array) {
                    return _this.dropSuccessHandler($event, $index, array);
                };

                $scope.$watch('filterByType', function () {
                    return _this.loadWorkItems();
                });
                $scope.$watch('filterByStatus', function () {
                    return _this.loadWorkItems();
                });
                this.loadWorkItems();

                this.eventBus.on(this.pageData.projectAlias + '.workitems.new', function (internalId) {
                    _this.loadWorkItem(internalId);
                });
            }
            /** Loads the list of work items */
            ProjectWorkItemBlocks.prototype.loadWorkItems = function () {
                var _this = this;
                this.loadWorkItemsByStatus(1 /* Open */, function (items) {
                    return _this.$scope.openedItems = items;
                });
                this.loadWorkItemsByStatus(3 /* Active */, function (items) {
                    return _this.$scope.activeItems = items;
                });
                this.loadWorkItemsByStatus(2 /* Closed */, function (items) {
                    return _this.$scope.closedItems = items;
                });
            };

            ProjectWorkItemBlocks.prototype.loadWorkItemsByStatus = function (status, callback) {
                var _this = this;
                var query = {
                    ProjectAlias: this.pageData.projectAlias,
                    Tag: null,
                    WorkItemType: this.$scope.filterByType,
                    WorkItemState: status
                };

                this.apiService.getAll("GetProjectWorkItemsQuery", query, 0, 20, function (result) {
                    _this.$scope.$apply(function () {
                        callback(_this.convertItemsToScope(result));
                        _this.$scope.dataLoaded = true;
                    });
                }, false);
            };

            ProjectWorkItemBlocks.prototype.loadWorkItem = function (internalId) {
                var _this = this;
                var query = {
                    ProjectAlias: this.pageData.projectAlias,
                    InternalId: internalId
                };

                this.$scope.dataLoaded = false;

                this.apiService.getOne("GetProjectWorkItemQuery", query, function (result) {
                    _this.$scope.$apply(function () {
                        var item = _this.convertItemToScope(result);
                        _this.$scope.dataLoaded = true;
                    });
                }, false);
            };

            /** Converts DTO to the scope object */
            ProjectWorkItemBlocks.prototype.convertItemsToScope = function (items) {
                var _this = this;
                return Enumerable.from(items).select(function (item) {
                    return _this.convertItemToScope(item);
                }).toArray();
            };

            /** Converts DTO to the scope object */
            ProjectWorkItemBlocks.prototype.convertItemToScope = function (item) {
                var _this = this;
                var workItem = {
                    type: MirGames.Wip.WorkItemType[item.ItemType],
                    internalId: item.InternalId,
                    state: MirGames.Wip.WorkItemState[item.State],
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
                    changeState: function () {
                        return _this.changeWorkItemState(workItem, item);
                    }
                };

                return workItem;
            };

            /** Converts tag to the scope item */
            ProjectWorkItemBlocks.prototype.convertTagsToScope = function (item) {
                var _this = this;
                return Enumerable.from((item || '').split(',')).where(function (t) {
                    return t != null && t != '';
                }).select(function (tag) {
                    return _this.convertTagToScope(tag);
                }).toArray();
            };

            /** Converts tag to the scope item */
            ProjectWorkItemBlocks.prototype.convertTagToScope = function (item) {
                return {
                    text: item.trim(),
                    url: Router.action("Projects", "WorkItems", { projectAlias: this.pageData.projectAlias, tag: item.trim() })
                };
            };

            /** Changes state of the work item */
            ProjectWorkItemBlocks.prototype.changeWorkItemState = function (workItem, viewModel) {
                var _this = this;
                if (!workItem.canBeEdited) {
                    return;
                }

                var command = {
                    WorkItemId: viewModel.WorkItemId
                };

                this.apiService.executeCommand('ChangeWorkItemStateCommand', command, function (newState) {
                    viewModel.State = newState;

                    _this.$scope.$apply(function () {
                        workItem.state = MirGames.Wip.WorkItemState[viewModel.State];
                    });
                });
            };

            /** Sets filter by type  */
            ProjectWorkItemBlocks.prototype.setFilterByType = function (itemType) {
                this.$scope.filterByType = itemType;
                this.loadWorkItems();
            };

            /** Handles drop */
            ProjectWorkItemBlocks.prototype.onDrop = function ($event, $data, array) {
                array.push($data);
            };

            /** Handles successfull drops */
            ProjectWorkItemBlocks.prototype.dropSuccessHandler = function ($event, $index, array) {
                array.splice($index, 1);
            };
            ProjectWorkItemBlocks.$inject = ['$scope', 'eventBus', 'apiService'];
            return ProjectWorkItemBlocks;
        })(MirGames.BasePage);
        Wip.ProjectWorkItemBlocks = ProjectWorkItemBlocks;
    })(MirGames.Wip || (MirGames.Wip = {}));
    var Wip = MirGames.Wip;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=ProjectWorkItemBlocks.js.map
