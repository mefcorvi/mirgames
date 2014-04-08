/// <reference path="../_references.ts" />
module Core {
    export interface IEventBus extends IEventEmitter {
    }

    class EventBus extends EventEmitter implements IEventBus  {
    }

    var eventBus = new EventBus();

    angular
        .module('core.eventBus', [])
        .factory('eventBus', [() => eventBus]);
}