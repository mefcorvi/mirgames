/// <reference path="_references.ts" />
var MirGames;
(function (MirGames) {
    var NavigationController = (function () {
        function NavigationController($scope, $element, settings, eventBus) {
            var _this = this;
            this.$scope = $scope;
            this.$element = $element;
            this.settings = settings;
            this.eventBus = eventBus;
            this.$scope.changeMenuState = function () {
                return _this.changeMenuState();
            };
            this.$scope.isMenuCollapsed = this.isMenuCollapsed;
            this.$scope.headerTooltip = this.headerTooltip;

            if ($("body").hasClass("nav-hidden")) {
                $element.click(function () {
                    return _this.changeMenuState();
                });
            }
        }
        Object.defineProperty(NavigationController.prototype, "headerTooltip", {
            get: function () {
                return this.isMenuCollapsed ? 'Развернуть меню' : 'Свернуть меню';
            },
            enumerable: true,
            configurable: true
        });

        Object.defineProperty(NavigationController.prototype, "isMenuCollapsed", {
            /**
            * Gets a value indicating whether menu is collaped.
            */
            get: function () {
                return this.settings.getIsMenuCollapsed();
            },
            /**
            * Sets a value indicating whether menu is collapsed.
            */
            set: function (value) {
                this.settings.setIsMenuCollapsed(value);
                this.$scope.isMenuCollapsed = value;
                this.$scope.headerTooltip = this.headerTooltip;
            },
            enumerable: true,
            configurable: true
        });


        /**
        * Changes the state of the menu.
        */
        NavigationController.prototype.changeMenuState = function () {
            var _this = this;
            this.isMenuCollapsed = !this.isMenuCollapsed;
            setTimeout(function () {
                _this.eventBus.emit('section.resized');
            }, 600);
        };
        NavigationController.$inject = ['$scope', '$element', 'settings', 'eventBus'];
        return NavigationController;
    })();
    MirGames.NavigationController = NavigationController;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=NavigationController.js.map
