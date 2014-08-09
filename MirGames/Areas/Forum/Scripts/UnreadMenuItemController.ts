
/// <reference path="_references.ts" />
module MirGames.Forum {
    export class UnreadMenuItemController {
        static $inject = ['$scope', '$element', 'pageData', 'socketService'];

        constructor(private $scope: IUnreadMenuItemControllerScope, $element: JQuery, private pageData: IPageData, socketService: Core.ISocketService) {
            this.$scope.goToUnread = (url, newWindow) => this.goToUnread(url, newWindow);
            this.$scope.unreadCount = pageData.forumTopicsUnreadCount;

            socketService.addHandler('eventsHub', 'NewNotification', (data: MirGames.Domain.Notifications.ViewModels.NotificationViewModel) => {
                console.log(data);
                if (data.NotificationType == 'Forum.NewAnswer' || data.NotificationType == 'Forum.NewTopic') {
                    $scope.$apply(() => {
                        $scope.unreadCount++;
                    });
                }
            });

            socketService.addHandler('eventsHub', 'RemoveNotification', (data: MirGames.Domain.Notifications.ViewModels.NotificationViewModel) => {
                console.log(data);
                if (data.NotificationType == 'Forum.NewAnswer' || data.NotificationType == 'Forum.NewTopic') {
                    $scope.$apply(() => {
                        $scope.unreadCount--;
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
        goToUnread: (url: string, newWindow: boolean) => void;
    }
}