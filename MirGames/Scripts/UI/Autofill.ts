/// <reference path="../_references.ts" />

module UI {
    var autoFillModule = angular.module('ui.autofill', []);

    autoFillModule.directive("ngAutofill", [
        '$timeout',
        function ($timeout: any) {
            var INTERVAL_MS = 500;

            return {
                require: 'ngModel',
                link: function (scope: ng.IScope, element: JQuery, attrs: ng.IAttributes, ngModel: any) {

                    var timer: number;
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
}