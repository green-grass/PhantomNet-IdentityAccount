using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PhantomNet.IdentityAccount
{
    [Authorize]
    [Route("Accounts")]
    public abstract class IdentityAccountsControllerBase : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
