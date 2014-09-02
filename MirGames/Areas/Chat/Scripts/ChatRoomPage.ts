/// <reference path="_references.ts" />
module MirGames.Chat {
    export class ChatRoomPage extends MirGames.BasePage<IChatRoomPageData, IChatRoomPageScope> {
        static $inject = ['$scope', 'commandBus', 'eventBus', 'socketService', 'notificationService', 'apiService', 'currentUser', '$timeout'];

        private isActive: boolean = true;
        private markNextMessageUnread: boolean;
        private unreadCount: number = 0;
        private $textArea: JQuery;
        private $chatMessages: JQuery;
        private $footer: JQuery;
        private textWriting: boolean;
        private loadedPageNum: number;

        constructor($scope: IChatRoomPageScope, private commandBus: Core.ICommandBus, eventBus: Core.IEventBus, private socketService: Core.ISocketService, private notificationService: UI.INotificationService, private apiService: Core.IApiService, private currentUser: Core.ICurrentUser, private $timeout: ng.ITimeoutService) {
            super($scope, eventBus);

            this.$scope.editMode = false;
            this.$scope.playSound = true;
            this.$scope.historyAvailable = true;
            this.$scope.cancelEdit = () => this.cancelEdit();
            this.$scope.editMessage = message => this.editMessage(message);
            this.$scope.messages = [];
            this.$scope.useEnterToPost = this.pageData.currentUser ? this.pageData.currentUser.Settings.UseEnterToSendChatMessage : false;
            this.$scope.changeSendKey = () => this.changeSendKey();
            this.$scope.quoteLogin = text => this.quoteLogin(text);

            this.$textArea = $('.new-answer-form textarea');
            this.$footer = $('body > footer');
            this.$chatMessages = $('.chat-messages');

            $scope.reply = {
                message: null,
                attachments: [],
                post: this.reply.bind(this),
                focus: true,
                caret: 0
            };

            this.$scope.loadHistory = () => this.loadHistory();
            this.$scope.focusAnswer = () => this.focusAnswer();
            this.socketService.addHandler('chatHub', 'addNewMessageToPage', this.processReceivedMessage.bind(this));
            this.socketService.addHandler('chatHub', 'updateMessage', this.processUpdatedMessage.bind(this));

            this.eventBus.on('socket.disconnected', () => {
                this.addSystemMessage('Соединение разорвано');
            });

            this.eventBus.on('socket.reconnecting', () => {
                this.addSystemMessage('Переподключение...');
            });

            this.eventBus.on('socket.connected', () => {
                if (this.$scope.messages.length > 0) {
                    this.addSystemMessage('Соединение установлено');
                }

                this.loadLastMessages();
            });

            $(window).focus(() => this.handleChatActivation());
            $(window).blur(() => this.handleChatDeactivation());

            this.currentUserEnteredChat();
            setInterval(this.currentUserEnteredChat.bind(this), 10000);

            this.attachToTextEditor();
            $(window).on('beforeunload', this.currentUserLeavedChat.bind(this));

            $(() => {
                this.scrollToBottom();
                this.adjustTextAreaHeight();
                $('.new-answer-form .mdd_button').click(() => this.handleTextEditorToolbarClick());

                this.$textArea.css('max-height', '300px').autosize({
                    callback: () => this.adjustTextAreaHeight()
                });
            });
        }

        private quoteLogin(text: string): void {
            this.$scope.reply.message += '@' + text + ', ';
            this.$scope.reply.focus = true;
            this.$scope.reply.caret = this.$scope.reply.message.length;
        }

        private changeSendKey(): void {
            this.$scope.useEnterToPost = !this.$scope.useEnterToPost;
            var command: MirGames.Domain.Users.Commands.SaveAccountSettingsCommand = {
                Settings: {
                    UseEnterToSendChatMessage: this.$scope.useEnterToPost
                }
            };

            this.apiService.executeCommand("SaveAccountSettingsCommand", command, null, false);
        }

        /** Adds the system message. */
        private addSystemMessage(message: string) {
            var lastMessage = Enumerable.from(this.$scope.messages).lastOrDefault();

            this.$scope.$apply(() => {
                var scopeMessage: IChatMessageScope = {
                    authorId: -1,
                    avatarUrl: null,
                    date: lastMessage != null ? lastMessage.date : new Date(),
                    editDate: null,
                    id: -1,
                    login: null,
                    showAuthor: false,
                    showDate: true,
                    text: message,
                    dateFormat: 'HH:mm',
                    inChain: false,
                    isEditing: false,
                    ownMessage: false,
                    firstUnreadMessage: false,
                    canBeDeleted: false,
                    canBeEdited: false,
                    isSystem: true
                };

                this.$scope.messages.push(scopeMessage);
            });

            if (this.isScrollBottom()) {
                this.scrollToBottom();
            }
        }

