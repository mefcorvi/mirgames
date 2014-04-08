/// <reference path="../_references.ts" />
var Utils;
(function (Utils) {
    function isUndefined(obj) {
        return typeof (obj) == 'undefined';
    }
    Utils.isUndefined = isUndefined;

    function isArray(obj) {
        return obj instanceof Array;
    }
    Utils.isArray = isArray;

    function isDefined(obj) {
        return !isUndefined(obj);
    }
    Utils.isDefined = isDefined;

    function isFunction(obj) {
        return typeof (obj) == 'function';
    }
    Utils.isFunction = isFunction;

    function isNullOrEmpty(obj) {
        return obj == null || obj == '';
    }
    Utils.isNullOrEmpty = isNullOrEmpty;
})(Utils || (Utils = {}));
//# sourceMappingURL=Utils.js.map
