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
namespace MirGames.Controllers
{
    public partial class GitController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected GitController(Dummy d) { }

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
        public virtual System.Web.Mvc.ActionResult GetInfoRefs()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.GetInfoRefs);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult UploadPack()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UploadPack);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult ReceivePack()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ReceivePack);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public GitController Actions { get { return MVC.Git; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Git";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Git";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string GetInfoRefs = "GetInfoRefs";
            public readonly string UploadPack = "UploadPack";
            public readonly string ReceivePack = "ReceivePack";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string GetInfoRefs = "GetInfoRefs";
            public const string UploadPack = "UploadPack";
            public const string ReceivePack = "ReceivePack";
        }


        static readonly ActionParamsClass_GetInfoRefs s_params_GetInfoRefs = new ActionParamsClass_GetInfoRefs();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_GetInfoRefs GetInfoRefsParams { get { return s_params_GetInfoRefs; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_GetInfoRefs
        {
            public readonly string project = "project";
            public readonly string service = "service";
        }
        static readonly ActionParamsClass_UploadPack s_params_UploadPack = new ActionParamsClass_UploadPack();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_UploadPack UploadPackParams { get { return s_params_UploadPack; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_UploadPack
        {
            public readonly string project = "project";
        }
        static readonly ActionParamsClass_ReceivePack s_params_ReceivePack = new ActionParamsClass_ReceivePack();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ReceivePack ReceivePackParams { get { return s_params_ReceivePack; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ReceivePack
        {
            public readonly string project = "project";
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
            }
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_GitController : MirGames.Controllers.GitController
    {
        public T4MVC_GitController() : base(Dummy.Instance) { }

        [NonAction]
        partial void GetInfoRefsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string project, string service);

        [NonAction]
        public override System.Web.Mvc.ActionResult GetInfoRefs(string project, string service)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.GetInfoRefs);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "project", project);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "service", service);
            GetInfoRefsOverride(callInfo, project, service);
            return callInfo;
        }

        [NonAction]
        partial void UploadPackOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string project);

        [NonAction]
        public override System.Web.Mvc.ActionResult UploadPack(string project)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.UploadPack);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "project", project);
            UploadPackOverride(callInfo, project);
            return callInfo;
        }

        [NonAction]
        partial void ReceivePackOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string project);

        [NonAction]
        public override System.Web.Mvc.ActionResult ReceivePack(string project)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ReceivePack);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "project", project);
            ReceivePackOverride(callInfo, project);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009
