/// <reference path="../_references.ts" />
var UI;
(function (UI) {
    var FileUploadController = (function () {
        function FileUploadController($scope, $element, attrs, attachmentService, $parse) {
            var _this = this;
            this.$scope = $scope;
            this.$element = $element;
            this.attrs = attrs;
            this.attachmentService = attachmentService;
            this.$parse = $parse;
            var $fileInput = $('input:file', this.$element);

            if (attrs['uploaded']) {
                this.uploadedHandler = $parse(attrs['uploaded']);
            }

            if (!attrs['entityType']) {
                throw new Error('File control requires entityType attribute.');
            }

            this.entityType = attrs['entityType'];

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
        }
        FileUploadController.prototype.attachFile = function (file) {
            var _this = this;
            this.attachmentService.uploadFile(this.entityType, file, function (err, file, attachmentId) {
                if (err) {
                    alert(err.message);
                    return;
                }

                if (_this.uploadedHandler) {
                    _this.uploadedHandler(_this.$scope, { '$attachmentId': attachmentId });
                }
            });
        };
        FileUploadController.$inject = ['$scope', '$element', '$attrs', 'attachmentService', '$parse'];
        return FileUploadController;
    })();

    angular.module('ui.file', []).directive('file', function () {
        return {
            restrict: 'E',
            replace: true,
            transclude: false,
            controller: FileUploadController,
            template: '<form class="upload-file">Загрузить файл: <input type="file" multiple /></form>'
        };
    });
})(UI || (UI = {}));
//# sourceMappingURL=File.js.map
