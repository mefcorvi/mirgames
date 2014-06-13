/// <reference path="../_references.ts" />

module Core {
    export interface ICurrentUser {
        getUserId(): number;
    }

    class CurrentUser implements ICurrentUser {
        private roleType: string = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
        private actionClaimType: string = 'http://mirgames.ru/claims/action';

        private userId: number;

        constructor(private pageData: ICurrentUserPageData) {
            this.userId = pageData.currentUser ? pageData.currentUser.Id : 0;
        }

        public getUserId(): number {
            return this.userId;
        }
    }

    interface ICurrentUserPageData {
        currentUser: MirGames.Domain.Users.ViewModels.CurrentUserViewModel;
    }

    angular
        .module('core.currentUser', ['core.application'])
        .factory('currentUser', ['pageData', (pageDate: ICurrentUserPageData) => new CurrentUser(pageDate)]);
} 