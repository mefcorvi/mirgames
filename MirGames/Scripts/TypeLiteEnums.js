var MirGames;
(function (MirGames) {
    (function (Domain) {
        (function (Wip) {
            (function (ViewModels) {
                (function (WorkItemState) {
                    WorkItemState[WorkItemState["Undefined"] = 0] = "Undefined";
                    WorkItemState[WorkItemState["Open"] = 1] = "Open";
                    WorkItemState[WorkItemState["Closed"] = 2] = "Closed";
                    WorkItemState[WorkItemState["Active"] = 3] = "Active";
                    WorkItemState[WorkItemState["Queued"] = 4] = "Queued";
                    WorkItemState[WorkItemState["Removed"] = 5] = "Removed";
                })(ViewModels.WorkItemState || (ViewModels.WorkItemState = {}));
                var WorkItemState = ViewModels.WorkItemState;
                (function (WorkItemType) {
                    WorkItemType[WorkItemType["Undefined"] = 0] = "Undefined";
                    WorkItemType[WorkItemType["Bug"] = 1] = "Bug";
                    WorkItemType[WorkItemType["Task"] = 2] = "Task";
                    WorkItemType[WorkItemType["Feature"] = 3] = "Feature";
                })(ViewModels.WorkItemType || (ViewModels.WorkItemType = {}));
                var WorkItemType = ViewModels.WorkItemType;
                (function (WorkItemsOrderType) {
                    WorkItemsOrderType[WorkItemsOrderType["StartDate"] = 0] = "StartDate";
                    WorkItemsOrderType[WorkItemsOrderType["Priority"] = 1] = "Priority";
                })(ViewModels.WorkItemsOrderType || (ViewModels.WorkItemsOrderType = {}));
                var WorkItemsOrderType = ViewModels.WorkItemsOrderType;
                (function (WipProjectRepositoryItemType) {
                    WipProjectRepositoryItemType[WipProjectRepositoryItemType["Other"] = 0] = "Other";
                    WipProjectRepositoryItemType[WipProjectRepositoryItemType["File"] = 1] = "File";
                    WipProjectRepositoryItemType[WipProjectRepositoryItemType["Directory"] = 2] = "Directory";
                })(ViewModels.WipProjectRepositoryItemType || (ViewModels.WipProjectRepositoryItemType = {}));
                var WipProjectRepositoryItemType = ViewModels.WipProjectRepositoryItemType;
            })(Wip.ViewModels || (Wip.ViewModels = {}));
            var ViewModels = Wip.ViewModels;
        })(Domain.Wip || (Domain.Wip = {}));
        var Wip = Domain.Wip;
    })(MirGames.Domain || (MirGames.Domain = {}));
    var Domain = MirGames.Domain;
})(MirGames || (MirGames = {}));

var MirGames;
(function (MirGames) {
    (function (Infrastructure) {
        (function (Logging) {
            (function (EventLogType) {
                EventLogType[EventLogType["Error"] = 0] = "Error";
                EventLogType[EventLogType["Warning"] = 1] = "Warning";
                EventLogType[EventLogType["Information"] = 2] = "Information";
                EventLogType[EventLogType["Verbose"] = 3] = "Verbose";
            })(Logging.EventLogType || (Logging.EventLogType = {}));
            var EventLogType = Logging.EventLogType;
        })(Infrastructure.Logging || (Infrastructure.Logging = {}));
        var Logging = Infrastructure.Logging;
    })(MirGames.Infrastructure || (MirGames.Infrastructure = {}));
    var Infrastructure = MirGames.Infrastructure;
})(MirGames || (MirGames = {}));
//# sourceMappingURL=TypeLiteEnums.js.map
