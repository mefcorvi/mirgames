/// <reference path="_references.ts" />
module MirGames.Forum {
    export class TopicPage extends MirGames.BasePage<ITopicPageData, ITopicPageScope> {
        static $inject = ['$scope', 'commandBus', 'eventBus', 'apiService', '$compile', '$rootScope'];

        constructor($scope: ITopicPageScope, private commandBus: Core.ICommandBus, eventBus: Core.IEventBus, private apiService: Core.IApiService, private $compile: ng.ICompileService, private $rootScope: ng.IRootScopeService) {
            super($scope, eventBus);

            $scope.posts = [];
            $scope.nextPage = this.pageData.page;
            $scope.pageSize = this.pageData.pageSize;
            $scope.pagesCount = this.pageData.pagesCount;
            $scope.answersLoading = false;
            $scope.returnBack = this.returnBack.bind(this);

            $scope.reply = {
                text: "",
                attachments: [],
                topicId: this.pageData.topicId,
                post: this.reply.bind(this),
                focus: false,
                caret: 0
            };

            $scope.hidePost = this.hidePost.bind(this);
            $scope.reloadPost = this.reloadPost.bind(this);
            $scope.addMention = login => this.addMention(login);

            this.$scope.focusAnswer = this.focusAnswer.bind(this);

            $(window).hashchange({
                hash: '#new-answer',
                onSet: () => {
                    this.$scope.focusAnswer();
                }
            });

            if (this.pageData.currentUser && this.pageData.currentUser.Settings.ForumContiniousPagination) {
                $(window).scroll(() => {
                    if (this.isScrollBottom(500)) {
                        if (this.$scope.nextPage < this.$scope.pagesCount) {
                            this.loadAnswers();
                            this.$scope.$apply();
                        }
                    }
                });
            }
        }

        private loadAnswers() {
            if (this.$scope.answersLoading) {
                return;
            }

            this.$scope.answersLoading = true;
            var query: MirGames.Domain.Forum.Queries.GetForumTopicPostsQuery = {
                TopicId: this.pageData.topicId,
                LoadStartPost: false
            };
            this.apiService.getAll('GetForumTopicPostsQuery', query, this.$scope.nextPage, this.$scope.pageSize, (result: MirGames.Domain.Forum.ViewModels.ForumPostViewModel[]) => {
                Enumerable.from(result).forEach(item => this.$scope.posts.push(item));
                this.$scope.nextPage++;
                this.$scope.answersLoading = false;
                this.$scope.$apply();
            }, false);
        }

        private reloadPost(postId: number, withTopic: boolean): void {
            if (withTopic) {
                var topicQuery: MirGames.Domain.Forum.Queries.GetForumTopicQuery = {
                    TopicId: this.pageData.topicId
                };

                this.apiService.getOne('GetForumTopicQuery', topicQuery, (result: MirGames.Domain.Forum.ViewModels.ForumTopicViewModel) => {
                    $('section > header > h3').text(result.Title);
                });
            }

            var query: MirGames.Domain.Forum.Queries.GetForumPostQuery = {
                PostId: postId
            };

            this.apiService.getOne('GetForumPostQuery', query, (result: MirGames.Domain.Forum.ViewModels.ForumPostsListItemViewModel) => {
                $('article[post-id=' + postId.toString() + '] .text').html(result.Text);
            });
        }

        private hidePost(postId: number): void {
            $('article[post-id=' + postId.toString() + ']').fadeOut();
        }

        private showPost(postId: number): void {
            $('article[post-id=' + postId.toString() + ']').fadeOut(0).fadeIn();
        }

        private focusAnswer(): void {
            if (this.pageData.page != this.pageData.pagesCount) {
                return;
            }

            setTimeout(() => {
                this.$scope.reply.focus = true;
                this.$scope.$apply();
            }, 100);
        }

        private getFirstUnread(): JQuery {
            return $('.topic-posts #first-unread');
        }

        private goToFirstUnread(): void {
            var unreadPost = this.getFirstUnread();

            if (unreadPost.length > 0) {
                this.scrollToItem(unreadPost, 100);
            }
        }

        private markPostsAsRead(): void {
            $('.topic-posts .message.unread').removeClass('unread');
        }

        private returnBack(): void {
            window.location.href = Router.action('Forum', 'Index');
        }

        private reply(): void {
            if (this.$scope.postAnswerForm.$invalid) {
                return;
            }

            this.markPostsAsRead();

            var command: MirGames.Domain.Forum.Commands.ReplyForumTopicCommand = {
                Attachments: this.$scope.reply.attachments,
                Text: this.$scope.reply.text,
                TopicId: this.$scope.reply.topicId
            };

            this.apiService.executeCommand('ReplyForumTopicCommand', command, (result: number) => {
                var query: MirGames.Domain.Forum.Queries.GetForumPostQuery = {
                    PostId: result
                };
                this.apiService.getOne('GetForumPostQuery', query, (result: MirGames.Domain.Forum.ViewModels.ForumPostViewModel) => {
                    this.$scope.posts.push(result);
                    this.$scope.reply.text = "";
                    this.$scope.reply.attachments = [];
                    this.$scope.$apply();
                    this.showPost(result.PostId);
                    this.scrollToBottom();
                });
            });
        }

        private addMention(login: string): void {
            if (this.$scope.reply.text.length > 0) {
                this.$scope.reply.text += "\r\n\r\n";
            }

            this.$scope.reply.text += '**' + login + '**  \r\n';
            this.$scope.reply.focus = true;
            this.$scope.reply.caret = this.$scope.reply.text.length;
            this.scrollToBottom();
        }
    }

    export interface ITopicPageData extends IPageData {
        topicId: number;
        pagesCount: number;
        page: number;
        pageSize: number;
    }

    export interface ITopicPageScope extends IPageScope {
        reply: {
            text: string;
            topicId: number;
            attachments: number[];
            post(): void;
            focus: boolean;
            caret: number;
        };
        posts: MirGames.Domain.Forum.ViewModels.ForumPostViewModel[];
        hidePost(postId: number): void;
        nextPage: number;
        pageSize: number;
        pagesCount: number;
        reloadPost(postId: number, withTopic: boolean): void;
        focusAnswer: () => void;
        answersLoading: boolean;
        returnBack: () => void;
        postAnswerForm: ng.IFormController;
        addMention: (login: string) => void;
}
}