/// <reference path="../_references.ts" />
module MirGames {
    export class UnreadItemsController {
        static $inject = ['$scope', 'unreadItemsService'];

        constructor(private $scope: IUnreadItemsController, private unreadItemsService: IUnreadItemsService) {
            this.$scope.unreadCount = this.unreadItemsService.getUnreadCount();

            var unreadCountChanged = (unreadCount: number) => {
                this.$scope.unreadCount = unreadCount;
                UI.safeDigest(this.$scope);
            }

            this.unreadItemsService.addHandler(unreadCountChanged);

            this.$scope.$on('$destroy', () => {
                this.unreadItemsService.removeHandler(unreadCountChanged);
            });
        }
    }

    export interface IUnreadItemsController extends ng.IScope {
        unreadCount: number;
    }
}