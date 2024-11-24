using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContactManagement.Auth
{
    public class Access : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = (EF.User)httpContext.Session["info"];
            if (user != null)
            {
                return true;
            }
            return false;
        }
    }
}