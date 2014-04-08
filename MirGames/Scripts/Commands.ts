/// <reference path="_references.ts" />
module MirGames.Domain {
    export class Command {
        public data: any;
        public static name: string;

        constructor() {
            this.data = {};
        }
    }

    export class CreateAttachmentCommand extends Command {
        public static name: string = 'CreateAttachmentCommand';

        get filePath(): any {
            return this.data['command.FilePath'];
        }

        set filePath(value: any) {
            this.data['command.FilePath'] = value;
        }
        get fileName(): any {
            return this.data['command.FileName'];
        }

        set fileName(value: any) {
            this.data['command.FileName'] = value;
        }
    }

    export class PublishAttachmentsCommand extends Command {
        public static name: string = 'PublishAttachmentsCommand';

        get identifiers(): any {
            return this.data['command.Identifiers'];
        }

        set identifiers(value: any) {
            this.data['command.Identifiers'] = value;
        }
        get entityId(): any {
            return this.data['command.EntityId'];
        }

        set entityId(value: any) {
            this.data['command.EntityId'] = value;
        }
        get entityType(): any {
            return this.data['command.EntityType'];
        }

        set entityType(value: any) {
            this.data['command.EntityType'] = value;
        }
    }

    export class RemoveAttachmentsCommand extends Command {
        public static name: string = 'RemoveAttachmentsCommand';

        get entityId(): any {
            return this.data['command.EntityId'];
        }

        set entityId(value: any) {
            this.data['command.EntityId'] = value;
        }
        get entityType(): any {
            return this.data['command.EntityType'];
        }

        set entityType(value: any) {
            this.data['command.EntityType'] = value;
        }
    }

    export class ActivateUserCommand extends Command {
        public static name: string = 'ActivateUserCommand';

        get activationKey(): any {
            return this.data['command.ActivationKey'];
        }

        set activationKey(value: any) {
            this.data['command.ActivationKey'] = value;
        }
    }

    export class CollapseMenuCommand extends Command {
        public static name: string = 'CollapseMenuCommand';

    }

    export class DeleteUserCommand extends Command {
        public static name: string = 'DeleteUserCommand';

        get userId(): any {
            return this.data['command.UserId'];
        }

        set userId(value: any) {
            this.data['command.UserId'] = value;
        }
    }

    export class LoginAsUserCommand extends Command {
        public static name: string = 'LoginAsUserCommand';

        get userId(): any {
            return this.data['command.UserId'];
        }

        set userId(value: any) {
            this.data['command.UserId'] = value;
        }
    }

    export class LoginCommand extends Command {
        public static name: string = 'LoginCommand';

        get emailOrLogin(): any {
            return this.data['command.EmailOrLogin'];
        }

        set emailOrLogin(value: any) {
            this.data['command.EmailOrLogin'] = value;
        }
        get password(): any {
            return this.data['command.Password'];
        }

        set password(value: any) {
            this.data['command.Password'] = value;
        }
    }

    export class LogoutCommand extends Command {
        public static name: string = 'LogoutCommand';

    }

    export class PostWallRecordCommand extends Command {
        public static name: string = 'PostWallRecordCommand';

        get userId(): any {
            return this.data['command.UserId'];
        }

        set userId(value: any) {
            this.data['command.UserId'] = value;
        }
        get text(): any {
            return this.data['command.Text'];
        }

        set text(value: any) {
            this.data['command.Text'] = value;
        }
    }

    export class SaveAccountSettingsCommand extends Command {
        public static name: string = 'SaveAccountSettingsCommand';

        get timeZone(): any {
            return this.data['command.TimeZone'];
        }

        set timeZone(value: any) {
            this.data['command.TimeZone'] = value;
        }
    }

    export class SignUpCommand extends Command {
        public static name: string = 'SignUpCommand';

        get login(): any {
            return this.data['command.Login'];
        }

        set login(value: any) {
            this.data['command.Login'] = value;
        }
        get email(): any {
            return this.data['command.Email'];
        }

        set email(value: any) {
            this.data['command.Email'] = value;
        }
        get password(): any {
            return this.data['command.Password'];
        }

        set password(value: any) {
            this.data['command.Password'] = value;
        }
    }

    export class UncollapseMenuCommand extends Command {
        public static name: string = 'UncollapseMenuCommand';

    }

    export class DeleteForumPostCommand extends Command {
        public static name: string = 'DeleteForumPostCommand';

        get postId(): any {
            return this.data['command.PostId'];
        }

        set postId(value: any) {
            this.data['command.PostId'] = value;
        }
    }

    export class ImportFromIpbCommand extends Command {
        public static name: string = 'ImportFromIpbCommand';

    }

    export class MarkAllTopicsAsReadCommand extends Command {
        public static name: string = 'MarkAllTopicsAsReadCommand';

    }

    export class MarkTopicAsReadCommand extends Command {
        public static name: string = 'MarkTopicAsReadCommand';

        get topicId(): any {
            return this.data['command.TopicId'];
        }

        set topicId(value: any) {
            this.data['command.TopicId'] = value;
        }
    }

    export class MarkTopicAsUnreadForUsersCommand extends Command {
        public static name: string = 'MarkTopicAsUnreadForUsersCommand';

        get topicId(): any {
            return this.data['command.TopicId'];
        }

        set topicId(value: any) {
            this.data['command.TopicId'] = value;
        }
        get topicDate(): any {
            return this.data['command.TopicDate'];
        }

