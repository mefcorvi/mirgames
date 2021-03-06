/// <reference path="_references.ts" />
module MirGames {
    class Settings implements ISettings {
        private pageData: IPageData;

        constructor(private service: Core.IService) {
            this.pageData = window.pageData;
        }

        public setIsMenuCollapsed(value: boolean) {
            if (value) {
                $('body').addClass('menu-collapsed');
            }
            else {
                $('body').removeClass('menu-collapsed');
            }


            if (this.pageData.currentUser != null) {
                this.service.callAction('Settings', 'SetIsMenuCollapsed', {
                    value: value
                });
            }
        }

        public getIsMenuCollapsed(): boolean {
            return $('body').hasClass('menu-collapsed');
        }
    }

    export interface ISettings {
        setIsMenuCollapsed(value: boolean): void;
        getIsMenuCollapsed(): boolean
    }

    angular
        .module('mirgames.settings', ['core.service'])
        .factory('settings', ['service', (service: Core.IService) => new Settings(service)]);
}