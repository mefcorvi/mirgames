/// <reference path="../_references.ts" />
var UI;
(function (UI) {
    var autoFillModule = angular.module('ui.autofill', []);

    autoFillModule.directive("ngAutofill", [
        '$timeout',
        function ($timeout) {
            var INTERVAL_MS = 500;

            return {
                require: 'ngModel',
                link: function (scope, element, attrs, ngModel) {
                    var timer;
                    function startTimer() {
                        timer = $timeout(function () {
                            var value = element.val();
                            if (value && ngModel.$viewValue !== value) {
                                ngModel.$setViewValue(value);
                            }
                            startTimer();
                        }, INTERVAL_MS);
                    }

                    scope.$on('$destroy', function () {
                        $timeout.cancel(timer);
                    });

                    startTimer();
                }
            };
        }
    ]);
})(UI || (UI = {}));
//# sourceMappingURL=Autofill.js.map
