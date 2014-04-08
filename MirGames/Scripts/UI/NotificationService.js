/// <reference path="../_references.ts" />
var UI;
(function (UI) {
    var NotificationService = (function () {
        function NotificationService() {
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
        NotificationService.prototype.setBubble = function (count, playSound) {
            if (typeof playSound === "undefined") { playSound = true; }
            if (playSound && (this.lastPlayDate === null || new Date().getTime() - this.lastPlayDate.getTime() > 5000)) {
                $.ionSound.play('notify');
                this.lastPlayDate = new Date();
            }

            this.setTitle(count);
            Tinycon.setBubble(count);
        };

        NotificationService.prototype.reset = function () {
            this.setTitle(0);
            Tinycon.setBubble('');
        };

        NotificationService.prototype.setTitle = function (count) {
            var newTitle = count > 0 ? document.title = '(' + count + ') ' + this.originalTitle : this.originalTitle;

            var setTitleFunc = function () {
                return document.title = newTitle;
            };
            setTimeout(setTitleFunc, 50);
            setTitleFunc();
        };
        return NotificationService;
    })();

    angular.module('ui.notification', []).factory('notificationService', [function () {
            return new NotificationService();
        }]);
})(UI || (UI = {}));
//# sourceMappingURL=NotificationService.js.map
