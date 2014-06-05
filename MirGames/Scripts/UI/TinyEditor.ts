/// <reference path="../_references.ts" />

module UI {
    export class TinyEditorController {
        static $inject = ['$scope', '$element', 'attachmentService'];

        private markdownEditor: any;

        constructor(private $scope: ITinyEditorScope, private $element: JQuery, private attachmentService: MirGames.Attachment.IAttachmentService) {
            this.$scope.attachments = [];
            this.$scope.showUploadForm = false;
            this.$scope.useEnterToPost = 'false';
            this.$scope.showPreview = false;

            var textArea = $('textarea', $element);
            textArea.keydown(this.handleKeyDownEvent.bind(this));
            textArea.bind('paste', this.handlePasteEvent.bind(this));

            (<any>textArea).MarkdownDeep({
                help_location: "/Content/mdd_help.html",
                onPreTransform: () => {
                    setTimeout(() => {
                        this.$scope.text = textArea.val();
                        this.$scope.$digest();
                    }, 0);
                }
            });

            this.markdownEditor = textArea.data('mdd');

            this.$scope.$watch('text', () => {
                setTimeout(() => {
                    this.markdownEditor.onMarkdownChanged();
                }, 0);
            });

            this.initializeFileUploading();
            this.initializePreviewMode();
        }

        private initializePreviewMode() {
            var $previewButton = $('.btn-preview', this.$element);
            $previewButton.click(() => {
                this.$scope.$apply(() => {
                    $('.mdd_preview', this.$element).css('height', $('.mdd_editor_wrap textarea', this.$element).outerHeight());
                    this.$scope.showPreview = !this.$scope.showPreview;
                });

                if (this.$scope.showPreview) {
                    $previewButton.addClass('selected');
                } else {
                    $previewButton.removeClass('selected');
                }
            });
        }

        private initializeFileUploading() {
            var $uploadButton = $('.fa-upload', this.$element).parent();
            $uploadButton.click(() => {
                this.$scope.$apply(() => {
                    this.$scope.showUploadForm = !this.$scope.showUploadForm;
                });

                if (this.$scope.showUploadForm) {
                    $uploadButton.addClass('selected');
                } else {
                    $uploadButton.removeClass('selected');
                }
            });

            var $fileInput = $('input:file', this.$element);

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

        private handleKeyDownEvent(ev: JQueryKeyEventObject): void {
            if (this.$scope.useEnterToPost == 'true') {
                if (ev.which == 13 && !ev.ctrlKey && !ev.metaKey && !ev.altKey && !ev.shiftKey) {
                    this.$scope.post();
                    ev.preventDefault();
                }
            }
            else if ((ev.ctrlKey || ev.metaKey) && ev.which == 13) {
                this.$scope.post();
                ev.preventDefault();
            }
        }

        private handlePasteEvent(ev: any): void {
            var clipboardData = ev.originalEvent.clipboardData;

            if (!clipboardData || clipboardData.items.length == 0) {
                return;
            }

            for (var i = 0; i < clipboardData.items.length; i++) {
                var item = <any>clipboardData.items[i];

                if (!item) {
                    return;
                }

                var matches = item.type.match(/^image\/(.+?)$/);

                if (matches) {
                    var file = <File>item.getAsFile();
                    file.name = 'clipboard.' + matches[1];
                    this.attachFile(file);
                }
            }
        }

        private attachFile(file: File) {
            this.attachmentService.uploadFile(this.$scope.entityType, file, (err, file, attachmentId) => {
                if (err) {
                    alert(err.message);
                    return;
                }

                var isImage = file.type.match(/^image\/(.+?)$/);
                if (isImage) {
                    this.markdownEditor.InvokeCommand('img', { url: Router.action('Attachment', 'Index', { attachmentId: attachmentId }), title: file.name });
                    this.$scope.attachments.push(attachmentId);
                } else {
                    this.markdownEditor.InvokeCommand('link', { url: Router.action('Attachment', 'Index', { attachmentId: attachmentId }), title: file.name });
                    this.$scope.attachments.push(attachmentId);
                }
            });
        }
    }

    export interface ITinyEditorScope extends ng.IScope {
        text: string;
        entityType: string;
        attachments: number[];
        post: () => void;
        showUploadForm: boolean;
        focus: boolean;
        useEnterToPost: string;
        showPreview: boolean;
    }

    angular
        .module('ui.tinyeditor', ['core.config', 'core.eventBus', 'mirgames.attachment'])
        .directive('tinyeditor', () => {
        return {
                restrict: 'E',
                replace: true,
                scope: {
                    'text': '=text',
                    'attachments': '=attachments',
                    'post': '&post',
                    'required': '=?required',
                    'focus': '=focus',
                    'entityType': '@',
                    'useEnterToPost': '@useEnterToPost'
                },
                controller: TinyEditorController,
                transclude: false,
                template:
                    '<div ng-class="{ \'tiny-editor\': true, \'tiny-editor-show-preview\': showPreview }">' +
                        '<div class="mdd_toolbar_wrap"><div class="mdd_toolbar"></div></div>' +
                        '<form class="upload-file" ng-show="showUploadForm">Добавить файл: <input type="file" multiple> <span>Также вы можете добавить файл, перетащив его в редактор, или вставить картинку из буфера обмена.</span></form>' +
                        '<div class="mdd_editor_wrap"><textarea cols="50" rows="10" class="mdd_editor" ng-model="text" ng-maxlength="65536" ng-required="required" ng-focused="focus"></textarea></div>' +
                        '<div class="mdd_preview text"></div>' +
                        '<div class="mdd_resizer_wrap"></div>' +
                    '</div>'
            }
    });
}