var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var MirGames;
(function (MirGames) {
    /// <reference path="../_references.ts" />
    (function (Wip) {
        var ProjectWorkItemPage = (function (_super) {
            __extends(ProjectWorkItemPage, _super);
            function ProjectWorkItemPage($scope, eventBus, apiService) {
                _super.call(this, $scope, eventBus);
                this.apiService = apiService;
                this.$scope.comment = this.getCommentForm();
                this.$scope.comments = [];
                this.$scope.commentsLoading = false;
            }
            ProjectWorkItemPage.prototype.getCommentForm = function () {
                var _this = this;
                return {
                    attachments: [],
                    focus: false,
                    post: function () {
                        return _this.postComment();
                    },
                    text: ''
                };
            };

            ProjectWorkItemPage.prototype.postComment = function () {
                var _this = this;
                var command = {
                    Attachments: this.$scope.comment.attachments,
                    Text: this.$scope.comment.text,
                    WorkItemId: this.pageData.workItemId
                };

                this.apiService.executeCommand('PostWorkItemCommentCommand', command, function (commentId) {
                    _this.$scope.$apply(function () {
                        _this.loadComment(commentId);
                        _this.$scope.comment = _this.getCommentForm();
                    });
                });
            };

            ProjectWorkItemPage.prototype.loadComment = function (commentId) {
                var _this = this;
                var query = {
                    CommentId: commentId
                };

                this.$scope.commentsLoading = true;

                this.apiService.getOne('GetProjectWorkItemCommentQuery', query, function (comment) {
                    _this.$scope.comments.push(comment);
                    _this.$scope.commentsLoading = false;
                    _this.$scope.$apply();
                });
            };
            ProjectWorkItemPage.$inject = ['$scope', 'eventBus', 'apiService'];
            return ProjectWorkItemPage;
        })(MirGames.BasePage);
        Wip.ProjectWorkItemPage = ProjectWorkItemPage;
    })(MirGames.Wip || (MirGames.Wip = {}));
    var Wip = MirGames.Wip;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=ProjectWorkItemPage.js.map
