 /// <reference path="_references.ts" />
module MirGames.Forum {
    export class PostRatingController {
        static $inject = ['$scope', '$element', 'apiService'];

        constructor(private $scope: IPostRatingScope, private $element: JQuery, private apiService: Core.IApiService) {
            this.$scope.voteRating = parseInt($('.vote-rating', $element).text(), 10);
            this.$scope.postId = parseInt($element.attr('data-post-id'), 10);
            this.$scope.voteUp = () => this.vote(true);
            this.$scope.voteDown = () => this.vote(false);
        }

        private vote(positive: boolean) {
            var command: MirGames.Domain.Forum.Commands.VoteForForumPostCommand = {
                Positive: positive,
                PostId: this.$scope.postId
            }

            this.apiService.executeCommand('VoteForForumPostCommand', command, (result: number) => {
                this.$scope.voteRating = result;
                this.$scope.votedUp = positive;
                this.$scope.votedDown = !positive;
                $('.vote-rating', this.$element).text(result);
                this.$scope.$apply();
            });
        }
    }

    export interface IPostRatingScope extends ng.IScope {
        postId: number;
        voteRating: number;
        votedUp: boolean;
        votedDown: boolean;
        voteUp(): void;
        voteDown(): void;
    }
}