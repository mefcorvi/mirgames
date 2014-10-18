/// <reference path="../_references.ts" />
module MirGames.Users {
    export interface IProfilePageData {
        userId: number;
    }

    export class ProfilePage extends MirGames.BasePage<IProfilePageData, IProfilePageScope> {
        static $inject = ['$scope', 'apiService', 'eventBus'];

        private userId: number;

        constructor($scope: IProfilePageScope, private apiService: Core.IApiService, eventBus: Core.IEventBus) {
            super($scope, eventBus);
            this.userId = this.pageData.userId;

            $scope.deleteUser = this.deleteUser.bind(this);
            $scope.switchUser = this.switchUser.bind(this);
            $scope.newRecord = {
                text: "",
                post: this.postWallRecord.bind(this)
            };
        }

        private switchUser(): void {
            var command: MirGames.Domain.Users.Commands.LoginAsUserCommand = {
                UserId: this.userId
            };

            this.apiService.executeCommand('LoginAsUserCommand', command, (response) => {
                if (response != null) {
                    $.cookie('key', response, {
                        expires: 365 * 24 * 60 * 60,
                        path: '/'
                    });
                    Core.Application.getInstance().navigateToUrl(Router.action("Dashboard", "Index"));
                }
            });
        }

        private postWallRecord(): void {
            /*var command = new Domain.PostWallRecordCommand();
            command.userId = this.userId;
            command.text = this.$scope.newRecord.text;
            this.commandBus.executeCommand(Router.action("Users", "NewWallRecord"), command, (result) => {
                this.wallRecordAdded(result);
            });*/
        }

        private wallRecordAdded(recordHtml: string) {
            var $comment = $(recordHtml);
            $('.new-wall-record').after($comment);
            this.$scope.newRecord.text = "";
            this.$scope.$apply();
            $comment.fadeOut(0).fadeIn();
        }

        private deleteUser(): void {
            var command: MirGames.Domain.Users.Commands.DeleteUserCommand = {
                UserId: this.userId
            };
            this.apiService.executeCommand('DeleteUserCommand', command, () => {
                Core.Application.getInstance().navigateToUrl(Router.action("Users", "Index"));
            });
        }
    }

    export interface IProfilePageScope extends IPageScope {
        deleteUser(): void;
        switchUser(): void;
        newRecord: {
            text: string;
            post(): void;
        }
    }
}