/// <reference path="../_references.ts" />
var UI;
(function (UI) {
    angular.module('ng').run([
        '$rootScope', function ($rootScope) {
            $rootScope.safeApply = function (fn) {
                var phase = $rootScope.$$phase;
                if (phase == '$apply' || phase == '$digest') {
                    if (fn && (typeof (fn) === 'function')) {
                        fn();
                    }
                } else {
                    $rootScope.$apply(fn);
                }
            };
        }]);
})(UI || (UI = {}));
//# sourceMappingURL=AppScope.js.map
