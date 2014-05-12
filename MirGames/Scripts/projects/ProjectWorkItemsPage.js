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
                this.$scope.items = this.convertItemsToScope(this.pageData.workItems);
                this.$scope.dataLoaded = true;

                this.$scope.viewMode = 0 /* List */;
                this.$scope.typeNames = ['Неизвестный', 'Ошибка', 'Задача', 'Фича'];
                this.$scope.statusNames = ['Неизестный', 'Открытая', 'Закрытая', 'Активная', 'В очереди', 'Удаленная'];
                this.$scope.newItem = this.getEmptyNewItem();
                this.$scope.filterByType = this.pageData.filterByType;
                this.$scope.filterByStatus = null;
                this.$scope.setFilterByType = function (itemType) {
                    return _this.setFilterByType(itemType);
                };
                this.$scope.setFilterByStatus = function (itemStatus) {
                    return _this.setFilterByStatus(itemStatus);
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

            /** Loads the list of work items */
            ProjectWorkItemsPage.prototype.loadWorkItems = function () {
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

            ProjectWorkItemsPage.prototype.loadWorkItem = function (internalId) {
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

            /** Shows work items as a blocks */
            ProjectWorkItemsPage.prototype.showBlocks = function () {
                this.$scope.viewMode = 1 /* Blocks */;
            };

            /** Show work items as a list */
            ProjectWorkItemsPage.prototype.showList = function () {
                this.$scope.viewMode = 0 /* List */;
            };

            /** Converts DTO to the scope object */
            ProjectWorkItemsPage.prototype.convertItemsToScope = function (items) {
                var _this = this;
                return Enumerable.from(items).select(function (item) {
                    return _this.convertItemToScope(item);
                }).toArray();
            };

            /** Converts DTO to the scope object */
            ProjectWorkItemsPage.prototype.convertItemToScope = function (item) {
                var _this = this;
                var workItem = {
                    type: WorkItemType[item.ItemType],
                    internalId: item.InternalId,
                    state: WorkItemState[item.State],
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
            ProjectWorkItemsPage.prototype.convertTagsToScope = function (item) {
                var _this = this;
                return Enumerable.from((item || '').split(',')).where(function (t) {
                    return t != null && t != '';
                }).select(function (tag) {
                    return _this.convertTagToScope(tag);
                }).toArray();
            };

            /** Converts tag to the scope item */
            ProjectWorkItemsPage.prototype.convertTagToScope = function (item) {
                return {
                    text: item.trim(),
                    url: Router.action("Projects", "WorkItems", { projectAlias: this.pageData.projectAlias, tag: item.trim() })
                };
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
                    _this.loadWorkItem(internalId);
                    _this.$scope.$apply(function () {
                        _this.$scope.newItem = _this.getEmptyNewItem();
                        _this.$scope.newItem.focus = true;
                    });
                });
            };

            /** Changes state of the work item */
            ProjectWorkItemsPage.prototype.changeWorkItemState = function (workItem, viewModel) {
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
                        workItem.state = WorkItemState[viewModel.State];
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
            ProjectWorkItemsPage.$inject = ['$scope', 'eventBus', 'apiService'];
            return ProjectWorkItemsPage;
        })(MirGames.BasePage);
        Wip.ProjectWorkItemsPage = ProjectWorkItemsPage;

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

        (function (ViewMode) {
            ViewMode[ViewMode["List"] = 0] = "List";
            ViewMode[ViewMode["Blocks"] = 1] = "Blocks";
        })(Wip.ViewMode || (Wip.ViewMode = {}));
        var ViewMode = Wip.ViewMode;
    })(MirGames.Wip || (MirGames.Wip = {}));
    var Wip = MirGames.Wip;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=ProjectWorkItemsPage.js.map
