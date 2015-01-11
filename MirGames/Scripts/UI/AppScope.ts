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

    export function safeDigest(scope: ng.IScope) {
        var phase = scope.$$phase;
        if (phase != '$apply' && phase != '$digest') {
            scope.$digest();
        }
    }
}