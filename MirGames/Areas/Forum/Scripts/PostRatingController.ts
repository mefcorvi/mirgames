 /// <reference path="_references.ts" />
module MirGames.Forum {
    export class PostRatingController {
        static $inject = ['$scope', '$element', 'apiService'];

        constructor(private $scope: IPostRatingScope, private $element: JQuery, private apiService: Core.IApiService) {
            this.$scope.voteUp = () => this.vote(true);
            this.$scope.voteDown = () => this.vote(false);
        }

        private vote(positive: boolean) {
            ; var command: MirGames.Domain.Forum.Commands.VoteForForumPostCommand = {
                Positive: positive,
                PostId: parseInt(this.$element.attr('data-post-id'), 10)
            }

            this.apiService.executeCommand('VoteForForumPostCommand', command, (result: number) => {
                this.$scope.voteRating = result;
                this.$scope.votedUp = positive;
                this.$scope.votedDown = !positive;

                if (positive) {
                    this.$element.removeClass('voted-down');
                } else {
                    this.$element.removeClass('voted-up');
                }

                if (result > 0) {
                    $('.negative', this.$element).removeClass('negative');
                } else {
                    $('.positive', this.$element).removeClass('positive');
                }

                $('.vote-rating', this.$element).text(result);
                this.$scope.$apply();
            });
        }
    }

    export interface IPostRatingScope extends ng.IScope {
        voteRating: number;
        votedUp: boolean;
        votedDown: boolean;
        voteUp(): void;
        voteDown(): void;
    }
}