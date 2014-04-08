var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var MirGames;
(function (MirGames) {
    /// <reference path="../_references.ts" />
    (function (Users) {
        var ProfilePage = (function (_super) {
            __extends(ProfilePage, _super);
            function ProfilePage($scope, commandBus, eventBus) {
                _super.call(this, $scope, eventBus);
                this.commandBus = commandBus;
                this.userId = this.pageData.userId;

                $scope['delete'] = this.deleteUser.bind(this);
                $scope.switchUser = this.switchUser.bind(this);
                $scope.newRecord = {
                    text: "",
                    post: this.postWallRecord.bind(this)
                };
            }
            ProfilePage.prototype.switchUser = function () {
                var command = new MirGames.Domain.LoginAsUserCommand();
                command.userId = this.userId;
                this.commandBus.executeCommand(Router.action("Account", "LoginAs"), command, function (response) {
                    if (response.result == 0) {
                        Core.Application.getInstance().navigateToUrl(Router.action("Dashboard", "Index"));
                    }
                });
            };

            ProfilePage.prototype.postWallRecord = function () {
                var _this = this;
                var command = new MirGames.Domain.PostWallRecordCommand();
                command.userId = this.userId;
                command.text = this.$scope.newRecord.text;
                this.commandBus.executeCommand(Router.action("Users", "NewWallRecord"), command, function (result) {
                    _this.wallRecordAdded(result);
                });
            };

            ProfilePage.prototype.wallRecordAdded = function (recordHtml) {
                var $comment = $(recordHtml);
                $('.new-wall-record').after($comment);
                this.$scope.newRecord.text = "";
                this.$scope.$apply();
                $comment.fadeOut(0).fadeIn();
            };

            ProfilePage.prototype.deleteUser = function () {
                var command = new MirGames.Domain.DeleteUserCommand();
                command.userId = this.userId;
                this.commandBus.executeCommand(Router.action("Users", "DeleteUser"), command, function (result) {
                    Core.Application.getInstance().navigateToUrl(Router.action("Users", "Index"));
                });
            };
            ProfilePage.$inject = ['$scope', 'commandBus', 'eventBus'];
            return ProfilePage;
        })(MirGames.BasePage);
        Users.ProfilePage = ProfilePage;
    })(MirGames.Users || (MirGames.Users = {}));
    var Users = MirGames.Users;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=ProfilePage.js.map
