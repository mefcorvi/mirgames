/// <reference path="_references.ts" />
module MirGames {
    export class SocketNotificationController {
        static $inject = ['$scope', '$element', 'eventBus'];

        constructor(private $scope: ISocketNotificationScope, private $element: JQuery, private eventBus: Core.IEventBus) {
            eventBus.on('socket.connecting.socketNotifications', () => this.setState('connecting'));
            eventBus.on('socket.reconnecting.socketNotifications', () => this.setState('reconnecting'));
            eventBus.on('socket.connected.socketNotifications', () => this.setState('connected'));
            eventBus.on('socket.disconnected.socketNotifications', () => this.setState('disconnected'));
            eventBus.on('socket.connection-slow.socketNotifications', () => this.setState('connection-slow'));

            $scope.$on('$destroy', () => {
                eventBus.removeAllListeners('socket.connecting.socketNotifications');
                eventBus.removeAllListeners('socket.reconnecting.socketNotifications');
                eventBus.removeAllListeners('socket.connected.socketNotifications');
                eventBus.removeAllListeners('socket.disconnected.socketNotifications');
                eventBus.removeAllListeners('socket.connection-slow.socketNotifications');
            });
        }

        private setState(state: string): void {
            this.$scope.state = state;
            UI.safeDigest(this.$scope);
        }
    }

    export interface ISocketNotificationScope extends UI.IAppScope {
        state: string;
    }
}