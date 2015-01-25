
/// <reference path="_references.ts" />
module MirGames.Forum {
    export interface ISearchPanelController extends ng.IScope {
        searchString: string;
        search(): void;
    }

    export class SearchPanelController {
        static $inject = ['$scope', '$element', 'pageDataService', '$location'];

        private pageData: ISearchPanelPageData;

        constructor(private $scope: ISearchPanelController, $element: JQuery, pageDataService: IPageDataProvider, private $location: ng.ILocationService) {
            this.pageData = pageDataService.getPageData<ISearchPanelPageData>();
            this.$scope.searchString = this.pageData.searchString;
            this.$scope.search = () => this.search();
        }

        private search() {
            var args: any = {
                area: 'Forum'
            };

            if (this.$scope.searchString) {
                args.searchString = this.$scope.searchString;
            }

            if (this.pageData.forumAlias) {
                args.forumAlias = this.pageData.forumAlias;
            }

            if (this.pageData.tag) {
                args.tag = this.pageData.tag;
            }

            var address = Router.action('Forum', 'Topics', args);

            this.$location.url(address);
        }
    }

    export interface ISearchPanelPageData {
        searchString: string;
        forumAlias: string;
        tag: string;
    }
}