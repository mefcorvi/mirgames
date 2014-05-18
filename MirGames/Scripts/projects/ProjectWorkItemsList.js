﻿/// <reference path="../_references.ts" />
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
        var ProjectWorkItemsList = (function (_super) {
            __extends(ProjectWorkItemsList, _super);
            function ProjectWorkItemsList($scope, eventBus, apiService) {
                _super.call(this, $scope, eventBus);
                this.apiService = apiService;
                this.$scope.items = this.convertItemsToScope(this.pageData.workItems);
                this.$scope.dataLoaded = true;

                this.$scope.typeNames = ['Неизвестный', 'Ошибка', 'Задача', 'Фича'];
                this.$scope.statusNames = ['Неизестный', 'Открытая', 'Закрытая', 'Активная', 'В очереди', 'Удаленная'];
                this.$scope.filterByType = this.pageData.filterByType;
                this.$scope.filterByStatus = null;

                this.eventBus.on('workitems.filterChanged', function (args) {
                    console.log(args);
                });
            }
            /** Loads the list of work items */
            ProjectWorkItemsList.prototype.loadWorkItems = function () {
                var _this = this;
                var query = {
                    ProjectAlias: this.pageData.projectAlias,
                    Tag: null,
                    WorkItemType: this.$scope.filterByType,
                    WorkItemState: this.$scope.filterByStatus
                };

                this.apiService.getAll("GetProjectWorkItemsQuery", query, 0, 20, function (result) {
                    _this.$scope.$apply(function () {
                        _this.$scope.items = _this.convertItemsToScope(result);
                        _this.$scope.dataLoaded = true;
                    });
                });
            };

            ProjectWorkItemsList.prototype.loadWorkItem = function (internalId) {
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
            ProjectWorkItemsList.prototype.convertItemsToScope = function (items) {
                var _this = this;
                return Enumerable.from(items).select(function (item) {
                    return _this.convertItemToScope(item);
                }).toArray();
            };

            /** Converts DTO to the scope object */
            ProjectWorkItemsList.prototype.convertItemToScope = function (item) {
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
            ProjectWorkItemsList.prototype.convertTagsToScope = function (item) {
                var _this = this;
                return Enumerable.from((item || '').split(',')).where(function (t) {
                    return t != null && t != '';
                }).select(function (tag) {
                    return _this.convertTagToScope(tag);
                }).toArray();
            };

            /** Converts tag to the scope item */
            ProjectWorkItemsList.prototype.convertTagToScope = function (item) {
                return {
                    text: item.trim(),
                    url: Router.action("Projects", "WorkItems", { projectAlias: this.pageData.projectAlias, tag: item.trim() })
                };
            };

            /** Changes state of the work item */
            ProjectWorkItemsList.prototype.changeWorkItemState = function (workItem, viewModel) {
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
            ProjectWorkItemsList.prototype.setFilterByType = function (itemType) {
                this.$scope.filterByType = itemType;
                this.loadWorkItems();
            };

            /** Sets filter by status  */
            ProjectWorkItemsList.prototype.setFilterByStatus = function (itemStatus) {
                this.$scope.filterByStatus = itemStatus;
                this.loadWorkItems();
            };
            ProjectWorkItemsList.$inject = ['$scope', 'eventBus', 'apiService'];
            return ProjectWorkItemsList;
        })(MirGames.BasePage);
        Wip.ProjectWorkItemsList = ProjectWorkItemsList;
    })(MirGames.Wip || (MirGames.Wip = {}));
    var Wip = MirGames.Wip;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=ProjectWorkItemsList.js.map
