var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var MirGames;
(function (MirGames) {
    /// <reference path="../_references.ts" />
    (function (Topics) {
        var TopicPage = (function (_super) {
            __extends(TopicPage, _super);
            function TopicPage($scope, commandBus, eventBus, apiService) {
                _super.call(this, $scope, eventBus);
                this.commandBus = commandBus;
                this.apiService = apiService;
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

                $('.comment p').mouseup(function (e) {
                    var selection = window.getSelection();
                });

                this.loadTopicTitles();
            }
            Object.defineProperty(TopicPage.prototype, "commentsCount", {
                get: function () {
                    return parseInt($('body > nav > h1 .comments-count').text());
                },
                set: function (value) {
                    $('body > nav > h1 .comments-count').text(value.toString());
                },
                enumerable: true,
                configurable: true
            });

            TopicPage.prototype.reloadComment = function (commentId) {
                var query = {
                    CommentId: commentId
                };

                this.apiService.getOne('GetCommentByIdQuery', query, function (result) {
                    $('.comment[comment-id=' + commentId.toString() + '] > .text').html(result.Text);
                });
            };

            TopicPage.prototype.hideComment = function (commentId) {
                $('.comment[comment-id=' + commentId.toString() + ']').fadeOut();
            };

            TopicPage.prototype.showComment = function (commentId) {
                $('.comment[comment-id=' + commentId.toString() + ']').fadeOut(0).fadeIn();
            };

            TopicPage.prototype.loadTopicTitles = function () {
                var _this = this;
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
                    titles[topicId].link.click(function () {
                        return thisObj.scrollToTopic(topic);
                    });
                });

                if (titles.length > 0) {
                    this.$scrollBar = $('<div class="scrollbar">&nbsp;</div>');
                    $('.topic-info').append(this.$scrollBar);

                    $(window).scroll(function (ev) {
                        return _this.updateTopicScrollbar();
                    });
                    $(window).resize(function (ev) {
                        return _this.updateTopicScrollbar();
                    });
                    this.updateTopicScrollbar();
                }
            };

            TopicPage.prototype.scrollToTopic = function (topic) {
                $('html,body').animate({
                    scrollTop: topic.position
                }, 250);
            };

            TopicPage.prototype.updateTopicScrollbar = function () {
                var scrollTop = $(window).scrollTop();
                var height = $(window).innerHeight();
                var titles = this.titles;

                var scrollBarStart = null;
                var scrollBarEnd = null;

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
            };

            TopicPage.prototype.submitComment = function () {
                var _this = this;
                var command = {
                    Attachments: this.$scope.comment.attachments,
                    Text: this.$scope.comment.text,
                    TopicId: this.pageData.topicId
                };

                this.apiService.executeCommand('PostNewCommentCommand', command, function (commentId) {
                    var query = {
                        CommentId: commentId
                    };

                    _this.apiService.getOne('GetCommentByIdQuery', query, function (result) {
                        _this.commentAdded(result);
                        _this.scrollToBottom();
                    });
                });

                this.clearCommentForm();
                return false;
            };

            TopicPage.prototype.deleteTopic = function () {
                var command = new MirGames.Domain.DeleteTopicCommand();
                command.topicId = this.topicId;
                this.commandBus.executeCommand(Router.action("Topics", "DeleteTopic"), command, function (result) {
                    Core.Application.getInstance().navigateToUrl(Router.action("Topics", "Index"));
                });
            };

            TopicPage.prototype.getCommentForm = function () {
                return $('.comment-form form');
            };

            TopicPage.prototype.clearCommentForm = function () {
                this.$scope.comment.text = "";
            };

            TopicPage.prototype.commentAdded = function (comment) {
                this.commentsCount++;
                this.$scope.comments.push(comment);
                this.$scope.$apply();
                this.showComment(comment.Id);
            };
            TopicPage.$inject = ['$scope', 'commandBus', 'eventBus', 'apiService'];
            return TopicPage;
        })(MirGames.BasePage);
        Topics.TopicPage = TopicPage;
    })(MirGames.Topics || (MirGames.Topics = {}));
    var Topics = MirGames.Topics;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=Topic.js.map
