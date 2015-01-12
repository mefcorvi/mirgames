/// <reference path="_references.ts" />
module MirGames.Chat {
    export class ChatRoomPage extends MirGames.BasePage<IChatRoomPageData, IChatRoomPageScope> {
        static $inject = ['$scope', 'eventBus', 'socketService', 'notificationService', 'apiService', 'currentUser', '$timeout'];

        private isActive: boolean = true;
        private markNextMessageUnread: boolean;
        private unreadCount: number = 0;
        private $textArea: JQuery;
        private $chatMessages: JQuery;
        private $footer: JQuery;
        private textWriting: boolean;
        private loadedPageNum: number;
        private newMessageHandler: Core.ISocketHandler;
        private updatedMessageHandler: Core.ISocketHandler;

        constructor($scope: IChatRoomPageScope, eventBus: Core.IEventBus, private socketService: Core.ISocketService, private notificationService: UI.INotificationService, private apiService: Core.IApiService, private currentUser: Core.ICurrentUser, private $timeout: ng.ITimeoutService) {
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
            this.$scope.loadHistory = () => this.loadHistory();
            this.$scope.focusAnswer = () => this.focusAnswer();

            this.$textArea = $('.new-answer-form textarea');
            this.$footer = $('body > .chat-answer > .answer-form');
            this.$chatMessages = $('.chat-messages');

            $scope.reply = {
                message: null,
                attachments: [],
                post: this.reply.bind(this),
                focus: true,
                caret: 0,
                adjustTextAreaHeight: () => this.adjustTextAreaHeight()
            };

            this.loadLastMessages();

            this.currentUserEnteredChat();
            setInterval(this.currentUserEnteredChat.bind(this), 5000);

            this.attachToTextEditor();

            $(() => {
                this.scrollToBottom();
                this.adjustTextAreaHeight();
                $('.new-answer-form .mdd_button').click(() => this.handleTextEditorToolbarClick());

                this.$textArea.css('max-height', '300px');
            });

            this.attachHandlers();
            this.$scope.$on('$destroy', () => {
                this.detachHandlers();
            });
        }

        private attachHandlers() {
            this.newMessageHandler = this.socketService.addHandler('chatHub', 'addNewMessageToPage', this.processReceivedMessage.bind(this));
            this.updatedMessageHandler = this.socketService.addHandler('chatHub', 'updateMessage', this.processUpdatedMessage.bind(this));

            this.eventBus.on('socket.disconnected.chatPage', () => {
                this.addSystemMessage('Соединение разорвано');
            });

            this.eventBus.on('socket.reconnecting.chatPage', () => {
                this.addSystemMessage('Переподключение...');
            });

            this.eventBus.on('socket.connected.chatPage', () => {
                if (this.$scope.messages.length > 0) {
                    this.addSystemMessage('Соединение установлено');
                }
            });

            $(window).bind('focus.chatPage', () => this.handleChatActivation());
            $(window).bind('blur.chatPage', () => this.handleChatDeactivation());
            $(window).bind('beforeunload.chatPage', this.currentUserLeavedChat.bind(this));
        }

