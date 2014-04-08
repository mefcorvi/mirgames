/// <reference path="../_references.ts" />
module MirGames.Attachment {
    export interface IAttachmentService {
        uploadFile(entityType: string, file: File, callback: (err: Error, file: File, attachmentId: number) => void): void;
    }

    class AttachmentService implements IAttachmentService {
        constructor(private config: Core.IConfig, private eventBus: Core.IEventBus) {

        }

        public uploadFile(entityType: string, file: File, callback: (err: Error, file: File, attachmentId: number) => void): void {
            var xhr = new XMLHttpRequest();
            xhr.open('POST', Router.action('Attachment', 'Upload'));
            xhr.setRequestHeader('Content-type', 'multipart/form-data');

            xhr.setRequestHeader('X-File-Name', Core.Base64.encode(file.name));
            xhr.setRequestHeader('X-Entity-Type', entityType);
            xhr.setRequestHeader('X-File-Type', file.type);
            xhr.setRequestHeader('X-File-Size', file.size.toString());
            xhr.setRequestHeader('__RequestVerificationToken', this.config.antiForgery);

            this.eventBus.emit('ajax-request.executing');

            xhr.send(file);
            xhr.onreadystatechange = () => {
                if (xhr.readyState == 4) {
                    this.eventBus.emit('ajax-request.executed');

                    if (xhr.status == 200) {
                        var response: any = JSON.parse(xhr.responseText);

                        if (response.error) {
                            var err = new Error(response.error);
                            callback(err, null, null);
                        } else {
                            callback(null, file, response.attachmentId);
                        }
                    } else {
                        callback(new Error("Internal server error"), null, null);
                    }
                }
            }
        }
    }

    angular
        .module('mirgames.attachment', ['core.config', 'core.eventBus'])
        .factory('attachmentService', ['config', 'eventBus', (config: Core.IConfig, eventBus: Core.IEventBus) => new AttachmentService(config, eventBus)]);
}