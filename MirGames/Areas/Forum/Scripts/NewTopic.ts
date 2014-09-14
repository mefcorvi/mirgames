/// <reference path="_references.ts" />
module MirGames.Forum {

    export class NewTopicPage extends MirGames.BasePage<INewTopicPageData, INewTopicPageScope> {
        static $inject = ['$scope', '$element', 'eventBus', 'apiService'];

        constructor($scope: INewTopicPageScope, private $element: JQuery, eventBus: Core.IEventBus, private apiService: Core.IApiService) {
            super($scope, eventBus);
            this.$scope.forumAlias = this.pageData.forumAlias;
            this.$scope.post = this.submit.bind(this);
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
                    var command: MirGames.Domain.Forum.Queries.GetForumTagsQuery = {
                        Filter: query
                    };

                    this.apiService.getAll('GetForumTagsQuery', command, 1, 30, (result: string[]) => {
                        var items = Enumerable.from(result).select(r => {
                            return { text: r, value: r };
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
            var command: MirGames.Domain.Forum.Commands.PostNewForumTopicCommand = {
                Attachments: this.$scope.attachments,
                ForumAlias: this.$scope.forumAlias,
                Tags: this.$scope.tags,
                Text: this.$scope.text,
                Title: this.$scope.title
            };

            this.apiService.executeCommand('PostNewForumTopicCommand', command, (result: number) => {
                var url = Router.action("Forum", "Topic", { topicId: result, area: 'Forum', forumAlias: this.$scope.forumAlias });
                Core.Application.getInstance().navigateToUrl(url);
            });
        }
    }

    export interface INewTopicPageData extends IPageData {
        forumAlias: string;
    }

    export interface INewTopicPageScope extends IPageScope {
        text: string;
        showPreview: boolean;
        title: string;
        tags: string;
        forumAlias: string;
        post(): void;
        attachments: number[];
        isTitleFocused: boolean;
        switchPreviewMode(): void;
    }
}