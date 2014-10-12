/// <reference path="_references.ts" />
module MirGames.Wip {
    export class ProjectGalleryPage extends MirGames.BasePage<IProjectGalleryPageData, IProjectGalleryPageScope> {
        static $inject = ['$scope', 'apiService', 'eventBus'];

        constructor($scope: IProjectGalleryPageScope, private apiService: Core.IApiService, eventBus: Core.IEventBus) {
            super($scope, eventBus);
            this.$scope.images = [];
            this.$scope.fileUploaded = (attachmentId) => this.fileUploaded(attachmentId);
        }

        private fileUploaded(attachmentId: number) {
            var command: MirGames.Domain.Wip.Commands.AddWipGalleryImageCommand = {
                Attachments: [attachmentId],
                ProjectAlias: this.pageData.projectAlias
            };

            this.apiService.executeCommand('AddWipGalleryImageCommand', command, () => {
                var attachmentUrl = Router.action('Attachment', 'Index', { attachmentId: attachmentId });
                var item = {
                    attachmentId: attachmentId,
                    attachmentUrl: attachmentUrl
                };

                this.$scope.images.push(item);
                this.$scope.$apply();
            });
        }
    }

    export interface IProjectGalleryPageScope extends IPageScope {
        images: { attachmentId: number; attachmentUrl: string; }[];
        fileUploaded: (attachmentId: number) => void;
    }

    export interface IProjectGalleryPageData {
        projectAlias: string;
    }
}