/// <reference path="_references.ts" />
module MirGames.Projects {
    export class AddNewWorkItemDialogController {
        static $inject = ['$scope', 'dialog', 'apiService', 'dialogOptions', 'eventBus'];

        constructor(private $scope: IAddNewWorkItemDialogControllerScope, private dialog: UI.IDialog, private apiService: Core.IApiService, private options: any, private eventBus: Core.IEventBus) {
            $scope.type = <MirGames.Domain.Wip.ViewModels.WorkItemType>options['item-type'];
            $scope.projectAlias = <string>options['project-alias'];
            $scope.focus = true;
            $scope.post = () => this.postNewItem();
            $scope.typeNames = ['', 'бага', 'задача', 'фича'];

            $scope.close = () => {
                dialog.cancel();
            }
        }

        /** Posts the new item */
        private postNewItem(): void {
            var command: Domain.Wip.Commands.CreateNewProjectWorkItemCommand = {
                ProjectAlias: this.$scope.projectAlias,
                Title: this.$scope.title,
                Tags: this.$scope.tags,
                Type: this.$scope.type,
                Attachments: this.$scope.attachments,
                Description: this.$scope.text,
                AssignedTo: null
            };

            this.apiService.executeCommand('CreateNewProjectWorkItemCommand', command, (internalId: number) => {
                this.eventBus.emit(this.$scope.projectAlias + '.workitems.new', internalId);
                this.eventBus.emit('user.notification', 'Новая ' + this.$scope.typeNames[this.$scope.type] + ' успешно создана');
                this.$scope.close();
            });
        }
    }

    export interface IAddNewWorkItemDialogControllerScope extends ng.IScope {
        projectAlias: string;

        title: string;
        text: string;
        tags: string;
        post: () => void;
        attachments: number[];
        type: Domain.Wip.ViewModels.WorkItemType;
        typeNames: string[];
        focus: boolean;

        close(): void;
    }
} 