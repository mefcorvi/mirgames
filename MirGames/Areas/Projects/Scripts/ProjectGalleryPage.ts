/// <reference path="_references.ts" />
module MirGames.Wip {
    export class ProjectGalleryPage extends MirGames.BasePage<IProjectGalleryPageData, IProjectGalleryPageScope> {
        static $inject = ['$scope', 'apiService', 'eventBus'];

        constructor($scope: IProjectGalleryPageScope, private apiService: Core.IApiService, eventBus: Core.IEventBus) {
            super($scope, eventBus);
            this.$scope.fileUploaded = (attachmentId) => this.fileUploaded(attachmentId);
        }

        private fileUploaded(attachmentId: number) {
            var command: MirGames.Domain.Wip.Commands.AddWipGalleryImageCommand = {
                Attachments: [attachmentId],
                ProjectAlias: this.pageData.projectAlias
            };

            this.apiService.executeCommand('AddWipGalleryImageCommand', command, () => {

                this.$scope.$apply();
            });
        }
    }

    export interface IProjectGalleryPageScope extends IPageScope {
        fileUploaded: (attachmentId: number) => void;
    }

    export interface IProjectGalleryPageData {
        projectAlias: string;
    }
}