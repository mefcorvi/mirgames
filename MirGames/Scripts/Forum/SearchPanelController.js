var MirGames;
(function (MirGames) {
    /// <reference path="../_references.ts" />
    (function (Forum) {
        var SearchPanelController = (function () {
            function SearchPanelController($scope, $element, pageData) {
                this.$scope = $scope;
                this.pageData = pageData;
                this.$scope.search = this.search.bind(this);
                this.$scope.searchQuery = pageData.searchString;
            }
            SearchPanelController.prototype.search = function () {
                var params = {};

                if (!Utils.isNullOrEmpty(this.$scope.searchQuery)) {
                    params['searchString'] = this.$scope.searchQuery;
                }

                if (!Utils.isNullOrEmpty(this.pageData.tag)) {
                    params['tag'] = this.pageData.tag;
                }
                Core.Application.getInstance().navigateToUrl(Router.action("Forum", "Index", params));
            };
            SearchPanelController.$inject = ['$scope', '$element', 'pageData'];
            return SearchPanelController;
        })();
        Forum.SearchPanelController = SearchPanelController;
    })(MirGames.Forum || (MirGames.Forum = {}));
    var Forum = MirGames.Forum;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=SearchPanelController.js.map
