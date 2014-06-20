/// <reference path="_references.ts" />
module MirGames.Topics {

    export class NewTopicPage {
        static $inject = ['$scope', '$element', 'commandBus', 'eventBus'];

        constructor(private $scope: INewTopicPageScope, private $element: JQuery, private commandBus: Core.ICommandBus, private eventBus: Core.IEventBus) {
            this.$scope.save = this.submit.bind(this);
            this.$scope.attachments = [];

            this.$scope.switchPreviewMode = () => {
                this.$scope.showPreview = !this.$scope.showPreview;
            };
            this.$scope.isTitleFocused = true;
        }

        private submit() {
            var command = this.commandBus.createCommandFromScope(Domain.AddNewTopicCommand, this.$scope);

            this.commandBus.executeCommand(Router.action("Topics", "AddTopic"), command, (result) => {
                Core.Application.getInstance().navigateToUrl(Router.action("Topics", "Topic", { topicId: result.topicId }));
            });
        }
    }

    export interface INewTopicPageScope extends IPageScope {
        text: string;
        showPreview: boolean;
        title: string;
        tags: string;
        save(): void;
        isTitleFocused: boolean;
        switchPreviewMode(): void;
        attachments: number[];
    }
}