var MirGames;
(function (MirGames) {
    /// <reference path="../_references.ts" />
    (function (Users) {
        var SearchPanelController = (function () {
            function SearchPanelController($scope, $element, pageData) {
                this.$scope = $scope;
                this.pageData = pageData;
                this.$scope.search = this.search.bind(this);
                this.$scope.searchQuery = pageData.searchString;
                this.$scope.action = pageData.action;
            }
            SearchPanelController.prototype.search = function () {
                var params = {};

                if (!Utils.isNullOrEmpty(this.$scope.searchQuery)) {
                    params['searchString'] = this.$scope.searchQuery;
                }

                Core.Application.getInstance().navigateToUrl(Router.action("Users", this.$scope.action, params));
            };
            SearchPanelController.$inject = ['$scope', '$element', 'pageData'];
            return SearchPanelController;
        })();
        Users.SearchPanelController = SearchPanelController;
    })(MirGames.Users || (MirGames.Users = {}));
    var Users = MirGames.Users;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=SearchPanelController.js.map
