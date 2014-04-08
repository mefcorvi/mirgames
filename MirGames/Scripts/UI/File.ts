/// <reference path="../_references.ts" />
module UI {
    class FileUploadController {
        static $inject = ['$scope', '$element', '$attrs', 'attachmentService', '$parse'];
        private uploadedHandler: ng.ICompiledExpression;
        private entityType: string;

        constructor(private $scope: IFileUploadScope, private $element: JQuery, private attrs: any, private attachmentService: MirGames.Attachment.IAttachmentService, private $parse: ng.IParseService) {
            var $fileInput = $('input:file', this.$element);

            if (attrs['uploaded']) {
                this.uploadedHandler = $parse(attrs['uploaded']);
            }

            if (!attrs['entityType']) {
                throw new Error('File control requires entityType attribute.');
            }

            this.entityType = attrs['entityType'];

            var fileSelected = (ev: JQueryEventObject) => {
                var fileInput: any = $fileInput[0];

                for (var i = 0; i < fileInput.files.length; i++) {
                    this.attachFile(fileInput.files[i]);
                }

                $fileInput.replaceWith($fileInput.clone());
                $fileInput = $('input:file', this.$element);
                $fileInput.change(fileSelected);
            };

            $fileInput.change(fileSelected);

            this.$element[0].ondragover = () => { this.$element.addClass('drag-over'); return false; };
            this.$element[0].ondragend = () => { this.$element.removeClass('drag-over'); return false; };
            this.$element[0].ondrop = (e: any) => {
                this.$element.removeClass('drag-over');
                e.preventDefault();
                var files = e.dataTransfer.files;

                for (var i = 0; i < files.length; i++) {
                    this.attachFile(files[i]);
                }
            }
        }

        private attachFile(file: File) {
            this.attachmentService.uploadFile(this.entityType, file, (err, file, attachmentId) => {
                if (err) {
                    alert(err.message);
                    return;
                }

                if (this.uploadedHandler) {
                    this.uploadedHandler(this.$scope, { '$attachmentId': attachmentId });
                }
            });
        }
    }

    interface IFileUploadScope extends ng.IScope {
    }

    angular
        .module('ui.file', [])
        .directive('file', () => {
            return {
                restrict: 'E',
                replace: true,
                transclude: false,
                controller: FileUploadController,
                template: '<form class="upload-file">Загрузить файл: <input type="file" multiple /></form>'
            };
        });
}