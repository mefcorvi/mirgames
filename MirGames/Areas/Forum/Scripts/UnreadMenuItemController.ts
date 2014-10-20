
/// <reference path="_references.ts" />
module MirGames.Forum {
    export class UnreadMenuItemController {
        static $inject = ['$scope', '$element', 'pageData', 'socketService', 'notificationService'];

        constructor(private $scope: IUnreadMenuItemControllerScope, $element: JQuery, private pageData: IPageData, socketService: Core.ISocketService, private notificationService: UI.INotificationService) {
            this.$scope.unreadCount = pageData.forumTopicsUnreadCount;

            socketService.addHandler('eventsHub', 'NewNotification', (data: MirGames.Domain.Notifications.ViewModels.NotificationViewModel) => {
                if (data.NotificationType == 'Forum.NewAnswer' || data.NotificationType == 'Forum.NewTopic') {
                    $scope.$apply(() => {
                        $scope.unreadCount++;
                        this.notificationService.notifyEvent(true, true);
                    });
                }
            });

            socketService.addHandler('eventsHub', 'RemoveNotification', (data: MirGames.Domain.Notifications.ViewModels.NotificationViewModel) => {
                if (data.NotificationType == 'Forum.NewAnswer' || data.NotificationType == 'Forum.NewTopic') {
                    $scope.$apply(() => {
                        $scope.unreadCount--;
                        this.notificationService.notifyEvent(false, false);
                    });
                }
            });
        }

        private goToUnread(url: string, newWindow: boolean) {
            Core.Application.getInstance().navigateToUrl(url, newWindow);
        }
    }

    export interface IUnreadMenuItemControllerScope extends ng.IScope {
        unreadCount: number;
    }
}