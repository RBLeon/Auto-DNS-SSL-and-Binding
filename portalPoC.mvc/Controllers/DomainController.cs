using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using portalPoC.lib.Models;
using portalPoC.lib.Services;
using portalPoC.lib.Services.Interfaces;
using portalPoC.mvc.Models;

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
        public async Task<IActionResult> RegisterDomain(DomainModel bedrijfsnaam)
        {
            var result = await _service.CreateDomainAsync(bedrijfsnaam);

            return RedirectToAction("Index", "Home", new {message = result.Message});
        }
    }
}