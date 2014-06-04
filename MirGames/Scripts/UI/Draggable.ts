/// <reference path="../_references.ts" />

module UI {
    angular.module("ui.draggable", [])
        .directive("uiDraggable", [
            '$parse',
            '$rootScope',
            ($parse: ng.IParseService, $rootScope: ng.IRootScopeService) => {
                return (scope: ng.IScope, element: JQuery, attrs: any) => {
                    var jq: any = jQuery;
                    if (!jq.event.props.dataTransfer) {
                        jq.event.props.push('dataTransfer');
                    }
                    element.attr("draggable", false);
                    attrs.$observe("uiDraggable", (newValue: any) => {
                        element.attr("draggable", newValue);
                    });
                    var dragData = "";
                    scope.$watch(attrs.drag, (newValue: any) => {
                        dragData = newValue;
                    });
                    element.bind("dragstart", (e: any) => {
                        element.addClass('on-drag-start');
                        var sendData = angular.toJson(dragData);
                        var sendChannel = attrs.dragChannel || "defaultchannel";
                        var dragImage = attrs.dragImage || null;
                        if (dragImage) {
                            var dragImageFn = $parse(attrs.dragImage);
                            scope.$apply(() => {
                                var dragImageParameters = dragImageFn(scope, { $event: e });
                                if (dragImageParameters && dragImageParameters.image) {
                                    var xOffset = dragImageParameters.xOffset || 0,
                                        yOffset = dragImageParameters.yOffset || 0;
                                    e.dataTransfer.setDragImage(dragImageParameters.image, xOffset, yOffset);
                                }
                            });
                        }

                        e.dataTransfer.setData("Text", sendData);
                        $rootScope.$broadcast("ANGULAR_DRAG_START", sendChannel);

                    });

                    element.bind("dragend", (e: any) => {
                        element.removeClass('on-drag-start');
                        var sendChannel = attrs.dragChannel || "defaultchannel";
                        $rootScope.$broadcast("ANGULAR_DRAG_END", sendChannel);
                        if (e.dataTransfer && e.dataTransfer.dropEffect !== "none") {
                            if (attrs.onDropSuccess) {
                                var fn = $parse(attrs.onDropSuccess);
                                scope.$apply(() => {
                                    fn(scope, { $event: e });
                                });
                            }
                        }
                    });

                };
            }
        ])
        .directive("uiOnDrop", [
            '$parse',
            '$rootScope',
            ($parse: ng.IParseService, $rootScope: ng.IRootScopeService) => {
                return (scope: ng.IScope, element: JQuery, attr: any) => {
                    var dragging = 0; //Ref. http://stackoverflow.com/a/10906204
                    var dropChannel = "defaultchannel";
                    var dragChannel: string;
                    var dragEnterClass = attr.dragEnterClass || "on-drag-enter";
                    var dragHoverClass = attr.dragHoverClass || "on-drag-hover";

                    function onDragOver(e: any) {

                        if (e.preventDefault) {
                            e.preventDefault(); // Necessary. Allows us to drop.
                        }

                        if (e.stopPropagation) {
                            e.stopPropagation();
                        }
                        e.dataTransfer.dropEffect = 'move';
                        return false;
                    }

                    function onDragLeave() {
                        dragging--;
                        if (dragging == 0) {
                            element.removeClass(dragHoverClass);
                        }
                    }

                    function onDragEnter() {
                        dragging++;
                        $rootScope.$broadcast("ANGULAR_HOVER", dropChannel);
                        element.addClass(dragHoverClass);
                    }

                    function onDrop(e: any) {
                        if (e.preventDefault) {
                            e.preventDefault(); // Necessary. Allows us to drop.
                        }
                        if (e.stopPropagation) {
                            e.stopPropagation(); // Necessary. Allows us to drop.
                        }
                        var data = e.dataTransfer.getData("Text");
                        data = angular.fromJson(data);
                        var fn = $parse(attr.uiOnDrop);
                        scope.$apply(() => {
                            fn(scope, { $data: data, $event: e });
                        });
                        element.removeClass(dragEnterClass);
                    }

                    $rootScope.$on("ANGULAR_DRAG_START", (event: any, channel: any) => {
                        dragChannel = channel;
                        if (dropChannel === channel) {

                            element.bind("dragover", onDragOver);
                            element.bind("dragenter", onDragEnter);
                            element.bind("dragleave", onDragLeave);

                            element.bind("drop", onDrop);
                            element.addClass(dragEnterClass);
                        }

                    });

                    $rootScope.$on("ANGULAR_DRAG_END", (e: any, channel: any) => {
                        dragChannel = "";
                        if (dropChannel === channel) {

                            element.unbind("dragover", onDragOver);
                            element.unbind("dragenter", onDragEnter);
                            element.unbind("dragleave", onDragLeave);

                            element.unbind("drop", onDrop);
                            element.removeClass(dragHoverClass);
                            element.removeClass(dragEnterClass);
                        }
                    });

                    $rootScope.$on("ANGULAR_HOVER", (e: any, channel: any) => {
                        if (dropChannel === channel) {
                            element.removeClass(dragHoverClass);
                        }
                    });

                    attr.$observe('dropChannel', (value: any) => {
                        if (value) {
                            dropChannel = value;
                        }
                    });

                };
            }
        ]);
}