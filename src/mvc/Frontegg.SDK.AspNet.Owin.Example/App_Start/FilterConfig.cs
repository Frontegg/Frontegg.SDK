using System.Web.Mvc;

namespace Frontegg.SDK.AspNet.Owin.Example
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}