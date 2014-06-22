/// <reference path="../_references.ts" />
var UI;
(function (UI) {
    var TextEditorController = (function () {
        function TextEditorController($scope, $element, eventBus) {
            this.$scope = $scope;
            this.$element = $element;
            this.eventBus = eventBus;
            this.topicPreview = $('.mdd_preview', $element);
            this.topicEditor = $('textarea', $element);
            this.$scope.showPreview = true;
            this.$scope.focus = false;
            this.$scope.useEnterToPost = false;

            this.initializeScrollSynchronization();
            this.onResize();

            $(window).bind('resize', this.onResize.bind(this));
            this.eventBus.on('section.resized', this.onResize.bind(this));
        }
        TextEditorController.prototype.initializeScrollSynchronization = function () {
            var _this = this;
            var oldScrollTop = 0;

            this.topicEditor.bind('scroll', function () {
                var overallScrollHeight = _this.topicEditor.prop('scrollHeight');
                var scrollTop = _this.topicEditor.prop('scrollTop');

                var previewScrollTop = _this.topicPreview.scrollTop();
                var previewScrollHeight = _this.topicPreview.prop('scrollHeight');

                var delta = (scrollTop - oldScrollTop) / overallScrollHeight * previewScrollHeight;
                _this.topicPreview.prop('scrollTop', previewScrollTop + delta);

                oldScrollTop = scrollTop;
            });
        };

        TextEditorController.prototype.onResize = function () {
            this.topicPreview.height(this.topicEditor.height);
        };
        TextEditorController.$inject = ['$scope', '$element', 'eventBus'];
        return TextEditorController;
    })();
    UI.TextEditorController = TextEditorController;

    angular.module('ui.texteditor', ['core.eventBus']).directive('texteditor', function () {
        return {
            restrict: 'E',
            replace: true,
            controller: TextEditorController,
            scope: {
                'text': '=text',
                'showPreview': '=?showPreview',
                'attachments': '=attachments',
                'required': '=?required',
                'post': '&post',
                'focus': '=?focus',
                'entityType': '@',
                'useEnterToPost': '@useEnterToPost'
            },
            transclude: false,
            template: '<div ng-class="{ \'text-editor-field\': true, \'text-editor-show-preview\': showPreview }">' + '<tinyeditor text="text" post="post()" focus="focus" attachments="attachments" entity-type="{{entityType}}" required="required" use-enter-to-post="{{useEnterToPost}}"></tinyeditor>' + '</div>'
        };
    });
})(UI || (UI = {}));
//# sourceMappingURL=TextEditor.js.map