        /** Loads history page */
        private loadHistory() {
            if (!this.$scope.historyAvailable) {
                return;
            }

            var currentFirst = Enumerable.from(this.$scope.messages).firstOrDefault(item => !item.isSystem);

            var query: MirGames.Domain.Chat.Queries.GetChatMessagesQuery = {
                LastIndex: currentFirst != null ? currentFirst.id : null,
                FirstIndex: null
            };

            var $firstItem = $('article[message-id]').first();
            this.loadMessages(query, $firstItem);
        }

        /** Loads last messages */
        private loadLastMessages() {
            var currentLast = Enumerable.from(this.$scope.messages).lastOrDefault(item => !item.isSystem);

            var query: MirGames.Domain.Chat.Queries.GetChatMessagesQuery = {
                FirstIndex: currentLast != null ? currentLast.id : null,
                LastIndex: null
            };

            this.loadMessages(query);
        }

        /** Loads messages */
        private loadMessages(query: MirGames.Domain.Chat.Queries.GetChatMessagesQuery, anchorItem?: JQuery) {
            var oldTop: number = null;

            if (anchorItem != null && anchorItem.length > 0) {
                oldTop = anchorItem.position().top;
            }

            this.apiService.getAll('GetChatMessagesQuery', query, 0, 50, (result: MirGames.Domain.Chat.ViewModels.ChatMessageViewModel[]) => {
                this.$scope.messages = Enumerable.from(result)
                    .select(message => this.convertMessage(message))
                    .concat(this.$scope.messages)
                    .orderBy(message => message.date)
                    .toArray();

                this.$scope.historyAvailable = result.length == 50;
                this.prepareMessages(0, this.$scope.messages.length);
                this.$scope.$apply();

                if (oldTop != null) {
                    var newPosition = anchorItem.position();
                    var oldScrollTop = this.getScrollTop();
                    this.setScrollTop(oldScrollTop + newPosition.top - oldTop);
                } else {
                    this.scrollToBottom();
                }
            }, false);
        }

        /** Prepares list of messages */
        private prepareMessages(startIndex: number, count: number) {
            for (var i = count - 1; i >= startIndex; i--) {
                var message = this.$scope.messages[i];
                var prevMessage = i > 0 ? this.$scope.messages[i - 1] : null;
                this.prepareMessage(message, prevMessage);
            }
        }

        /** Loads message for editing */
        private editMessage(message: IChatMessageScope) {
            var query: MirGames.Domain.Chat.Queries.GetChatMessageForEditQuery = {
                MessageId: message.id
            };

            this.apiService.getOne('GetChatMessageForEditQuery', query, (result: MirGames.Domain.Chat.ViewModels.ChatMessageForEditViewModel) => {
                this.$scope.$apply(() => {
                    this.$scope.reply.message = result.SourceText;
                    this.$scope.editMode = true;
                    message.isEditing = true;
                    this.$scope.editedMessage = message;
                    this.focusAnswer();
                });
                this.$textArea.trigger('autosize.resize');
            });
        }

        /** Loads last own message for edit */
        private loadLastMessageForEdit() {
            var lastOwnMessage = Enumerable.from(this.$scope.messages).last(item => item.ownMessage);
            this.editMessage(lastOwnMessage);
        }

        /** Cancels the editing. */
        private cancelEdit(): void {
            if (!this.$scope.editMode) {
                return;
            }

            this.$scope.reply.attachments = [];
            this.$scope.reply.message = '';
            this.$scope.editedMessage.isEditing = false;
            this.$scope.editedMessage = null;
            this.$scope.editMode = false;
            this.$scope.focusAnswer();

            setTimeout(() => this.adjustTextAreaHeight(), 0);
        }

        /** Handles text writing event. */
        private attachToTextEditor() {
            var timeout: number;
            var oldText: string;

            this.$textArea.keydown((ev: JQueryEventObject) => {
                if (ev.which == 38 && this.$textArea.val() == '') {
                    this.loadLastMessageForEdit();
                }

                if (ev.which == 27 && this.$scope.editMode) {
                    this.cancelEdit();
                    this.$scope.$apply();
                }

                setTimeout(() => {
                    var newText = this.$textArea.val();

                    if (newText == oldText) {
                        return;
                    }

                    this.sendTextWriting();
                    oldText = newText;

                    if (timeout) {
                        clearTimeout(timeout);
                        timeout = null;
                    }

                    timeout = setTimeout(() => {
                        this.sendTextWritingStopped();
                    }, 1000);
                }, 0);
            });
        }

