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
        var ProjectSettingsPage = (function (_super) {
            __extends(ProjectSettingsPage, _super);
            function ProjectSettingsPage($scope, apiService, eventBus) {
                var _this = this;
                _super.call(this, $scope, eventBus);
                this.apiService = apiService;
                this.$scope.attachments = [];
                this.$scope.showPreview = true;
                this.$scope.isTitleFocused = true;
                this.$scope.repositoryUrl = this.pageData.project.RepositoryUrl;
                this.$scope.repositoryType = this.pageData.project.RepositoryType;
                this.$scope.title = this.pageData.project.Title;
                this.$scope.tags = this.pageData.project.Tags.join(', ');
                this.$scope.description = this.pageData.project.Description;
                this.$scope.logoUrl = this.pageData.project.LogoUrl;
                this.$scope.isPrivate = this.pageData.project.IsRepositoryPrivate;

                this.$scope.save = function () {
                    return _this.save();
                };
                this.$scope.fileUploaded = function (attachmentId) {
                    return _this.fileUploaded(attachmentId);
                };
            }
            ProjectSettingsPage.prototype.fileUploaded = function (attachmentId) {
                this.$scope.attachmentId = attachmentId;
                this.$scope.logoUrl = Router.action('Attachment', 'Index', { attachmentId: attachmentId });
                this.$scope.$apply();
            };

            ProjectSettingsPage.prototype.save = function () {
                var _this = this;
                var command = {
                    Title: this.$scope.title,
                    Alias: this.pageData.project.Alias,
                    Tags: this.$scope.tags,
                    LogoAttachmentId: this.$scope.attachmentId,
                    Attachments: this.$scope.attachments,
                    Description: this.$scope.description,
                    IsRepositoryPrivate: this.$scope.isPrivate
                };

                this.apiService.executeCommand("SaveWipProjectCommand", command, function () {
                    _this.eventBus.emit('user.notification', 'Настройки проекта сохранены');
                    _this.$scope.$apply();
                });
            };
            ProjectSettingsPage.$inject = ['$scope', 'apiService', 'eventBus'];
            return ProjectSettingsPage;
        })(MirGames.BasePage);
        Wip.ProjectSettingsPage = ProjectSettingsPage;
    })(MirGames.Wip || (MirGames.Wip = {}));
    var Wip = MirGames.Wip;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=ProjectSettingsPage.js.map
