/// <reference path="../_references.ts" />
module MirGames.Account {
    export class AccountSettingsPage extends MirGames.BasePage<IAccountSettingsPageData, IAccountSettingsPageScope> {
        static $inject = ['$scope', 'apiService', 'eventBus', 'config'];

        constructor($scope: IAccountSettingsPageScope, private apiService: Core.IApiService, eventBus: Core.IEventBus, private config: Core.IConfig) {
            super($scope, eventBus);
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
            this.$scope.useDarkTheme = settings.Theme == 'dark';
            this.$scope.headerType = settings.HeaderType || 'Fixed';

            this.$scope.save = this.save.bind(this);
            this.$scope.saveProfile = () => this.saveProfile();
            this.$scope.fileUploaded = this.fileUploaded.bind(this);

            this.$scope.linkAuthProvider = provider => this.linkProvider(provider);
            this.$scope.unlinkAuthProvider = provider => this.unlinkProvider(provider);

            this.$scope.birthdayDate = (d: Date) => {
                var currentYear = new Date().getFullYear();

                return d.getFullYear() > 1900 && d.getFullYear() <= (currentYear - 10);
            }

            this.$scope.updateTheme = () => setTimeout(() => this.updateTheme(), 0);
        }

        private linkProvider(provider: MirGames.Domain.Users.ViewModels.OAuthProviderViewModel) {
            var link = Router.action('OAuth', 'Authorize', { provider: provider.ProviderName });

            this.eventBus.emit('ajax-request.executing');
            $('<form action="' + link + '" method="POST"><input type="hidden" name="__RequestVerificationToken" value="' + this.config.antiForgery + '"></form>').submit();
        }

        private unlinkProvider(provider: MirGames.Domain.Users.ViewModels.OAuthProviderViewModel) {
            var command: MirGames.Domain.Users.Commands.DetachOAuthProviderCommand = {
                ProviderName: provider.ProviderName
            };

            this.apiService.executeCommand('DetachOAuthProviderCommand', command, () => {
                provider.IsLinked = false;
                this.eventBus.emit('user.notification', 'Аккаунт был успешно отсоединен от ' + provider.DisplayName);
                this.$scope.$apply();
            });
        }

        /** Saves the account settings. */
        private save(): void {
            var command: MirGames.Domain.Users.Commands.SaveAccountSettingsCommand = {
                Settings: {
                    TimeZone: this.$scope.timeZone,
                    UseEnterToSendChatMessage: this.$scope.useEnterToSendChatMessage,
                    UseWebSocket: this.$scope.useWebSocket,
                    ForumContiniousPagination: this.$scope.forumContiniousPagination,
                    Theme: this.getTheme(),
                    HeaderType: this.$scope.headerType
                }
            };

            this.apiService.executeCommand("SaveAccountSettingsCommand", command, () => {
                this.eventBus.emit('user.notification', 'Настройки сохранены');
            });
        }

        private getTheme(): string {
            return this.$scope.useDarkTheme ? 'dark' : 'light';
        }

        private updateTheme(): void {
            var theme = this.getTheme();
            var cssHref = $('link[rel=stylesheet]').attr('href');
            cssHref = cssHref.replace('dark', theme).replace('light', theme);
            $('link[rel=stylesheet]').attr('href', cssHref);
        }

        /** Saves the user profile. */
        private saveProfile(): void {
            var profile = this.$scope.profile;

            var command: MirGames.Domain.Users.Commands.SaveUserProfileCommand = {
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

            this.apiService.executeCommand("SaveUserProfileCommand", command, () => {
                if (this.$scope.attachmentId != null) {
                    this.saveAvatar(() => {
                        this.eventBus.emit('user.notification', 'Профиль сохранен');
                        window.location.reload();
                    });
                } else {
                    this.eventBus.emit('user.notification', 'Профиль сохранен');
                }
            });
        }

        private saveAvatar(callback: () => void): void {
            var command: MirGames.Domain.Users.Commands.SetUserAvatarCommand = {
                AvatarAttachmentId: this.$scope.attachmentId
            };

            this.apiService.executeCommand("SetUserAvatarCommand", command, callback);
            this.$scope.attachmentId = null;
        }

        private fileUploaded(attachmentId: number) {
            this.$scope.attachmentId = attachmentId;
            this.$scope.avatarUrl = Router.action('Attachment', 'Index', { attachmentId: attachmentId });
            this.$scope.$apply();
        }
    }

    export interface IAccountSettingsPageData extends IPageData {
        timeZone: string;
        user: MirGames.Domain.Users.ViewModels.UserViewModel;
        oauthProviders: MirGames.Domain.Users.ViewModels.OAuthProviderViewModel[];
    }

    export interface IAccountSettingsPageScope extends IPageScope {
        oauthProviders: MirGames.Domain.Users.ViewModels.OAuthProviderViewModel[];
        timeZone: string;
        useDarkTheme: boolean;
        attachmentId?: number;
        avatarUrl: string;
        headerType: string;
        useEnterToSendChatMessage: boolean;
        forumContiniousPagination: boolean;
        useWebSocket: boolean;
        save: () => void;
        fileUploaded(attachmentId: number): void;
        profile: IUserProfileScope;
        saveProfile: () => void;
        linkAuthProvider: (provider: MirGames.Domain.Users.ViewModels.OAuthProviderViewModel) => void;
        unlinkAuthProvider: (provider: MirGames.Domain.Users.ViewModels.OAuthProviderViewModel) => void;
        birthdayDate: (d: Date) => boolean;
        updateTheme: () => void;
    }

    export interface IUserProfileScope {
        name: string;
        city: string;
        birthday: Date;
        company: string;
        career: string;
        about: string;
        github: string;
        bitbucket: string;
        habrahabr: string;
    }
}