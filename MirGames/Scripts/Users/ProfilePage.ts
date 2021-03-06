﻿/// <reference path="../_references.ts" />
module MirGames.Users {
    export interface IProfilePageData {
        userId: number;
        blogId: number;
    }

    export class ProfilePage extends MirGames.BasePage<IProfilePageData, IProfilePageScope> {
        static $inject = ['$scope', 'apiService', 'eventBus', '$http'];

        private userId: number;
        private intervalId: number;

        constructor($scope: IProfilePageScope, private apiService: Core.IApiService, eventBus: Core.IEventBus, private $http: ng.IHttpService) {
            super($scope, eventBus);
            this.userId = this.pageData.userId;

            $scope.deleteUser = this.deleteUser.bind(this);
            $scope.switchUser = this.switchUser.bind(this);
            $scope.newRecord = {
                focused: false,
                isTextFocused: false,
                isMicroPost: true,
                text: null,
                title: null,
                tags: null,
                charsCount: 0,
                maxChars: 512,
                attachments: [],
                isRepost: false,
                isTutorial: false,
                sourceAuthor: null,
                sourceLink: null,
                cancel: () => this.cancelNewRecord(),
                focus: () => this.focusNewRecord(),
                save: () => this.postRecord()
            };
        }

        private switchUser(): void {
            var command: MirGames.Domain.Users.Commands.LoginAsUserCommand = {
                UserId: this.userId
            };

            this.apiService.executeCommand('LoginAsUserCommand', command, (response) => {
                if (response != null) {
                    $.cookie('key', response, {
                        expires: 365 * 24 * 60 * 60,
                        path: '/'
                    });
                    Core.Application.getInstance().navigateToUrl(Router.action("Dashboard", "Index"));
                }
            });
        }

        private recordSizeChanged() {
            var $textarea = $('.new-topic-form textarea');
            var scrollHeight = $textarea.prop('scrollHeight');
            var height = $textarea.innerHeight();

            if (height < scrollHeight) {
                this.hideScrollbars();
            } else {
                this.showScrollbars();
            }
        }

        private focusNewRecord(): void {
            this.$scope.newRecord.focused = true;
            this.$scope.newRecord.text = null;
            this.$scope.newRecord.tags = null;
            this.$scope.newRecord.title = null;
            this.$scope.newRecord.sourceAuthor = null;
            this.$scope.newRecord.sourceLink = null;
            this.$scope.newRecord.isRepost = false;
            this.$scope.newRecord.isTutorial = false;
            this.$scope.newRecord.charsCount = 0;
            this.$scope.newRecord.isMicroPost = true;
            this.$scope.newRecord.attachments = [];

            setTimeout(() => {
                var $textarea = $('.new-topic-form textarea');
                $textarea.trigger('autosize.resize');

                this.intervalId = setInterval(() => {
                    var length = $textarea.val().length;
                    this.$scope.newRecord.isMicroPost = length <= this.$scope.newRecord.maxChars;

                    if (this.$scope.newRecord.charsCount != length) {
                        this.$scope.newRecord.charsCount = length;
                        this.$scope.$digest();
                    }
                }, 100);

                this.$scope.newRecord.isTextFocused = true;
                this.$scope.$digest();
            }, 0);
        }

        private cancelNewRecord(): void {
            this.$scope.newRecord.focused = false;
            clearInterval(this.intervalId);
        }

        private postRecord(): void {
            var command: MirGames.Domain.Topics.Commands.AddNewTopicCommand = {
                Attachments: this.$scope.newRecord.attachments,
                BlogId: this.pageData.blogId,
                Tags: this.$scope.newRecord.tags,
                Text: this.$scope.newRecord.text,
                Title: this.$scope.newRecord.title,
                IsRepost: this.$scope.newRecord.isRepost,
                IsTutorial: this.$scope.newRecord.isTutorial,
                SourceAuthor: this.$scope.newRecord.sourceAuthor,
                SourceLink: this.$scope.newRecord.sourceLink
            };

            this.apiService.executeCommand('AddNewTopicCommand', command, (topicId: number) => {
                var url = Router.action("Topics", "TopicListItem", { topicId: topicId, area: 'Topics' });

                this.$http.get(url).then((result) => {
                    $('.blog-posts').prepend(result.data);
                    $('.user-blog .not-found').hide();
                    this.cancelNewRecord();
                    this.eventBus.emit('user.notification', 'Пост успешно добавлен');
                });
            });
        }

        private wallRecordAdded(recordHtml: string) {
            var $comment = $(recordHtml);
            $('.new-wall-record').after($comment);
            this.$scope.newRecord.text = "";
            this.$scope.$apply();
            $comment.fadeOut(0).fadeIn();
        }

        private deleteUser(): void {
            var command: MirGames.Domain.Users.Commands.DeleteUserCommand = {
                UserId: this.userId
            };
            this.apiService.executeCommand('DeleteUserCommand', command, () => {
                Core.Application.getInstance().navigateToUrl(Router.action("Users", "Index"));
            });
        }
    }

    export interface IProfilePageScope extends IPageScope {
        deleteUser(): void;
        switchUser(): void;
        newRecord: {
            focused: boolean;
            text: string;
            title: string;
            tags: string;
            charsCount: number;
            attachments: number[];
            focus(): void;
            save(): void;
            cancel(): void;
            isMicroPost: boolean;
            isTextFocused: boolean;
            maxChars: number;
            isRepost: boolean;
            isTutorial: boolean;
            sourceAuthor: string;
            sourceLink: string;
        }
    }
}