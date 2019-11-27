using System.Web.Mvc;

namespace ClosestAddress.Controllers
{
    public class ClosestAddressController : Controller
    {
        /// <summary>
        /// Displays an index page.
        /// </summary>
        /// <returns>Action Result</returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}