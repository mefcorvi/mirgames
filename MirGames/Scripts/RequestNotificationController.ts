/// <reference path="_references.ts" />
module MirGames {
    export class RequestNotificationController {
        static $inject = ['$scope', '$element', 'eventBus'];

        constructor(private $scope: IRequestNotificationControllerScope, private $element: JQuery, private eventBus: Core.IEventBus) {
            this.$scope.requestsExecutingCount = 0;

            eventBus.addListener('ajax-request.executing', this.onRequestExecuting.bind(this));
            eventBus.addListener('ajax-request.executed', this.onRequestExecuted.bind(this));
            eventBus.addListener('ajax-request.failed', this.onRequestExecuted.bind(this));
        }

        private onRequestExecuting(): void {
            this.$scope.requestsExecutingCount++;
            this.$scope.safeApply();
        }

        private onRequestExecuted(): void {
            this.$scope.requestsExecutingCount--;

            if (this.$scope.requestsExecutingCount < 0) {
                this.$scope.requestsExecutingCount = 0;
            }

            this.$scope.safeApply();
        }
    }

    export interface IRequestNotificationControllerScope extends UI.IAppScope {
        requestsExecutingCount: number;
    }
}