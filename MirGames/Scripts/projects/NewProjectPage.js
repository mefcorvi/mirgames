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
        var NewProjectPage = (function (_super) {
            __extends(NewProjectPage, _super);
            function NewProjectPage($scope, apiService, eventBus) {
                var _this = this;
                _super.call(this, $scope, eventBus);
                this.apiService = apiService;
                this.$scope.attachments = [];
                this.$scope.showPreview = true;
                this.$scope.isTitleFocused = true;
                this.$scope.repository = "";

                this.$scope.save = function () {
                    return _this.save();
                };
                this.$scope.fileUploaded = function (attachmentId) {
                    return _this.fileUploaded(attachmentId);
                };
            }
            NewProjectPage.prototype.fileUploaded = function (attachmentId) {
                this.$scope.attachmentId = attachmentId;
                this.$scope.logoUrl = Router.action('Attachment', 'Index', { attachmentId: attachmentId });
                this.$scope.$apply();
            };

            NewProjectPage.prototype.save = function () {
                var _this = this;
                var command = {
                    Title: this.$scope.title,
                    Alias: this.$scope.name,
                    Tags: this.$scope.tags,
                    RepositoryType: this.$scope.repository,
                    LogoAttachmentId: this.$scope.attachmentId,
                    Attachments: this.$scope.attachments,
                    Description: this.$scope.description
                };

                this.apiService.executeCommand("CreateNewWipProjectCommand", command, function () {
                    Core.Application.getInstance().navigateTo("Projects", "Project", { projectAlias: _this.$scope.name });
                });
            };
            NewProjectPage.$inject = ['$scope', 'apiService', 'eventBus'];
            return NewProjectPage;
        })(MirGames.BasePage);
        Wip.NewProjectPage = NewProjectPage;

        angular.module('ng').directive('uniqueProjectName', [
            'apiService',
            function (apiService) {
                var toId;

                return {
                    restrict: 'A',
                    require: 'ngModel',
                    link: function (scope, elem, attr, ctrl) {
                        scope.$watch(attr.ngModel, function (value) {
                            if (toId != null) {
                                clearTimeout(toId);
                            }

                            toId = setTimeout(function () {
                                var query = {
                                    Alias: value
                                };

                                apiService.getOne('GetIsProjectNameUniqueQuery', query, function (result) {
                                    ctrl.$setValidity('uniqueProjectName', result);
                                    scope.$apply();
                                }, false);
                            }, 200);
                        });
                    }
                };
            }
        ]);
    })(MirGames.Wip || (MirGames.Wip = {}));
    var Wip = MirGames.Wip;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=NewProjectPage.js.map