        set topicDate(value: any) {
            this.data['command.TopicDate'] = value;
        }
    }

    export class PostNewForumTopicCommand extends Command {
        public static name: string = 'PostNewForumTopicCommand';

        get title(): any {
            return this.data['command.Title'];
        }

        set title(value: any) {
            this.data['command.Title'] = value;
        }
        get text(): any {
            return this.data['command.Text'];
        }

        set text(value: any) {
            this.data['command.Text'] = value;
        }
        get tags(): any {
            return this.data['command.Tags'];
        }

        set tags(value: any) {
            this.data['command.Tags'] = value;
        }
        get attachments(): any {
            return this.data['command.Attachments'];
        }

        set attachments(value: any) {
            this.data['command.Attachments'] = value;
        }
    }

    export class ReindexForumTopicCommand extends Command {
        public static name: string = 'ReindexForumTopicCommand';

        get topicId(): any {
            return this.data['command.TopicId'];
        }

        set topicId(value: any) {
            this.data['command.TopicId'] = value;
        }
    }

    export class ReindexForumTopicsCommand extends Command {
        public static name: string = 'ReindexForumTopicsCommand';

    }

    export class ReplyForumTopicCommand extends Command {
        public static name: string = 'ReplyForumTopicCommand';

        get attachments(): any {
            return this.data['command.Attachments'];
        }

        set attachments(value: any) {
            this.data['command.Attachments'] = value;
        }
        get text(): any {
            return this.data['command.Text'];
        }

        set text(value: any) {
            this.data['command.Text'] = value;
        }
        get topicId(): any {
            return this.data['command.TopicId'];
        }

        set topicId(value: any) {
            this.data['command.TopicId'] = value;
        }
    }

    export class UpdateForumPostCommand extends Command {
        public static name: string = 'UpdateForumPostCommand';

        get attachments(): any {
            return this.data['command.Attachments'];
        }

        set attachments(value: any) {
            this.data['command.Attachments'] = value;
        }
        get text(): any {
            return this.data['command.Text'];
        }

        set text(value: any) {
            this.data['command.Text'] = value;
        }
        get topicTitle(): any {
            return this.data['command.TopicTitle'];
        }

        set topicTitle(value: any) {
            this.data['command.TopicTitle'] = value;
        }
        get topicsTags(): any {
            return this.data['command.TopicsTags'];
        }

        set topicsTags(value: any) {
            this.data['command.TopicsTags'] = value;
        }
        get postId(): any {
            return this.data['command.PostId'];
        }

        set postId(value: any) {
            this.data['command.PostId'] = value;
        }
    }

    export class AddNewTopicCommand extends Command {
        public static name: string = 'AddNewTopicCommand';

        get title(): any {
            return this.data['command.Title'];
        }

        set title(value: any) {
            this.data['command.Title'] = value;
        }
        get text(): any {
            return this.data['command.Text'];
        }

        set text(value: any) {
            this.data['command.Text'] = value;
        }
        get tags(): any {
            return this.data['command.Tags'];
        }

        set tags(value: any) {
            this.data['command.Tags'] = value;
        }
        get attachments(): any {
            return this.data['command.Attachments'];
        }

        set attachments(value: any) {
            this.data['command.Attachments'] = value;
        }
    }

    export class DeleteTopicCommand extends Command {
        public static name: string = 'DeleteTopicCommand';

        get topicId(): any {
            return this.data['command.TopicId'];
        }

        set topicId(value: any) {
            this.data['command.TopicId'] = value;
        }
    }

    export class PostNewCommentCommand extends Command {
        public static name: string = 'PostNewCommentCommand';

        get topicId(): any {
            return this.data['command.TopicId'];
        }

        set topicId(value: any) {
            this.data['command.TopicId'] = value;
        }
        get text(): any {
            return this.data['command.Text'];
        }

        set text(value: any) {
            this.data['command.Text'] = value;
        }
        get attachments(): any {
            return this.data['command.Attachments'];
        }

        set attachments(value: any) {
            this.data['command.Attachments'] = value;
        }
    }

    export class ReindexTopicsCommand extends Command {
        public static name: string = 'ReindexTopicsCommand';

    }

    export class SaveTopicCommand extends Command {
        public static name: string = 'SaveTopicCommand';

        get topicId(): any {
            return this.data['command.TopicId'];
        }

        set topicId(value: any) {
            this.data['command.TopicId'] = value;
        }
        get title(): any {
            return this.data['command.Title'];
        }

        set title(value: any) {
            this.data['command.Title'] = value;
        }
        get text(): any {
            return this.data['command.Text'];
        }

        set text(value: any) {
            this.data['command.Text'] = value;
        }
        get tags(): any {
            return this.data['command.Tags'];
        }

        set tags(value: any) {
            this.data['command.Tags'] = value;
        }
        get attachments(): any {
            return this.data['command.Attachments'];
        }

        set attachments(value: any) {
            this.data['command.Attachments'] = value;
        }
    }

    export class PostChatMessageCommand extends Command {
        public static name: string = 'PostChatMessageCommand';

        get message(): any {
            return this.data['command.Message'];
        }

        set message(value: any) {
            this.data['command.Message'] = value;
        }
        get attachments(): any {
            return this.data['command.Attachments'];
        }

        set attachments(value: any) {
            this.data['command.Attachments'] = value;
        }
    }

}
