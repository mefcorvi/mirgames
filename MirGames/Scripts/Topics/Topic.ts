/// <reference path="../_references.ts" />
module MirGames.Topics {
    export interface ITopicPageData {
        topicId: number;
    }

    export class TopicPage extends MirGames.BasePage<ITopicPageData, ITopicScope> {
        static $inject = ['$scope', 'commandBus', 'eventBus', 'apiService'];

        private topicId: number;
        private titles: IPageTitle[];
        private $scrollBar: JQuery;
        private postsCount: number;
        private page: number;
        private pageSize: number;

        constructor($scope: ITopicScope, private commandBus: Core.ICommandBus, eventBus: Core.IEventBus, private apiService: Core.IApiService) {
            super($scope, eventBus);
            this.topicId = this.pageData.topicId;

            $scope.comment = {
                attachments: [],
                text: '',
                isFocused: false
            };
            $scope.comments = [];
            $scope.addComment = this.submitComment.bind(this);
            $scope.deleteTopic = this.deleteTopic.bind(this);
            $scope.reloadComment = this.reloadComment.bind(this);
            $scope.hideComment = this.hideComment.bind(this);

            $('.comment p').mouseup((e) => {
                var selection = window.getSelection();
            });

            this.loadTopicTitles();
        }

        get commentsCount(): number {
            return parseInt($('body > nav > h1 .comments-count').text());
        }
        set commentsCount(value: number) {
            $('body > nav > h1 .comments-count').text(value.toString());
        }

        private reloadComment(commentId: number) {
            var query: MirGames.Domain.Topics.Queries.GetCommentByIdQuery = {
                CommentId: commentId
            };

            this.apiService.getOne('GetCommentByIdQuery', query, (result: MirGames.Domain.Topics.ViewModels.CommentViewModel) => {
                $('.comment[comment-id=' + commentId.toString() + '] > .text').html(result.Text);
            });
        }

        private hideComment(commentId: number) {
            $('.comment[comment-id=' + commentId.toString() + ']').fadeOut();
        }

        private showComment(commentId: number) {
            $('.comment[comment-id=' + commentId.toString() + ']').fadeOut(0).fadeIn();
        }

        private loadTopicTitles() {
            var titles = this.titles = [];
            var thisObj = this;

            $('.topic-text h1, .topic-text h2, .topic-text h3, .topic-text h4, .topic-text h5, .topic-text h6').each(function () {
                var topicId = titles.length;
                var topic = {
                    link: $('<a href="javascript:void(0);"></a>'),
                    name: $(this).attr('id', 'title-' + topicId).text(),
                    position: $(this).position().top
                };

                titles[topicId] = topic;
                $('.topic-info').append(topic.link);
                titles[topicId].link.text(topic.name);
                titles[topicId].link.click(() => thisObj.scrollToTopic(topic));
            });

            if (titles.length > 0) {
                this.$scrollBar = $('<div class="scrollbar">&nbsp;</div>');
                $('.topic-info').append(this.$scrollBar);

                $(window).scroll((ev: JQueryEventObject) => this.updateTopicScrollbar());
                $(window).resize((ev: JQueryEventObject) => this.updateTopicScrollbar());
                this.updateTopicScrollbar();
            }
        }

        private scrollToTopic(topic: IPageTitle) {
            $('html,body').animate({
                scrollTop: topic.position
            }, 250);
        }

        private updateTopicScrollbar() {
            var scrollTop = $(window).scrollTop();
            var height = $(window).innerHeight();
            var titles = this.titles;

            var scrollBarStart: number = null;
            var scrollBarEnd: number = null;

            for (var i = 0; i < titles.length; i++) {
                var item = titles[i];
                var nextItemPosition = titles[i + 1] ? titles[i + 1].position : $('#comments').position().top;
                var itemHeight = nextItemPosition - item.position;

                var shownTopPart = (scrollTop - item.position) / itemHeight;
                var shownBottomPart = (scrollTop + height - item.position) / itemHeight;

                if (shownTopPart >= 0 && shownTopPart < 1) {
                    scrollBarStart = item.link.position().top + Math.round(item.link.outerHeight() * shownTopPart);
                }

                if (shownBottomPart > 0 && shownBottomPart <= 1) {
                    scrollBarEnd = item.link.position().top + Math.round(item.link.outerHeight() * shownBottomPart);
                }
            }

            if (scrollBarStart === null && scrollTop < titles[0].position) {
                scrollBarStart = titles[0].link.position().top;
            }

            if (scrollBarEnd === null && (scrollTop + height) > titles[titles.length - 1].position) {
                var lastTitle = titles[titles.length - 1];
                scrollBarEnd = lastTitle.link.position().top + lastTitle.link.outerHeight();
            }

            if (scrollBarStart !== null && scrollBarEnd > scrollBarStart) {
                this.$scrollBar.css({
                    top: scrollBarStart,
                    height: scrollBarEnd - scrollBarStart
                }).show();
            } else {
                this.$scrollBar.hide();
            }
        }

        private submitComment(): boolean {
            var command: MirGames.Domain.Topics.Commands.PostNewCommentCommand = {
                Attachments: this.$scope.comment.attachments,
                Text: this.$scope.comment.text,
                TopicId: this.pageData.topicId
            };

            this.apiService.executeCommand('PostNewCommentCommand', command, (commentId: number) => {
                var query: MirGames.Domain.Topics.Queries.GetCommentByIdQuery = {
                    CommentId: commentId
                };

                this.apiService.getOne('GetCommentByIdQuery', query, (result: MirGames.Domain.Topics.ViewModels.CommentViewModel) => {
                    this.commentAdded(result);
                    this.scrollToBottom();
                });
            });

            this.clearCommentForm();
            return false;
        }

        private deleteTopic() {
            var command = new Domain.DeleteTopicCommand();
            command.topicId = this.topicId;
            this.commandBus.executeCommand(Router.action("Topics", "DeleteTopic"),  command, (result) => {
                Core.Application.getInstance().navigateToUrl(Router.action("Topics", "Index"));
            });
        }

        private getCommentForm(): JQuery {
            return $('.comment-form form');
        }

        private clearCommentForm() {
            this.$scope.comment.text = "";
        }

        private commentAdded(comment: MirGames.Domain.Topics.ViewModels.CommentViewModel) {
            this.commentsCount++;
            this.$scope.comments.push(comment);
            this.$scope.$apply();
            this.showComment(comment.Id);
        }
    }

    export interface ITopicScope extends IPageScope {
        comment: {
            text: string;
            attachments: number[];
            isFocused: boolean;
        };
        comments: MirGames.Domain.Topics.ViewModels.CommentViewModel[];
        addComment: () => void;
        deleteTopic: () => void;
        reloadComment(commentId: number): void;
        hideComment(commentId: number): void;
    }

    interface IPageTitle {
        link: JQuery;
        name: string;
        position: number;
    }
}