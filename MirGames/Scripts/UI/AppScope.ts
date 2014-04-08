/// <reference path="../_references.ts" />
module UI {
    angular
        .module('ng')
        .run(['$rootScope', ($rootScope: any) => {
            $rootScope.safeApply = (fn?: () => void) => {
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

    export interface IAppScope extends ng.IScope {
        safeApply(fn?: () => void ): void;
    }
}