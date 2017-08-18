using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestorMapeos.App_Start
{
    public class BasicAuthenticationAttribute : ActionFilterAttribute 
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var authentication = ConfigurationManager.AppSettings["authentication"];
            var userName = authentication.Split(';')[0].Split(':')[1];
            var password = authentication.Split(';')[1].Split(':')[1];
            var req = filterContext.HttpContext.Request;
            var auth = req.Headers["Authorization"];
            if (!string.IsNullOrEmpty(auth))
            {
                var bytes = Convert.FromBase64String(password);
                password = System.Text.Encoding.UTF8.GetString(bytes);

                var cred = System.Text.Encoding.ASCII.GetString(Convert.FromBase64String(auth.Substring(6))).Split(':');
                var user = new { Name = cred[0], Pass = cred[1] };
                if (user.Name == userName && user.Pass == password) return;
            }
            var res = filterContext.HttpContext.Response;
            res.StatusCode = 401;
            res.AddHeader("WWW-Authenticate", "Basic realm=\"Gestor de Mapeos\"");
            res.End();
        }
    }
}