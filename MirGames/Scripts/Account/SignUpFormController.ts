/// <reference path="../_references.ts" />
module Account {
    export class SignUpFormController {
        static $inject = ['$scope', 'commandBus', 'vcRecaptchaService', 'dialog'];

        constructor($scope: ISignUpFormControllerScope, commandBus: Core.ICommandBus, recaptchaService: any, dialog: UI.IDialog) {
            $scope.login = '';
            $scope.email = '';
            $scope.password = '';
            $scope.isFocused = true;
            $scope.agreement = false;

            $scope.processSignUp = url => {
                if ($scope.signUpForm.$invalid) {
                    return;
                }

                $scope.activationUrl = '';
                var command = commandBus.createCommandFromScope(MirGames.Domain.SignUpCommand, $scope);
                $scope.isFocused = false;

                commandBus.executeCommand(url, command, response => {
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
            }

            $scope.close = () => {
                dialog.close(false);
            }
        }
    }

    export interface ISignUpFormControllerScope extends ng.IScope {
        login: string;
        email: string;
        password: string;
        isFocused: boolean;
        alreadyRegistered: boolean;
        internalError: boolean;
        wrongCaptcha: boolean;
        loginFailed: boolean;
        signUpForm: ng.IFormController;
        activationUrl: string;
        agreement: boolean;

        processSignUp(url: string): void;
        close(): void;
    }
}