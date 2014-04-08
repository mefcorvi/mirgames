/// <reference path="../_references.ts" />
var Core;
(function (Core) {
    var CurrentUser = (function () {
        function CurrentUser(pageData) {
            this.pageData = pageData;
            this.roleType = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
            this.actionClaimType = 'http://mirgames.ru/claims/action';
            this.userId = pageData.currentUser ? pageData.currentUser.Id : 0;
            this.claims = Enumerable.from(this.pageData.userClaims);
        }
        CurrentUser.prototype.getUserId = function () {
            return this.userId;
        };

        CurrentUser.prototype.can = function (action, entityType, entityId) {
            var overallClaimType = [this.actionClaimType, action, entityType].join('/') + '/';
            var claimType = [this.actionClaimType, action, entityType, entityId].join('/');

            return this.claims.any(function (claim) {
                return (claim.Type == claimType || claim.Type == overallClaimType) && claim.Value == 'allowed';
            });
        };

        CurrentUser.prototype.hasRole = function (role) {
            var _this = this;
            return this.claims.any(function (claim) {
                return claim.Type == _this.roleType && claim.Value == role;
            });
        };
        return CurrentUser;
    })();

    angular.module('core.currentUser', ['core.application']).factory('currentUser', ['pageData', function (pageDate) {
            return new CurrentUser(pageDate);
        }]);
})(Core || (Core = {}));
//# sourceMappingURL=CurrentUser.js.map
