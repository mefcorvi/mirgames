/// <reference path="_references.ts" />
var MirGames;
(function (MirGames) {
    var Settings = (function () {
        function Settings(service, pageData) {
            this.service = service;
            this.pageData = pageData;
        }
        Settings.prototype.setIsMenuCollapsed = function (value) {
            if (value) {
                $('body').addClass('menu-collapsed');
            } else {
                $('body').removeClass('menu-collapsed');
            }

            if (this.pageData.currentUser != null) {
                this.service.callAction('Settings', 'SetIsMenuCollapsed', {
                    value: value
                });
            }
        };

        Settings.prototype.getIsMenuCollapsed = function () {
            return $('body').hasClass('menu-collapsed');
        };
        return Settings;
    })();

    angular.module('mirgames.settings', ['core.service']).factory('settings', ['service', 'pageData', function (service, pageData) {
            return new Settings(service, pageData);
        }]);
})(MirGames || (MirGames = {}));
//# sourceMappingURL=Settings.js.map