        /** Mark current user as entering */
        private currentUserEnteredChat() {
            var command: MirGames.Domain.Users.Commands.AddOnlineUserTagCommand = {
                Tag: 'in-chat',
                ExpirationTime: 10000
            };
            this.socketService.executeCommand('AddOnlineUserTagCommand', command);
        }

        /** Marks current user as leaving */
        private currentUserLeavedChat() {
            var command: MirGames.Domain.Users.Commands.RemoveOnlineUserTagCommand = {
                Tag: 'in-chat',
            };
            this.socketService.executeCommand('RemoveOnlineUserTagCommand', command);
        }

        /** Handles click by toolbar button */
        private handleTextEditorToolbarClick() {
            setTimeout(() => {
                this.$textArea.trigger('autosize.resize');
                this.adjustTextAreaHeight();
            }, 0);
        }

        /** Processes the received message */
        private processReceivedMessage(messageJson: string) {
            var messageViewModel: MirGames.Domain.Chat.ViewModels.ChatMessageViewModel = JSON.parse(messageJson);
            var message = this.convertMessage(messageViewModel);

            if (!this.isActive) {
                this.unreadCount++;
                this.updateNotifications();

                if (this.markNextMessageUnread) {
                    message.firstUnreadMessage = true;
                    this.markNextMessageUnread = false;
                }
            }

            this.$scope.$apply(() => {
                this.$scope.messages.push(message);

                var prevMessage = this.$scope.messages[this.$scope.messages.length - 2];
                this.prepareMessage(message, prevMessage);

                if (this.isActive) {
                    if (this.isScrollBottom()) {
                        this.scrollToBottom();
                    }
                } else {
                    setTimeout(() => {
                        this.scrollToItem($('article.first-unread.message:last'));
                    }, 0);
                }
            });
        }

        /** Processes update message. */
        private processUpdatedMessage(message: MirGames.Domain.Chat.ViewModels.ChatMessageViewModel): void {
            var scopeMessage = Enumerable.from(this.$scope.messages).single(item => item.id == message.MessageId);
            this.$scope.$apply(() => {
                scopeMessage.text = message.Text;
                scopeMessage.editDate = new Date(message.UpdatedDate.toString());
            });
        }

        /** Sends notification whether user writing the message */
        private sendTextWriting() {
            if (this.textWriting) {
                return;
            }

            var command: MirGames.Domain.Users.Commands.AddOnlineUserTagCommand = {
                Tag: 'chat-writing',
                ExpirationTime: 30000
            };
            this.socketService.executeCommand('AddOnlineUserTagCommand', command);
            this.textWriting = true;
        }

        /** Sends notification whether user stops the writing */
        private sendTextWritingStopped() {
            if (!this.textWriting) {
                return;
            }

            var command: MirGames.Domain.Users.Commands.RemoveOnlineUserTagCommand = {
                Tag: 'chat-writing'
            };
            this.socketService.executeCommand('RemoveOnlineUserTagCommand', command);
            this.textWriting = false;
        }

        /** Adjusts text area height */
        private adjustTextAreaHeight() {
            var newHeight = $('body > footer .answer-form').height() + 20;

            if (newHeight > 20) {
                $('body').css('padding-bottom', newHeight);
                this.$footer.css('height', newHeight);
            }
        }

        /** Updates notifications */
        private updateNotifications() {
            if (this.unreadCount > 0) {
                this.notificationService.setBubble(this.unreadCount, this.$scope.playSound);
            } else {
                this.notificationService.reset();
            }
        }

        /** Handles chat activation */
        private handleChatActivation(): void {
            this.isActive = true;
            this.unreadCount = 0;
            this.updateNotifications();
        }

        /** Handles chat deactivation */
        private handleChatDeactivation(): void {
            this.isActive = false;
            this.markNextMessageUnread = true;
        }

