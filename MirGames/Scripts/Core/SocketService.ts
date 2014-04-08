module Core {
    export interface ISocketService {
        addHandler(hub: string, messageType: string, callback: (...msg: any[]) => void): void;
        executeCommand(commandType: string, command: any): void;
    }

    class SocketService implements ISocketService {
        private static isConnected: boolean;
        private static connection: HubConnection;
        private static connectionPromise: JQueryPromise<any>;
        private static proxies: { [hub: string]: HubProxy } = {};

        constructor(private eventBus: IEventBus) {
        }

        public addHandler(hub: string, messageType: string, callback: (...msg: any[]) => void): void {
            this.ensureConnection();

            var hubProxy = this.getProxy(hub);
            hubProxy.on(messageType, callback);

            this.start();
        }

        public executeCommand(commandType: string, command: any): void {
            command.type = commandType;

            var hubProxy = this.getProxy("CommandsHub");

            this.start(connection => {
                hubProxy.invoke('Execute', JSON.stringify(command))
            });
        }

        private getProxy(hub: string): HubProxy {
            if (!SocketService.proxies[hub]) {
                SocketService.proxies[hub] = SocketService.connection.createHubProxy(hub);
            }

            return SocketService.proxies[hub];
        }

        private ensureConnection() {
            if (SocketService.isConnected) {
                return;
            }

            if (SocketService.connection == null) {
                SocketService.connection = $.hubConnection();

                SocketService.connection.stateChanged((change) => {
                    switch (change.newState) {
                        case $.signalR.connectionState.connecting:
                            this.eventBus.emit('socket.connecting');
                            break;
                        case $.signalR.connectionState.connected:
                            this.eventBus.emit('socket.connected');
                            break;
                        case $.signalR.connectionState.reconnecting:
                            this.eventBus.emit('socket.reconnecting');
                            break;
                        case $.signalR.connectionState.disconnected:
                            this.eventBus.emit('socket.disconnected');
                            break;
                    }
                });

                SocketService.connection.connectionSlow(() => {
                    this.eventBus.emit('socket.connection-slow');
                });

                SocketService.connection.disconnected(() => {
                    setTimeout(() => {
                        SocketService.isConnected = false;
                        SocketService.connectionPromise = null;
                        this.start();
                    }, 5000);
                });
            }
        }

        private start(callback?: (connection: SignalR) => void) {
            if (SocketService.isConnected) {
                if (callback != null) {
                    callback($.connection);
                }

                return;
            }

            if (SocketService.connectionPromise == null) {
                var settings: ConnectionSettings = null;

                if (this.getPageData().currentUser != null && !this.getPageData().currentUser.Settings.UseWebSocket) {
                    settings = { transport: 'longPolling' };
                }

                SocketService.connectionPromise = SocketService.connection.start(settings).done(() => {
                    SocketService.isConnected = true;
                    SocketService.connectionPromise = null;
                }).fail(() => {
                    SocketService.isConnected = false;
                    SocketService.connectionPromise = null;
                });
            }

            if (callback != null) {
                SocketService.connectionPromise.done(() => {
                    callback(SocketService.connection);
                });
            }
        }

        private getPageData(): MirGames.IPageData {
            return <MirGames.IPageData>window['pageData'];
        }
    }

    angular
        .module('core.socketService', ['core.eventBus'])
        .factory('socketService', ['eventBus', (eventBus: IEventBus) => new SocketService(eventBus)]);
}