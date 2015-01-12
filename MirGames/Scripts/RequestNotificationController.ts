/// <reference path="_references.ts" />
module MirGames {
    export class RequestNotificationController {
        static $inject = ['$scope', '$element', 'eventBus'];

        private loadingTimeout: number;

        constructor(private $scope: IRequestNotificationControllerScope, private $element: JQuery, private eventBus: Core.IEventBus) {
            this.$scope.requestsExecutingCount = 0;

            eventBus.on('ajax-request.executing', this.onRequestExecuting.bind(this));
            eventBus.on('ajax-request.executed', this.onRequestExecuted.bind(this));
            eventBus.on('ajax-request.failed', this.onRequestExecuted.bind(this));
        }

        private onRequestExecuting(): void {
            this.$scope.requestsExecutingCount++;
            UI.safeDigest(this.$scope);

            if (!this.loadingTimeout) {
                this.loadingTimeout = setTimeout(() => {
                    this.showLoadingSpinner();
                }, 500);
            }
        }

        private onRequestExecuted(): void {
            this.$scope.requestsExecutingCount--;

            if (this.$scope.requestsExecutingCount < 0) {
                this.$scope.requestsExecutingCount = 0;
            }

            if (this.loadingTimeout) {
                this.$scope.showLoadingSpinner = false;
                this.$scope.showLongLoading = false;
                clearTimeout(this.loadingTimeout);
                this.loadingTimeout = null;
            }

            UI.safeDigest(this.$scope);
        }

        private showLoadingSpinner() {
            this.$scope.showLoadingSpinner = true;
            UI.safeDigest(this.$scope);

            this.loadingTimeout = setTimeout(() => {
                this.$scope.showLongLoading = true;
                UI.safeDigest(this.$scope);
            }, 500);
        }
    }

    export interface IRequestNotificationControllerScope extends UI.IAppScope {
        requestsExecutingCount: number;
        showLoadingSpinner: boolean;
        showLongLoading: boolean;
    }
}