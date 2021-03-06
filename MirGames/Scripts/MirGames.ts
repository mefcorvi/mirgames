﻿/// <reference path="_references.ts" />
moment.lang("ru");

declare var hljs: any;
declare var Headroom: any;
declare var pageData: MirGames.IPageData;

var headroom: any = null;

angular.module('mirgames', [
    'core.application',
    'ui.dialog',
    'ui.singlePage',
    'ui.texteditor',
    'ui.tinyeditor',
    'ui.autofill',
    'ui.notification',
    'ui.dateinput',
    'ui.file',
    'ui.bootstrap.dropdownToggle',
    'ui.bootstrap.buttons',
    'ui.draggable',
    'ngQuickDate',
    'core.config',
    'core.currentUser',
    'core.apiService',
    'core.socketService',
    'core.eventBus',
    'vcRecaptcha',
    'mirgames.settings',
    'timeRelative',
    'ui.pageData',
    'ui.unreadItems'
]).config(['$locationProvider', ($locationProvider: ng.ILocationProvider) => {
    $locationProvider.html5Mode(true);
}]).run(['singlePageService', (singlePageService: UI.ISinglePageService) => {
    singlePageService.enable();
}]);

angular
    .module('mirgames')
    .directive('ngFocused', ['$timeout', ($timeout: ng.ITimeoutService) => {
        return {
            link: function (scope: ng.IScope, element: JQuery, attrs: any) {
                scope.$watch(attrs.ngFocused, (val: boolean) => {
                    if (angular.isDefined(val) && val && !element.is(':focus')) {
                        $timeout(() => { element[0].focus(); });
                    }
                }, true);

                element.bind('blur', () => {
                    if (angular.isDefined(attrs.ngFocused)) {
                        scope.$apply(() => {
                            scope.$eval(attrs.ngFocused + '=false');
                        });
                    }
                });
            }
        };
    }])
    .directive('staticInclude', ['$http', '$templateCache', '$compile', ($http: ng.IHttpService, $templateCache: ng.ITemplateCacheService, $compile: ng.ICompileService) => {
        return (scope: ng.IScope, element: JQuery, attrs: any) => {
            var templatePath = attrs.staticInclude;

            $http.get<string>(templatePath, { cache: $templateCache }).success((response) => {
                var contents = $('<div/>').html(response).contents();
                element.html(contents);
                $compile(contents)(scope);
            });
        };
    }])
    .directive('ngSubmit', () => {
        return {
            link: (scope: ng.IScope, element: JQuery, attrs: any) => {
                element.append('<input type="submit" style="position: absolute; left: -9999px; width: 1px; height: 1px;"/>');
            }
        }
    })
    .directive('code', () => {
        return {
            restrict: 'E',
            link: (scope: ng.IScope, element: JQuery) => {
                hljs.highlightBlock(element[0]);
            }
        }
    })
    .directive('avatarImage', () => {
        return {
            scope: {
                'avatarImage': '=avatarImage'
            },
            link: (scope: ng.IScope, element: JQuery, attrs: any) => {
                scope.$watch('avatarImage', (newValue) => {
                    element.addClass('avatar-background');
                    element.css({ 'background-image': 'url("' + newValue + '")' });
                });
            }
        }
    })
    .directive('authorLink', () => {
        return {
            restrict: 'A',
            scope: {
                'userId': '=authorLink'
            },
            link: (scope: any, element: JQuery, attrs: any) => {
                element.attr('href', Router.action('Users', 'Profile', { userId: scope.userId }));
            }
        }
    })
    .directive('headroom', () => {
        return {
            restrict: 'A',
            link: (scope: ng.IScope, element: JQuery) => {
                var myElement = element[0];
                if (element.hasClass('fixed')) {
                    element.css('position', 'absolute').css('position', 'fixed');
                }

                if (pageData.currentUser != null && pageData.currentUser.Settings.HeaderType == 'AutoHide') {
                    headroom = new Headroom(myElement);
                    headroom.init();

                    scope.$on('$destroy', () => {
                        headroom.destroy();
                    });
                }
                else if (window.innerHeight <= 480) {
                    element.removeClass('fixed');
                }
            }
        }
    })
    .directive('embed', () => {
        return {
            restrict: 'A',
            link: (scope: any, element: JQuery, attrs: any) => {
                element.one('click', () => {
                    element.html(attrs.embed);
                });
            }
        }
    })
    .directive("selectize", ($timeout: ng.ITimeoutService) => {
        return {
            restrict: "AE",
            link: (scope: ng.IScope, element: JQuery, attrs: any) => {
                return $timeout(() => {
                    return $(element).selectize(scope.$eval(attrs.selectize));
                });
            }
        };
    })
    .config(['$httpProvider', function ($httpProvider: ng.IHttpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }])
    .filter('unsafe', ['$sce', function ($sce: ng.ISCEService) {
        return function (val: string) {
            return $sce.trustAsHtml(val);
        };
    }]);;

angular
    .module('mirgames')
    .config(['ngQuickDateDefaultsProvider', (ngQuickDateDefaultsProvider: any) => {
        return ngQuickDateDefaultsProvider.set({
            closeButtonHtml: "<i class='fa fa-times'></i>",
            buttonIconHtml: "<i class='fa fa-clock-o'></i>",
            nextLinkHtml: "<i class='fa fa-chevron-right'></i>",
            prevLinkHtml: "<i class='fa fa-chevron-left'></i>",
            parseDateFunction: function (str: string) {
                var d = moment(str, 'DD.MM.YYYY', true);
                if (d.isValid()) {
                    return d.toDate();
                }

                d = moment(str, 'DD.MM.YY', true);
                if (d.isValid()) {
                    return d.toDate();
                }

                var d = moment(str, null, true);
                if (d.isValid()) {
                    return d.toDate();
                }

                d = moment(str, 'L');
                if (d.isValid()) {
                    return d.toDate();
                }

                return null;
            }
        });
    }]);

$(() => {
    var repositionOnlineUsers = () => {
        var bodyPosition = $(document.body).offset();
        var bodyWidth = $(document.body).outerWidth();
        var windowHeight = window.innerHeight;

        var getScrollTop = () => {
            return $('html').scrollTop() || $('body').scrollTop();
        }

        $('.online-users').css({
            'display': 'block',
            'top': 0,
            'left': bodyPosition.left + bodyWidth
        });

        $('.up-nav').css({
            'left': bodyPosition.left - 40,
            'opacity': Math.min(getScrollTop() / windowHeight / 5, 1),
            'cursor': getScrollTop() > 0 ? 'pointer' : 'default'
        }).click(() => {
            $('body, html').scrollTop(0);
        });

        $(window).scroll(() => {
            $('.up-nav').css({
                'opacity': Math.min(getScrollTop() / windowHeight / 5, 1),
                'cursor': getScrollTop() > 0 ? 'pointer' : 'default'
            });
        });
    };

    repositionOnlineUsers();
    $(window).resize(repositionOnlineUsers);
})