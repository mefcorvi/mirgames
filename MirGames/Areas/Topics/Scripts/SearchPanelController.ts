/// <reference path="_references.ts" />
module MirGames.Topics {
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
                area: 'Topics'
            };

            var action: string = 'Index';

            if (this.$scope.searchString) {
                args.searchString = this.$scope.searchString;
            }

            if (this.pageData.tag) {
                args.tag = this.pageData.tag;
            }

            switch (this.pageData.subsection) {
                case 'Microtopics':
                    action = 'MicroTopics';
                    break;
                case 'Tutorials':
                    action = 'Tutorials';
                    break;
                case 'All':
                    action = 'AllPosts';
                    break;
            }

            var address = Router.action('Topics', action, args);
            this.$location.url(address);
        }
    }

    export interface ISearchPanelPageData {
        searchString: string;
        tag: string;
        subsection: string;
    }

    export interface ISearchPanelController extends ng.IScope {
        search(): void;
        searchString: string;
    }
}