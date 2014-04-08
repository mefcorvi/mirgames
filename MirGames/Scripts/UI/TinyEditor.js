/// <reference path="../_references.ts" />
var UI;
(function (UI) {
    var TinyEditorController = (function () {
        function TinyEditorController($scope, $element, attachmentService) {
            var _this = this;
            this.$scope = $scope;
            this.$element = $element;
            this.attachmentService = attachmentService;
            this.$scope.attachments = [];
            this.$scope.showUploadForm = false;
            this.$scope.useEnterToPost = 'false';

            var textArea = $('textarea', $element);
            textArea.keydown(this.handleKeyDownEvent.bind(this));
            textArea.bind('paste', this.handlePasteEvent.bind(this));

            textArea.MarkdownDeep({
                help_location: "/Content/mdd_help.html",
                onPreTransform: function () {
                    setTimeout(function () {
                        _this.$scope.text = textArea.val();
                        _this.$scope.$digest();
                    }, 0);
                }
            });

            this.markdownEditor = textArea.data('mdd');

            this.$scope.$watch('text', function (newValue, oldValue, scope) {
                setTimeout(function () {
                    _this.markdownEditor.onMarkdownChanged();
                }, 0);
            });

            this.initializeFileUploading();
        }
        TinyEditorController.prototype.initializeFileUploading = function () {
            var _this = this;
            var $uploadButton = $('.fa-upload', this.$element).parent();
            $uploadButton.click(function () {
                _this.$scope.$apply(function () {
                    _this.$scope.showUploadForm = !_this.$scope.showUploadForm;
                });

                if (_this.$scope.showUploadForm) {
                    $uploadButton.addClass('selected');
                } else {
                    $uploadButton.removeClass('selected');
                }
            });

            var $fileInput = $('input:file', this.$element);

            var fileSelected = function (ev) {
                var fileInput = $fileInput[0];

                for (var i = 0; i < fileInput.files.length; i++) {
                    _this.attachFile(fileInput.files[i]);
                }

                $fileInput.replaceWith($fileInput.clone());
                $fileInput = $('input:file', _this.$element);
                $fileInput.change(fileSelected);
            };

            $fileInput.change(fileSelected);

            this.$element[0].ondragover = function () {
                _this.$element.addClass('drag-over');
                return false;
            };
            this.$element[0].ondragend = function () {
                _this.$element.removeClass('drag-over');
                return false;
            };
            this.$element[0].ondrop = function (e) {
                _this.$element.removeClass('drag-over');
                e.preventDefault();
                var files = e.dataTransfer.files;

                for (var i = 0; i < files.length; i++) {
                    _this.attachFile(files[i]);
                }
            };
        };

        TinyEditorController.prototype.handleKeyDownEvent = function (ev) {
            if (this.$scope.useEnterToPost == 'true') {
                if (ev.which == 13 && !ev.ctrlKey && !ev.metaKey && !ev.altKey && !ev.shiftKey) {
                    this.$scope.post();
                    ev.preventDefault();
                }
            } else if ((ev.ctrlKey || ev.metaKey) && ev.which == 13) {
                this.$scope.post();
                ev.preventDefault();
            }
        };

        TinyEditorController.prototype.handlePasteEvent = function (ev) {
            var clipboardData = ev.originalEvent.clipboardData;

            if (!clipboardData || clipboardData.items.length == 0) {
                return;
            }

            for (var i = 0; i < clipboardData.items.length; i++) {
                var item = clipboardData.items[i];

                if (!item) {
                    return;
                }

                var matches = item.type.match(/^image\/(.+?)$/);

                if (matches) {
                    var file = item.getAsFile();
                    file.name = 'clipboard.' + matches[1];
                    this.attachFile(file);
                }
            }
        };

        TinyEditorController.prototype.attachFile = function (file) {
            var _this = this;
            this.attachmentService.uploadFile(this.$scope.entityType, file, function (err, file, attachmentId) {
                if (err) {
                    alert(err.message);
                    return;
                }

                var isImage = file.type.match(/^image\/(.+?)$/);
                if (isImage) {
                    _this.markdownEditor.InvokeCommand('img', { url: Router.action('Attachment', 'Index', { attachmentId: attachmentId }), title: file.name });
                    _this.$scope.attachments.push(attachmentId);
                } else {
                    _this.markdownEditor.InvokeCommand('link', { url: Router.action('Attachment', 'Index', { attachmentId: attachmentId }), title: file.name });
                    _this.$scope.attachments.push(attachmentId);
                }
            });
        };
        TinyEditorController.$inject = ['$scope', '$element', 'attachmentService'];
        return TinyEditorController;
    })();
    UI.TinyEditorController = TinyEditorController;

    angular.module('ui.tinyeditor', ['core.config', 'core.eventBus', 'mirgames.attachment']).directive('tinyeditor', function () {
        return {
            restrict: 'E',
            replace: true,
            scope: {
                'text': '=text',
                'attachments': '=attachments',
                'post': '&post',
                'focus': '=focus',
                'entityType': '@',
                'useEnterToPost': '@useEnterToPost'
            },
            controller: TinyEditorController,
            transclude: false,
            template: '<div class="tiny-editor">' + '<div class="mdd_toolbar_wrap"><div class="mdd_toolbar"></div></div>' + '<form class="upload-file" ng-show="showUploadForm">Добавить файл: <input type="file" multiple> <span>Также вы можете добавить файл, перетащив его в редактор, или вставить картинку из буфера обмена.</span></form>' + '<div class="mdd_editor_wrap"><textarea cols="50" rows="10" class="mdd_editor" ng-model="text" ng-maxlength="65536" required ng-focused="focus"></textarea></div>' + '<div class="mdd_resizer_wrap"></div>' + '<div class="mdd_preview text"></div>' + '</div>'
        };
    });
})(UI || (UI = {}));
//# sourceMappingURL=TinyEditor.js.map
