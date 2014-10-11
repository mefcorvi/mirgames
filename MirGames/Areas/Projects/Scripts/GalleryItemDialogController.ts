/// <reference path="_references.ts" />
module MirGames.Projects {
    export class GalleryItemDialogController {
        static $inject = ['$scope', 'dialog', 'apiService', 'dialogOptions', 'eventBus'];

        constructor(private $scope: IGalleryItemDialogControllerScope, private dialog: UI.IDialog, private apiService: Core.IApiService, private options: any, private eventBus: Core.IEventBus) {
            $scope.entityId = <number>options['entity-id'];
            $scope.attachmentId = <number>options['attachment-id'];
            $scope.attachmentUrl = <string>options['attachment-url'];

            var query: MirGames.Domain.Attachments.Queries.GetAttachmentsQuery = {
                EntityId: $scope.entityId,
                EntityType: 'project-gallery',
                IsImage: true
            };

            this.apiService.getAll('GetAttachmentsQuery', query, 0, 100, (result: MirGames.Domain.Attachments.ViewModels.AttachmentViewModel[]) => {
                $scope.attachments = result;
                $scope.currentIndex = Enumerable.from(result).indexOf(e => e.AttachmentId == $scope.attachmentId);
            });

            $scope.next = () => {
                $scope.currentIndex++;

                if ($scope.currentIndex >= $scope.attachments.length) {
                    $scope.currentIndex = 0;
                }

                $scope.attachmentId = $scope.attachments[$scope.currentIndex].AttachmentId;
                $scope.attachmentUrl = $scope.attachments[$scope.currentIndex].AttachmentUrl;
            };

            $scope.prev = () => {
                $scope.currentIndex--;

                if ($scope.currentIndex < 0) {
                    $scope.currentIndex = $scope.attachments.length - 1;
                }

                $scope.attachmentId = $scope.attachments[$scope.currentIndex].AttachmentId;
                $scope.attachmentUrl = $scope.attachments[$scope.currentIndex].AttachmentUrl;
            };

            $scope.close = () => {
                dialog.cancel();
            }
        }
    }

    export interface IGalleryItemDialogControllerScope extends ng.IScope {
        close(): void;
        attachmentUrl: string;
        attachmentId: number;
        entityId: number;
        attachments: MirGames.Domain.Attachments.ViewModels.AttachmentViewModel[];
        currentIndex: number;
        next(): void;
        prev(): void;
    }
} 