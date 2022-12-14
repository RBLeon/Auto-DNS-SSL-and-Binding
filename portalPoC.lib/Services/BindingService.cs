using portalPoC.lib.Models;
using portalPoC.lib.Services.Interfaces;
using Microsoft.Web.Administration;
using Microsoft.Extensions.Options;

namespace portalPoC.lib.Services
{
    public class BindingCreationResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string PoShError { get; set; } // check how this works = PowerShell.Streams.Error
    }

    public class BindingService : IBindingService
    {
        //attributes
        private string? _businessName;
        //private readonly string _command = "New-IISSiteBinding -Name \"portalPoC\" -BindingInformation \"*:80:" + _businessName + ".auxil-portaal.nl\" -Protocol http";

        private readonly DomainRegistrationSettings _domainRegistrationSettings;
        public BindingService(IOptions<DomainRegistrationSettings> domainRegistrationSettings)
        {
            _domainRegistrationSettings = domainRegistrationSettings.Value;
        }

        public async Task<BindingCreationResult> AddBindingAsync(DomainModel businessName)
        {
            _businessName =  businessName.DomainName;
            //PowerShell ps = PowerShell.Create();
            //ps.AddCommand(_command);

            var result = new BindingCreationResult()
            {
                IsSuccess = false,
                Message = "Unknown problem occurred",
                PoShError = ""
            };

            try
            {
                // * 
                var bindName = "*:80:" + _businessName + "." + _domainRegistrationSettings.mainDomain;
                var server = new ServerManager();
                var site = server.Sites.FirstOrDefault(a => a.Name == _domainRegistrationSettings.iisSiteName);
                site.Bindings.Add(bindName, "http");
                server.CommitChanges();
                result.IsSuccess = true;
                result.Message = "OK";

            }
            catch (Exception e)
            {
                result.Message = $"Exception occurred during creation of entry: {e.Message}";
            }

            return result;
        }
    }
}

// * :
// heeft blijkbaar ook elevated rechten nodig, heb dit blijkbaar getest in powershell prompt run as admin

//PowerShell ps = PowerShell.Create();

//ps.AddCommand("New-IISSiteBinding")
//    .AddParameter("Name", "portalPoC")
//    .AddParameter("BindingInformation", "*:80:" + _businessName + ".auxil-portaal.nl")
//    .AddParameter("Protocol", "http")
//    .Invoke();

//    result.IsSuccess = true;
//    result.Message = "OK";


//proberen zonder powershell maar met microsoft.web.adminstration

//voorbeeld internet:
//using (ServerManager manager = new ServerManager())
//{
//    Site site = manager.Sites["portalPoC"];
//    site.Bindings.Clear();
//    site.Bindings.Add("*:80:", "http");

//    manager.CommitChanges();
//}


// ** : 
//public static bool AddBindings(IConfiguration configuration, ILogger logger, string subdomain)
//{
//    if (string.IsNullOrWhiteSpace(subdomain)) return false;

//    try
//    {
//        var server = new ServerManager();
//        var sitename = EnvironmentHelper.IsProd(configuration) ? "<domain>.no" : "test.<domain>.no";
//        var hostname = $"{subdomain.ToLowerInvariant().Trim()}.<domain>.no";

//        var site = server.Sites.FirstOrDefault(a => a.Name == sitename);
//        if (site == null) return false;

//        logger.LogInformation($"IIS: Sitename {sitename} found. Trying to add bindings...");

//        var hostHttpsBinding = site.Bindings.FirstOrDefault(a => a.BindingInformation == $"*:443:{sitename}");
//        var httpBinding = site.Bindings.FirstOrDefault(a => a.BindingInformation == $"*:80:{hostname}");
//        var httpsBinding = site.Bindings.FirstOrDefault(a => a.BindingInformation == $"*:443:{hostname}");

//        if (httpBinding != null && httpsBinding != null) return false;

//        if (httpBinding == null)
//        {
//            site.Bindings.Add($"*:80:{hostname}", "http");

//            logger.LogInformation($"IIS: Http-binding added for {hostname}");
//        }

//        if (httpsBinding == null)
//        {
//            logger.LogInformation($"SSLName: {hostHttpsBinding.CertificateStoreName}, Hash: {hostHttpsBinding.CertificateHash}");
//            site.Bindings.Add($"*:443:{hostname}", hostHttpsBinding.CertificateHash, hostHttpsBinding.CertificateStoreName, SslFlags.Sni);

//            logger.LogInformation($"IIS: Https-binding added for {hostname}");
//        }

//        server.CommitChanges();
//        server.Dispose();

//        return true;
//    }

//    catch (Exception e)
//    {
//        logger.LogError($"IIS: Something went wrong when adding bindings: {e.Message}, {e.InnerException.Message}");
//        return false;
//    }
//}