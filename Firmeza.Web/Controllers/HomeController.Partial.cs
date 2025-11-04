using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.Web.Controllers;

public partial class HomeController
{
    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }
}

