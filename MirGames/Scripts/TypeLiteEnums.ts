module MirGames.Infrastructure.Commands {
}
module MirGames.Infrastructure.Queries {
}
module MirGames.Domain.Tools.Queries {
}
module MirGames.Domain.Tools.ViewModels {
}
module MirGames.Domain.Topics.Commands {
}
module MirGames.Domain.Topics.Queries {
}
module MirGames.Domain.Topics.ViewModels {
}
module MirGames.Domain.Users.ViewModels {
}
module MirGames.Domain.Users.Commands {
}
module System.Collections.Generic {
}
module MirGames.Domain.Users.Queries {
}
module MirGames.Domain.Forum.Commands {
}
module MirGames.Domain.Forum.Queries {
}
module MirGames.Domain.Forum.ViewModels {
}
module MirGames.Domain.Chat.Commands {
}
module MirGames.Domain.Chat.Queries {
}
module MirGames.Domain.Chat.ViewModels {
}
module MirGames.Domain.Notifications.ViewModels {
}
module MirGames.Domain.Attachments.ViewModels {
}
module MirGames.Domain.Wip.Commands {
}
module MirGames.Domain.Wip.Queries {
}
module MirGames.Domain.Wip.ViewModels {
export enum WorkItemState {
  Undefined = 0,
  Open = 1,
  Closed = 2,
  Active = 3,
  Queued = 4,
  Removed = 5
}
export enum WorkItemType {
  Undefined = 0,
  Bug = 1,
  Task = 2,
  Feature = 3
}
export enum WorkItemsOrderType {
  StartDate = 0,
  Priority = 1
}
export enum WipProjectRepositoryItemType {
  Other = 0,
  File = 1,
  Directory = 2
}
}
module System {
}
module MirGames.Infrastructure.Logging {
export enum EventLogType {
  Error = 0,
  Warning = 1,
  Information = 2,
  Verbose = 3
}
}

