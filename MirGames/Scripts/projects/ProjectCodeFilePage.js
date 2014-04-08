var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var MirGames;
(function (MirGames) {
    /// <reference path="../_references.ts" />
    (function (Wip) {
        var ProjectCodeFilePage = (function (_super) {
            __extends(ProjectCodeFilePage, _super);
            function ProjectCodeFilePage($scope, commandBus, eventBus) {
                _super.call(this, $scope, eventBus);
                this.commandBus = commandBus;
            }
            ProjectCodeFilePage.$inject = ['$scope', 'commandBus', 'eventBus'];
            return ProjectCodeFilePage;
        })(MirGames.BasePage);
        Wip.ProjectCodeFilePage = ProjectCodeFilePage;
    })(MirGames.Wip || (MirGames.Wip = {}));
    var Wip = MirGames.Wip;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=ProjectCodeFilePage.js.map
