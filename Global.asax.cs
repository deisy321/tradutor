using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace tradutor
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session == null)
                return; // Sess�o n�o iniciada ainda

            string idiomaSelecionado = null;

            HttpCookie cookie = HttpContext.Current.Request.Cookies["_lang"];
            if (cookie != null)
            {
                idiomaSelecionado = cookie.Value;
            }
            else
            {
                var idiomasNavegador = HttpContext.Current.Request.UserLanguages;
                if (idiomasNavegador != null && idiomasNavegador.Length > 0)
                {
                    idiomaSelecionado = idiomasNavegador[0].Split('-')[0];
                }
            }

            HttpContext.Current.Session["Idioma"] = idiomaSelecionado ?? "pt";
        }
    }
}