/// <reference path="../_references.ts" />
module MirGames.Users {
    export interface IFeedPageData {
        userId: number;
        blogId: number;
    }

    export interface IFeedPageScope extends IPageScope {
        markAllAsRead(): void;
    }

    export class FeedPage extends MirGames.BasePage<IFeedPageData, IFeedPageScope> {
        static $inject = ['$scope', 'apiService', 'eventBus'];

        constructor($scope: IFeedPageScope, private apiService: Core.IApiService, eventBus: Core.IEventBus) {
            super($scope, eventBus);

            $scope.markAllAsRead = () => this.markAllAsRead();
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