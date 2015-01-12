/// <reference path="../_references.ts" />
module UI {
    export interface IAppScope extends ng.IScope {
    }

    export function safeDigest(scope: ng.IScope) {
        var phase = scope.$$phase;
        if (phase != '$apply' && phase != '$digest') {
            scope.$digest();
        }
    }
}