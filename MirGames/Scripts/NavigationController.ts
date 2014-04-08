/// <reference path="_references.ts" />
module MirGames {
    export class NavigationController {
        static $inject = ['$scope', '$element', 'settings', 'eventBus'];

        constructor(private $scope: INavigationControllerScope, private $element: JQuery, private settings: ISettings, private eventBus: Core.IEventBus) {
            this.$scope.changeMenuState = () => this.changeMenuState();
            this.$scope.isMenuCollapsed = this.isMenuCollapsed;
            this.$scope.headerTooltip = this.headerTooltip;

            if ($("body").hasClass("nav-hidden")) {
                $element.click(() => this.changeMenuState());
            }
        }

        public get headerTooltip(): string {
            return this.isMenuCollapsed ? 'Развернуть меню' : 'Свернуть меню';
        }

        /**
         * Gets a value indicating whether menu is collaped.
         */
        public get isMenuCollapsed(): boolean {
            return this.settings.getIsMenuCollapsed();
        }

        /**
         * Sets a value indicating whether menu is collapsed.
         */
        public set isMenuCollapsed(value: boolean) {
            this.settings.setIsMenuCollapsed(value);
            this.$scope.isMenuCollapsed = value;
            this.$scope.headerTooltip = this.headerTooltip;
        }

        /**
         * Changes the state of the menu.
         */
        public changeMenuState() {
            this.isMenuCollapsed = !this.isMenuCollapsed;
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