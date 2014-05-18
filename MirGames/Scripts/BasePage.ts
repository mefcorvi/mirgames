/// <reference path="_references.ts" />
module MirGames {
    export class BasePage<T, TScope extends ng.IScope> {
        public get pageData(): T {
            return <T>window['pageData'];
        }
        
        constructor(public $scope: TScope, public eventBus: Core.IEventBus) {
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
            var position = item.offset();

            if (position) {
                $('body > section').animate({ scrollTop: position.top + $("body > section").scrollTop() }, duration);
            }
        }

        public getScrollTop(): number {
            return $("body > section").scrollTop();
        }

        public setScrollTop(scrollTop: number) {
            $("body > section").scrollTop(scrollTop);
        }

        public isScrollBottom(): boolean {
            return $("body > section").scrollTop() + $(window).height() > $("body > section").prop('scrollHeight') - 50;
        }

        public scrollToBottom() {
            $("body > section").stop().animate({
                scrollTop: $("body > section").prop('scrollHeight')
            }, { easing: 'swing', duration: 600 });
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
        forumTopicsUnreadCount: number;
    }
}