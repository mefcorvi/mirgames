/// <reference path="../_references.ts" />
var Core;
(function (Core) {
    var ApiService = (function () {
        function ApiService(config, eventBus) {
            this.config = config;
            this.eventBus = eventBus;
        }
        ApiService.prototype.getAll = function (queryType, query, pageNum, pageSize, callback, blockInput) {
            if (typeof blockInput === "undefined") { blockInput = true; }
            query.$_type = queryType;
            this.invokeQuery('all', query, { pageNum: pageNum, pageSize: pageSize }, function (res) {
                return callback(res);
            }, blockInput);
        };

        ApiService.prototype.getOne = function (queryType, query, callback, blockInput) {
            if (typeof blockInput === "undefined") { blockInput = true; }
            query.$_type = queryType;
            this.invokeQuery('one', query, {}, function (res) {
                return callback(res);
            }, blockInput);
        };

        ApiService.prototype.executeCommand = function (commandType, command, callback, blockInput) {
            var _this = this;
            if (typeof blockInput === "undefined") { blockInput = true; }
            command.$_type = commandType;

            if (blockInput) {
                this.eventBus.emit('ajax-request.executing');
            }

            $.ajax({
                url: this.config.rootUrl + 'api',
                data: JSON.stringify({ Command: JSON.stringify(command), SessionId: $.cookie('key') }),
                type: 'POST',
                dataType: 'json',
                contentType: 'application/json; charset=utf-8'
            }).done(function (result) {
                if (blockInput) {
                    _this.eventBus.emit('ajax-request.executed');
                }

                if (callback != null) {
                    callback(result);
                }
            }).fail(function (context, failType, failText) {
                _this.eventBus.emit('ajax-request.failed', failType, failText);
            });
        };

        ApiService.prototype.invokeQuery = function (type, query, data, callback, blockInput) {
            var _this = this;
            if (typeof blockInput === "undefined") { blockInput = true; }
            if (blockInput) {
                this.eventBus.emit('ajax-request.executing');
            }

            data['query'] = JSON.stringify(query);
            data['sessionId'] = $.cookie('key');

            $.ajax({
                url: this.config.rootUrl + 'api/' + type,
                data: data,
                type: 'GET'
            }).done(function (result) {
                if (blockInput) {
                    _this.eventBus.emit('ajax-request.executed');
                }

                if (callback != null) {
                    callback(result);
                }
            }).fail(function (context, failType, failText) {
                _this.eventBus.emit('ajax-request.failed', failType, failText);
            });
        };
        return ApiService;
    })();

    angular.module('core.apiService', ['core.config', 'core.eventBus']).factory('apiService', ['config', 'eventBus', function (config, eventBus) {
            return new ApiService(config, eventBus);
        }]);
})(Core || (Core = {}));
//# sourceMappingURL=ApiService.js.map
