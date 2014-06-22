/// <reference path="_references.ts" />
module MirGames.Topics {
    export class AddNewTopicDialogController {
        static $inject = ['$scope', 'dialog', 'apiService', 'dialogOptions'];

        constructor(private $scope: IAddNewTopicDialogControllerScope, private dialog: UI.IDialog, private apiService: Core.IApiService, private options: any) {
            this.$scope.blogId = options['blog-id'] || null;
            this.$scope.attachments = [];
            this.$scope.isTitleFocused = true;
            this.$scope.width = '60%';

            this.$scope.save = () => this.submit();
            this.$scope.close = () => dialog.cancel();
        }

        private submit() {
            var command: MirGames.Domain.Topics.Commands.AddNewTopicCommand = {
                Attachments: this.$scope.attachments,
                BlogId: this.$scope.blogId,
                Tags: this.$scope.tags,
                Text: this.$scope.text,
                Title: this.$scope.title
            };

            this.apiService.executeCommand('AddNewTopicCommand', command, (result) => {
                Core.Application.getInstance().navigateToUrl(Router.action("Topics", "Topic", { topicId: result, area: 'Topics' }));
            });
        }
    }

    export interface IAddNewTopicDialogControllerScope extends ng.IScope {
        text: string;
        title: string;
        tags: string;
        blogId?: number;
        save(): void;
        attachments: number[];
        isTitleFocused: boolean;
        width: string;
        close(): void;
    }
}