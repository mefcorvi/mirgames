/// <reference path="_references.ts" />
module MirGames.Wip {
    export class NewProjectPage extends MirGames.BasePage<IProjectNewPageData, IProjectNewPageScope> {
        static $inject = ['$scope', 'apiService', 'eventBus'];

        constructor($scope: IProjectNewPageScope, private apiService: Core.IApiService, eventBus: Core.IEventBus) {
            super($scope, eventBus);
            this.$scope.attachments = [];
            this.$scope.showPreview = true;
            this.$scope.isTitleFocused = true;
            this.$scope.repository = "";

            this.$scope.save = () => this.save();
            this.$scope.fileUploaded = (attachmentId) => this.fileUploaded(attachmentId);

            this.initializeTags();
        }

        private initializeTags() {
            $('.topic-tags').selectize({
                load: (query, callback) => {
                    var command: MirGames.Domain.Wip.Queries.GetWipTagsQuery = {
                        Filter: query
                    };

                    this.apiService.getAll('GetWipTagsQuery', command, 1, 30, (result: string[]) => {
                        var items = Enumerable.from(result).select(r => {
                            return { text: r, value: r };
                        }).toArray();

                        callback(items);
                    });
                },
                create: (input) => {
                        return { text: input, value: input }
                    }
            });
        }

        private fileUploaded(attachmentId: number) {
            this.$scope.attachmentId = attachmentId;
            this.$scope.logoUrl = Router.action('Attachment', 'Index', { attachmentId: attachmentId });
            this.$scope.$apply();
        }

        private save() {
            var command: MirGames.Domain.Wip.Commands.CreateNewWipProjectCommand = 
            {
                Title: this.$scope.title,
                Alias: this.$scope.name,
                Tags: this.$scope.tags,
                RepositoryType: this.$scope.repository,
                LogoAttachmentId: this.$scope.attachmentId,
                Attachments: this.$scope.attachments,
                Description: this.$scope.description,
            };

            this.apiService.executeCommand("CreateNewWipProjectCommand", command, () => {
                Core.Application.getInstance().navigateTo("Projects", "Project", { projectAlias: this.$scope.name });
            });
        }
    }

    export interface IProjectNewPageScope extends IPageScope {
        title: string;
        name: string;
        tags: string;
        repository: string;
        attachmentId: number;
        attachments: number[];
        description: string;

        logoUrl: string;
        showPreview: boolean;
        isTitleFocused: boolean;
        fileUploaded: (attachmentId: number) => void;
        save: () => void;
    }

    export interface IProjectNewPageData {
    }

    angular
        .module('mirgames')
        .directive('uniqueProjectName', ['apiService',
            (apiService: Core.IApiService) => {
                var toId: number;

                return {
                    restrict: 'A',
                    require: 'ngModel',
                    link: (scope: ng.IScope, elem: JQuery, attr: any, ctrl: any) => {
                        scope.$watch(attr.ngModel, (value: string) => {
                            if (toId != null) {
                                clearTimeout(toId);
                            }

                            toId = setTimeout(() => {
                                var query: MirGames.Domain.Wip.Queries.GetIsProjectNameUniqueQuery = {
                                    Alias: value
                                };

                                apiService.getOne('GetIsProjectNameUniqueQuery', query, (result: boolean) => {
                                    ctrl.$setValidity('uniqueProjectName', result);
                                    scope.$apply();
                                }, false);
                            }, 200);
                        });
                    }
                }
            }
        ]);
}