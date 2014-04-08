/// <reference path="_references.ts" />
module MirGames {
    export class SocketNotificationController {
        static $inject = ['$scope', '$element', 'eventBus'];

        constructor(private $scope: ISocketNotificationScope, private $element: JQuery, private eventBus: Core.IEventBus) {
            eventBus.addListener('socket.connecting', () => this.setState('connecting'));
            eventBus.addListener('socket.reconnecting', () => this.setState('reconnecting'));
            eventBus.addListener('socket.connected', () => this.setState('connected'));
            eventBus.addListener('socket.disconnected', () => this.setState('disconnected'));
            eventBus.addListener('socket.connection-slow', () => this.setState('connection-slow'));
        }

        private setState(state: string): void {
            this.$scope.state = state;
            this.$scope.safeApply();
        }
    }

    export interface ISocketNotificationScope extends UI.IAppScope {
        state: string;
    }
}