var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var MirGames;
(function (MirGames) {
    /// <reference path="_references.ts" />
    (function (Domain) {
        var Command = (function () {
            function Command() {
                this.data = {};
            }
            return Command;
        })();
        Domain.Command = Command;

        var CreateAttachmentCommand = (function (_super) {
            __extends(CreateAttachmentCommand, _super);
            function CreateAttachmentCommand() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(CreateAttachmentCommand.prototype, "filePath", {
                get: function () {
                    return this.data['command.FilePath'];
                },
                set: function (value) {
                    this.data['command.FilePath'] = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(CreateAttachmentCommand.prototype, "fileName", {
                get: function () {
                    return this.data['command.FileName'];
                },
                set: function (value) {
                    this.data['command.FileName'] = value;
                },
                enumerable: true,
                configurable: true
            });

            CreateAttachmentCommand.name = 'CreateAttachmentCommand';
            return CreateAttachmentCommand;
        })(Command);
        Domain.CreateAttachmentCommand = CreateAttachmentCommand;

        var PublishAttachmentsCommand = (function (_super) {
            __extends(PublishAttachmentsCommand, _super);
            function PublishAttachmentsCommand() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(PublishAttachmentsCommand.prototype, "identifiers", {
                get: function () {
                    return this.data['command.Identifiers'];
                },
                set: function (value) {
                    this.data['command.Identifiers'] = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(PublishAttachmentsCommand.prototype, "entityId", {
                get: function () {
                    return this.data['command.EntityId'];
                },
                set: function (value) {
                    this.data['command.EntityId'] = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(PublishAttachmentsCommand.prototype, "entityType", {
                get: function () {
                    return this.data['command.EntityType'];
                },
                set: function (value) {
                    this.data['command.EntityType'] = value;
                },
                enumerable: true,
                configurable: true
            });

            PublishAttachmentsCommand.name = 'PublishAttachmentsCommand';
            return PublishAttachmentsCommand;
        })(Command);
        Domain.PublishAttachmentsCommand = PublishAttachmentsCommand;

        var RemoveAttachmentsCommand = (function (_super) {
            __extends(RemoveAttachmentsCommand, _super);
            function RemoveAttachmentsCommand() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(RemoveAttachmentsCommand.prototype, "entityId", {
                get: function () {
                    return this.data['command.EntityId'];
                },
                set: function (value) {
                    this.data['command.EntityId'] = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(RemoveAttachmentsCommand.prototype, "entityType", {
                get: function () {
                    return this.data['command.EntityType'];
                },
                set: function (value) {
                    this.data['command.EntityType'] = value;
                },
                enumerable: true,
                configurable: true
            });

            RemoveAttachmentsCommand.name = 'RemoveAttachmentsCommand';
            return RemoveAttachmentsCommand;
        })(Command);
        Domain.RemoveAttachmentsCommand = RemoveAttachmentsCommand;

        var ActivateUserCommand = (function (_super) {
            __extends(ActivateUserCommand, _super);
            function ActivateUserCommand() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(ActivateUserCommand.prototype, "activationKey", {
                get: function () {
                    return this.data['command.ActivationKey'];
                },
                set: function (value) {
                    this.data['command.ActivationKey'] = value;
                },
                enumerable: true,
                configurable: true
            });

            ActivateUserCommand.name = 'ActivateUserCommand';
            return ActivateUserCommand;
        })(Command);
        Domain.ActivateUserCommand = ActivateUserCommand;

        var CollapseMenuCommand = (function (_super) {
            __extends(CollapseMenuCommand, _super);
            function CollapseMenuCommand() {
                _super.apply(this, arguments);
            }
            CollapseMenuCommand.name = 'CollapseMenuCommand';
            return CollapseMenuCommand;
        })(Command);
        Domain.CollapseMenuCommand = CollapseMenuCommand;

        var DeleteUserCommand = (function (_super) {
            __extends(DeleteUserCommand, _super);
            function DeleteUserCommand() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(DeleteUserCommand.prototype, "userId", {
                get: function () {
                    return this.data['command.UserId'];
                },
                set: function (value) {
                    this.data['command.UserId'] = value;
                },
                enumerable: true,
                configurable: true
            });

            DeleteUserCommand.name = 'DeleteUserCommand';
            return DeleteUserCommand;
        })(Command);
        Domain.DeleteUserCommand = DeleteUserCommand;

        var LoginAsUserCommand = (function (_super) {
            __extends(LoginAsUserCommand, _super);
            function LoginAsUserCommand() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(LoginAsUserCommand.prototype, "userId", {
                get: function () {
                    return this.data['command.UserId'];
                },
                set: function (value) {
                    this.data['command.UserId'] = value;
                },
                enumerable: true,
                configurable: true
            });

            LoginAsUserCommand.name = 'LoginAsUserCommand';
            return LoginAsUserCommand;
        })(Command);
        Domain.LoginAsUserCommand = LoginAsUserCommand;

        var LoginCommand = (function (_super) {
            __extends(LoginCommand, _super);
            function LoginCommand() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(LoginCommand.prototype, "emailOrLogin", {
                get: function () {
                    return this.data['command.EmailOrLogin'];
                },
                set: function (value) {
                    this.data['command.EmailOrLogin'] = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(LoginCommand.prototype, "password", {
                get: function () {
                    return this.data['command.Password'];
                },
                set: function (value) {
                    this.data['command.Password'] = value;
                },
                enumerable: true,
                configurable: true
            });

            LoginCommand.name = 'LoginCommand';
            return LoginCommand;
        })(Command);
        Domain.LoginCommand = LoginCommand;

        var LogoutCommand = (function (_super) {
            __extends(LogoutCommand, _super);
            function LogoutCommand() {
                _super.apply(this, arguments);
            }
            LogoutCommand.name = 'LogoutCommand';
            return LogoutCommand;
        })(Command);
        Domain.LogoutCommand = LogoutCommand;

        var PostWallRecordCommand = (function (_super) {
            __extends(PostWallRecordCommand, _super);
            function PostWallRecordCommand() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(PostWallRecordCommand.prototype, "userId", {
                get: function () {
                    return this.data['command.UserId'];
                },
                set: function (value) {
                    this.data['command.UserId'] = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(PostWallRecordCommand.prototype, "text", {
                get: function () {
                    return this.data['command.Text'];
                },
                set: function (value) {
                    this.data['command.Text'] = value;
                },
                enumerable: true,
                configurable: true
            });

            PostWallRecordCommand.name = 'PostWallRecordCommand';
            return PostWallRecordCommand;
        })(Command);
        Domain.PostWallRecordCommand = PostWallRecordCommand;

        var SaveAccountSettingsCommand = (function (_super) {
            __extends(SaveAccountSettingsCommand, _super);
            function SaveAccountSettingsCommand() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(SaveAccountSettingsCommand.prototype, "timeZone", {
                get: function () {
                    return this.data['command.TimeZone'];
                },
                set: function (value) {
                    this.data['command.TimeZone'] = value;
                },
                enumerable: true,
                configurable: true
            });

            SaveAccountSettingsCommand.name = 'SaveAccountSettingsCommand';
            return SaveAccountSettingsCommand;
        })(Command);
        Domain.SaveAccountSettingsCommand = SaveAccountSettingsCommand;

        var SignUpCommand = (function (_super) {
            __extends(SignUpCommand, _super);
            function SignUpCommand() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(SignUpCommand.prototype, "login", {
                get: function () {
                    return this.data['command.Login'];
                },
                set: function (value) {
                    this.data['command.Login'] = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(SignUpCommand.prototype, "email", {
                get: function () {
                    return this.data['command.Email'];
                },
                set: function (value) {
                    this.data['command.Email'] = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(SignUpCommand.prototype, "password", {
                get: function () {
                    return this.data['command.Password'];
                },
                set: function (value) {
                    this.data['command.Password'] = value;
                },
                enumerable: true,
                configurable: true
            });

            SignUpCommand.name = 'SignUpCommand';
            return SignUpCommand;
        })(Command);
        Domain.SignUpCommand = SignUpCommand;

        var UncollapseMenuCommand = (function (_super) {
            __extends(UncollapseMenuCommand, _super);
            function UncollapseMenuCommand() {
                _super.apply(this, arguments);
            }
            UncollapseMenuCommand.name = 'UncollapseMenuCommand';
            return UncollapseMenuCommand;
        })(Command);
        Domain.UncollapseMenuCommand = UncollapseMenuCommand;

        var DeleteForumPostCommand = (function (_super) {
            __extends(DeleteForumPostCommand, _super);
            function DeleteForumPostCommand() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(DeleteForumPostCommand.prototype, "postId", {
                get: function () {
                    return this.data['command.PostId'];
                },
                set: function (value) {
                    this.data['command.PostId'] = value;
                },
                enumerable: true,
                configurable: true
            });

            DeleteForumPostCommand.name = 'DeleteForumPostCommand';
            return DeleteForumPostCommand;
        })(Command);
        Domain.DeleteForumPostCommand = DeleteForumPostCommand;

        var ImportFromIpbCommand = (function (_super) {
            __extends(ImportFromIpbCommand, _super);
            function ImportFromIpbCommand() {
                _super.apply(this, arguments);
            }
            ImportFromIpbCommand.name = 'ImportFromIpbCommand';
            return ImportFromIpbCommand;
        })(Command);
        Domain.ImportFromIpbCommand = ImportFromIpbCommand;

        var MarkAllTopicsAsReadCommand = (function (_super) {
            __extends(MarkAllTopicsAsReadCommand, _super);
            function MarkAllTopicsAsReadCommand() {
                _super.apply(this, arguments);
            }
            MarkAllTopicsAsReadCommand.name = 'MarkAllTopicsAsReadCommand';
            return MarkAllTopicsAsReadCommand;
        })(Command);
        Domain.MarkAllTopicsAsReadCommand = MarkAllTopicsAsReadCommand;

        var MarkTopicAsReadCommand = (function (_super) {
            __extends(MarkTopicAsReadCommand, _super);
            function MarkTopicAsReadCommand() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(MarkTopicAsReadCommand.prototype, "topicId", {
                get: function () {
                    return this.data['command.TopicId'];
                },
                set: function (value) {
                    this.data['command.TopicId'] = value;
                },
                enumerable: true,
                configurable: true
            });

            MarkTopicAsReadCommand.name = 'MarkTopicAsReadCommand';
            return MarkTopicAsReadCommand;
        })(Command);
        Domain.MarkTopicAsReadCommand = MarkTopicAsReadCommand;

        var MarkTopicAsUnreadForUsersCommand = (function (_super) {
            __extends(MarkTopicAsUnreadForUsersCommand, _super);
            function MarkTopicAsUnreadForUsersCommand() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(MarkTopicAsUnreadForUsersCommand.prototype, "topicId", {
                get: function () {
                    return this.data['command.TopicId'];
                },
                set: function (value) {
                    this.data['command.TopicId'] = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(MarkTopicAsUnreadForUsersCommand.prototype, "topicDate", {
                get: function () {
                    return this.data['command.TopicDate'];
                },
                set: function (value) {
                    this.data['command.TopicDate'] = value;
                },
                enumerable: true,
                configurable: true
            });

            MarkTopicAsUnreadForUsersCommand.name = 'MarkTopicAsUnreadForUsersCommand';
            return MarkTopicAsUnreadForUsersCommand;
        })(Command);
        Domain.MarkTopicAsUnreadForUsersCommand = MarkTopicAsUnreadForUsersCommand;

        var PostNewForumTopicCommand = (function (_super) {
            __extends(PostNewForumTopicCommand, _super);
            function PostNewForumTopicCommand() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(PostNewForumTopicCommand.prototype, "title", {
                get: function () {
                    return this.data['command.Title'];
                },
                set: function (value) {
                    this.data['command.Title'] = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(PostNewForumTopicCommand.prototype, "text", {
                get: function () {
                    return this.data['command.Text'];
                },
                set: function (value) {
                    this.data['command.Text'] = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(PostNewForumTopicCommand.prototype, "tags", {
                get: function () {
                    return this.data['command.Tags'];
                },
                set: function (value) {
                    this.data['command.Tags'] = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(PostNewForumTopicCommand.prototype, "attachments", {
                get: function () {
                    return this.data['command.Attachments'];
                },
                set: function (value) {
                    this.data['command.Attachments'] = value;
                },
                enumerable: true,
                configurable: true
            });

            PostNewForumTopicCommand.name = 'PostNewForumTopicCommand';
            return PostNewForumTopicCommand;
        })(Command);
        Domain.PostNewForumTopicCommand = PostNewForumTopicCommand;

        var ReindexForumTopicCommand = (function (_super) {
            __extends(ReindexForumTopicCommand, _super);
            function ReindexForumTopicCommand() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(ReindexForumTopicCommand.prototype, "topicId", {
                get: function () {
                    return this.data['command.TopicId'];
                },
                set: function (value) {
                    this.data['command.TopicId'] = value;
                },
                enumerable: true,
                configurable: true
            });

            ReindexForumTopicCommand.name = 'ReindexForumTopicCommand';
            return ReindexForumTopicCommand;
        })(Command);
        Domain.ReindexForumTopicCommand = ReindexForumTopicCommand;

        var ReindexForumTopicsCommand = (function (_super) {
            __extends(ReindexForumTopicsCommand, _super);
            function ReindexForumTopicsCommand() {
                _super.apply(this, arguments);
            }
            ReindexForumTopicsCommand.name = 'ReindexForumTopicsCommand';
            return ReindexForumTopicsCommand;
        })(Command);
        Domain.ReindexForumTopicsCommand = ReindexForumTopicsCommand;

        var ReplyForumTopicCommand = (function (_super) {
            __extends(ReplyForumTopicCommand, _super);
            function ReplyForumTopicCommand() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(ReplyForumTopicCommand.prototype, "attachments", {
                get: function () {
                    return this.data['command.Attachments'];
                },
                set: function (value) {
                    this.data['command.Attachments'] = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(ReplyForumTopicCommand.prototype, "text", {
                get: function () {
                    return this.data['command.Text'];
                },
                set: function (value) {
                    this.data['command.Text'] = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(ReplyForumTopicCommand.prototype, "topicId", {
                get: function () {
                    return this.data['command.TopicId'];
                },
                set: function (value) {
                    this.data['command.TopicId'] = value;
                },
                enumerable: true,
                configurable: true
            });

            ReplyForumTopicCommand.name = 'ReplyForumTopicCommand';
            return ReplyForumTopicCommand;
        })(Command);
        Domain.ReplyForumTopicCommand = ReplyForumTopicCommand;

        var UpdateForumPostCommand = (function (_super) {
            __extends(UpdateForumPostCommand, _super);
            function UpdateForumPostCommand() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(UpdateForumPostCommand.prototype, "attachments", {
                get: function () {
                    return this.data['command.Attachments'];
                },
                set: function (value) {
                    this.data['command.Attachments'] = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(UpdateForumPostCommand.prototype, "text", {
                get: function () {
                    return this.data['command.Text'];
                },
                set: function (value) {
                    this.data['command.Text'] = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(UpdateForumPostCommand.prototype, "topicTitle", {
                get: function () {
                    return this.data['command.TopicTitle'];
                },
                set: function (value) {
                    this.data['command.TopicTitle'] = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(UpdateForumPostCommand.prototype, "topicsTags", {
                get: function () {
                    return this.data['command.TopicsTags'];
                },
                set: function (value) {
                    this.data['command.TopicsTags'] = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(UpdateForumPostCommand.prototype, "postId", {
                get: function () {
                    return this.data['command.PostId'];
                },
                set: function (value) {
                    this.data['command.PostId'] = value;
                },
                enumerable: true,
                configurable: true
            });

            UpdateForumPostCommand.name = 'UpdateForumPostCommand';
            return UpdateForumPostCommand;
        })(Command);
        Domain.UpdateForumPostCommand = UpdateForumPostCommand;

        var AddNewTopicCommand = (function (_super) {
            __extends(AddNewTopicCommand, _super);
            function AddNewTopicCommand() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(AddNewTopicCommand.prototype, "title", {
                get: function () {
                    return this.data['command.Title'];
                },
                set: function (value) {
                    this.data['command.Title'] = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(AddNewTopicCommand.prototype, "text", {
                get: function () {
                    return this.data['command.Text'];
                },
                set: function (value) {
                    this.data['command.Text'] = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(AddNewTopicCommand.prototype, "tags", {
                get: function () {
                    return this.data['command.Tags'];
                },
                set: function (value) {
                    this.data['command.Tags'] = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(AddNewTopicCommand.prototype, "attachments", {
                get: function () {
                    return this.data['command.Attachments'];
                },
                set: function (value) {
                    this.data['command.Attachments'] = value;
                },
                enumerable: true,
                configurable: true
            });

            AddNewTopicCommand.name = 'AddNewTopicCommand';
            return AddNewTopicCommand;
        })(Command);
        Domain.AddNewTopicCommand = AddNewTopicCommand;

        var DeleteTopicCommand = (function (_super) {
            __extends(DeleteTopicCommand, _super);
            function DeleteTopicCommand() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(DeleteTopicCommand.prototype, "topicId", {
                get: function () {
                    return this.data['command.TopicId'];
                },
                set: function (value) {
                    this.data['command.TopicId'] = value;
                },
                enumerable: true,
                configurable: true
            });

            DeleteTopicCommand.name = 'DeleteTopicCommand';
            return DeleteTopicCommand;
        })(Command);
        Domain.DeleteTopicCommand = DeleteTopicCommand;

        var PostNewCommentCommand = (function (_super) {
            __extends(PostNewCommentCommand, _super);
            function PostNewCommentCommand() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(PostNewCommentCommand.prototype, "topicId", {
                get: function () {
                    return this.data['command.TopicId'];
                },
                set: function (value) {
                    this.data['command.TopicId'] = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(PostNewCommentCommand.prototype, "text", {
                get: function () {
                    return this.data['command.Text'];
                },
                set: function (value) {
                    this.data['command.Text'] = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(PostNewCommentCommand.prototype, "attachments", {
                get: function () {
                    return this.data['command.Attachments'];
                },
                set: function (value) {
                    this.data['command.Attachments'] = value;
                },
                enumerable: true,
                configurable: true
            });

            PostNewCommentCommand.name = 'PostNewCommentCommand';
            return PostNewCommentCommand;
        })(Command);
        Domain.PostNewCommentCommand = PostNewCommentCommand;

        var ReindexTopicsCommand = (function (_super) {
            __extends(ReindexTopicsCommand, _super);
            function ReindexTopicsCommand() {
                _super.apply(this, arguments);
            }
            ReindexTopicsCommand.name = 'ReindexTopicsCommand';
            return ReindexTopicsCommand;
        })(Command);
        Domain.ReindexTopicsCommand = ReindexTopicsCommand;

        var SaveTopicCommand = (function (_super) {
            __extends(SaveTopicCommand, _super);
            function SaveTopicCommand() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(SaveTopicCommand.prototype, "topicId", {
                get: function () {
                    return this.data['command.TopicId'];
                },
                set: function (value) {
                    this.data['command.TopicId'] = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(SaveTopicCommand.prototype, "title", {
                get: function () {
                    return this.data['command.Title'];
                },
                set: function (value) {
                    this.data['command.Title'] = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(SaveTopicCommand.prototype, "text", {
                get: function () {
                    return this.data['command.Text'];
                },
                set: function (value) {
                    this.data['command.Text'] = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(SaveTopicCommand.prototype, "tags", {
                get: function () {
                    return this.data['command.Tags'];
                },
                set: function (value) {
                    this.data['command.Tags'] = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(SaveTopicCommand.prototype, "attachments", {
                get: function () {
                    return this.data['command.Attachments'];
                },
                set: function (value) {
                    this.data['command.Attachments'] = value;
                },
                enumerable: true,
                configurable: true
            });

            SaveTopicCommand.name = 'SaveTopicCommand';
            return SaveTopicCommand;
        })(Command);
        Domain.SaveTopicCommand = SaveTopicCommand;

        var PostChatMessageCommand = (function (_super) {
            __extends(PostChatMessageCommand, _super);
            function PostChatMessageCommand() {
                _super.apply(this, arguments);
            }
            Object.defineProperty(PostChatMessageCommand.prototype, "message", {
                get: function () {
                    return this.data['command.Message'];
                },
                set: function (value) {
                    this.data['command.Message'] = value;
                },
                enumerable: true,
                configurable: true
            });

            Object.defineProperty(PostChatMessageCommand.prototype, "attachments", {
                get: function () {
                    return this.data['command.Attachments'];
                },
                set: function (value) {
                    this.data['command.Attachments'] = value;
                },
                enumerable: true,
                configurable: true
            });

            PostChatMessageCommand.name = 'PostChatMessageCommand';
            return PostChatMessageCommand;
        })(Command);
        Domain.PostChatMessageCommand = PostChatMessageCommand;
    })(MirGames.Domain || (MirGames.Domain = {}));
    var Domain = MirGames.Domain;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=Commands.js.map
