// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
#pragma warning disable 1591, 3008, 3009
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace MirGames.Areas.Topics.Controllers
{
    public partial class TopicsController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected TopicsController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(Task<ActionResult> taskResult)
        {
            return RedirectToAction(taskResult.Result);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(Task<ActionResult> taskResult)
        {
            return RedirectToActionPermanent(taskResult.Result);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Topic()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Topic);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult TopicListItem()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.TopicListItem);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Edit()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Edit);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult AddTopic()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.AddTopic);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult SaveTopic()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SaveTopic);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult DeleteTopic()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.DeleteTopic);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public TopicsController Actions { get { return MVC.Topics.Topics; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "Topics";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Topics";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Topics";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Rss = "Rss";
            public readonly string CommentsRss = "CommentsRss";
            public readonly string Index = "Index";
            public readonly string Tutorials = "Tutorials";
            public readonly string Topic = "Topic";
            public readonly string TopicListItem = "TopicListItem";
            public readonly string New = "New";
            public readonly string Edit = "Edit";
            public readonly string AddTopic = "AddTopic";
            public readonly string SaveTopic = "SaveTopic";
            public readonly string DeleteTopic = "DeleteTopic";
            public readonly string EditCommentDialog = "EditCommentDialog";
            public readonly string DeleteCommentDialog = "DeleteCommentDialog";
            public readonly string AddTopicDialog = "AddTopicDialog";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Rss = "Rss";
            public const string CommentsRss = "CommentsRss";
            public const string Index = "Index";
            public const string Tutorials = "Tutorials";
            public const string Topic = "Topic";
            public const string TopicListItem = "TopicListItem";
            public const string New = "New";
            public const string Edit = "Edit";
            public const string AddTopic = "AddTopic";
            public const string SaveTopic = "SaveTopic";
            public const string DeleteTopic = "DeleteTopic";
            public const string EditCommentDialog = "EditCommentDialog";
            public const string DeleteCommentDialog = "DeleteCommentDialog";
            public const string AddTopicDialog = "AddTopicDialog";
        }


        static readonly ActionParamsClass_Rss s_params_Rss = new ActionParamsClass_Rss();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Rss RssParams { get { return s_params_Rss; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Rss
        {
            public readonly string tag = "tag";
            public readonly string searchString = "searchString";
        }
        static readonly ActionParamsClass_Index s_params_Index = new ActionParamsClass_Index();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Index IndexParams { get { return s_params_Index; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Index
        {
            public readonly string tag = "tag";
            public readonly string searchString = "searchString";
            public readonly string page = "page";
            public readonly string onlyUnread = "onlyUnread";
        }
        static readonly ActionParamsClass_Tutorials s_params_Tutorials = new ActionParamsClass_Tutorials();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Tutorials TutorialsParams { get { return s_params_Tutorials; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Tutorials
        {
            public readonly string tag = "tag";
            public readonly string searchString = "searchString";
            public readonly string page = "page";
        }
        static readonly ActionParamsClass_Topic s_params_Topic = new ActionParamsClass_Topic();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Topic TopicParams { get { return s_params_Topic; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Topic
        {
            public readonly string topicId = "topicId";
        }
        static readonly ActionParamsClass_TopicListItem s_params_TopicListItem = new ActionParamsClass_TopicListItem();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_TopicListItem TopicListItemParams { get { return s_params_TopicListItem; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_TopicListItem
        {
            public readonly string topicId = "topicId";
        }
        static readonly ActionParamsClass_Edit s_params_Edit = new ActionParamsClass_Edit();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Edit EditParams { get { return s_params_Edit; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Edit
        {
            public readonly string topicId = "topicId";
        }
        static readonly ActionParamsClass_AddTopic s_params_AddTopic = new ActionParamsClass_AddTopic();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_AddTopic AddTopicParams { get { return s_params_AddTopic; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_AddTopic
        {
            public readonly string command = "command";
        }
        static readonly ActionParamsClass_SaveTopic s_params_SaveTopic = new ActionParamsClass_SaveTopic();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_SaveTopic SaveTopicParams { get { return s_params_SaveTopic; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_SaveTopic
        {
            public readonly string command = "command";
        }
        static readonly ActionParamsClass_DeleteTopic s_params_DeleteTopic = new ActionParamsClass_DeleteTopic();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_DeleteTopic DeleteTopicParams { get { return s_params_DeleteTopic; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_DeleteTopic
        {
            public readonly string command = "command";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
                public readonly string _AddTopicDialog = "_AddTopicDialog";
                public readonly string _Comment = "_Comment";
                public readonly string _DeleteCommentDialog = "_DeleteCommentDialog";
                public readonly string _EditCommentDialog = "_EditCommentDialog";
                public readonly string _LastComments = "_LastComments";
                public readonly string _TopicListItem = "_TopicListItem";
                public readonly string _TopicMenu = "_TopicMenu";
                public readonly string _TopicsHeader = "_TopicsHeader";
                public readonly string _TopicsList = "_TopicsList";
                public readonly string Edit = "Edit";
                public readonly string Index = "Index";
                public readonly string New = "New";
                public readonly string Topic = "Topic";
            }
            public readonly string _AddTopicDialog = "~/Areas/Topics/Views/Topics/_AddTopicDialog.cshtml";
            public readonly string _Comment = "~/Areas/Topics/Views/Topics/_Comment.cshtml";
            public readonly string _DeleteCommentDialog = "~/Areas/Topics/Views/Topics/_DeleteCommentDialog.cshtml";
            public readonly string _EditCommentDialog = "~/Areas/Topics/Views/Topics/_EditCommentDialog.cshtml";
            public readonly string _LastComments = "~/Areas/Topics/Views/Topics/_LastComments.cshtml";
            public readonly string _TopicListItem = "~/Areas/Topics/Views/Topics/_TopicListItem.cshtml";
            public readonly string _TopicMenu = "~/Areas/Topics/Views/Topics/_TopicMenu.cshtml";
            public readonly string _TopicsHeader = "~/Areas/Topics/Views/Topics/_TopicsHeader.cshtml";
            public readonly string _TopicsList = "~/Areas/Topics/Views/Topics/_TopicsList.cshtml";
            public readonly string Edit = "~/Areas/Topics/Views/Topics/Edit.cshtml";
            public readonly string Index = "~/Areas/Topics/Views/Topics/Index.cshtml";
            public readonly string New = "~/Areas/Topics/Views/Topics/New.cshtml";
            public readonly string Topic = "~/Areas/Topics/Views/Topics/Topic.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_TopicsController : MirGames.Areas.Topics.Controllers.TopicsController
    {
        public T4MVC_TopicsController() : base(Dummy.Instance) { }

        [NonAction]
        partial void RssOverride(T4MVC_MirGames_RssActionResult callInfo, string tag, string searchString);

        [NonAction]
        public override MirGames.RssActionResult Rss(string tag, string searchString)
        {
            var callInfo = new T4MVC_MirGames_RssActionResult(Area, Name, ActionNames.Rss);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "tag", tag);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "searchString", searchString);
            RssOverride(callInfo, tag, searchString);
            return callInfo;
        }

        [NonAction]
        partial void CommentsRssOverride(T4MVC_MirGames_RssActionResult callInfo);

        [NonAction]
        public override MirGames.RssActionResult CommentsRss()
        {
            var callInfo = new T4MVC_MirGames_RssActionResult(Area, Name, ActionNames.CommentsRss);
            CommentsRssOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void IndexOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string tag, string searchString, int page, bool onlyUnread);

        [NonAction]
        public override System.Web.Mvc.ActionResult Index(string tag, string searchString, int page, bool onlyUnread)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Index);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "tag", tag);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "searchString", searchString);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "page", page);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "onlyUnread", onlyUnread);
            IndexOverride(callInfo, tag, searchString, page, onlyUnread);
            return callInfo;
        }

        [NonAction]
        partial void TutorialsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string tag, string searchString, int page);

        [NonAction]
        public override System.Web.Mvc.ActionResult Tutorials(string tag, string searchString, int page)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Tutorials);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "tag", tag);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "searchString", searchString);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "page", page);
            TutorialsOverride(callInfo, tag, searchString, page);
            return callInfo;
        }

        [NonAction]
        partial void TopicOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int topicId);

        [NonAction]
        public override System.Web.Mvc.ActionResult Topic(int topicId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Topic);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "topicId", topicId);
            TopicOverride(callInfo, topicId);
            return callInfo;
        }

        [NonAction]
        partial void TopicListItemOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int topicId);

        [NonAction]
        public override System.Web.Mvc.ActionResult TopicListItem(int topicId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.TopicListItem);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "topicId", topicId);
            TopicListItemOverride(callInfo, topicId);
            return callInfo;
        }

        [NonAction]
        partial void NewOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult New()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.New);
            NewOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void EditOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, int topicId);

        [NonAction]
        public override System.Web.Mvc.ActionResult Edit(int topicId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Edit);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "topicId", topicId);
            EditOverride(callInfo, topicId);
            return callInfo;
        }

        [NonAction]
        partial void AddTopicOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, MirGames.Domain.Topics.Commands.AddNewTopicCommand command);

        [NonAction]
        public override System.Web.Mvc.ActionResult AddTopic(MirGames.Domain.Topics.Commands.AddNewTopicCommand command)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.AddTopic);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "command", command);
            AddTopicOverride(callInfo, command);
            return callInfo;
        }

        [NonAction]
        partial void SaveTopicOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, MirGames.Domain.Topics.Commands.SaveTopicCommand command);

        [NonAction]
        public override System.Web.Mvc.ActionResult SaveTopic(MirGames.Domain.Topics.Commands.SaveTopicCommand command)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SaveTopic);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "command", command);
            SaveTopicOverride(callInfo, command);
            return callInfo;
        }

        [NonAction]
        partial void DeleteTopicOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, MirGames.Domain.Topics.Commands.DeleteTopicCommand command);

        [NonAction]
        public override System.Web.Mvc.ActionResult DeleteTopic(MirGames.Domain.Topics.Commands.DeleteTopicCommand command)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.DeleteTopic);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "command", command);
            DeleteTopicOverride(callInfo, command);
            return callInfo;
        }

        [NonAction]
        partial void EditCommentDialogOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult EditCommentDialog()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.EditCommentDialog);
            EditCommentDialogOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void DeleteCommentDialogOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult DeleteCommentDialog()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.DeleteCommentDialog);
            DeleteCommentDialogOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void AddTopicDialogOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult AddTopicDialog()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.AddTopicDialog);
            AddTopicDialogOverride(callInfo);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009
