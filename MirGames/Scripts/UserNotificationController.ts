/// <reference path="_references.ts" />
module MirGames {
    export class UserNotificationController {
        static $inject = ['$scope', '$element', 'eventBus', '$timeout'];
        private timeout: number;

        constructor(private $scope: IUserNotificationControllerScope, private $element: JQuery, private eventBus: Core.IEventBus, private $timeout: ng.ITimeoutService) {
            this.$scope.notifications = [];

            eventBus.on('user.notification', (arg: string) => this.onUserNotification(arg));
        }

        private onUserNotification(notification: string): void {
            this.$scope.notifications.push({ isHiding: false, text: notification });
            this.$scope.$apply();

            $('body').one('mousemove', null, null, ev => {
                this.hideNotification(notification);
            });
        }

        private hideNotification(notification: string): void {
            this.$timeout(() => {
                Enumerable.from(this.$scope.notifications).where(n => n.text == notification).forEach(item => item.isHiding = true);

                this.$timeout(() => {
                    this.$scope.notifications = Enumerable.from(this.$scope.notifications).where(n => n.text != notification).toArray();
                }, 1000);
            }, 1000);
        }
    }

    export interface IUserNotification {
        isHiding: boolean;
        text: string;
    }

    export interface IUserNotificationControllerScope extends UI.IAppScope {
        notifications: IUserNotification[];
    }
}