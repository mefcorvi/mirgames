/// <reference path="../_references.ts" />
var Account;
(function (Account) {
    var SignUpFormController = (function () {
        function SignUpFormController($scope, commandBus, recaptchaService, dialog) {
            $scope.login = '';
            $scope.email = '';
            $scope.password = '';
            $scope.isFocused = true;

            $scope.processSignUp = function (url) {
                if ($scope.signUpForm.$invalid) {
                    return;
                }

                $scope.activationUrl = '';
                var command = commandBus.createCommandFromScope(MirGames.Domain.SignUpCommand, $scope);
                $scope.isFocused = false;

                commandBus.executeCommand(url, command, function (response) {
                    if (response.result == "Success") {
                        window.location.reload();
                        return;
                    }

                    $scope.isFocused = true;
                    $scope.wrongCaptcha = response.result == "WrongCaptcha";
                    $scope.alreadyRegistered = response.result == "AlreadyRegistered";
                    $scope.internalError = response.result == "UnknownError";
                    $scope.loginFailed = response.result == "LoginFailed";
                    recaptchaService.reload();

                    $scope.$apply();
                });
            };

            $scope.close = function () {
                dialog.close(false);
            };
        }
        SignUpFormController.$inject = ['$scope', 'commandBus', 'vcRecaptchaService', 'dialog'];
        return SignUpFormController;
    })();
    Account.SignUpFormController = SignUpFormController;
})(Account || (Account = {}));
//# sourceMappingURL=SignUpFormController.js.map
