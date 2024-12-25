using System.Web;
using System.Web.Mvc;

namespace PruebaASPNETEmbocador.Filters
{
    public class SessionCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Verificar si hay un usuario con sesión iniciada
            if (HttpContext.Current.Session["IdUsuario"] == null)
            {
                // Redirigir al login si no hay un usuario con sesión iniciada
                filterContext.Result = new RedirectResult("~/Home/Index");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
