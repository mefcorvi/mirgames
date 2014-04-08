/// <reference path="../_references.ts" />
var UI;
(function (UI) {
    var Dialog = (function () {
        function Dialog($http, $document, $compile, $rootScope, $controller, $templateCache, $q, $transition, $injector, opts, eventBus) {
            this.$http = $http;
            this.$document = $document;
            this.$compile = $compile;
            this.$rootScope = $rootScope;
            this.$controller = $controller;
            this.$templateCache = $templateCache;
            this.$q = $q;
            this.$transition = $transition;
            this.$injector = $injector;
            this.eventBus = eventBus;
            var options = this.options = angular.extend({}, Dialog.defaults, Dialog.globalOptions, opts);
            this._open = false;

            this.backdropEl = this.createElement(options.backdropClass);
            if (options.backdropFade) {
                this.backdropEl.addClass(options.transitionClass);
                this.backdropEl.removeClass(options.triggerClass);
            }

            this.modalEl = this.createElement(options.dialogClass);
            if (options.dialogFade) {
                this.modalEl.addClass(options.transitionClass);
                this.modalEl.removeClass(options.triggerClass);
            }

            this.$dialogContainer = this.createElement('dialog-container');
        }
        Dialog.prototype.isOpen = function () {
            return this._open;
        };

        Dialog.prototype.open = function (templateUrl, controller) {
            var _this = this;
            var options = this.options;
            this.eventBus.emit('ajax-request.executing');

            if (templateUrl) {
                options.templateUrl = templateUrl;
            }

            if (controller) {
                options.controller = controller;
            }

            if (!(options.template || options.templateUrl)) {
                throw new Error('Dialog.open expected template or templateUrl, neither found. Use options or open method to specify them.');
            }

            this._loadResolves().then(function (locals) {
                var $scope = locals.$scope = _this.$scope = (locals.$scope ? locals.$scope : _this.$rootScope.$new());

                _this.$dialogContainer.html(locals.$template);

                if (_this.options.controller) {
                    var ctrl = _this.$controller(_this.options.controller, locals);
                    _this.$dialogContainer.children().data('ngControllerController', ctrl);
                }

                _this.$compile(_this.$dialogContainer)($scope);
                _this._addElementsToDom();

                // trigger tranisitions
                setTimeout(function () {
                    if (_this.options.dialogFade) {
                        _this.modalEl.addClass(_this.options.triggerClass);
                    }

                    if (_this.options.backdropFade) {
                        _this.backdropEl.addClass(_this.options.triggerClass);
                    }
                });

                _this._bindEvents();
                _this.eventBus.emit('ajax-request.executed');
            });

            this.deferred = this.$q.defer();
            return this.deferred.promise;
        };

        Dialog.prototype.cancel = function () {
            this.close(Dialog.cancellationToken);
        };

        Dialog.prototype.close = function (result) {
            var _this = this;
            if (typeof result === "undefined") { result = null; }
            var fadingElements = this._getFadingElements();

            if (fadingElements.length > 0) {
                for (var i = fadingElements.length - 1; i >= 0; i--) {
                    this.$transition(fadingElements[i], removeTriggerClass).then(onCloseComplete);
                }
                return;
            }

            this._onCloseComplete(result);

            var removeTriggerClass = function (el) {
                el.removeClass(_this.options.triggerClass);
            };

            var onCloseComplete = function () {
                if (_this._open) {
                    _this._onCloseComplete(result);
                }
            };
        };

        Dialog.prototype.handleBackDropClick = function (e) {
            this.cancel();
            e.preventDefault();
            this.$scope.$apply();
        };

        Dialog.prototype.handledDialogKey = function (e) {
            if (e.which === 27) {
                this.cancel();
                e.preventDefault();
                this.$scope.$apply();
            }
        };

        Dialog.prototype._getFadingElements = function () {
            var elements = [];

            if (this.options.dialogFade) {
                elements.push(this.modalEl);
            }

            if (this.options.backdropFade) {
                elements.push(this.backdropEl);
            }

            return elements;
        };

        Dialog.prototype._bindEvents = function () {
            if (this.options.keyboard) {
                $(document.body).bind('keydown.dialog', this.handledDialogKey.bind(this));
            }

            if (this.options.backdrop && this.options.backdropClick) {
                this.backdropEl.bind('click.dialog', this.handleBackDropClick.bind(this));
            }
        };

        Dialog.prototype._unbindEvents = function () {
            if (this.options.keyboard) {
                $(document.body).unbind('keydown.dialog');
            }

            if (this.options.backdrop && this.options.backdropClick) {
                this.backdropEl.unbind('click.dialog');
            }
        };

        Dialog.prototype._onCloseComplete = function (result) {
            this._removeElementsFromDom();
            this._unbindEvents();

            this.options.onClose(result);
            this.deferred.resolve(result);
        };

        Dialog.prototype._addElementsToDom = function () {
            Dialog.maxZIndex++;
            $(document.body).append(this.modalEl);
            this.modalEl.append(this.$dialogContainer);

            this.modalEl.css({
                zIndex: Dialog.maxZIndex
            });

            if (this.options.backdrop) {
                this.$dialogContainer.append(this.backdropEl);
            }

            this._open = true;
        };

        Dialog.prototype._removeElementsFromDom = function () {
            this.modalEl.remove();
            this.$dialogContainer.remove();

            if (this.options.backdrop) {
                this.backdropEl.remove();
            }

            this._open = false;
            Dialog.maxZIndex--;
        };

        Dialog.prototype._loadResolves = function () {
            var _this = this;
            var values = [], keys = [], templatePromise;

            if (this.options.template) {
                templatePromise = this.$q.when(this.options.template);
            } else if (this.options.templateUrl) {
                templatePromise = this.$http.get(this.options.templateUrl, { cache: this.$templateCache }).then(function (response) {
                    return response.data;
                });
            }

            angular.forEach(this.options.resolve || [], function (value, key) {
                keys.push(key);
                values.push(angular.isString(value) ? _this.$injector.get(value) : _this.$injector.invoke(value));
            });

            keys.push('$template');
            values.push(templatePromise);

            return this.$q.all(values).then(function (values) {
                var locals = { dialog: null };
                angular.forEach(values, function (value, index) {
                    locals[keys[index]] = value;
                });
                locals.dialog = _this;
                return locals;
            });
        };

        Dialog.prototype.createElement = function (clazz) {
            var el = angular.element("<div>");
            el.addClass(clazz);
            return el;
        };
        Dialog.maxZIndex = 1000;
        Dialog.defaults = {
            backdrop: true,
            dialogClass: 'modal',
            backdropClass: 'modal-backdrop',
            transitionClass: 'fade',
            triggerClass: 'in',
            resolve: {},
            backdropFade: false,
            dialogFade: false,
            keyboard: true,
            backdropClick: true,
            onClose: function (result) {
            },
            onOk: function () {
            }
        };

        Dialog.globalOptions = {};
        Dialog.cancellationToken = {};
        return Dialog;
    })();

    // The `$dialogProvider` can be used to configure global defaults for your
    // `$dialog` service.
    var dialogModule = angular.module('ui.bootstrap.dialog', ['ui.bootstrap.transition', 'core.eventBus']);

    dialogModule.controller('MessageBoxController', [
        '$scope', 'dialog', 'model',
        function ($scope, dialog, model) {
            $scope.title = model.title;
            $scope.message = model.message;
            $scope.buttons = model.buttons;
            $scope.close = function (res) {
                dialog.close(res);
            };
        }]);

    dialogModule.provider("$dialog", function () {
        this.options = function (value) {
            Dialog.globalOptions = value;
        };

        this.$get = [
            "$http", "$document", "$compile", "$rootScope", "$controller", "$templateCache", "$q", "$transition", "$injector", "eventBus",
            function ($http, $document, $compile, $rootScope, $controller, $templateCache, $q, $transition, $injector, eventBus) {
                var service;

                service = {
                    dialog: function (opts) {
                        return new Dialog($http, $document, $compile, $rootScope, $controller, $templateCache, $q, $transition, $injector, opts, eventBus);
                    },
                    messageBox: function (title, message, buttons) {
                        var options = {
                            templateUrl: 'template/dialog/message.html',
                            controller: 'MessageBoxController',
                            resolve: {
                                model: function () {
                                    return {
                                        title: title,
                                        message: message,
                                        buttons: buttons
                                    };
                                }
                            }
                        };

                        return new Dialog($http, $document, $compile, $rootScope, $controller, $templateCache, $q, $transition, $injector, options, eventBus);
                    }
                };

                return service;
            }];
    });

    angular.module('ui.dialog', ['ui.bootstrap.dialog']).directive('dialog', function () {
        return {
            restrict: 'A',
            replace: true,
            link: function (scope, element, attributes, controller) {
                element.bind('click', function (ev) {
                    scope.$apply(function () {
                        var resolve = null;

                        if (attributes.resolve) {
                            resolve = scope.$eval(attributes.resolve);
                        }

                        controller.openDialog(attributes.dialog, resolve);
                    });
                });
            },
            controller: [
                '$scope', '$dialog', function (scope, dialogService) {
                    return {
                        openDialog: function (templateUrl, resolve) {
                            var dialog = dialogService.dialog({
                                resolve: {
                                    'dialogOptions': function () {
                                        return resolve;
                                    }
                                },
                                onClose: function (result) {
                                    scope.result = result;

                                    if (scope.dialogClose && result !== Dialog.cancellationToken) {
                                        scope.dialogClose();
                                    }
                                }
                            });

                            dialog.open(templateUrl, scope.controller);
                        }
                    };
                }],
            scope: {
                'controller': '@dialogController',
                'dialogClose': '&',
                'dialogOk': '&'
            },
            transclude: false
        };
    });
})(UI || (UI = {}));
//# sourceMappingURL=Dialog.js.map
