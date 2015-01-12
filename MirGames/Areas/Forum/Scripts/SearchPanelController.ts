
/// <reference path="_references.ts" />
module MirGames.Forum {
    export interface ITopicsPageData extends IPageData {
        tag: string;
        searchString: string;
    }

    export interface ISearchPanelController extends ng.IScope {
        searchQuery: string;
        search(): void;
    }

    export class SearchPanelController {
        static $inject = ['$scope', '$element', 'pageDataService'];

        private pageData: ITopicsPageData;

        constructor(private $scope: ISearchPanelController, $element: JQuery, pageDataService: IPageDataProvider) {
            this.pageData = pageDataService.getPageData<ITopicsPageData>();
            this.$scope.search = this.search.bind(this);
            this.$scope.searchQuery = this.pageData.searchString;
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
}