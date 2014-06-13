var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var MirGames;
(function (MirGames) {
    /// <reference path="../_references.ts" />
    (function (Chat) {
        var ChatRoomPage = (function (_super) {
            __extends(ChatRoomPage, _super);
            function ChatRoomPage($scope, commandBus, eventBus, socketService, notificationService, apiService, currentUser, $timeout) {
                var _this = this;
                _super.call(this, $scope, eventBus);
                this.commandBus = commandBus;
                this.socketService = socketService;
                this.notificationService = notificationService;
                this.apiService = apiService;
                this.currentUser = currentUser;
                this.$timeout = $timeout;
                this.isActive = true;
                this.unreadCount = 0;

                this.$scope.editMode = false;
                this.$scope.playSound = true;
                this.$scope.historyAvailable = true;
                this.$scope.cancelEdit = this.cancelEdit.bind(this);
                this.$scope.editMessage = this.editMessage.bind(this);
                this.$scope.messages = [];
                this.$scope.useEnterToPost = this.pageData.currentUser ? this.pageData.currentUser.Settings.UseEnterToSendChatMessage : false;
                this.$scope.changeSendKey = this.changeSendKey.bind(this);

                this.$textArea = $('.new-answer-form textarea');
                this.$footer = $('body > footer');
                this.$chatMessages = $('.chat-messages');

                $scope.reply = {
                    message: "",
                    attachments: [],
                    post: this.reply.bind(this),
                    focus: true
                };

                this.$scope.loadHistory = this.loadHistory.bind(this);
                this.$scope.focusAnswer = this.focusAnswer.bind(this);
                this.socketService.addHandler('chatHub', 'addNewMessageToPage', this.processReceivedMessage.bind(this));
                this.socketService.addHandler('chatHub', 'updateMessage', this.processUpdatedMessage.bind(this));

                this.eventBus.on('socket.disconnected', function () {
                    _this.addSystemMessage('Соединение разорвано');
                });

                this.eventBus.on('socket.reconnecting', function () {
                    _this.addSystemMessage('Переподключение...');
                });

                this.eventBus.on('socket.connected', function () {
                    if (_this.$scope.messages.length > 0) {
                        _this.addSystemMessage('Соединение установлено');
                    }

                    _this.loadLastMessages();
                });

                $(window).focus(function () {
                    return _this.handleChatActivation();
                });
                $(window).blur(function () {
                    return _this.handleChatDeactivation();
                });

                this.currentUserEnteredChat();
                setInterval(this.currentUserEnteredChat.bind(this), 10000);

                this.attachToTextEditor();
                $(window).on('beforeunload', this.currentUserLeavedChat.bind(this));

                $(function () {
                    _this.scrollToBottom();
                    _this.adjustTextAreaHeight();
                    $('.new-answer-form .mdd_button').click(function () {
                        return _this.handleTextEditorToolbarClick();
                    });

                    _this.$textArea.css('max-height', '300px').autosize({
                        callback: function () {
                            return _this.adjustTextAreaHeight();
                        }
                    });
                });
            }
            ChatRoomPage.prototype.changeSendKey = function () {
                this.$scope.useEnterToPost = !this.$scope.useEnterToPost;
                var command = {
                    Settings: {
                        UseEnterToSendChatMessage: this.$scope.useEnterToPost
                    }
                };

                this.apiService.executeCommand("SaveAccountSettingsCommand", command, null, false);
            };

            /** Adds the system message. */
            ChatRoomPage.prototype.addSystemMessage = function (message) {
                var _this = this;
                var lastMessage = Enumerable.from(this.$scope.messages).lastOrDefault();

                this.$scope.$apply(function () {
                    var scopeMessage = {
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

                    _this.$scope.messages.push(scopeMessage);
                });

                if (this.isScrollBottom()) {
                    this.scrollToBottom();
                }
            };

            /** Loads history page */
            ChatRoomPage.prototype.loadHistory = function () {
                if (!this.$scope.historyAvailable) {
                    return;
                }

                var currentFirst = Enumerable.from(this.$scope.messages).firstOrDefault(function (item) {
                    return !item.isSystem;
                });

                var query = {
                    LastIndex: currentFirst != null ? currentFirst.id : null,
                    FirstIndex: null
                };

                var $firstItem = $('article[message-id]').first();
                this.loadMessages(query, $firstItem);
            };

            /** Loads last messages */
            ChatRoomPage.prototype.loadLastMessages = function () {
                var currentLast = Enumerable.from(this.$scope.messages).lastOrDefault(function (item) {
                    return !item.isSystem;
                });

                var query = {
                    FirstIndex: currentLast != null ? currentLast.id : null,
                    LastIndex: null
                };

                this.loadMessages(query);
            };

            /** Loads messages */
            ChatRoomPage.prototype.loadMessages = function (query, anchorItem) {
                var _this = this;
                var oldTop = null;

                if (anchorItem != null && anchorItem.length > 0) {
                    oldTop = anchorItem.position().top;
                }

                this.apiService.getAll('GetChatMessagesQuery', query, 0, 50, function (result) {
                    _this.$scope.messages = Enumerable.from(result).select(function (message) {
                        return _this.convertMessage(message);
                    }).concat(_this.$scope.messages).orderBy(function (message) {
                        return message.date;
                    }).toArray();

                    _this.$scope.historyAvailable = result.length == 50;
                    _this.prepareMessages(0, _this.$scope.messages.length);
                    _this.$scope.$apply();

                    if (oldTop != null) {
                        var newPosition = anchorItem.position();
                        var oldScrollTop = _this.getScrollTop();
                        _this.setScrollTop(oldScrollTop + newPosition.top - oldTop);
                    } else {
                        _this.scrollToBottom();
                    }
                }, false);
            };

            /** Prepares list of messages */
            ChatRoomPage.prototype.prepareMessages = function (startIndex, count) {
                for (var i = count - 1; i >= startIndex; i--) {
                    var message = this.$scope.messages[i];
                    var prevMessage = i > 0 ? this.$scope.messages[i - 1] : null;
                    this.prepareMessage(message, prevMessage);
                }
            };

            /** Loads message for editing */
            ChatRoomPage.prototype.editMessage = function (message) {
                var _this = this;
                var query = {
                    MessageId: message.id
                };

                this.apiService.getOne('GetChatMessageForEditQuery', query, function (result) {
                    _this.$scope.$apply(function () {
                        _this.$scope.reply.message = result.SourceText;
                        _this.$scope.editMode = true;
                        message.isEditing = true;
                        _this.$scope.editedMessage = message;
                        _this.focusAnswer();
                    });
                    _this.$textArea.trigger('autosize.resize');
                });
            };

            /** Loads last own message for edit */
            ChatRoomPage.prototype.loadLastMessageForEdit = function () {
                var lastOwnMessage = Enumerable.from(this.$scope.messages).last(function (item) {
                    return item.ownMessage;
                });
                this.editMessage(lastOwnMessage);
            };

            /** Cancels the editing. */
            ChatRoomPage.prototype.cancelEdit = function () {
                var _this = this;
                if (!this.$scope.editMode) {
                    return;
                }

                this.$scope.reply.attachments = [];
                this.$scope.reply.message = '';
                this.$scope.editedMessage.isEditing = false;
                this.$scope.editedMessage = null;
                this.$scope.editMode = false;
                this.$scope.focusAnswer();

                setTimeout(function () {
                    return _this.adjustTextAreaHeight();
                }, 0);
            };

            /** Handles text writing event. */
            ChatRoomPage.prototype.attachToTextEditor = function () {
                var _this = this;
                var timeout;
                var oldText;

                this.$textArea.keydown(function (ev) {
                    if (ev.which == 38 && _this.$textArea.val() == '') {
                        _this.loadLastMessageForEdit();
                    }

                    if (ev.which == 27 && _this.$scope.editMode) {
                        _this.cancelEdit();
                        _this.$scope.$apply();
                    }

                    setTimeout(function () {
                        var newText = _this.$textArea.val();

                        if (newText == oldText) {
                            return;
                        }

                        _this.sendTextWriting();
                        oldText = newText;

                        if (timeout) {
                            clearTimeout(timeout);
                            timeout = null;
                        }

                        timeout = setTimeout(function () {
                            _this.sendTextWritingStopped();
                        }, 1000);
                    }, 0);
                });
            };

            /** Mark current user as entering */
            ChatRoomPage.prototype.currentUserEnteredChat = function () {
                var command = {
                    Tag: 'in-chat',
                    ExpirationTime: 10000
                };
                this.socketService.executeCommand('AddOnlineUserTagCommand', command);
            };

            /** Marks current user as leaving */
            ChatRoomPage.prototype.currentUserLeavedChat = function () {
                var command = {
                    Tag: 'in-chat'
                };
                this.socketService.executeCommand('RemoveOnlineUserTagCommand', command);
            };

            /** Handles click by toolbar button */
            ChatRoomPage.prototype.handleTextEditorToolbarClick = function () {
                var _this = this;
                setTimeout(function () {
                    _this.$textArea.trigger('autosize.resize');
                    _this.adjustTextAreaHeight();
                }, 0);
            };

            /** Processes the received message */
            ChatRoomPage.prototype.processReceivedMessage = function (messageJson) {
                var _this = this;
                var messageViewModel = JSON.parse(messageJson);
                var message = this.convertMessage(messageViewModel);

                if (!this.isActive) {
                    this.unreadCount++;
                    this.updateNotifications();

                    if (this.markNextMessageUnread) {
                        message.firstUnreadMessage = true;
                        this.markNextMessageUnread = false;
                    }
                }

                this.$scope.$apply(function () {
                    _this.$scope.messages.push(message);

                    var prevMessage = _this.$scope.messages[_this.$scope.messages.length - 2];
                    _this.prepareMessage(message, prevMessage);

                    if (_this.isActive) {
                        if (_this.isScrollBottom()) {
                            _this.scrollToBottom();
                        }
                    } else {
                        setTimeout(function () {
                            _this.scrollToItem($('article.first-unread.message:last'));
                        }, 0);
                    }
                });
            };

            /** Processes update message. */
            ChatRoomPage.prototype.processUpdatedMessage = function (message) {
                var scopeMessage = Enumerable.from(this.$scope.messages).single(function (item) {
                    return item.id == message.MessageId;
                });
                this.$scope.$apply(function () {
                    scopeMessage.text = message.Text;
                    scopeMessage.editDate = new Date(message.UpdatedDate.toString());
                });
            };

            /** Sends notification whether user writing the message */
            ChatRoomPage.prototype.sendTextWriting = function () {
                if (this.textWriting) {
                    return;
                }

                var command = {
                    Tag: 'chat-writing',
                    ExpirationTime: 30000
                };
                this.socketService.executeCommand('AddOnlineUserTagCommand', command);
                this.textWriting = true;
            };

            /** Sends notification whether user stops the writing */
            ChatRoomPage.prototype.sendTextWritingStopped = function () {
                if (!this.textWriting) {
                    return;
                }

                var command = {
                    Tag: 'chat-writing'
                };
                this.socketService.executeCommand('RemoveOnlineUserTagCommand', command);
                this.textWriting = false;
            };

            /** Adjusts text area height */
            ChatRoomPage.prototype.adjustTextAreaHeight = function () {
                var newHeight = $('body > footer .answer-form').height() + 20;

                if (newHeight > 20) {
                    $('body').css('padding-bottom', newHeight);
                    this.$footer.css('height', newHeight);
                }
            };

            /** Updates notifications */
            ChatRoomPage.prototype.updateNotifications = function () {
                if (this.unreadCount > 0) {
                    this.notificationService.setBubble(this.unreadCount, this.$scope.playSound);
                } else {
                    this.notificationService.reset();
                }
            };

            /** Handles chat activation */
            ChatRoomPage.prototype.handleChatActivation = function () {
                this.isActive = true;
                this.unreadCount = 0;
                this.updateNotifications();
            };

            /** Handles chat deactivation */
            ChatRoomPage.prototype.handleChatDeactivation = function () {
                this.isActive = false;
                this.markNextMessageUnread = true;
            };

            /** Prepares received message */
            ChatRoomPage.prototype.prepareMessage = function (message, prevMessage) {
                if (message.isSystem) {
                    return;
                }

                if (prevMessage && !prevMessage.isSystem) {
                    message.showAuthor = message.authorId != prevMessage.authorId;
                    message.showDate = message.date.getTime() > (prevMessage.date.getTime() + 60000) || message.date.getMinutes() != prevMessage.date.getMinutes();

                    if (!message.showAuthor) {
                        prevMessage.inChain = true;
                    }
                }

                var currentDate = new Date();
                var day = 24 * 60 * 60 * 1000;

                if (message.date.getTime() > (currentDate.getTime() + day) || message.date.getDate() != currentDate.getDate()) {
                    message.dateFormat = 'dd.MM.yy HH:mm';
                }
            };

            /** Converts message from viewmodel to scope */
            ChatRoomPage.prototype.convertMessage = function (message) {
                var scopeMessage = {
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
            };

            /** Checks whether user still have an access to the message */
            ChatRoomPage.prototype.updateAccessRight = function (message) {
                var _this = this;
                var messageCreatedMoment = moment(message.date);
                var isMessageFrozen = messageCreatedMoment.add('m', 5).isBefore();

                message.canBeDeleted = (message.ownMessage && !isMessageFrozen) || message.canBeEdited;
                message.canBeEdited = (message.ownMessage && !isMessageFrozen) || message.canBeDeleted;

                if (!isMessageFrozen) {
                    this.$timeout(function () {
                        return _this.updateAccessRight(message);
                    }, messageCreatedMoment.diff(moment()) + 50, true);
                }
            };

            /** Focus text area */
            ChatRoomPage.prototype.focusAnswer = function () {
                this.$scope.reply.focus = true;
            };

            /** Sends the reply */
            ChatRoomPage.prototype.reply = function () {
                var _this = this;
                if (this.$scope.postAnswerForm.$invalid) {
                    return;
                }

                if (this.$scope.editMode) {
                    var updateCommand = {
                        Attachments: this.$scope.reply.attachments,
                        Message: this.$scope.reply.message,
                        MessageId: this.$scope.editedMessage.id
                    };
                    this.socketService.executeCommand('UpdateChatMessageCommand', updateCommand);
                    this.cancelEdit();
                } else {
                    var command = {
                        Attachments: this.$scope.reply.attachments,
                        Message: this.$scope.reply.message
                    };

                    this.socketService.executeCommand('PostChatMessageCommand', command);
                }

                this.$scope.safeApply(function () {
                    _this.$scope.editedMessage = null;
                    _this.$scope.editMode = false;
                    _this.$scope.reply.attachments = [];
                    _this.$scope.reply.message = "";

                    setTimeout(function () {
                        _this.$textArea.trigger('autosize.resize');
                    }, 0);
                });

                this.sendTextWritingStopped();
            };
            ChatRoomPage.$inject = ['$scope', 'commandBus', 'eventBus', 'socketService', 'notificationService', 'apiService', 'currentUser', '$timeout'];
            return ChatRoomPage;
        })(MirGames.BasePage);
        Chat.ChatRoomPage = ChatRoomPage;
    })(MirGames.Chat || (MirGames.Chat = {}));
    var Chat = MirGames.Chat;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=ChatRoomPage.js.map
