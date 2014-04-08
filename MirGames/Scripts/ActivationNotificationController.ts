/// <reference path="_references.ts" />
module MirGames {
    export class ActivationNotificationController {
        static $inject = ['$scope', '$element', 'apiService'];

        constructor(private $scope: IActivationNotificationControllerScope, private $element: JQuery, private apiService: Core.IApiService) {
            this.$scope.sendNotification = () => this.sendNotification();
            this.$scope.notificationSent = false;
        }

        public sendNotification() {
            this.$scope.notificationSent = false;

            var command: MirGames.Domain.Users.Commands.ResendActivationCommand = {};
            this.apiService.executeCommand('ResendActivationCommand', command, () => {
                this.$scope.$apply(() => {
                    this.$scope.notificationSent = true;
                });
            });
        }
    }

    export interface IActivationNotificationControllerScope extends ng.IScope {
        sendNotification(): void;
        notificationSent: boolean;
    }
}