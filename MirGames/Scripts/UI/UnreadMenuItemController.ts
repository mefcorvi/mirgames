/// <reference path="../_references.ts" />
module MirGames {
    export class UnreadMenuItemController {
        static $inject = ['$scope', '$element', 'pageData', 'socketService', 'notificationService'];

        constructor(private $scope: IUnreadMenuItemControllerScope, $element: JQuery, private pageData: IPageData, socketService: Core.ISocketService, private notificationService: UI.INotificationService) {
            this.$scope.unreadCount = pageData.notificationsCount;

            socketService.addHandler('eventsHub', 'NewNotification', (data: MirGames.Domain.Notifications.ViewModels.NotificationViewModel) => {
                $scope.$apply(() => {
                    $scope.unreadCount++;
                    this.notificationService.notifyEvent(true, true);
                });
            });

            socketService.addHandler('eventsHub', 'RemoveNotification', (data: MirGames.Domain.Notifications.ViewModels.NotificationViewModel) => {
                $scope.$apply(() => {
                    $scope.unreadCount--;
                    this.notificationService.notifyEvent(false, false);
                });
            });
        }
    }

    export interface IUnreadMenuItemControllerScope extends ng.IScope {
        unreadCount: number;
    }
}