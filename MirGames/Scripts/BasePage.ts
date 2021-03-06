/// <reference path="_references.ts" />
module MirGames {
    export class BasePage<T, TScope extends ng.IScope> {
        public pageData: T;
        
        constructor(public $scope: TScope, public eventBus: Core.IEventBus) {
            this.pageData = <T>window['pageData'];

            $('.text img').each((idx: number, element: Element) => {
                var $element = $(element);
                var naturalWidth = $element.naturalWidth();
                var naturalHeight = $element.naturalHeight();

                var width = $element.width();
                var height = $element.height();

                if (naturalWidth > width * 1.1 || naturalHeight > height * 1.1) {
                    $element.addClass('resizable-image');
                    $element.click(() => {
                        window.open($element.attr('src'), '_blank');
                    });
                }
            });
        }

        public scrollToItem(item: JQuery, duration: number = 250) {
            var position = this.getOffset(item);
            this.scrollTo(position.top, duration);
        }

        public getOffset(item: JQuery): { left: number; top: number; } {
            var position = item.offset();
            position.top += this.getContentSection().scrollTop();

            return position;
        }

        public scrollTo(scrollTop: number, duration: number = 250) {
            $('body, html').animate({ scrollTop: scrollTop }, duration);
        }

        public getScrollTop(): number {
            return $(window).scrollTop();
        }

        public setScrollTop(scrollTop: number) {
            $(window).scrollTop(scrollTop);
        }

        public isScrollBottom(zone: number = 50): boolean {
            return $(window).scrollTop() + $(window).height() > $("body").prop('scrollHeight') - zone;
        }

        public scrollToBottom(duration: number = 600) {
            $('body, html').stop().animate({
                scrollTop: $("body").prop('scrollHeight')
            }, { easing: 'swing', duration: duration });
        }

        public setTitle(title: string) {
            document.title = title;
            setTimeout(() => {
                if (document.title == title) {
                    document.title = "";
                    document.title = title;
                }
            }, 100);
        }

        public hideScrollbars() {
            $('html, body > header').width($('html').width());
            $(document.body).addClass('dialog-opened');
        }

        public showScrollbars() {
            $(document.body).removeClass('dialog-opened');
            $('html, body > header').width('');
        }

        public getContentSection(): JQuery {
            return $('body > section');
        }
    }

    export interface IPageScope extends UI.IAppScope {
    }

    export interface IPageData {
        currentUser: MirGames.Domain.Users.ViewModels.CurrentUserViewModel;
        onlineUsers: MirGames.Domain.Users.ViewModels.OnlineUserViewModel[];
        onlineUserTags: { [userId: number]: string[] };
        isAdmin: boolean;
        notificationsCount: number;
    }

    export interface IPageDataProvider {
        getPageData<T>(): T;
    }

    class PageDataProvider {
        public getPageData<T extends IPageData>(): T {
            return <T>window.pageData;
        }
    }

    angular
        .module('ui.pageData', [])
        .service('pageDataService', PageDataProvider);
}

interface Window {
    pageData: any;
}