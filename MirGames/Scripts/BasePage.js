/// <reference path="_references.ts" />
var MirGames;
(function (MirGames) {
    var BasePage = (function () {
        function BasePage($scope, eventBus) {
            this.$scope = $scope;
            this.eventBus = eventBus;
            $('.text img').each(function (idx, element) {
                var $element = $(element);
                var naturalWidth = $element.naturalWidth();
                var naturalHeight = $element.naturalHeight();

                var width = $element.width();
                var height = $element.height();

                if (naturalWidth > width * 1.1 || naturalHeight > height * 1.1) {
                    $element.addClass('resizable-image');
                    $element.click(function () {
                        window.open($element.attr('src'), '_blank');
                    });
                }
            });
        }
        Object.defineProperty(BasePage.prototype, "pageData", {
            get: function () {
                return window['pageData'];
            },
            enumerable: true,
            configurable: true
        });

        BasePage.prototype.scrollToItem = function (item, duration) {
            if (typeof duration === "undefined") { duration = 250; }
            var position = this.getOffset(item);
            this.scrollTo(position.top, duration);
        };

        BasePage.prototype.getOffset = function (item) {
            var position = item.offset();
            position.top += this.getContentSection().scrollTop();

            return position;
        };

        BasePage.prototype.scrollTo = function (scrollTop, duration) {
            if (typeof duration === "undefined") { duration = 250; }
            $('body > section').animate({ scrollTop: scrollTop }, duration);
        };

        BasePage.prototype.getScrollTop = function () {
            return $("body > section").scrollTop();
        };

        BasePage.prototype.setScrollTop = function (scrollTop) {
            $("body > section").scrollTop(scrollTop);
        };

        BasePage.prototype.isScrollBottom = function () {
            return $("body > section").scrollTop() + $(window).height() > $("body > section").prop('scrollHeight') - 50;
        };

        BasePage.prototype.scrollToBottom = function () {
            $("body > section").stop().animate({
                scrollTop: $("body > section").prop('scrollHeight')
            }, { easing: 'swing', duration: 600 });
        };

        BasePage.prototype.setTitle = function (title) {
            document.title = title;
            setTimeout(function () {
                if (document.title == title) {
                    document.title = "";
                    document.title = title;
                }
            }, 100);
        };

        BasePage.prototype.getContentSection = function () {
            return $('body > section');
        };
        return BasePage;
    })();
    MirGames.BasePage = BasePage;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=BasePage.js.map
