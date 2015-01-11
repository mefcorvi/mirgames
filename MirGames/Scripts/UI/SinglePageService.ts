/// <reference path="../_references.ts" />

module UI {
    export interface ISinglePageService {
        enable(): void;
        disable(): void;
    }

    class SinglePageService implements ISinglePageService {
        static $inject = ['$rootScope', '$location', '$http', '$compile', 'eventBus'];

        private disregistrationFunction: Function;

        constructor(
            private $rootScope: ng.IRootScopeService,
            private $location: ng.ILocationService,
            private $http: ng.IHttpService,
            private $compile: ng.ICompileService,
            private eventBus: Core.IEventBus) {
        }

        public enable(): void {
            if (this.disregistrationFunction) {
                this.disable();
            }

            this.disregistrationFunction = this.$rootScope.$on('$locationChangeStart', () => {
                this.handleLocationChange();
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
            this.$http.get<string>(this.$location.url()).then(response => {
                this.eventBus.emit('ajax-request.executed');
                this.pageLoaded(response.data);
            });
        }

        private pageLoaded(html: string): void {
            var titleRegexp = /<title[^>]*>([\s\S]*)<\/title>/i;
            var title = titleRegexp.exec(html)[1];
            document.title = title;

            var bodyRegexp = /<body([^>]*)>([\s\S]*)<\/body>/i;
            var bodyHtml = bodyRegexp.exec(html);

            var result = $('<div' + bodyHtml[1] + '>' + bodyHtml[2] + '</div>');
            $('body').attr('class', result.attr('class'));
            $('body > section').replaceWith(result.children('section'));
            $('body > header').replaceWith(result.children('header'));
            this.restoreScrollPosition(false);

            window.pageData = JSON.parse((<HTMLInputElement>document.getElementById('page-data')).value);

            setTimeout(() => {
                var scope = this.$rootScope.$new();
                this.$compile($('body > section'))(scope);
                this.$compile($('body > header'))(scope);
                scope.$digest();
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

            document.body.scrollTop = scrollTop;
            document.documentElement.scrollTop = scrollTop;            
        }
    }

    angular
        .module('ui.singlePage', [])
        .service('singlePageService', SinglePageService);
} 