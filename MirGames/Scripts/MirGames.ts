/// <reference path="_references.ts" />
angular.module('mirgames', [
    'core.application',
    'ui.dialog',
    'ui.texteditor',
    'ui.tinyeditor',
    'ui.autofill',
    'ui.notification',
    'ui.dateinput',
    'ui.file',
    'ui.bootstrap.dropdownToggle',
    'ngQuickDate',
    'core.config',
    'core.currentUser',
    'core.apiService',
    'core.socketService',
    'core.commandBus',
    'core.eventBus',
    'vcRecaptcha',
    'mirgames.settings'
]);

angular
    .module('ng')
    .directive('ngFocused', ['$timeout', ($timeout: ng.ITimeoutService) => {
        return {
            link: function (scope: ng.IScope, element: JQuery, attrs: any) {
                scope.$watch(attrs.ngFocused, (val: boolean) => {
                    if (angular.isDefined(val) && val) {
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
    .directive('ngSubmit', () => {
        return {
            link: (scope: ng.IScope, element: JQuery, attrs: any) => {
                element.append('<input type="submit" style="position: absolute; left: -9999px; width: 1px; height: 1px;"/>');
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


$(function () {
    var $element = <any>$(".nano");
    $element.nanoScroller();
});