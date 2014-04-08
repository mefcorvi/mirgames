/// <reference path="../_references.ts" />
module UI {
    export interface IDialogService {
        dialog(options?: IDialogOptions): IDialog;
        messageBox(title: string, message: string, buttons: any): IDialog;
    }

    export interface IDialogOptions {
        backdrop?: boolean;
        dialogClass?: string;
        backdropClass?: string;
        transitionClass?: string;
        triggerClass?: string;
        resolve?: any;
        backdropFade?: boolean;
        dialogFade?: boolean;
        keyboard?: boolean;
        backdropClick?: boolean;
        templateUrl?: string;
        template?: string;
        controller?: any;
        zIndex?: number;
        onClose?: (result: any) => void;
    }

    export interface IDialog {
        isOpen(): boolean;
        open(templateUrl?: string, controller?: any): ng.IPromise<any>;
        close(result: any): void;
        cancel(): void;
    }
    
    class Dialog implements IDialog {
        private static maxZIndex: number = 1000;
        private static defaults: IDialogOptions = {
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
            onClose: (result: any) => { },
            onOk: () => { }
        };

        public static globalOptions: IDialogOptions = {};
        public static cancellationToken: {} = {};

        private options: IDialogOptions;
        private _open: boolean;
        private backdropEl: JQuery;
        private modalEl: JQuery;
        private $dialogContainer: JQuery;
        private $scope: ng.IScope;
        private deferred: ng.IDeferred<any>;

        constructor(private $http: ng.IHttpService, private $document: ng.IDocumentService,
            private $compile: ng.ICompileService, private $rootScope: ng.IScope,
            private $controller: ng.IControllerService, private $templateCache: ng.ITemplateCacheService,
            private $q: ng.IQService, private $transition: any, private $injector: any, opts: IDialogOptions, private eventBus: Core.IEventBus) {

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

        public isOpen(): boolean {
            return this._open;
        }

        public open(templateUrl?: string, controller?: any): ng.IPromise<any> {
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

            this._loadResolves().then((locals: any) => {
                var $scope = locals.$scope = this.$scope =
                    (locals.$scope ? locals.$scope : this.$rootScope.$new());

                this.$dialogContainer.html(locals.$template);

                if (this.options.controller) {
                    var ctrl = this.$controller(this.options.controller, locals);
                    this.$dialogContainer.children().data('ngControllerController', ctrl);
                }

                this.$compile(this.$dialogContainer)($scope);
                this._addElementsToDom();

                // trigger tranisitions
                setTimeout(() => {
                    if (this.options.dialogFade) {
                        this.modalEl.addClass(this.options.triggerClass);
                    }

                    if (this.options.backdropFade) {
                        this.backdropEl.addClass(this.options.triggerClass);
                    }
                });

                this._bindEvents();
                this.eventBus.emit('ajax-request.executed');
            });

            this.deferred = this.$q.defer();
            return this.deferred.promise;
        }

        public cancel(): void {
            this.close(Dialog.cancellationToken);
        }

        public close(result: any = null): void {
            var fadingElements = this._getFadingElements();

            if (fadingElements.length > 0) {
                for (var i = fadingElements.length - 1; i >= 0; i--) {
                    this.$transition(fadingElements[i], removeTriggerClass).then(onCloseComplete);
                }
                return;
            }

            this._onCloseComplete(result);

            var removeTriggerClass = (el: JQuery) => {
                el.removeClass(this.options.triggerClass);
            }

            var onCloseComplete = () => {
                if (this._open) {
                    this._onCloseComplete(result);
                }
            }
        }

        private handleBackDropClick(e: JQueryEventObject) {
            this.cancel();
            e.preventDefault();
            this.$scope.$apply();
        }

        private handledDialogKey(e: JQueryEventObject) {
            if (e.which === 27) {
                this.cancel();
                e.preventDefault();
                this.$scope.$apply();
            }
        }

        private _getFadingElements(): JQuery[] {
            var elements: JQuery[] = [];

            if (this.options.dialogFade) {
                elements.push(this.modalEl);
            }

            if (this.options.backdropFade) {
                elements.push(this.backdropEl);
            }

            return elements;
        }

        private _bindEvents(): void {
            if (this.options.keyboard) {
                $(document.body).bind('keydown.dialog', this.handledDialogKey.bind(this));
            }

            if (this.options.backdrop && this.options.backdropClick) {
                this.backdropEl.bind('click.dialog', this.handleBackDropClick.bind(this));
            }
        }

        private _unbindEvents(): void {
            if (this.options.keyboard) {
                $(document.body).unbind('keydown.dialog');
            }

            if (this.options.backdrop && this.options.backdropClick) {
                this.backdropEl.unbind('click.dialog');
            }
        }

        private _onCloseComplete(result: any): void {
            this._removeElementsFromDom();
            this._unbindEvents();

            this.options.onClose(result);
            this.deferred.resolve(result);
        }

        private _addElementsToDom(): void {
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
        }

        private _removeElementsFromDom(): void {
            this.modalEl.remove();
            this.$dialogContainer.remove();

            if (this.options.backdrop) {
                this.backdropEl.remove();
            }

            this._open = false;
            Dialog.maxZIndex--;
        }

        private _loadResolves(): ng.IPromise<any> {
            var values: any[] = [], keys: string[] = [], templatePromise: ng.IPromise<string>;

            if (this.options.template) {
                templatePromise = this.$q.when(this.options.template);
            } else if (this.options.templateUrl) {
                templatePromise = this.$http.get(this.options.templateUrl, { cache: this.$templateCache })
                    .then(response => { return response.data; });
            }

            angular.forEach(this.options.resolve || [], (value, key) => {
                keys.push(key);
                values.push(angular.isString(value) ? this.$injector.get(value) : this.$injector.invoke(value));
            });

            keys.push('$template');
            values.push(templatePromise);

            return this.$q.all(values).then((values) => {
                var locals: { dialog: IDialog; [key: string]: any } = { dialog: null };
                angular.forEach(values, (value, index) => {
                    locals[keys[index]] = value;
                });
                locals.dialog = this;
                return locals;
            });
        }

        private createElement(clazz: string): JQuery {
            var el = angular.element("<div>");
            el.addClass(clazz);
            return el;
        }
    }

