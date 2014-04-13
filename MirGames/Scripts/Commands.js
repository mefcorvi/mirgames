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

        var MarkAllTopicsAsReadCommand = (function (_super) {
            __extends(MarkAllTopicsAsReadCommand, _super);
            function MarkAllTopicsAsReadCommand() {
                _super.apply(this, arguments);
            }
            MarkAllTopicsAsReadCommand.name = 'MarkAllTopicsAsReadCommand';
            return MarkAllTopicsAsReadCommand;
        })(Command);
        Domain.MarkAllTopicsAsReadCommand = MarkAllTopicsAsReadCommand;

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
    })(MirGames.Domain || (MirGames.Domain = {}));
    var Domain = MirGames.Domain;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=Commands.js.map
