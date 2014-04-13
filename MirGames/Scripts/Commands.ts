/// <reference path="_references.ts" />
module MirGames.Domain {
    export class Command {
        public data: any;
        public static name: string;

        constructor() {
            this.data = {};
        }
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

    export class MarkAllTopicsAsReadCommand extends Command {
        public static name: string = 'MarkAllTopicsAsReadCommand';

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
}
 