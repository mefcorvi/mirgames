var MirGames;
(function (MirGames) {
    /// <reference path="../_references.ts" />
    (function (Attachment) {
        var AttachmentService = (function () {
            function AttachmentService(config, eventBus) {
                this.config = config;
                this.eventBus = eventBus;
            }
            AttachmentService.prototype.uploadFile = function (entityType, file, callback) {
                var _this = this;
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
                xhr.onreadystatechange = function () {
                    if (xhr.readyState == 4) {
                        _this.eventBus.emit('ajax-request.executed');

                        if (xhr.status == 200) {
                            var response = JSON.parse(xhr.responseText);

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
                };
            };
            return AttachmentService;
        })();

        angular.module('mirgames.attachment', ['core.config', 'core.eventBus']).factory('attachmentService', ['config', 'eventBus', function (config, eventBus) {
                return new AttachmentService(config, eventBus);
            }]);
    })(MirGames.Attachment || (MirGames.Attachment = {}));
    var Attachment = MirGames.Attachment;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=AttachmentService.js.map
