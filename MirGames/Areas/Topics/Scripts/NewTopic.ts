/// <reference path="_references.ts" />
module MirGames.Topics {

    export class NewTopicPage {
        static $inject = ['$scope', '$element', 'eventBus', 'apiService'];

        constructor(private $scope: INewTopicPageScope, private $element: JQuery, private eventBus: Core.IEventBus, private apiService: Core.IApiService) {
            this.$scope.save = this.submit.bind(this);
            this.$scope.attachments = [];

            this.$scope.switchPreviewMode = () => {
                this.$scope.showPreview = !this.$scope.showPreview;
            };
            this.$scope.isTitleFocused = true;
            this.initializeTags();
        }

        private initializeTags() {
            $('.topic-tags').selectize({
                load: (query, callback) => {
                    var command: MirGames.Domain.Topics.Queries.GetMainTagsQuery = {
                        Filter: query,
                        IsMicroTopic: null,
                        IsTutorial: null,
                        ShowOnMain: null
                    };

                    this.apiService.getAll('GetMainTagsQuery', command, 0, 30, (result: MirGames.Domain.Topics.ViewModels.TagViewModel[]) => {
                        var items = Enumerable.from(result).select(r => {
                            return { text: r.Tag, value: r.Tag };
                        }).toArray();

                        callback(items);
                    });
                },
                create: (input) => {
                    return { text: input, value: input }
                }
            });
        }

        private submit() {
            var command: MirGames.Domain.Topics.Commands.AddNewTopicCommand = {
                Attachments: this.$scope.attachments,
                BlogId: null,
                Tags: this.$scope.tags,
                Text: this.$scope.text,
                Title: this.$scope.title,
                IsRepost: this.$scope.isRepost,
                IsTutorial: this.$scope.isTutorial,
                SourceAuthor: this.$scope.sourceAuthor,
                SourceLink: this.$scope.sourceLink
            };

            this.apiService.executeCommand('AddNewTopicCommand', command, (topicId: number) => {
                Core.Application.getInstance().navigateToUrl(Router.action("Topics", "Topic", { topicId: topicId, area: 'Topics' }));
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
        isRepost: boolean;
        isTutorial: boolean;
        sourceAuthor: string;
        sourceLink: string;
    }
}