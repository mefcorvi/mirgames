/// <reference path="../_references.ts" />

module Core {
    export interface ICurrentUser {
        getUserId(): number;
        hasRole(role: string): boolean;
        can(action: string, entityType: string, entityId?: number): boolean;
    }

    class CurrentUser implements ICurrentUser {
        private roleType: string = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
        private actionClaimType: string = 'http://mirgames.ru/claims/action';

        private userId: number;
        private claims: linqjs.GenericEnumerable<MirGames.Domain.Users.ViewModels.UserClaimViewModel>;

        constructor(private pageData: ICurrentUserPageData) {
            this.userId = pageData.currentUser ? pageData.currentUser.Id : 0;
            this.claims = Enumerable.from(this.pageData.userClaims);
        }

        public getUserId(): number {
            return this.userId;
        }

        public can(action: string, entityType: string, entityId?: number): boolean {
            var overallClaimType = [this.actionClaimType, action, entityType].join('/') + '/';
            var claimType = [this.actionClaimType, action, entityType, entityId].join('/');

            return this.claims.any(claim => (claim.Type == claimType || claim.Type == overallClaimType) && claim.Value == 'allowed');
        }

        public hasRole(role: string): boolean {
            return this.claims.any(claim => claim.Type == this.roleType && claim.Value == role);
        }
    }

    interface ICurrentUserPageData {
        currentUser: MirGames.Domain.Users.ViewModels.CurrentUserViewModel;
        userClaims: MirGames.Domain.Users.ViewModels.UserClaimViewModel[];
    }

    angular
        .module('core.currentUser', ['core.application'])
        .factory('currentUser', ['pageData', (pageDate: ICurrentUserPageData) => new CurrentUser(pageDate)]);
} 