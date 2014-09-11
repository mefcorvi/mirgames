/// <reference path="_references.ts" />
module MirGames.Forum {

    export class NewTopicPage {
        static $inject = ['$scope', '$element', 'commandBus', 'eventBus', 'apiService'];

        constructor(private $scope: INewTopicPageScope, private $element: JQuery, private commandBus: Core.ICommandBus, private eventBus: Core.IEventBus, private apiService: Core.IApiService) {
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