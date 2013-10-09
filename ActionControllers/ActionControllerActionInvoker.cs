using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ActionControllers
{
    public class ActionControllerActionInvoker : ControllerActionInvoker
    {
        protected override ActionDescriptor FindAction(ControllerContext controllerContext, ControllerDescriptor controllerDescriptor, string actionName)
        {
            var method = controllerContext.HttpContext.Request.HttpMethod;

            var actionDescriptor = controllerDescriptor.FindAction(controllerContext, method);
            return actionDescriptor;
        }
    }
}