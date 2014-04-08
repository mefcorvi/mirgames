/// <reference path="../_references.ts" />
module MirGames.Users {
    export interface IProfilePageData {
        userId: number;
    }

    export class ProfilePage extends MirGames.BasePage<IProfilePageData, IProfilePageScope> {
        static $inject = ['$scope', 'commandBus', 'eventBus'];

        private userId: number;

        constructor($scope: IProfilePageScope, private commandBus: Core.ICommandBus, eventBus: Core.IEventBus) {
            super($scope, eventBus);
            this.userId = this.pageData.userId;

            $scope['delete'] = this.deleteUser.bind(this);
            $scope.switchUser = this.switchUser.bind(this);
            $scope.newRecord = {
                text: "",
                post: this.postWallRecord.bind(this)
            };
        }

        private switchUser(): void {
            var command = new Domain.LoginAsUserCommand();
            command.userId = this.userId;
            this.commandBus.executeCommand(Router.action("Account", "LoginAs"), command, (response) => {
                if (response.result == 0) {
                    Core.Application.getInstance().navigateToUrl(Router.action("Dashboard", "Index"));
                }
            });
        }

        private postWallRecord(): void {
            var command = new Domain.PostWallRecordCommand();
            command.userId = this.userId;
            command.text = this.$scope.newRecord.text;
            this.commandBus.executeCommand(Router.action("Users", "NewWallRecord"), command, (result) => {
                this.wallRecordAdded(result);
            });
        }

        private wallRecordAdded(recordHtml: string) {
            var $comment = $(recordHtml);
            $('.new-wall-record').after($comment);
            this.$scope.newRecord.text = "";
            this.$scope.$apply();
            $comment.fadeOut(0).fadeIn();
        }

        private deleteUser(): void {
            var command = new Domain.DeleteUserCommand();
            command.userId = this.userId;
            this.commandBus.executeCommand(Router.action("Users", "DeleteUser"), command, (result) => {
                Core.Application.getInstance().navigateToUrl(Router.action("Users", "Index"));
            });
        }
    }

    export interface IProfilePageScope extends IPageScope {
        delete(): void;
        switchUser(): void;
        newRecord: {
            text: string;
            post(): void;
        }
    }
}