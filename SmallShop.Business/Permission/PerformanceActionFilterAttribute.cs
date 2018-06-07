using System;
using System.Diagnostics;
using System.Web.Mvc;

namespace SmallShop.BackStage.Business
{
    public class PerformanceActionFilterAttribute : ActionFilterAttribute
    {
        private Stopwatch GetTimer(ControllerContext context, string name)
        {
            string key = "__timer__" + name;
            if (context.HttpContext.Items.Contains(key))
            {
                return (Stopwatch)context.HttpContext.Items[key];
            }

            var result = new Stopwatch();
            context.HttpContext.Items[key] = result;

            return result;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            GetTimer(filterContext, "action").Start();

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            GetTimer(filterContext, "action").Stop();

            base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            GetTimer(filterContext, "render").Start();

            base.OnResultExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var renderTimer = GetTimer(filterContext, "render");
            renderTimer.Stop();

            var actionTimer = GetTimer(filterContext, "action");
            var msg = String.Format(
                            "Action '{0} :: {1}', Execute: {2}ms, Render: {3}ms.",
                            filterContext.RouteData.Values["controller"],
                            filterContext.RouteData.Values["action"],
                            actionTimer.ElapsedMilliseconds,
                            renderTimer.ElapsedMilliseconds
                        );

            Log.WriteExceptionLog(msg);

            base.OnResultExecuted(filterContext);
        }
    }
}
