var Core;
(function (Core) {
    var SocketService = (function () {
        function SocketService(eventBus) {
            this.eventBus = eventBus;
        }
        SocketService.prototype.addHandler = function (hub, messageType, callback) {
            this.ensureConnection();

            var hubProxy = this.getProxy(hub);
            hubProxy.on(messageType, callback);

            this.start();
        };

        SocketService.prototype.executeCommand = function (commandType, command) {
            command.$_type = commandType;

            var hubProxy = this.getProxy("CommandsHub");

            this.start(function (connection) {
                hubProxy.invoke('Execute', JSON.stringify(command));
            });
        };

        SocketService.prototype.getProxy = function (hub) {
            if (!SocketService.proxies[hub]) {
                SocketService.proxies[hub] = SocketService.connection.createHubProxy(hub);
            }

            return SocketService.proxies[hub];
        };

        SocketService.prototype.ensureConnection = function () {
            var _this = this;
            if (SocketService.isConnected) {
                return;
            }

            if (SocketService.connection == null) {
                SocketService.connection = $.hubConnection();

                SocketService.connection.stateChanged(function (change) {
                    switch (change.newState) {
                        case $.signalR.connectionState.connecting:
                            _this.eventBus.emit('socket.connecting');
                            break;
                        case $.signalR.connectionState.connected:
                            _this.eventBus.emit('socket.connected');
                            break;
                        case $.signalR.connectionState.reconnecting:
                            _this.eventBus.emit('socket.reconnecting');
                            break;
                        case $.signalR.connectionState.disconnected:
                            _this.eventBus.emit('socket.disconnected');
                            break;
                    }
                });

                SocketService.connection.connectionSlow(function () {
                    _this.eventBus.emit('socket.connection-slow');
                });

                SocketService.connection.disconnected(function () {
                    setTimeout(function () {
                        SocketService.isConnected = false;
                        SocketService.connectionPromise = null;
                        _this.start();
                    }, 5000);
                });
            }
        };

        SocketService.prototype.start = function (callback) {
            if (SocketService.isConnected) {
                if (callback != null) {
                    callback($.connection);
                }

                return;
            }

            if (SocketService.connectionPromise == null) {
                var settings = null;

                if (this.getPageData().currentUser != null && !this.getPageData().currentUser.Settings.UseWebSocket) {
                    settings = { transport: 'longPolling' };
                }

                SocketService.connectionPromise = SocketService.connection.start(settings).done(function () {
                    SocketService.isConnected = true;
                    SocketService.connectionPromise = null;
                }).fail(function () {
                    SocketService.isConnected = false;
                    SocketService.connectionPromise = null;
                });
            }

            if (callback != null) {
                SocketService.connectionPromise.done(function () {
                    callback(SocketService.connection);
                });
            }
        };

        SocketService.prototype.getPageData = function () {
            return window['pageData'];
        };
        SocketService.proxies = {};
        return SocketService;
    })();

    angular.module('core.socketService', ['core.eventBus']).factory('socketService', ['eventBus', function (eventBus) {
            return new SocketService(eventBus);
        }]);
})(Core || (Core = {}));
//# sourceMappingURL=SocketService.js.map