        private detachHandlers() {
            $(window).unbind('focus.chatPage');
            $(window).unbind('blur.chatPage');
            $(window).unbind('beforeunload.chatPage');
            this.eventBus.removeAllListeners('socket.disconnected.chatPage');
            this.eventBus.removeAllListeners('socket.reconnecting.chatPage');
            this.eventBus.removeAllListeners('socket.connected.chatPage');
            this.socketService.removeHandler(this.newMessageHandler);
            this.socketService.removeHandler(this.updatedMessageHandler);
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
            var scopeMessage: IChatMessageScope = {
                createdDate: moment(),
                authorId: -1,
                avatarUrl: null,
                date: null,
                editDate: null,
                id: -1,
                login: null,
                showAuthor: false,
                showDate: false,
                text: message,
                firstInChain: false,
                firstInDay: false,
                isEditing: false,
                ownMessage: false,
                firstUnreadMessage: false,
                canBeDeleted: false,
                canBeEdited: false,
                isSystem: true
            };

            this.$scope.messages.push(scopeMessage);
            this.$scope.$digest();

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
            var oldScrollTop: number = null;

            if (anchorItem != null && anchorItem.length > 0) {
                oldTop = anchorItem.position().top;
                oldScrollTop = this.getScrollTop();
            }

            this.apiService.getAll('GetChatMessagesQuery', query, 0, 50, (result: MirGames.Domain.Chat.ViewModels.ChatMessageViewModel[]) => {
                this.$scope.messages = Enumerable.from(result)
                    .select(message => this.convertMessage(message))
                    .concat(this.$scope.messages)
                    .orderBy(message => message.id)
                    .toArray();

                this.$scope.historyAvailable = result.length == 50;
                this.prepareMessages(0, this.$scope.messages.length);
                this.$scope.$digest();

                if (oldTop != null) {
                    var newTop = anchorItem.position().top;
                    this.setScrollTop(oldScrollTop + newTop - oldTop);
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
            this.cancelEdit();

            var query: MirGames.Domain.Chat.Queries.GetChatMessageForEditQuery = {
                MessageId: message.id
            };

            this.apiService.getOne('GetChatMessageForEditQuery', query, (result: MirGames.Domain.Chat.ViewModels.ChatMessageForEditViewModel) => {
                this.$scope.reply.message = result.SourceText;
                this.$scope.editMode = true;
                message.isEditing = true;
                this.$scope.editedMessage = message;
                this.$scope.reply.caret = result.SourceText.length;
                this.focusAnswer();
                this.$scope.$digest();
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
                    this.$scope.$digest();
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
            this.$scope.$digest();
        }

        /** Processes update message. */
        private processUpdatedMessage(message: MirGames.Domain.Chat.ViewModels.ChatMessageViewModel): void {
            var scopeMessage = Enumerable.from(this.$scope.messages).single(item => item.id == message.MessageId);
            this.$scope.$apply(() => {
                var newScopeMessage = this.convertMessage(message);
                scopeMessage.text = newScopeMessage.text;
                scopeMessage.editDate = newScopeMessage.editDate;
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
            var newHeight = $('body > .chat-answer .new-answer-form').outerHeight() + 10;

            if (newHeight > 10) {
                var oldScrollTop = this.getScrollTop();
                var isBottom = this.isScrollBottom();

                $('body').css('padding-bottom', newHeight);
                this.$footer.css('height', newHeight);

                if (isBottom) {
                    this.scrollToBottom(0);
                } else {
                    this.scrollTo(oldScrollTop, 0);
                }
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
                message.showDate = message.date != prevMessage.date;
                message.firstInDay = message.createdDate.format('L') != prevMessage.createdDate.format('L');

                if (message.showAuthor) {
                    message.firstInChain = true;
                }
            }
        }


        /** Converts message from viewmodel to scope */
        private convertMessage(message: MirGames.Domain.Chat.ViewModels.ChatMessageViewModel): IChatMessageScope {
            var createdDate = moment(message.CreatedDate.toString());
            var editDate = message.UpdatedDate ? moment(message.UpdatedDate.toString()) : null;

            var createdDateString = createdDate.format('HH:mm');
            var editDateString = editDate != null ? editDate.format('HH:mm') : null;

            var scopeMessage: IChatMessageScope = {
                createdDate: createdDate,
                authorId: message.Author.Id,
                avatarUrl: message.Author.AvatarUrl,
                date: createdDateString,
                editDate: editDateString,
                id: message.MessageId,
                login: message.Author.Login,
                showAuthor: true,
                showDate: true,
                text: message.Text,
                firstInChain: false,
                firstInDay: false,
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
            var messageFreezeMoment = message.createdDate.add('m', 5);
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

            this.$scope.editedMessage = null;
            this.$scope.editMode = false;
            this.$scope.reply.attachments = [];
            this.$scope.reply.message = "";

            setTimeout(() => {
                this.$textArea.trigger('autosize.resize');
            }, 0);

            this.$scope.$digest();

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
            adjustTextAreaHeight: () => void;
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
        createdDate: Moment;
        date: string;
        editDate?: string;
        showDate: boolean;
        showAuthor: boolean;
        firstInChain: boolean;
        firstInDay: boolean;
        isSystem: boolean;
        ownMessage: boolean;
        isEditing: boolean;
        firstUnreadMessage: boolean;
        canBeEdited: boolean;
        canBeDeleted: boolean;
    }
}