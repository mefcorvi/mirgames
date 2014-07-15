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
    'ui.bootstrap.buttons',
    'ui.draggable',
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

angular.module('ng').directive('ngFocused', [
    '$timeout', function ($timeout) {
        return {
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngFocused, function (val) {
                    if (angular.isDefined(val) && val && !element.is(':focus')) {
                        $timeout(function () {
                            element[0].focus();
                        });
                    }
                }, true);

                element.bind('blur', function () {
                    if (angular.isDefined(attrs.ngFocused)) {
                        scope.$apply(function () {
                            scope.$eval(attrs.ngFocused + '=false');
                        });
                    }
                });
            }
        };
    }]).directive('staticInclude', [
    '$http', '$templateCache', '$compile', function ($http, $templateCache, $compile) {
        return function (scope, element, attrs) {
            var templatePath = attrs.staticInclude;

            $http.get(templatePath, { cache: $templateCache }).success(function (response) {
                var contents = $('<div/>').html(response).contents();
                element.html(contents);
                $compile(contents)(scope);
            });
        };
    }]).directive('ngSubmit', function () {
    return {
        link: function (scope, element, attrs) {
            element.append('<input type="submit" style="position: absolute; left: -9999px; width: 1px; height: 1px;"/>');
        }
    };
}).directive('avatarImage', function () {
    return {
        scope: {
            'avatarImage': '=avatarImage'
        },
        link: function (scope, element, attrs) {
            scope.$watch('avatarImage', function (newValue) {
                element.addClass('avatar-background');
                element.css({ 'background-image': 'url("' + newValue + '")' });
            });
        }
    };
}).directive('authorLink', function () {
    return {
        restrict: 'A',
        scope: {
            'userId': '=authorLink'
        },
        link: function (scope, element, attrs) {
            element.attr('href', Router.action('Users', 'Profile', { userId: scope.userId }));
        }
    };
}).config([
    '$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
    }]).filter('unsafe', [
    '$sce', function ($sce) {
        return function (val) {
            return $sce.trustAsHtml(val);
        };
    }]);
;

angular.module('mirgames').config([
    'ngQuickDateDefaultsProvider', function (ngQuickDateDefaultsProvider) {
        return ngQuickDateDefaultsProvider.set({
            closeButtonHtml: "<i class='fa fa-times'></i>",
            buttonIconHtml: "<i class='fa fa-clock-o'></i>",
            nextLinkHtml: "<i class='fa fa-chevron-right'></i>",
            prevLinkHtml: "<i class='fa fa-chevron-left'></i>",
            parseDateFunction: function (str) {
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
    var $element = $(".nano");
    $element.nanoScroller();
});
//# sourceMappingURL=MirGames.js.map
