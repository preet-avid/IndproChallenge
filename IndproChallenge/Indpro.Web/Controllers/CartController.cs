using Indpro.Web.Helper;
using Microsoft.AspNetCore.Mvc;

namespace Indpro.Web.Controllers;

[SessionValidation]
public class CartController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
