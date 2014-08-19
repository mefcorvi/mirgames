var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var MirGames;
(function (MirGames) {
    /// <reference path="../_references.ts" />
    (function (Account) {
        var AccountSettingsPage = (function (_super) {
            __extends(AccountSettingsPage, _super);
            function AccountSettingsPage($scope, apiService, eventBus, config) {
                var _this = this;
                _super.call(this, $scope, eventBus);
                this.apiService = apiService;
                this.config = config;
                this.$scope.timeZone = this.pageData.timeZone;
                this.$scope.avatarUrl = this.pageData.currentUser.AvatarUrl;
                var settings = this.pageData.currentUser.Settings;
                this.$scope.oauthProviders = this.pageData.oauthProviders;

                this.$scope.profile = {
                    about: this.pageData.user.About,
                    birthday: this.pageData.user.Birthday,
                    bitbucket: null,
                    career: null,
                    city: this.pageData.user.Location,
                    company: null,
                    github: null,
                    habrahabr: null,
                    name: this.pageData.user.Name
                };

                this.$scope.useWebSocket = settings.UseWebSocket;
                this.$scope.useEnterToSendChatMessage = settings.UseEnterToSendChatMessage;
                this.$scope.forumContiniousPagination = settings.ForumContiniousPagination;

                this.$scope.save = this.save.bind(this);
                this.$scope.saveProfile = function () {
                    return _this.saveProfile();
                };
                this.$scope.fileUploaded = this.fileUploaded.bind(this);

                this.$scope.linkAuthProvider = function (provider) {
                    return _this.linkProvider(provider);
                };
                this.$scope.unlinkAuthProvider = function (provider) {
                    return _this.unlinkProvider(provider);
                };

                this.$scope.birthdayDate = function (d) {
                    var currentYear = new Date().getFullYear();

                    return d.getFullYear() > 1900 && d.getFullYear() <= (currentYear - 10);
                };
            }
            AccountSettingsPage.prototype.linkProvider = function (provider) {
                var link = Router.action('OAuth', 'Authorize', { provider: provider.ProviderName });

                this.eventBus.emit('ajax-request.executing');
                $('<form action="' + link + '" method="POST"><input type="hidden" name="__RequestVerificationToken" value="' + this.config.antiForgery + '"></form>').submit();
            };

            AccountSettingsPage.prototype.unlinkProvider = function (provider) {
                var _this = this;
                var command = {
                    ProviderName: provider.ProviderName
                };

                this.apiService.executeCommand('DetachOAuthProviderCommand', command, function () {
                    provider.IsLinked = false;
                    _this.eventBus.emit('user.notification', 'Аккаунт был успешно отсоединен от ' + provider.DisplayName);
                    _this.$scope.$apply();
                });
            };

            /** Saves the account settings. */
            AccountSettingsPage.prototype.save = function () {
                var _this = this;
                var command = {
                    Settings: {
                        TimeZone: this.$scope.timeZone,
                        UseEnterToSendChatMessage: this.$scope.useEnterToSendChatMessage,
                        UseWebSocket: this.$scope.useWebSocket,
                        ForumContiniousPagination: this.$scope.forumContiniousPagination
                    }
                };

                this.apiService.executeCommand("SaveAccountSettingsCommand", command, function () {
                    _this.eventBus.emit('user.notification', 'Настройки сохранены');
                });
            };

            /** Saves the user profile. */
            AccountSettingsPage.prototype.saveProfile = function () {
                var _this = this;
                var profile = this.$scope.profile;

                var command = {
                    About: profile.about,
                    Birthday: profile.birthday,
                    BitBucketLink: profile.bitbucket,
                    Career: profile.career,
                    Company: profile.company,
                    GitHubLink: profile.github,
                    HabrahabrLink: profile.habrahabr,
                    Location: profile.city,
                    Name: profile.name
                };

                this.apiService.executeCommand("SaveUserProfileCommand", command, function () {
                    if (_this.$scope.attachmentId != null) {
                        _this.saveAvatar(function () {
                            _this.eventBus.emit('user.notification', 'Профиль сохранен');
                            window.location.reload();
                        });
                    } else {
                        _this.eventBus.emit('user.notification', 'Профиль сохранен');
                    }
                });
            };

            AccountSettingsPage.prototype.saveAvatar = function (callback) {
                var command = {
                    AvatarAttachmentId: this.$scope.attachmentId
                };

                this.apiService.executeCommand("SetUserAvatarCommand", command, callback);
                this.$scope.attachmentId = null;
            };

            AccountSettingsPage.prototype.fileUploaded = function (attachmentId) {
                this.$scope.attachmentId = attachmentId;
                this.$scope.avatarUrl = Router.action('Attachment', 'Index', { attachmentId: attachmentId });
                this.$scope.$apply();
            };
            AccountSettingsPage.$inject = ['$scope', 'apiService', 'eventBus', 'config'];
            return AccountSettingsPage;
        })(MirGames.BasePage);
        Account.AccountSettingsPage = AccountSettingsPage;
    })(MirGames.Account || (MirGames.Account = {}));
    var Account = MirGames.Account;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=AccountSettingsPage.js.map
