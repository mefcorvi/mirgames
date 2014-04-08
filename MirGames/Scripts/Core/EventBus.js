var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
/// <reference path="../_references.ts" />
var Core;
(function (Core) {
    var EventBus = (function (_super) {
        __extends(EventBus, _super);
        function EventBus() {
            _super.apply(this, arguments);
        }
        return EventBus;
    })(EventEmitter);

    var eventBus = new EventBus();

    angular.module('core.eventBus', []).factory('eventBus', [function () {
            return eventBus;
        }]);
})(Core || (Core = {}));
//# sourceMappingURL=EventBus.js.map
