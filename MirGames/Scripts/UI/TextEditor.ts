/// <reference path="../_references.ts" />

module UI {
    export class TextEditorController {
        static $inject = ['$scope', '$element', 'eventBus'];
        private topicPreview: JQuery;
        private topicEditor: JQuery;

        constructor(private $scope: ITextEditorScope, private $element: JQuery, private eventBus: Core.IEventBus) {
            this.topicPreview = $('.mdd_preview', $element);
            this.topicEditor = $('textarea', $element);
            this.$scope.showPreview = true;
            this.$scope.focus = false;
            this.$scope.useEnterToPost = false;

            this.initializeScrollSynchronization();
            this.onResize();

            var onResizeCallback = () => {
                this.onResize();
            }

            $(window).bind('resize', onResizeCallback);
            this.eventBus.on('section.resized', onResizeCallback);

            $scope.$on('$destroy', () => {
                $(window).unbind('resize', onResizeCallback);
                this.eventBus.off('section.resized', onResizeCallback);
                this.topicEditor.unbind('scroll');
            });
        }

        private initializeScrollSynchronization() {
            var oldScrollTop = 0;

            this.topicEditor.bind('scroll', () => {
                var overallScrollHeight = this.topicEditor.prop('scrollHeight');
                var scrollTop = this.topicEditor.prop('scrollTop');

                var previewScrollTop = this.topicPreview.scrollTop();
                var previewScrollHeight = this.topicPreview.prop('scrollHeight');

                var delta = (scrollTop - oldScrollTop) / overallScrollHeight * previewScrollHeight;
                this.topicPreview.prop('scrollTop', previewScrollTop + delta);

                oldScrollTop = scrollTop;
            });
        }

        private onResize() {
            this.topicPreview.height(this.topicEditor.height);
        }
    }

    export interface ITextEditorScope extends UI.IAppScope {
        text: string;
        entityType: string;
        showPreview: boolean;
        attachment: number[];
        post: () => void;
        focus: boolean;
        useEnterToPost: boolean;
        sizeChanged: () => void;
        autoresize?: boolean;
    }

    angular
        .module('ui.texteditor', ['core.eventBus'])
        .directive('texteditor', () => {
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
                    'caret': '=?caret',
                    'entityType': '@',
                    'useEnterToPost': '@useEnterToPost',
                    'sizeChanged': '&sizeChanged',
                    'autoresize': '=?autoresize'
                },
                transclude: false,
                template:
                    '<div ng-class="{ \'text-editor-field\': true, \'text-editor-show-preview\': showPreview }">' +
                        '<tinyeditor text="text" post="post()" sizeChanged="sizeChanged()" autoresize="autoresize" focus="focus" attachments="attachments" entity-type="{{entityType}}" required="required" use-enter-to-post="{{useEnterToPost}}" caret="caret"></tinyeditor>' +
                    '</div>'
            }
    });
}