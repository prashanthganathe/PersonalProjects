using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace AngularJSAuthentication.API.Filters
{
    public class ElmahErrorAttribute :
    System.Web.Http.Filters.ExceptionFilterAttribute
    {

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception != null)
            {
                var context = ErrorSignal.FromCurrentContext();
                if (context != null)
                {
                    context.Raise(actionExecutedContext.Exception);
                }
            }
            base.OnException(actionExecutedContext);
        }
    }
}