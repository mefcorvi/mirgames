
 
 

  
/// <reference path="TypeLiteEnums.ts" />

declare module MirGames.Infrastructure.Commands {
interface Command {
}
interface Command1 extends MirGames.Infrastructure.Commands.Command {
}
interface Command1 extends MirGames.Infrastructure.Commands.Command {
}
interface Command1 extends MirGames.Infrastructure.Commands.Command {
}
interface Command1 extends MirGames.Infrastructure.Commands.Command {
}
}
declare module MirGames.Infrastructure.Queries {
interface Query {
}
interface Query1 extends MirGames.Infrastructure.Queries.Query {
}
interface SingleItemQuery1 extends MirGames.Infrastructure.Queries.Query1 {
}
interface Query1 extends MirGames.Infrastructure.Queries.Query {
}
interface SingleItemQuery1 extends MirGames.Infrastructure.Queries.Query1 {
}
interface Query1 extends MirGames.Infrastructure.Queries.Query {
}
interface SingleItemQuery1 extends MirGames.Infrastructure.Queries.Query1 {
}
interface Query1 extends MirGames.Infrastructure.Queries.Query {
}
interface Query1 extends MirGames.Infrastructure.Queries.Query {
}
interface Query1 extends MirGames.Infrastructure.Queries.Query {
}
interface Query1 extends MirGames.Infrastructure.Queries.Query {
}
interface SingleItemQuery1 extends MirGames.Infrastructure.Queries.Query1 {
}
interface Query1 extends MirGames.Infrastructure.Queries.Query {
}
interface SingleItemQuery1 extends MirGames.Infrastructure.Queries.Query1 {
}
interface SingleItemQuery1 extends MirGames.Infrastructure.Queries.Query1 {
}
interface Query1 extends MirGames.Infrastructure.Queries.Query {
}
interface Query1 extends MirGames.Infrastructure.Queries.Query {
}
interface SingleItemQuery1 extends MirGames.Infrastructure.Queries.Query1 {
}
interface Query1 extends MirGames.Infrastructure.Queries.Query {
}
interface Query1 extends MirGames.Infrastructure.Queries.Query {
}
interface Query1 extends MirGames.Infrastructure.Queries.Query {
}
interface SingleItemQuery1 extends MirGames.Infrastructure.Queries.Query1 {
}
interface Query1 extends MirGames.Infrastructure.Queries.Query {
}
interface SingleItemQuery1 extends MirGames.Infrastructure.Queries.Query1 {
}
interface Query1 extends MirGames.Infrastructure.Queries.Query {
}
interface SingleItemQuery1 extends MirGames.Infrastructure.Queries.Query1 {
}
interface Query1 extends MirGames.Infrastructure.Queries.Query {
}
interface Query1 extends MirGames.Infrastructure.Queries.Query {
}
interface SingleItemQuery1 extends MirGames.Infrastructure.Queries.Query1 {
}
interface Query1 extends MirGames.Infrastructure.Queries.Query {
}
interface Query1 extends MirGames.Infrastructure.Queries.Query {
}
}
declare module MirGames.Domain.Tools.Queries {
interface GetEventLogQuery extends MirGames.Infrastructure.Queries.Query1 {
  LogType: MirGames.Infrastructure.Logging.EventLogType;
  UserName: string;
  Source: string;
  Message: string;
  From: Date;
  To: Date;
}
}
declare module MirGames.Domain.Tools.ViewModels {
interface EventLogViewModel {
  Id: number;
  EventLogType: MirGames.Infrastructure.Logging.EventLogType;
  Login: string;
  Message: string;
  Source: string;
  Details: string;
  Date: Date;
}
}
declare module MirGames.Domain.Topics.Commands {
interface AddNewTopicCommand extends MirGames.Infrastructure.Commands.Command1 {
  Title: string;
  Text: string;
  Tags: string;
  IsTutorial: boolean;
  IsRepost: boolean;
  SourceAuthor: string;
  SourceLink: string;
  BlogId: number;
  Attachments: number[];
}
interface DeleteCommentCommand extends MirGames.Infrastructure.Commands.Command {
  CommentId: number;
}
interface DeleteTopicCommand extends MirGames.Infrastructure.Commands.Command {
  TopicId: number;
}
interface EditCommentCommand extends MirGames.Infrastructure.Commands.Command {
  CommentId: number;
  Text: string;
  Attachments: number[];
}
interface MarkAllBlogTopicsAsReadCommand extends MirGames.Infrastructure.Commands.Command {
}
interface PostNewCommentCommand extends MirGames.Infrastructure.Commands.Command1 {
  TopicId: number;
  Text: string;
  Attachments: number[];
}
interface SaveTopicCommand extends MirGames.Infrastructure.Commands.Command {
  TopicId: number;
  Title: string;
  Text: string;
  Tags: string;
  Attachments: number[];
}
}
declare module MirGames.Domain.Topics.Queries {
interface GetBlogByEntityQuery extends MirGames.Infrastructure.Queries.SingleItemQuery1 {
  EntityId: number;
  EntityType: string;
}
interface GetCommentForEditQuery extends MirGames.Infrastructure.Queries.SingleItemQuery1 {
  CommentId: number;
}
interface GetCommentByIdQuery extends MirGames.Infrastructure.Queries.SingleItemQuery1 {
  CommentId: number;
}
interface GetMainTagsQuery extends MirGames.Infrastructure.Queries.Query1 {
  Filter: string;
  ShowOnMain: boolean;
  IsTutorial: boolean;
  IsMicroTopic: boolean;
}
}
declare module MirGames.Domain.Topics.ViewModels {
interface BlogViewModel {
  BlogId: number;
  Title: string;
  Description: string;
  EntityId: number;
  EntityType: string;
  CanAddTopic: boolean;
}
interface CommentForEditViewModel {
  Id: number;
  SourceText: string;
}
interface CommentViewModel {
  Text: string;
  Author: MirGames.Domain.Users.ViewModels.AuthorViewModel;
  CreationDate: Date;
  UpdatedDate: Date;
  Id: number;
  TopicId: number;
  TopicTitle: string;
  CanBeEdited: boolean;
  CanBeDeleted: boolean;
  IsRead: boolean;
}
interface TagViewModel {
  Tag: string;
  Count: number;
}
interface TopicForEditViewModel {
  Id: number;
  Title: string;
  Tags: string;
  Text: string;
  IsMicroTopic: boolean;
}
interface TopicViewModel {
  Id: number;
  Title: string;
  Author: MirGames.Domain.Users.ViewModels.AuthorViewModel;
  Blog: MirGames.Domain.Topics.ViewModels.BlogViewModel;
  CommentsCount: number;
  Text: string;
  Comments: MirGames.Domain.Topics.ViewModels.CommentViewModel[];
  CanBeEdited: boolean;
  CanBeDeleted: boolean;
  CanBeCommented: boolean;
  IsRead: boolean;
  Tags: string[];
  TagsList: string;
  CreationDate: Date;
  ShowOnMain: boolean;
  IsMicroTopic: boolean;
}
}
declare module MirGames.Domain.Users.ViewModels {
interface AuthorViewModel {
  AvatarUrl: string;
  Login: string;
  Id: number;
  Title: string;
}
interface OAuthProviderViewModel {
  ProviderId: number;
  ProviderName: string;
  DisplayName: string;
  IsLinked: boolean;
}
interface OnlineUserViewModel {
  AvatarUrl: string;
  Login: string;
  Id: number;
  SessionDate: Date;
  LastRequestDate: Date;
  Tags: string[];
}
interface CurrentUserViewModel {
  AvatarUrl: string;
  Login: string;
  Id: number;
  Name: string;
  TimeZone: string;
  IsActivated: boolean;
  Settings: any;
}
interface UserClaimViewModel {
  Type: string;
  Value: string;
}
interface UserListItemViewModel {
  AvatarUrl: string;
  Login: string;
  Id: number;
  Name: string;
  RegistrationDate: Date;
  LastVisit: Date;
  Location: string;
  UserRating: number;
  IsOnline: boolean;
}
interface UserViewModel {
  AvatarUrl: string;
  Login: string;
  Id: number;
  Name: string;
  About: string;
  Location: string;
  Rating: number;
  Birthday: Date;
  RegistrationDate: Date;
  LastVisitDate: Date;
  CanBeDeleted: boolean;
  CanReceiveMessage: boolean;
  WallRecordCanBeAdded: boolean;
}
interface UserWallRecordViewModel {
  Author: MirGames.Domain.Users.ViewModels.AuthorViewModel;
  DateAdd: Date;
  Text: string;
}
}
declare module MirGames.Domain.Users.Commands {
interface AddOnlineUserTagCommand extends MirGames.Infrastructure.Commands.Command {
  Tag: string;
  ExpirationTime: number;
}
interface DetachOAuthProviderCommand extends MirGames.Infrastructure.Commands.Command {
  ProviderName: string;
}
interface RequestPasswordRestoreCommand extends MirGames.Infrastructure.Commands.Command {
  EmailOrLogin: string;
  NewPasswordHash: string;
}
interface RemoveOnlineUserTagCommand extends MirGames.Infrastructure.Commands.Command {
  Tag: string;
}
interface DeleteUserCommand extends MirGames.Infrastructure.Commands.Command {
  UserId: number;
}
interface LoginAsUserCommand extends MirGames.Infrastructure.Commands.Command1 {
  UserId: number;
}
interface LoginCommand extends MirGames.Infrastructure.Commands.Command1 {
  EmailOrLogin: string;
  Password: string;
}
interface ResendActivationCommand extends MirGames.Infrastructure.Commands.Command {
}
interface SaveAccountSettingsCommand extends MirGames.Infrastructure.Commands.Command {
  Settings: any;
}
interface SaveUserProfileCommand extends MirGames.Infrastructure.Commands.Command {
  Name: string;
  Location: string;
  Birthday: Date;
  Company: string;
  Career: string;
  About: string;
  GitHubLink: string;
  BitBucketLink: string;
  HabrahabrLink: string;
}
interface SetUserAvatarCommand extends MirGames.Infrastructure.Commands.Command {
  AvatarAttachmentId: number;
}
interface SignUpCommand extends MirGames.Infrastructure.Commands.Command1 {
  Login: string;
  Email: string;
  Password: string;
  CaptchaChallenge: string;
  CaptchaResponse: string;
}
}
declare module System.Collections.Generic {
interface KeyValuePair2 {
  Key: any;
  Value: any;
}
}
declare module MirGames.Domain.Users.Queries {
interface GetOAuthProvidersQuery extends MirGames.Infrastructure.Queries.Query1 {
}
}
declare module MirGames.Domain.Forum.Commands {
interface DeleteForumPostCommand extends MirGames.Infrastructure.Commands.Command {
  PostId: number;
}
interface DeleteForumTopicCommand extends MirGames.Infrastructure.Commands.Command {
  TopicId: number;
}
interface UpdateForumPostCommand extends MirGames.Infrastructure.Commands.Command {
  Attachments: number[];
  Text: string;
  TopicTitle: string;
  TopicsTags: string;
  PostId: number;
}
interface MarkAllTopicsAsReadCommand extends MirGames.Infrastructure.Commands.Command {
}
interface MarkTopicAsVisitedCommand extends MirGames.Infrastructure.Commands.Command {
  TopicId: number;
}
interface PostNewForumTopicCommand extends MirGames.Infrastructure.Commands.Command1 {
  ForumAlias: string;
  Title: string;
  Text: string;
  Tags: string;
  Attachments: number[];
}
interface ReplyForumTopicCommand extends MirGames.Infrastructure.Commands.Command1 {
  Attachments: number[];
  Text: string;
  TopicId: number;
}
interface VoteForForumPostCommand extends MirGames.Infrastructure.Commands.Command1 {
  PostId: number;
  Positive: boolean;
}
}
declare module MirGames.Domain.Forum.Queries {
interface GetForumTopicPostsQuery extends MirGames.Infrastructure.Queries.Query1 {
  TopicId: number;
  LoadStartPost: boolean;
}
interface GetForumPostForEditQuery extends MirGames.Infrastructure.Queries.SingleItemQuery1 {
  PostId: number;
}
interface GetForumPostQuery extends MirGames.Infrastructure.Queries.SingleItemQuery1 {
  PostId: number;
}
interface GetForumTopicQuery extends MirGames.Infrastructure.Queries.SingleItemQuery1 {
  TopicId: number;
}
interface GetForumTagsQuery extends MirGames.Infrastructure.Queries.Query1 {
  Filter: string;
}
}
declare module MirGames.Domain.Forum.ViewModels {
interface ForumPostForEditViewModel {
  PostId: number;
  SourceText: string;
  TopicTitle: string;
  TopicTags: string;
  CanChangeTitle: boolean;
  CanChangeTags: boolean;
}
interface ForumPostViewModel {
  PostId: number;
  TopicTitle: string;
  Text: string;
  CreatedDate: Date;
  UpdatedDate: Date;
  Author: MirGames.Domain.Users.ViewModels.AuthorViewModel;
  AuthorIP: string;
  IsHidden: boolean;
  TopicId: number;
  ForumId: number;
  ForumAlias: string;
  VotesRating: number;
  UserVote: number;
  CanBeVoted: boolean;
}
interface ForumPostsListItemViewModel {
  PostId: number;
  Text: string;
  CreatedDate: Date;
  UpdatedDate: Date;
  Author: MirGames.Domain.Users.ViewModels.AuthorViewModel;
  AuthorIP: string;
  IsHidden: boolean;
  TopicId: number;
  Index: number;
  IsRead: boolean;
  FirstUnread: boolean;
  VotesRating: number;
  UserVote: number;
  IsFirstPost: boolean;
  CanBeEdited: boolean;
  CanBeDeleted: boolean;
  CanBeVoted: boolean;
}
interface ForumTopicsListItemViewModel {
  TopicId: number;
  Author: MirGames.Domain.Users.ViewModels.AuthorViewModel;
  AuthorIp: string;
  LastPostAuthor: MirGames.Domain.Users.ViewModels.AuthorViewModel;
  Title: string;
  TagsList: string;
  Tags: string[];
  CreatedDate: Date;
  UpdatedDate: Date;
  PostsCount: number;
  UnreadPostsCount: number;
  Visits: number;
  IsRead: boolean;
  Forum: MirGames.Domain.Forum.ViewModels.ForumViewModel;
  ShortDescription: string;
}
interface ForumViewModel {
  ForumId: number;
  Title: string;
  Description: string;
  IsRetired: boolean;
  Alias: string;
  LastAuthor: MirGames.Domain.Users.ViewModels.AuthorViewModel;
  LastTopicTitle: string;
  LastTopicId: number;
  LastPostDate: Date;
  TopicsCount: number;
  PostsCount: number;
}
interface ForumTopicViewModel {
  TopicId: number;
  Author: MirGames.Domain.Users.ViewModels.AuthorViewModel;
  AuthorIp: string;
  Title: string;
  TagsList: string;
  StartPost: MirGames.Domain.Forum.ViewModels.ForumPostsListItemViewModel;
  Tags: string[];
  CreatedDate: Date;
  UpdatedDate: Date;
  CanBeAnswered: boolean;
  CanBeEdited: boolean;
  CanBeDeleted: boolean;
  IsRead: boolean;
  Forum: MirGames.Domain.Forum.ViewModels.ForumViewModel;
}
}
declare module MirGames.Domain.Chat.Commands {
interface PostChatMessageCommand extends MirGames.Infrastructure.Commands.Command1 {
  Message: string;
  Attachments: number[];
}
interface UpdateChatMessageCommand extends MirGames.Infrastructure.Commands.Command {
  MessageId: number;
  Message: string;
  Attachments: number[];
}
}
declare module MirGames.Domain.Chat.Queries {
interface GetChatMessageForEditQuery extends MirGames.Infrastructure.Queries.SingleItemQuery1 {
  MessageId: number;
}
interface GetChatMessagesQuery extends MirGames.Infrastructure.Queries.Query1 {
  LastIndex: number;
  FirstIndex: number;
}
}
declare module MirGames.Domain.Chat.ViewModels {
interface ChatMessageViewModel {
  MessageId: number;
  Text: string;
  CanBeEdited: boolean;
  CanBeDeleted: boolean;
  Author: MirGames.Domain.Users.ViewModels.AuthorViewModel;
  CreatedDate: Date;
  UpdatedDate: Date;
}
interface ChatMessageForEditViewModel {
  MessageId: number;
  SourceText: string;
}
}
declare module MirGames.Domain.Notifications.ViewModels {
interface NotificationSubscriptionViewModel {
  SubscriptionId: number;
  UserId: number;
  EntityType: string;
  NotificationType: string;
}
interface NotificationViewModel {
  NotificationId: string;
  NotificationType: string;
  UserId: number;
  EntityId: number;
  Data: MirGames.Domain.Notifications.ViewModels.NotificationData;
}
interface NotificationData {
  NotificationType: string;
}
}
declare module MirGames.Domain.Attachments.Queries {
interface GetAttachmentsQuery extends MirGames.Infrastructure.Queries.Query1 {
  EntityId: number;
  EntityType: string;
  IsImage: boolean;
  OrderingBy: MirGames.Domain.Attachments.Queries.AttachmentsOrderingType;
}
}
declare module MirGames.Domain.Attachments.ViewModels {
interface AttachmentViewModel {
  AttachmentId: number;
  ContentType: string;
  UserId: number;
  CreatedDate: Date;
  FileName: string;
  FilePath: string;
  AttachmentUrl: string;
  FileSize: number;
  EntityId: number;
  EntityType: string;
  IsImage: boolean;
}
}
declare module MirGames.Domain.Wip.Commands {
interface AddWipGalleryImageCommand extends MirGames.Infrastructure.Commands.Command {
  Attachments: number[];
  ProjectAlias: string;
}
interface AssignWorkItemCommand extends MirGames.Infrastructure.Commands.Command {
  WorkItemId: number;
  UserId: number;
}
interface ChangeWorkItemStateCommand extends MirGames.Infrastructure.Commands.Command1 {
  WorkItemId: number;
  State: MirGames.Domain.Wip.ViewModels.WorkItemState;
}
interface CreateNewProjectWorkItemCommand extends MirGames.Infrastructure.Commands.Command1 {
  ProjectAlias: string;
  Title: string;
  Tags: string;
  Type: MirGames.Domain.Wip.ViewModels.WorkItemType;
  Attachments: number[];
  Description: string;
  AssignedTo: number;
}
interface SaveWipProjectCommand extends MirGames.Infrastructure.Commands.Command {
  Title: string;
  Alias: string;
  Tags: string;
  LogoAttachmentId: number;
  Attachments: number[];
  Description: string;
  IsRepositoryPrivate: boolean;
  IsSiteEnabled: boolean;
  ShortDescription: string;
}
interface CreateNewWipProjectCommand extends MirGames.Infrastructure.Commands.Command1 {
  Title: string;
  Alias: string;
  Tags: string;
  RepositoryType: string;
  LogoAttachmentId: number;
  Attachments: number[];
  Description: string;
  ShortDescription: string;
}
interface PostWorkItemCommentCommand extends MirGames.Infrastructure.Commands.Command1 {
  WorkItemId: number;
  Text: string;
  Attachments: number[];
}
}
declare module MirGames.Domain.Wip.Queries {
interface GetIsProjectNameUniqueQuery extends MirGames.Infrastructure.Queries.SingleItemQuery1 {
  Alias: string;
}
interface GetProjectWorkItemCommentQuery extends MirGames.Infrastructure.Queries.SingleItemQuery1 {
  CommentId: number;
}
interface GetProjectWorkItemCommentsQuery extends MirGames.Infrastructure.Queries.Query1 {
  WorkItemId: number;
}
interface GetProjectWorkItemQuery extends MirGames.Infrastructure.Queries.SingleItemQuery1 {
  ProjectAlias: string;
  InternalId: number;
}
interface GetProjectWorkItemsQuery extends MirGames.Infrastructure.Queries.Query1 {
  ProjectAlias: string;
  Tag: string;
  WorkItemType: MirGames.Domain.Wip.ViewModels.WorkItemType;
  WorkItemState: MirGames.Domain.Wip.ViewModels.WorkItemState;
  OrderBy: MirGames.Domain.Wip.ViewModels.WorkItemsOrderType;
}
interface GetWipProjectTeamQuery extends MirGames.Infrastructure.Queries.Query1 {
  Alias: string;
}
interface GetWipProjectQuery extends MirGames.Infrastructure.Queries.SingleItemQuery1 {
  ProjectId: number;
  Alias: string;
}
interface GetWipTagsQuery extends MirGames.Infrastructure.Queries.Query1 {
  Filter: string;
}
}
declare module MirGames.Domain.Wip.ViewModels {
interface WipTagViewModel {
  Tag: string;
  Count: number;
}
interface WipProjectTeamMemberViewModel {
  MemberInfo: MirGames.Domain.Users.ViewModels.AuthorViewModel;
}
interface ProjectWorkItemStatisticsViewModel {
  OpenBugsCount: number;
  OpenTasksCount: number;
  OpenFeaturesCount: number;
}
interface ProjectWorkItemCommentViewModel {
  CommentId: number;
  WorkItemId: number;
  Author: MirGames.Domain.Users.ViewModels.AuthorViewModel;
  Text: string;
  Date: Date;
  UpdatedDate: Date;
  CanBeEdited: boolean;
  CanBeDeleted: boolean;
}
interface ProjectWorkItemViewModel {
  WorkItemId: number;
  InternalId: number;
  ProjectId: number;
  Title: string;
  ShortDescription: string;
  Description: string;
  TagsList: string;
  State: MirGames.Domain.Wip.ViewModels.WorkItemState;
  CreatedDate: Date;
  UpdatedDate: Date;
  ItemType: MirGames.Domain.Wip.ViewModels.WorkItemType;
  Priority: number;
  StartDate: Date;
  EndDate: Date;
  Duration: System.TimeSpan;
  ParentId: number;
  Author: MirGames.Domain.Users.ViewModels.AuthorViewModel;
  AssignedTo: MirGames.Domain.Users.ViewModels.AuthorViewModel;
  CanBeDeleted: boolean;
  CanBeEdited: boolean;
  CanBeCommented: boolean;
  Tags: string[];
}
interface WipProjectRepositoryItemViewModel {
  Path: string;
  Name: string;
  UpdatedDate: Date;
  CommitId: string;
  Message: string;
  ItemType: MirGames.Domain.Wip.ViewModels.WipProjectRepositoryItemType;
}
interface WipProjectCommitViewModel {
  Author: MirGames.Domain.Users.ViewModels.AuthorViewModel;
  Message: string;
  Date: Date;
}
interface WipProjectFileViewModel {
  FileName: string;
  UpdatedDate: Date;
  CommitId: string;
  Message: string;
  Content: System.IO.Stream;
  IsPreview: boolean;
}
interface WipProjectViewModel {
  ProjectId: number;
  Title: string;
  Alias: string;
  Description: string;
  Genre: string;
  Author: MirGames.Domain.Users.ViewModels.AuthorViewModel;
  LogoUrl: string;
  CreationDate: Date;
  UpdatedDate: Date;
  Version: string;
  Tags: string[];
  VotesCount: number;
  Votes: number;
  FollowersCount: number;
  RepositoryUrl: string;
  RepositoryType: string;
  CanEdit: boolean;
  CanCreateBug: boolean;
  CanCreateTask: boolean;
  CanCreateFeature: boolean;
  CanReadRepository: boolean;
  CanCreateBlogTopic: boolean;
  LastCommitMessage: string;
  ShortDescription: string;
  IsRepositoryPrivate: boolean;
  IsSiteEnabled: boolean;
  BlogId: number;
  CanEditGallery: boolean;
}
}
declare module System {
interface TimeSpan {
  Ticks: number;
  Days: number;
  Hours: number;
  Milliseconds: number;
  Minutes: number;
  Seconds: number;
  TotalDays: number;
  TotalHours: number;
  TotalMilliseconds: number;
  TotalMinutes: number;
  TotalSeconds: number;
}
interface MarshalByRefObject {
}
}
declare module System.IO {
interface Stream extends System.MarshalByRefObject {
  CanRead: boolean;
  CanSeek: boolean;
  CanTimeout: boolean;
  CanWrite: boolean;
  Length: number;
  Position: number;
  ReadTimeout: number;
  WriteTimeout: number;
}
}
declare module MirGames.Infrastructure.Logging {
}


