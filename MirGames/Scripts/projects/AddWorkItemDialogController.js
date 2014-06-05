var MirGames;
(function (MirGames) {
    /// <reference path="../_references.ts" />
    (function (Projects) {
        var AddNewWorkItemDialogController = (function () {
            function AddNewWorkItemDialogController($scope, commandBus, dialog, apiService, options, eventBus) {
                var _this = this;
                this.$scope = $scope;
                this.commandBus = commandBus;
                this.dialog = dialog;
                this.apiService = apiService;
                this.options = options;
                this.eventBus = eventBus;
                $scope.type = options['item-type'];
                $scope.projectAlias = options['project-alias'];
                $scope.focus = true;
                $scope.post = function () {
                    return _this.postNewItem();
                };
                $scope.typeNames = ['', 'бага', 'задача', 'фича'];

                $scope.close = function () {
                    dialog.cancel();
                };
            }
            /** Posts the new item */
            AddNewWorkItemDialogController.prototype.postNewItem = function () {
                var _this = this;
                var command = {
                    ProjectAlias: this.$scope.projectAlias,
                    Title: this.$scope.title,
                    Tags: this.$scope.tags,
                    Type: this.$scope.type,
                    Attachments: this.$scope.attachments,
                    Description: this.$scope.text
                };

                this.apiService.executeCommand('CreateNewProjectWorkItemCommand', command, function (internalId) {
                    _this.eventBus.emit(_this.$scope.projectAlias + '.workitems.new', internalId);
                    _this.eventBus.emit('user.notification', 'Новая ' + _this.$scope.typeNames[_this.$scope.type] + ' успешно создана');
                    _this.$scope.close();
                });
            };
            AddNewWorkItemDialogController.$inject = ['$scope', 'commandBus', 'dialog', 'apiService', 'dialogOptions', 'eventBus'];
            return AddNewWorkItemDialogController;
        })();
        Projects.AddNewWorkItemDialogController = AddNewWorkItemDialogController;
    })(MirGames.Projects || (MirGames.Projects = {}));
    var Projects = MirGames.Projects;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=AddWorkItemDialogController.js.map
