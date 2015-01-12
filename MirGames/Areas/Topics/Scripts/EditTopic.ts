/// <reference path="_references.ts" />
module MirGames.Topics {
    declare var ace: any;

    export class EditTopicPage {
        static $inject = ['$scope', '$element', 'apiService', 'pageDataService'];

        constructor(private $scope: IEditTopicPageScope, private $element: JQuery, private apiService: Core.IApiService, pageDataService: IPageDataProvider) {
            var pageData = pageDataService.getPageData<IEditTopicPageData>();
            this.$scope.topicId = pageData.topicId;
            this.$scope.title = pageData.title;
            this.$scope.text = pageData.text;
            this.$scope.tags = pageData.tags;
            this.$scope.save = this.submit.bind(this);
            this.$scope.attachments = [];

            this.$scope.switchPreviewMode = () => {
                this.$scope.showPreview = !this.$scope.showPreview;
            };
        }

        private submit() {
            var command: MirGames.Domain.Topics.Commands.SaveTopicCommand = {
                Attachments: this.$scope.attachments,
                Tags: this.$scope.tags,
                Text: this.$scope.text,
                Title: this.$scope.title,
                TopicId: this.$scope.topicId
            };

            this.apiService.executeCommand('SaveTopicCommand', command, () => {
                Core.Application.getInstance().navigateToUrl(Router.action("Topics", "Topic", { topicId: this.$scope.topicId }));
            });
        }
    }

    export interface IEditTopicPageData extends IPageData {
        topicId: number;
        text: string;
        tags: string;
        title: string;
    }

    export interface IEditTopicPageScope extends IPageScope {
        topicId: number;
        title: string;
        text: string;
        tags: string;
        attachments: number[];
        textConverted: string;
        showPreview: boolean;
        save(): void;
        switchPreviewMode(): void;
    }
}