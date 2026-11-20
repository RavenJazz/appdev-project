using Microsoft.AspNetCore.Mvc;

namespace GovernmentWebsite.Controllers;

public class AdminController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
