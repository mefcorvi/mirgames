/// <reference path="../_references.ts" />
module Account {
    export class SignUpFormController {
        static $inject = ['$scope', 'apiService', 'vcRecaptchaService', 'dialog'];

        constructor($scope: ISignUpFormControllerScope, apiService: Core.IApiService, recaptchaService: any, dialog: UI.IDialog) {
            $scope.login = '';
            $scope.email = '';
            $scope.password = '';
            $scope.isFocused = true;
            $scope.agreement = false;

            $scope.processSignUp = () => {
                if ($scope.signUpForm.$invalid) {
                    return;
                }

                $scope.activationUrl = '';
                var command: MirGames.Domain.Users.Commands.SignUpCommand = {
                    CaptchaChallenge: $scope.captcha.challenge,
                    CaptchaResponse: $scope.captcha.response,
                    Email: $scope.email,
                    Login: $scope.login,
                    Password: MD5($scope.password)
                };

                $scope.isFocused = false;

                apiService.executeCommand('SignUpCommand', command, response => {
                    if (response == 0) {
                        var loginCommand: MirGames.Domain.Users.Commands.LoginCommand = {
                            EmailOrLogin: $scope.email,
                            Password: MD5($scope.password)
                        };

                        apiService.executeCommand('LoginCommand', loginCommand, sessionId => {
                            $scope.loginFailed = sessionId == null;
                            recaptchaService.reload();

                            if (sessionId != null) {
                                $.cookie('key', sessionId, {
                                    expires: 365 * 24 * 60 * 60,
                                    path: '/'
                                });
                                window.location.reload();
                            }
                        });

                        return;
                    }

                    $scope.isFocused = true;
                    $scope.wrongCaptcha = response == 1;
                    $scope.alreadyRegistered = response == 2;
                    $scope.internalError = response == null;
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
        captcha: {
            challenge: string;
            response: string;
        };

        processSignUp(): void;
        close(): void;
    }
}