    interface IMessageBoxScope extends ng.IScope {
        title: string;
        message: string;
        buttons: any;
        close(res: any): void;
    }

    // The `$dialogProvider` can be used to configure global defaults for your
    // `$dialog` service.
    var dialogModule = angular.module('ui.bootstrap.dialog', ['ui.bootstrap.transition', 'core.eventBus']);

    dialogModule.controller('MessageBoxController', ['$scope', 'dialog', 'model',
        ($scope: IMessageBoxScope, dialog: Dialog, model: any) => {
            $scope.title = model.title;
            $scope.message = model.message;
            $scope.buttons = model.buttons;
            $scope.close = function (res) {
                dialog.close(res);
            };
        }]);

    dialogModule.provider("$dialog", function () {
        this.options = function (value: IDialogOptions) {
            Dialog.globalOptions = value;
        };

        this.$get = ["$http", "$document", "$compile", "$rootScope", "$controller", "$templateCache", "$q", "$transition", "$injector", "eventBus",
            function ($http: ng.IHttpService, $document: ng.IDocumentService, $compile: ng.ICompileService, $rootScope: ng.IScope, $controller: ng.IControllerService, $templateCache: ng.ITemplateCacheService, $q: ng.IQService, $transition: any, $injector: any, eventBus: Core.IEventBus) {
                var service: IDialogService;
                
                service = {
                    dialog: function (opts?: IDialogOptions) {
                        return new Dialog($http, $document, $compile, $rootScope,
                            $controller, $templateCache, $q, $transition, $injector, opts, eventBus);
                    },

                    messageBox: function (title, message, buttons) {
                        var options: IDialogOptions = {
                            templateUrl: 'template/dialog/message.html',
                            controller: 'MessageBoxController',
                            resolve: {
                                model: () => {
                                    return {
                                        title: title,
                                        message: message,
                                        buttons: buttons
                                    };
                                }
                            }
                        };

                        return new Dialog($http, $document, $compile, $rootScope,
                            $controller, $templateCache, $q, $transition, $injector, options, eventBus);
                    }
                };

                return service;
            }];
    });

    angular
        .module('ui.dialog', ['ui.bootstrap.dialog'])
        .directive('dialog', () => {
        return {
                restrict: 'A',
                replace: true,
                link: (scope: ng.IScope, element: JQuery, attributes: any, controller: any) => {
                    element.bind('click', (ev: JQueryEventObject) => {
                        scope.$apply(() => {
                            var resolve: any = null;

                            if (attributes.resolve) {
                                resolve = scope.$eval(attributes.resolve);
                            }

                            controller.openDialog(attributes.dialog, resolve);
                        });
                    });
                },
                controller: ['$scope', '$dialog', (scope: any, dialogService: IDialogService) => {
                    return {
                        openDialog: (templateUrl: string, resolve: any) => {
                            var dialog = dialogService.dialog({
                                resolve: {
                                    'dialogOptions': () => resolve
                                },
                                onClose: (result: any) => {
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
            }
    });
}