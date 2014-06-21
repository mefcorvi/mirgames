var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var MirGames;
(function (MirGames) {
    /// <reference path="_references.ts" />
    (function (Forum) {
        var TopicPage = (function (_super) {
            __extends(TopicPage, _super);
            function TopicPage($scope, commandBus, eventBus, apiService, $compile, $rootScope) {
                var _this = this;
                _super.call(this, $scope, eventBus);
                this.commandBus = commandBus;
                this.apiService = apiService;
                this.$compile = $compile;
                this.$rootScope = $rootScope;

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
                    focus: false
                };

                $scope.hidePost = this.hidePost.bind(this);
                $scope.reloadPost = this.reloadPost.bind(this);

                this.$scope.focusAnswer = this.focusAnswer.bind(this);

                $(window).hashchange({
                    hash: '#new-answer',
                    onSet: function () {
                        _this.$scope.focusAnswer();
                    }
                });

                if (this.pageData.currentUser && this.pageData.currentUser.Settings.ForumContiniousPagination) {
                    $('body > section').bind('scroll', function () {
                        if (_this.isScrollBottom()) {
                            if (_this.$scope.nextPage < _this.$scope.pagesCount) {
                                _this.loadAnswers();
                            }
                        }
                    });
                }
            }
            TopicPage.prototype.loadAnswers = function () {
                var _this = this;
                if (this.$scope.answersLoading) {
                    return;
                }

                this.$scope.answersLoading = true;
                var query = {
                    TopicId: this.pageData.topicId,
                    LoadStartPost: false
                };
                this.apiService.getAll('GetForumTopicPostsQuery', query, this.$scope.nextPage, this.$scope.pageSize, function (result) {
                    Enumerable.from(result).forEach(function (item) {
                        return _this.$scope.posts.push(item);
                    });
                    _this.$scope.nextPage++;
                    _this.$scope.answersLoading = false;
                    _this.$scope.$apply();
                }, false);
            };

            TopicPage.prototype.reloadPost = function (postId, withTopic) {
                if (withTopic) {
                    var topicQuery = {
                        TopicId: this.pageData.topicId
                    };

                    this.apiService.getOne('GetForumTopicQuery', topicQuery, function (result) {
                        $('section > header > h3').text(result.Title);
                    });
                }

                var query = {
                    PostId: postId
                };

                this.apiService.getOne('GetForumPostQuery', query, function (result) {
                    $('article[post-id=' + postId.toString() + '] .text').html(result.Text);
                });
            };

            TopicPage.prototype.hidePost = function (postId) {
                $('article[post-id=' + postId.toString() + ']').fadeOut();
            };

            TopicPage.prototype.showPost = function (postId) {
                $('article[post-id=' + postId.toString() + ']').fadeOut(0).fadeIn();
            };

            TopicPage.prototype.focusAnswer = function () {
                var _this = this;
                if (this.pageData.page != this.pageData.pagesCount) {
                    return;
                }

                setTimeout(function () {
                    _this.$scope.reply.focus = true;
                    _this.$scope.$apply();
                }, 100);
            };

            TopicPage.prototype.getFirstUnread = function () {
                return $('.topic-posts #first-unread');
            };

            TopicPage.prototype.goToFirstUnread = function () {
                var unreadPost = this.getFirstUnread();

                if (unreadPost.length > 0) {
                    this.scrollToItem(unreadPost, 100);
                }
            };

            TopicPage.prototype.markPostsAsRead = function () {
                $('.topic-posts .message.unread').removeClass('unread');
            };

            TopicPage.prototype.returnBack = function () {
                window.location.href = Router.action('Forum', 'Index');
            };

            TopicPage.prototype.reply = function () {
                var _this = this;
                if (this.$scope.postAnswerForm.$invalid) {
                    return;
                }

                this.markPostsAsRead();

                var command = {
                    Attachments: this.$scope.reply.attachments,
                    Text: this.$scope.reply.text,
                    TopicId: this.$scope.reply.topicId
                };

                this.apiService.executeCommand('ReplyForumTopicCommand', command, function (result) {
                    var query = {
                        PostId: result
                    };
                    _this.apiService.getOne('GetForumPostQuery', query, function (result) {
                        _this.$scope.posts.push(result);
                        _this.$scope.reply.text = "";
                        _this.$scope.reply.attachments = [];
                        _this.$scope.$apply();
                        _this.showPost(result.PostId);
                        _this.scrollToBottom();
                    });
                });
            };
            TopicPage.$inject = ['$scope', 'commandBus', 'eventBus', 'apiService', '$compile', '$rootScope'];
            return TopicPage;
        })(MirGames.BasePage);
        Forum.TopicPage = TopicPage;
    })(MirGames.Forum || (MirGames.Forum = {}));
    var Forum = MirGames.Forum;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=TopicPage.js.map
