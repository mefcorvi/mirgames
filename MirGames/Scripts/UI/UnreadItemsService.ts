/// <reference path="../_references.ts" />
module MirGames {
    export interface IUnreadItemsService {
        getUnreadCount(): number;
        addHandler(callback: (unreadCount: number) => void): void;
        removeHandler(callback: (unreadCount: number) => void): void;
    }

    interface IUnreadItemsCallback {
        (unreadCount: number): void;
    }

    class UnreadItemsService implements IUnreadItemsService {
        static $inject = ['pageDataService', 'socketService', 'notificationService'];

        private unreadCount: number;
        private newNotificationHandler: Core.ISocketHandler;
        private removeNotificationHandler: Core.ISocketHandler;
        private enabled: boolean;
        private handlers: IUnreadItemsCallback[];

        constructor(private pageDataService: IPageDataProvider, private socketService: Core.ISocketService, private notificationService: UI.INotificationService) {
            this.unreadCount = this.pageDataService.getPageData<IPageData>().notificationsCount;
            this.enabled = false;
            this.handlers = [];
        }

        public addHandler(callback: IUnreadItemsCallback): void {
            this.enable();
            this.handlers.push(callback);
        }

        public removeHandler(callback: IUnreadItemsCallback): void {
            var index = this.handlers.indexOf(callback);

            if (index >= 0) {
                this.handlers.splice(index, 1);
            }

            if (this.handlers.length == 0) {
                this.disable();
            }
        }

        public getUnreadCount(): number {
            return this.unreadCount;
        }

        private enable(): void {
            if (this.enabled) {
                return;
            }

            this.newNotificationHandler = this.socketService.addHandler('eventsHub', 'NewNotification', (data: MirGames.Domain.Notifications.ViewModels.NotificationViewModel) => {
                this.notificationAdded(data);
            });

            this.removeNotificationHandler = this.socketService.addHandler('eventsHub', 'RemoveNotification', (data: MirGames.Domain.Notifications.ViewModels.NotificationViewModel) => {
                this.notificationRemoved(data);
            });

            this.enabled = true;
        }

        private disable(): void {
            if (this.enabled) {
                this.socketService.removeHandler(this.newNotificationHandler);
                this.socketService.removeHandler(this.removeNotificationHandler);
                this.enabled = false;
            }
        }

        private notificationAdded(data: MirGames.Domain.Notifications.ViewModels.NotificationViewModel): void {
            this.unreadCount++;
            this.notificationService.notifyEvent(true, true);
            this.onUnreadCountChanged();
        }

        private notificationRemoved(data: MirGames.Domain.Notifications.ViewModels.NotificationViewModel): void {
            this.unreadCount--;
            this.notificationService.notifyEvent(false, false);
            this.onUnreadCountChanged();

        }

        private onUnreadCountChanged() {
            this.handlers.forEach(item => {
                item(this.unreadCount);
            });
        }
    }

    angular
        .module('ui.unreadItems', [])
        .service('unreadItemsService', UnreadItemsService);
}