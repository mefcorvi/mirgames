/// <reference path="../_references.ts" />
var Core;
(function (Core) {
    var CurrentUser = (function () {
        function CurrentUser(pageData) {
            this.pageData = pageData;
            this.roleType = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
            this.actionClaimType = 'http://mirgames.ru/claims/action';
            this.userId = pageData.currentUser ? pageData.currentUser.Id : 0;
        }
        CurrentUser.prototype.getUserId = function () {
            return this.userId;
        };
        return CurrentUser;
    })();

    angular.module('core.currentUser', ['core.application']).factory('currentUser', ['pageData', function (pageDate) {
            return new CurrentUser(pageDate);
        }]);
})(Core || (Core = {}));
//# sourceMappingURL=CurrentUser.js.map
