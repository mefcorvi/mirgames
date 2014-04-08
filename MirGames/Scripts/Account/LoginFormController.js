/// <reference path="../_references.ts" />
var Account;
(function (Account) {
    var LoginFormController = (function () {
        function LoginFormController($scope, commandBus, dialog, apiService, config, eventBus) {
            var _this = this;
            this.$scope = $scope;
            this.apiService = apiService;
            this.config = config;
            this.eventBus = eventBus;
            $scope.emailOrLogin = '';
            $scope.password = '';
            $scope.wrongLoginOrPassword = false;
            $scope.isLoginMode = true;
            $scope.isFocused = true;

            $scope.processLogin = function (url) {
                if ($scope.loginForm.$invalid) {
                    return;
                }

                var command = commandBus.createCommandFromScope(MirGames.Domain.LoginCommand, $scope);
                $scope.isFocused = false;

                commandBus.executeCommand(url, command, function (response) {
                    $scope.wrongLoginOrPassword = response.result === 0;

                    if (response.result === 1) {
                        _this.eventBus.emit('ajax-request.executing');
                        window.location.reload();
                    } else {
                        $scope.isFocused = true;
                    }

                    $scope.$apply();
                });
            };

            $scope.close = function () {
                dialog.close(false);
            };

            $scope.restorePassword = this.restorePassword.bind(this);
            $scope.processRestore = this.processRestorePassword.bind(this);
            $scope.auth = function (provider) {
                return _this.auth(provider);
            };
        }
        LoginFormController.prototype.auth = function (provider) {
            var link = Router.action('OAuth', 'Authorize', { provider: provider });

            this.eventBus.emit('ajax-request.executing');
            $('<form action="' + link + '" method="POST"><input type="hidden" name="__RequestVerificationToken" value="' + this.config.antiForgery + '"></form>').submit();
        };

        LoginFormController.prototype.processRestorePassword = function () {
            var _this = this;
            var command = {
                EmailOrLogin: this.$scope.emailOrLogin,
                NewPasswordHash: MD5(this.$scope.password)
            };

            this.apiService.executeCommand('RequestPasswordRestoreCommand', command, function (result) {
                if (result) {
                    _this.$scope.$apply(function () {
                        _this.$scope.restoreRequestSent = true;
                    });
                }
            });
        };

        LoginFormController.prototype.restorePassword = function () {
            this.$scope.isLoginMode = false;
            this.$scope.isRestoreMode = true;
            this.$scope.isFocused = true;
        };
        LoginFormController.$inject = ['$scope', 'commandBus', 'dialog', 'apiService', 'config', 'eventBus'];
        return LoginFormController;
    })();
    Account.LoginFormController = LoginFormController;
})(Account || (Account = {}));
//# sourceMappingURL=LoginFormController.js.map