        /** Prepares received message */
        private prepareMessage(message: IChatMessageScope, prevMessage: IChatMessageScope) {
            if (message.isSystem) {
                return;
            }

            if (prevMessage && !prevMessage.isSystem) {
                message.showAuthor = message.authorId != prevMessage.authorId;
                message.showDate = message.date.getTime() > (prevMessage.date.getTime() + 60000)
                || message.date.getMinutes() != prevMessage.date.getMinutes();

                if (!message.showAuthor) {
                    prevMessage.inChain = true;
                }
            }

            var currentDate = new Date();
            var day = 24 * 60 * 60 * 1000;

            if (message.date.getTime() > (currentDate.getTime() + day) || message.date.getDate() != currentDate.getDate()) {
                message.dateFormat = 'dd.MM.yy HH:mm';
            }
        }

        /** Converts message from viewmodel to scope */
        private convertMessage(message: MirGames.Domain.Chat.ViewModels.ChatMessageViewModel): IChatMessageScope {
            var scopeMessage: IChatMessageScope = {
                authorId: message.Author.Id,
                avatarUrl: message.Author.AvatarUrl,
                date: new Date(message.CreatedDate.toString()),
                editDate: message.UpdatedDate ? new Date(message.UpdatedDate.toString()) : null,
                id: message.MessageId,
                login: message.Author.Login,
                showAuthor: true,
                showDate: true,
                text: message.Text,
                dateFormat: 'HH:mm',
                inChain: false,
                isEditing: false,
                ownMessage: message.Author.Id == this.currentUser.getUserId(),
                firstUnreadMessage: false,
                canBeDeleted: message.CanBeDeleted,
                canBeEdited: message.CanBeEdited,
                isSystem: false
            };

            this.updateAccessRight(scopeMessage);

            return scopeMessage;
        }

        /** Checks whether user still have an access to the message */
        private updateAccessRight(message: IChatMessageScope) {
            var messageFreezeMoment = moment(message.date).add('m', 5);
            var isMessageFrozen = messageFreezeMoment.isBefore();

            message.canBeDeleted = this.pageData.isAdmin || (!isMessageFrozen && message.canBeEdited);
            message.canBeEdited = this.pageData.isAdmin || (!isMessageFrozen && message.canBeDeleted);

            if (!isMessageFrozen) {
                this.$timeout(() => this.updateAccessRight(message), messageFreezeMoment.diff(moment()) + 50, true);
            }
        }

        /** Focus text area */
        private focusAnswer(): void {
            this.$scope.reply.focus = true;
        }

        /** Sends the reply */
        private reply(): void {
            if (this.$scope.postAnswerForm.$invalid) {
                return;
            }

            if (this.$scope.editMode) {
                var updateCommand: MirGames.Domain.Chat.Commands.UpdateChatMessageCommand = {
                    Attachments: this.$scope.reply.attachments,
                    Message: this.$scope.reply.message,
                    MessageId: this.$scope.editedMessage.id
                };
                this.socketService.executeCommand('UpdateChatMessageCommand', updateCommand);
                this.cancelEdit();
            } else {
                var command: MirGames.Domain.Chat.Commands.PostChatMessageCommand = {
                    Attachments: this.$scope.reply.attachments,
                    Message: this.$scope.reply.message
                };

                this.socketService.executeCommand('PostChatMessageCommand', command);
            }

            this.$scope.safeApply(() => {
                this.$scope.editedMessage = null;
                this.$scope.editMode = false;
                this.$scope.reply.attachments = [];
                this.$scope.reply.message = "";

                setTimeout(() => {
                    this.$textArea.trigger('autosize.resize');
                }, 0);
            });

            this.sendTextWritingStopped();
        }
    }

    export interface IChatRoomPageData extends IPageData {
    }

    export interface IChatRoomPageScope extends IPageScope {
        reply: {
            message: string;
            attachments: number[];
            post(): void;
            focus: boolean;
            caret: number;
        };
        editedMessage?: IChatMessageScope;
        editMode: boolean;
        editMessage(message: IChatMessageScope): void;
        cancelEdit(): void;
        messages: IChatMessageScope[];
        focusAnswer: () => void;
        playSound: boolean;
        postAnswerForm: ng.IFormController;
        loadHistory: () => void;
        changeSendKey: () => void;
        historyAvailable: boolean;
        useEnterToPost: boolean;
        quoteLogin: (text: string) => void;
    }

    export interface IChatMessageScope {
        id: number;
        text: string;
        avatarUrl: string;
        login: string;
        authorId: number;
        date: Date;
        editDate?: Date;
        showDate: boolean;
        showAuthor: boolean;
        inChain: boolean;
        isSystem: boolean;
        dateFormat: string;
        ownMessage: boolean;
        isEditing: boolean;
        firstUnreadMessage: boolean;
        canBeEdited: boolean;
        canBeDeleted: boolean;
    }
}