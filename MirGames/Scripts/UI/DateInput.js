/// <reference path="../_references.ts" />
var UI;
(function (UI) {
    angular.module('ui.dateinput', []).directive('dateInput', function ($window) {
        return {
            require: '^ngModel',
            restrict: 'A',
            link: function (scope, elm, attrs, ctrl) {
                var dateFormat = attrs.dateInput;
                attrs.$observe('dateInput', function (newValue) {
                    if (dateFormat == newValue || !ctrl.$modelValue) {
                        return;
                    }

                    dateFormat = newValue;
                    ctrl.$modelValue = new Date(ctrl.$setViewValue);
                });

                ctrl.$formatters.unshift(function (modelValue) {
                    scope = scope;

                    if (!dateFormat || !modelValue) {
                        return '';
                    }

                    return moment(modelValue).format(dateFormat);
                });

                ctrl.$parsers.unshift(function (viewValue) {
                    scope = scope;
                    var date = moment(viewValue, dateFormat);
                    return (date && date.isValid() && date.year() > 1950) ? date.toDate() : null;
                });
            }
        };
    });
})(UI || (UI = {}));
//# sourceMappingURL=DateInput.js.map
