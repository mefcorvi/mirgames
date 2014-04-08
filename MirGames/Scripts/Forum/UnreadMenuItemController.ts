
/// <reference path="../_references.ts" />
module MirGames.Forum {
    export class UnreadMenuItemController {
        static $inject = ['$scope', '$element', 'pageData', 'socketService'];

        constructor(private $scope: IUnreadMenuItemControllerScope, $element: JQuery, private pageData: IPageData, socketService: Core.ISocketService) {
            this.$scope.goToUnread = this.goToUnread.bind(this);
            this.$scope.unreadCount = pageData.forumTopicsUnreadCount;

            socketService.addHandler('eventsHub', 'NewTopicUnread', () => {
                $scope.$apply(() => {
                    $scope.unreadCount++;
                });
            });

            socketService.addHandler('eventsHub', 'NewTopic', (ev?: { TopicId: number; AuthorId: number; }) => {
                if (this.pageData.currentUser && this.pageData.currentUser.Id != ev.AuthorId) {
                    $scope.$apply(() => {
                        $scope.unreadCount++;
                    });
                }
            });

            socketService.addHandler('eventsHub', 'ForumTopicRead', () => {
                $scope.$apply(() => {
                    $scope.unreadCount--;
                });
            });
        }

        private goToUnread(newWindow: boolean) {
            Core.Application.getInstance().navigateToUrl(Router.action("Forum", "Unread"), newWindow);
        }
    }

    export interface IUnreadMenuItemControllerScope extends ng.IScope {
        unreadCount: number;
        goToUnread: (newWindow: boolean) => void;
    }
}