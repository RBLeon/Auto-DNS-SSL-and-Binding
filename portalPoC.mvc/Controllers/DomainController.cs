using Microsoft.AspNetCore.Mvc;
using portalPoC.lib.Models;
using portalPoC.lib.Services.Interfaces;

namespace portalPoC.mvc.Controllers
{
    public class DomainController : Controller
    {
        private readonly IDomainService _service;

        public DomainController(IDomainService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterDomain(DomainModel businessName)
        {
            var result = await _service.CreateDomainAsync(businessName);

            return RedirectToAction("Index", "Home", new {message = result.Message});
        }
    }
}