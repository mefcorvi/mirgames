/// <reference path="_references.ts" />
module MirGames {
    export class NavigationController {
        static $inject = ['$scope', '$element', 'settings', 'eventBus'];

        constructor(private $scope: INavigationControllerScope, private $element: JQuery, private settings: ISettings, private eventBus: Core.IEventBus) {
            this.$scope.changeMenuState = () => this.changeMenuState();
            this.$scope.isMenuCollapsed = this.getIsMenuCollapsed();
            this.$scope.headerTooltip = this.getHeaderTooltip();

            if ($("body").hasClass("nav-hidden")) {
                $element.click(() => this.changeMenuState());
            }
        }

        public getHeaderTooltip(): string {
            return this.getIsMenuCollapsed ? 'Развернуть меню' : 'Свернуть меню';
        }

        /**
         * Gets a value indicating whether menu is collaped.
         */
        public getIsMenuCollapsed(): boolean {
            return this.settings.getIsMenuCollapsed();
        }

        /**
         * Sets a value indicating whether menu is collapsed.
         */
        public setIsMenuCollapsed(value: boolean) {
            this.settings.setIsMenuCollapsed(value);
            this.$scope.isMenuCollapsed = value;
            this.$scope.headerTooltip = this.getHeaderTooltip();
        }

        /**
         * Changes the state of the menu.
         */
        public changeMenuState() {
            this.setIsMenuCollapsed(!this.getIsMenuCollapsed());
            setTimeout(() => {
                this.eventBus.emit('section.resized');
            }, 600);
        }
    }

    export interface INavigationControllerScope extends ng.IScope {
        changeMenuState(): void;
        isMenuCollapsed: boolean;
        headerTooltip: string;
    }
}