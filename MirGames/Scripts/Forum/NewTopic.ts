/// <reference path="../_references.ts" />
module MirGames.Forum {

    export class NewTopicPage {
        static $inject = ['$scope', '$element', 'commandBus', 'eventBus'];

        constructor(private $scope: INewTopicPageScope, private $element: JQuery, private commandBus: Core.ICommandBus, private eventBus: Core.IEventBus) {
            this.$scope.post = this.submit.bind(this);
            this.$scope.attachments = [];
            this.$scope.switchPreviewMode = () => {
                this.$scope.showPreview = !this.$scope.showPreview;
            };
            this.$scope.isTitleFocused = true;
        }

        private submit() {
            var command = this.commandBus.createCommandFromScope(Domain.PostNewForumTopicCommand, this.$scope);

            this.commandBus.executeCommand(Router.action("Forum", "PostNewTopic"), command, (result) => {
                Core.Application.getInstance().navigateToUrl(Router.action("Forum", "Topic", { topicId: result.topicId }));
            });
        }
    }

    export interface INewTopicPageScope extends IPageScope {
        text: string;
        showPreview: boolean;
        title: string;
        tags: string;
        post(): void;
        attachments: number[];
        isTitleFocused: boolean;
        switchPreviewMode(): void;
    }
}