
/// <reference path="../_references.ts" />
module MirGames.Users {
    export class SearchPanelController {
        static $inject = ['$scope', '$element', 'pageData'];

        constructor(private $scope: ISearchPanelController, $element: JQuery, public pageData: IUsersPageData) {
            this.$scope.search = this.search.bind(this);
            this.$scope.searchQuery = pageData.searchString;
            this.$scope.action = pageData.action;
        }

        private search() {
            var params: {
                searchString?: string;
            } = {};

            if (!Utils.isNullOrEmpty(this.$scope.searchQuery)) {
                params['searchString'] = this.$scope.searchQuery;
            }

            Core.Application.getInstance().navigateToUrl(Router.action("Users", this.$scope.action, params));
        }
    }

    export interface IUsersPageData {
        action: string;
        searchString: string;
    }

    export interface ISearchPanelController extends ng.IScope {
        action: string;
        searchQuery: string;
        search(): void;
    }
}