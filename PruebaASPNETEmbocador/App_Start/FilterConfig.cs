using System.Web;
using System.Web.Mvc;

namespace PruebaASPNETEmbocador
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
