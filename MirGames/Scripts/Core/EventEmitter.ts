/// <reference path="../_references.ts" />
module Core {
    export interface IEventEmitter {
        on(event: string, listener: Function): void;
        off(event: string, listener: Function): void;
        once(event: string, listener: Function): void;
        removeAllListeners(event?: string): void;
        setMaxListeners(n: number): void;
        listeners(event: string): Function[];
        emit(event: string, ...args: any[]): void;
    }

    export class EventEmitter implements IEventEmitter {
        private rootNode: IEventNode;
        private maxListeners: number;
        private delimeter: string;

        constructor() {
            this.rootNode = this.createEventNode(null, null);
            this.maxListeners = 10;
            this.delimeter = '.';
        }

        public on(event: string, listener: Function) {
            var eventNode = this.getEventNode(event);
            this.addListener(eventNode, { listener: listener });
        }
        
        public off(event: string, listener: Function) {
            var eventNode = this.getEventNode(event);
            var index = this.findListenerIndex(eventNode, listener);

            if (index >= 0) {
                eventNode.listeners.splice(index, 1);
            } else {
                throw new Error('The specified listener on ' + event + ' event has not been found');
            }

            this.cleanUpEventNode(eventNode);
        }

        public once(event: string, listener: Function) {
            var eventNode = this.getEventNode(event);
            this.addListener(eventNode, { listener: listener, numberOfTimes: 1 });
        }

        public removeAllListeners(event?: string): void {
            var eventNode = this.getEventNode(event);
            eventNode.listeners = [];
            this.cleanUpEventNode(eventNode);
        }

        public setMaxListeners(n: number): void {
            this.maxListeners = n;
        }

        public listeners(event: string): Function[] {
            var eventNode = this.getEventNode(event);
            var listeners: Function[] = [];

            for (var i = 0; i < eventNode.listeners.length; i++) {
                listeners.push(eventNode.listeners[i].listener);
            }

            return listeners;
        }

        emit(event: string, ...args: any[]) {
            var eventNode = this.getEventNode(event);
            var indexesToRemove: number[] = [];

            for (var i = 0; i < eventNode.listeners.length; i++) {
                var listener = eventNode.listeners[i];
                listener.listener(args);

                if (listener.numberOfTimes) {
                    listener.numberOfTimes--;

                    if (listener.numberOfTimes <= 0) {
                        indexesToRemove.push(i);
                    }
                }
            }

            for (var i = 0; i < indexesToRemove.length; i++) {
                eventNode.listeners.splice(indexesToRemove[i], 1);
            }

            if (this.hasChilds(eventNode)) {
                for (var key in eventNode.childs) {
                    if (eventNode.childs.hasOwnProperty(key)) {
                        this.emit(event + this.delimeter + key, args);
                    }
                }
            }

            this.cleanUpEventNode(eventNode);
        }

        private findListenerIndex(eventNode: IEventNode, listenerFunction: Function) {
            for (var i = 0; i < eventNode.listeners.length; i++) {
                if (eventNode.listeners[i].listener == listenerFunction) {
                    return i;
                }
            }

            return -1;
        }

        private addListener(eventNode: IEventNode, listener: IEventListener): void {
            if (eventNode.listeners.indexOf(listener) < 0) {
                if (eventNode.listeners.length == this.maxListeners) {
                    throw new Error('Maximum amount of listeners has reached for event ' + eventNode.type);
                }

                eventNode.listeners.push(listener);
            }
        }

        private cleanUpEventNode(eventNode: IEventNode) {
            if (eventNode.listeners.length == 0 && !this.hasChilds(eventNode) && eventNode.parent) {
                delete eventNode.parent.childs[eventNode.type];
                this.cleanUpEventNode(eventNode.parent);
            }
        }

        private getEventNode(event: string): IEventNode {
            var parts = event.split(this.delimeter);
            var currentNode = this.rootNode;

            for (var i = 0; i < parts.length; i++) {
                var part = parts[i];

                if (!currentNode.childs[part]) {
                    currentNode.childs[part] = this.createEventNode(part, currentNode);
                }

                currentNode = currentNode.childs[part];
            }

            return currentNode;
        }

        private hasChilds(eventNode: IEventNode): boolean {
            for (var key in eventNode.childs) {
                if (eventNode.childs.hasOwnProperty(key)) {
                    return true;
                }
            }

            return false;
        }

        private createEventNode(type: string, parent: IEventNode): IEventNode {
            return {
                type: type,
                parent: parent,
                childs: {},
                listeners: []
            };
        }
    }

    interface IEventsMap {
        [t: string]: IEventNode;
    }

    interface IEventNode {
        type: string;
        listeners: IEventListener[];
        childs: IEventsMap;
        parent: IEventNode;
    }

    interface IEventListener {
        listener: Function;
        numberOfTimes?: number;
    }
} 