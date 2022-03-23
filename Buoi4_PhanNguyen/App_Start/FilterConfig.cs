using System.Web;
using System.Web.Mvc;

namespace Buoi4_PhanNguyen
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
