
/// <reference path="../_references.ts" />
module MirGames.Forum {
    export class SearchPanelController {
        static $inject = ['$scope', '$element', 'pageData'];

        constructor(private $scope: ISearchPanelController, $element: JQuery, public pageData: ITopicsPageData) {
            this.$scope.search = this.search.bind(this);
            this.$scope.searchQuery = pageData.searchString;
        }

        private search() {
            var params : {
                searchString?: string;
                tag?: string;
            } = {};

            if (!Utils.isNullOrEmpty(this.$scope.searchQuery)) {
                params['searchString'] = this.$scope.searchQuery;
            }

            if (!Utils.isNullOrEmpty(this.pageData.tag)) {
                params['tag'] = this.pageData.tag;

            }
            Core.Application.getInstance().navigateToUrl(Router.action("Forum", "Index", params));
        }
    }

    export interface ITopicsPageData {
        tag: string;
        searchString: string;
    }

    export interface ISearchPanelController extends ng.IScope {
        searchQuery: string;
        search(): void;
    }
}