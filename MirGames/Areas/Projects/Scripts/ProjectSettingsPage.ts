/// <reference path="_references.ts" />
module MirGames.Wip {
    export class ProjectSettingsPage extends MirGames.BasePage<IProjectSettingsPageData, IProjectSettingsPageScope> {
        static $inject = ['$scope', 'apiService', 'eventBus'];

        constructor($scope: IProjectSettingsPageScope, private apiService: Core.IApiService, eventBus: Core.IEventBus) {
            super($scope, eventBus);
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

            this.$scope.save = () => this.save();
            this.$scope.fileUploaded = (attachmentId) => this.fileUploaded(attachmentId);
        }

        private fileUploaded(attachmentId: number) {
            this.$scope.attachmentId = attachmentId;
            this.$scope.logoUrl = Router.action('Attachment', 'Index', { attachmentId: attachmentId });
            this.$scope.$apply();
        }

        private save() {
            var command: MirGames.Domain.Wip.Commands.SaveWipProjectCommand = 
            {
                Title: this.$scope.title,
                Alias: this.pageData.project.Alias,
                Tags: this.$scope.tags,
                LogoAttachmentId: this.$scope.attachmentId,
                Attachments: this.$scope.attachments,
                Description: this.$scope.description,
                IsRepositoryPrivate: this.$scope.isPrivate
            };

            this.apiService.executeCommand("SaveWipProjectCommand", command, () => {
                this.eventBus.emit('user.notification', 'Настройки проекта сохранены');
                this.$scope.$apply();
            });
        }
    }

    export interface IProjectSettingsPageScope extends IPageScope {
        title: string;
        name: string;
        tags: string;
        repositoryType: string;
        repositoryUrl: string;
        attachmentId: number;
        attachments: number[];
        description: string;

        isPrivate: boolean;
        logoUrl: string;
        showPreview: boolean;
        isTitleFocused: boolean;
        fileUploaded: (attachmentId: number) => void;
        save: () => void;
    }

    export interface IProjectSettingsPageData {
        project: MirGames.Domain.Wip.ViewModels.WipProjectForEditViewModel;
    }
}