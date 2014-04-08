/// <reference path="../_references.ts" />
module UI {
    angular
        .module('ui.dateinput', [])
        .directive('dateInput', ($window: ng.IWindowService) => {
            return {
                require: '^ngModel',
                restrict: 'A',
                link: function (scope: ng.IScope, elm: JQuery, attrs: any, ctrl: any) {
                    var dateFormat = attrs.dateInput;
                    attrs.$observe('dateInput', (newValue: string) => {
                        if (dateFormat == newValue || !ctrl.$modelValue) {
                            return;
                        }

                        dateFormat = newValue;
                        ctrl.$modelValue = new Date(ctrl.$setViewValue);
                    });

                    ctrl.$formatters.unshift((modelValue: string) => {
                        scope = scope;

                        if (!dateFormat || !modelValue) {
                            return '';
                        }

                        return moment(modelValue).format(dateFormat);
                    });

                    ctrl.$parsers.unshift((viewValue: string) => {
                        scope = scope;
                        var date = moment(viewValue, dateFormat);
                        return (date && date.isValid() && date.year() > 1950) ? date.toDate() : null;
                    });
                }
            };
        });
}