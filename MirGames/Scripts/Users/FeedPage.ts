/// <reference path="../_references.ts" />
module MirGames.Users {
    export interface IFeedPageData {
        userId: number;
        blogId: number;
    }

    export interface IFeedPageScope extends IPageScope {
        markAllAsRead(): void;
        unreadItemsCount: number;
    }

    export class FeedPage extends MirGames.BasePage<IFeedPageData, IFeedPageScope> {
        static $inject = ['$scope', 'apiService', 'eventBus', 'unreadItemsService'];

        constructor($scope: IFeedPageScope,
            private apiService: Core.IApiService,
            eventBus: Core.IEventBus,
            private unreadItemsService: IUnreadItemsService) {
            super($scope, eventBus);

            $scope.markAllAsRead = () => this.markAllAsRead();
            $scope.unreadItemsCount = unreadItemsService.getUnreadCount();

            var unreadCountChanged = (unreadCount: number) => {
                this.$scope.unreadItemsCount = unreadCount;
                UI.safeDigest(this.$scope);
            }

            this.unreadItemsService.addHandler(unreadCountChanged);

            this.$scope.$on('$destroy', () => {
                this.unreadItemsService.removeHandler(unreadCountChanged);
            });
        }

        private markAllAsRead(): void {
            var command: MirGames.Domain.Notifications.Commands.MarkAllAsReadCommand = {
            };

            this.apiService.executeCommand('MarkAllAsReadCommand', command, () => {
                $('.notification-item').addClass('read');
            });
        }
    }
}