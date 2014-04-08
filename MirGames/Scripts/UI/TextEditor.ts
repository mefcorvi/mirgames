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

            $(window).bind('resize', this.onResize.bind(this));
            this.eventBus.on('section.resized', this.onResize.bind(this));
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
                    'showPreview': '=showPreview',
                    'attachments': '=attachments',
                    'post': '&post',
                    'focus': '=focus',
                    'entityType': '@',
                    'useEnterToPost': '@useEnterToPost'
                },
                transclude: false,
                template:
                    '<div ng-class="{ \'text-editor-field\': true, \'text-editor-show-preview\': showPreview }">' +
                        '<tinyeditor text="text" post="post()" focus="focus" attachments="attachments" entity-type="{{entityType}}" use-enter-to-post="{{useEnterToPost}}"></tinyeditor>' +
                    '</div>'
            }
    });
}