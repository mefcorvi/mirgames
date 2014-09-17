/// <reference path="../_references.ts" />
module UI {
    export interface INotificationService {
        setBubble(count: number, playSound?: boolean): void;
        reset(): void;
        notifyEvent(playSound?: boolean): void;
    }

    class NotificationService implements INotificationService {
        private lastPlayDate: Date;
        private originalTitle: string;

        constructor() {
            Tinycon.setOptions({
                background: '#365e7b',
                colour: '#ffffff',
                fallback: true,
                font: '9px Tahoma',
                height: 10,
                width: 10
            });

            $.ionSound({
                sounds: ['notify'],
                path: 'sounds/',
                multiPlay: false,
                volume: 1
            });

            this.originalTitle = document.title;
            this.lastPlayDate = null;
        }

        public setBubble(count: number, playSound: boolean = true): void {
            if (playSound && (this.lastPlayDate === null || new Date().getTime() - this.lastPlayDate.getTime() > 5000)) {
                $.ionSound.play('notify');
                this.lastPlayDate = new Date();
            }

            this.setTitle(count);
            Tinycon.setBubble(count);
        }

        public reset() {
            this.setTitle(0);
            Tinycon.setBubble('');
        }

        public notifyEvent(playSound: boolean = true) {
            if (playSound) {
                $.ionSound.play('notify');
            }

            headroom.pin();
        }

        private setTitle(count: number) {
            var newTitle = count > 0 ? document.title = '(' + count + ') ' + this.originalTitle : this.originalTitle;

            var setTitleFunc = () => document.title = newTitle;
            setTimeout(setTitleFunc, 50);
            setTitleFunc();
        }
    }

    angular
        .module('ui.notification', [])
        .factory('notificationService', [() => new NotificationService()]);
}