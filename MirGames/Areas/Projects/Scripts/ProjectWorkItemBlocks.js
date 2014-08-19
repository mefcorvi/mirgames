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
        var ProjectWorkItemBlocks = (function (_super) {
            __extends(ProjectWorkItemBlocks, _super);
            function ProjectWorkItemBlocks($scope, eventBus, apiService) {
                var _this = this;
                _super.call(this, $scope, eventBus);
                this.apiService = apiService;
                this.$scope.dataLoaded = false;
                this.$scope.onDrop = function ($event, $data, state, target) {
                    return _this.onDrop($event, $data, state, target);
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
                    WorkItemState: status,
                    OrderBy: 1 /* Priority */
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

                        if (item.state == 'Open') {
                            _this.$scope.openedItems.push(item);
                        }

                        if (item.state == 'Active') {
                            _this.$scope.activeItems.push(item);
                        }

                        if (item.state == 'Closed') {
                            _this.$scope.closedItems.push(item);
                        }

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
                        return _this.changeWorkItemState(workItem);
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
            ProjectWorkItemBlocks.prototype.changeWorkItemState = function (workItem, newState) {
                var _this = this;
                if (!workItem.canBeEdited) {
                    return;
                }

                var command = {
                    WorkItemId: workItem.workItemId,
                    State: newState == null ? null : MirGames.Domain.Wip.ViewModels.WorkItemState[newState]
                };

                this.apiService.executeCommand('ChangeWorkItemStateCommand', command, function (newState) {
                    _this.$scope.$apply(function () {
                        workItem.state = MirGames.Domain.Wip.ViewModels.WorkItemState[newState];
                    });
                });
            };

            /** Sets filter by type  */
            ProjectWorkItemBlocks.prototype.setFilterByType = function (itemType) {
                this.$scope.filterByType = itemType;
                this.loadWorkItems();
            };

            /** Handles drop */
            ProjectWorkItemBlocks.prototype.onDrop = function ($event, $data, state, target) {
                var array = this.$scope.openedItems;

                if (state == 'Active') {
                    array = this.$scope.activeItems;
                }

                if (state == 'Closed') {
                    array = this.$scope.closedItems;
                }

                for (var i = 0; i < array.length; i++) {
                    if (array[i] == target) {
                        this.changeWorkItemState($data, target.state);

                        array.splice(i + 1, 0, $data);
                        return;
                    }
                }

                this.changeWorkItemState($data, state);
                array.splice(0, 0, $data);
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
