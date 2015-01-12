/// <reference path="EventEmitter.ts" />
module Core {
    export interface IEventBus extends IEventEmitter {
    }

    class EventBus extends Core.EventEmitter implements IEventBus  {
    }

    var eventBus = new EventBus();

    angular
        .module('core.eventBus', [])
        .factory('eventBus', [() => eventBus]);
}