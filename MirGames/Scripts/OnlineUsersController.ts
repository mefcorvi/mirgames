/// <reference path="_references.ts" />
module MirGames {
    export class OnlineUsersController {
        static $inject = ['$scope', '$element', 'socketService'];

        private isActive: boolean;

        constructor(private $scope: IOnlineUsersScope, private $element: JQuery, private socketService: Core.ISocketService) {
            var eventsHub = (<any>$).connection.eventsHub;
            var pageData = <IPageData>window['pageData'];
            this.$scope.users = [];

            if (pageData.currentUser != null && pageData.currentUser.Id > 0) {
                this.handleActivation();
                $(window).focus(() => this.handleActivation());
                $(window).blur(() => this.handleDeactivation());

                setInterval(() => {
                    if (this.isActive) {
                        this.handleActivation();
                    } else {
                        this.handleDeactivation();
                    }
                }, 5000);
            }

            for (var i = 0; i < pageData.onlineUsers.length; i++) {
                var onlineUser = pageData.onlineUsers[i];
                var tags = pageData.onlineUserTags[onlineUser.Id];

                this.addUser(onlineUser, tags);
            }

            socketService.addHandler('eventsHub', 'userOnlineTagAdded', (userId?: number, tag?: string) => {
                this.$scope.$apply(() => {
                    var user = this.getUser(userId);

                    if (user != null) {
                        user.tags.push(tag);
                    }
                });
            });

            socketService.addHandler('eventsHub', 'userOnlineTagRemoved', (userId?: number, tag?: string) => {
                this.$scope.$apply(() => {
                    var user = this.getUser(userId);

                    if (user == null) {
                        return;
                    }

                    user.tags = Enumerable.from(user.tags).where(item => item != tag).toArray();
                });
            });

            socketService.addHandler('eventsHub', 'userOnline', (userJson?: string) => {
                var user = JSON.parse(userJson);
                this.$scope.$apply(() => {
                    this.addUser(user);
                });
            });

            socketService.addHandler('eventsHub', 'userOffline', (userJson?: string) => {
                var user = JSON.parse(userJson);
                this.$scope.$apply(() => {
                    this.removeUser(user.Id);
                });
            });
        }

        private getUser(userId: number): IOnlineUserScope {
            return Enumerable.from(this.$scope.users).firstOrDefault(x => x.id == userId);
        }

        private convertUser(user: MirGames.Domain.Users.ViewModels.OnlineUserViewModel): IOnlineUserScope {
            var userScope: IOnlineUserScope;

            userScope = {
                avatarUrl: user.AvatarUrl,
                id: user.Id,
                login: user.Login,
                userUrl: Router.action('Users', 'Profile', { userId: user.Id }),
                tags: [],
                hasTag: (tag) => {
                    return userScope.tags.indexOf(tag) != -1;
                }
            };

            return userScope;
        }

        private addUser(user: MirGames.Domain.Users.ViewModels.OnlineUserViewModel, tags: string[] = []) {
            this.removeUser(user.Id);
            var scopeUser = this.convertUser(user);
            scopeUser.tags = tags;
            this.$scope.users.splice(0, 0, scopeUser);
        }

        private removeUser(userId: number) {
            for (var i = this.$scope.users.length - 1; i >= 0; i--) {
                var user = this.$scope.users[i];

                if (user.id == userId) {
                    this.$scope.users.splice(i, 1);
                }
            }
        }

        private handleActivation() {
            this.isActive = true;
            var command: MirGames.Domain.Users.Commands.RemoveOnlineUserTagCommand = {
                Tag: 'away',
                ExpirationTime: 10000
            };
            this.socketService.executeCommand('RemoveOnlineUserTagCommand', command);
        }

        private handleDeactivation() {
            this.isActive = false;
            var command: MirGames.Domain.Users.Commands.AddOnlineUserTagCommand = {
                Tag: 'away',
                ExpirationTime: 10000
            };
            this.socketService.executeCommand('AddOnlineUserTagCommand', command);
        }
    }

    export interface IOnlineUsersScope extends ng.IScope {
        users: IOnlineUserScope[];
    }

    export interface IOnlineUserScope {
        avatarUrl: string;
        login: string;
        id: number;
        userUrl: string;
        tags: string[];
        hasTag: (tag: string) => boolean;
    }
}