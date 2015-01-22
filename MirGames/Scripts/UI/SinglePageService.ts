/// <reference path="../_references.ts" />

module UI {
    export interface ISinglePageService {
        enable(): void;
        disable(): void;
    }

    class SinglePageService implements ISinglePageService {
        static $inject = ['$rootScope', '$location', '$http', '$compile', 'eventBus'];

        private disregistrationFunction: Function;
        private httpConfig: ng.IRequestShortcutConfig;

        constructor(
            private $rootScope: ng.IRootScopeService,
            private $location: ng.ILocationService,
            private $http: ng.IHttpService,
            private $compile: ng.ICompileService,
            private eventBus: Core.IEventBus) {
            this.httpConfig = {
                cache: false
            };
        }

        public enable(): void {
            if (this.disregistrationFunction) {
                this.disable();
            }

            this.disregistrationFunction = this.$rootScope.$on('$locationChangeStart', (ev: ng.IAngularEvent, oldUrl: string, newUrl: string) => {
                if (oldUrl != newUrl) {
                    this.handleLocationChange();
                }
            });
        }

        public disable(): void {
            if (this.disregistrationFunction) {
                this.disregistrationFunction();
                this.disregistrationFunction = null;
            }
        }

        private isHistoryApiSupported(): boolean {
            return Utils.isDefined(window.history) && Utils.isFunction(window.history.replaceState);
        }

        private handleLocationChange(): void {
            this.saveScrollPosition();

            this.eventBus.emit('ajax-request.executing');
            this.$http.get<string>(this.$location.url(), {
                cache: false,
                params: {
                    '_uid': new Date().getTime()
                }
            }).then(response => {
                this.eventBus.emit('ajax-request.executed');
                this.pageLoaded(response.data);
                delete response.data;
            });
        }

        private pageLoaded(html: string): void {
            var titleRegexp = /<title[^>]*>([\s\S]*)<\/title>/i;
            var title = titleRegexp.exec(html)[1];
            document.title = title;

            var bodyRegexp = /<body([^>]*)>([\s\S]*)<\/body>/i;
            var bodyHtml = bodyRegexp.exec(html);

            var result = angular.element('<div' + bodyHtml[1] + '>' + bodyHtml[2] + '</div>');
            angular.element('body').attr('class', result.attr('class'));

            var oldSectionScope = angular.element('body > section').scope();
            var oldHeaderScope = angular.element('body > header').scope();

            angular.element('body > section').replaceWith(result.children('section'));
            angular.element('body > header').replaceWith(result.children('header'));
            this.restoreScrollPosition(false);

            window.pageData = JSON.parse((<HTMLInputElement>document.getElementById('page-data')).value);

            setTimeout(() => {
                oldSectionScope.$destroy();
                oldHeaderScope.$destroy();

                var sectionScope = this.$rootScope.$new(true);
                var headerScope = this.$rootScope.$new(true);
                this.$compile($('body > section'))(sectionScope);
                this.$compile($('body > header'))(headerScope);
                sectionScope.$digest();
                headerScope.$digest();
                this.restoreScrollPosition(true);
            }, 0);            
        }

        private saveScrollPosition(): void {
            if (this.isHistoryApiSupported() && !window.history.state) {
                window.history.replaceState({
                    url: window.location.href,
                    scrollTop: document.documentElement.scrollTop || document.body.scrollTop
                }, document.title);
            }
        }

        private restoreScrollPosition(dropState: boolean): void {
            var scrollTop = 0;
            var hash = this.$location.hash();
            var hashOffset = $('#' + hash).offset();

            if (this.isHistoryApiSupported() && window.history.state) {
                scrollTop = window.history.state.scrollTop;

                if (dropState) {
                    window.history.replaceState(null, document.title);
                }
            } else if (hashOffset && hashOffset.top) {
                scrollTop = hashOffset.top - (parseInt($('body').css('padding-top'), 10) || 0);
            }

            if (scrollTop || !dropState) {
                document.body.scrollTop = scrollTop;
                document.documentElement.scrollTop = scrollTop;
            }
        }
    }

    angular
        .module('ui.singlePage', [])
        .service('singlePageService', SinglePageService);
} 