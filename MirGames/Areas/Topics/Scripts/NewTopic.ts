/// <reference path="_references.ts" />
module MirGames.Topics {

    export class NewTopicPage {
        static $inject = ['$scope', '$element', 'commandBus', 'eventBus', 'apiService'];

        constructor(private $scope: INewTopicPageScope, private $element: JQuery, private commandBus: Core.ICommandBus, private eventBus: Core.IEventBus, private apiService: Core.IApiService) {
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
                        Filter: query
                    };

                    this.apiService.getAll('GetMainTagsQuery', command, 1, 30, (result: string[]) => {
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