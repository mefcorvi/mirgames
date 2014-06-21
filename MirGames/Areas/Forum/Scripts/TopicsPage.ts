/// <reference path="_references.ts" />
module MirGames.Forum {
    export class TopicsPage extends MirGames.BasePage<IPageData, ITopicsPageScope> {
        static $inject = ['$scope', 'commandBus', 'eventBus'];

        constructor($scope: ITopicsPageScope, private commandBus: Core.ICommandBus, eventBus: Core.IEventBus) {
            super($scope, eventBus);
            this.$scope.markAllAsRead = this.markAllAsRead.bind(this);
        }

        private markAllAsRead(): void {
            var command = this.commandBus.createCommandFromScope(Domain.MarkAllTopicsAsReadCommand, this.$scope);
            this.commandBus.executeCommand(Router.action("Forum", "MarkAllTopicsAsRead"), command, (response: { result: boolean }) => {
                if (response.result) {
                    window.location.reload();
                }
            });
        }
    }

    export interface ITopicsPageScope extends IPageScope {
        markAllAsRead: () => void;
    }
}