using Microsoft.Extensions.Options;
using portalPoC.lib.Models;
using portalPoC.lib.Services.Interfaces;
using System.Diagnostics;

namespace portalPoC.lib.Services
{
    public class SslCreationResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        // get some form of errormessage in here
    }

    public class LesslService : ILesslService
    {
        private readonly DomainRegistrationSettings _domainRegistrationSettings;
        public LesslService(IOptions<DomainRegistrationSettings> domainRegistrationSettings)
        {
            _domainRegistrationSettings = domainRegistrationSettings.Value;
        }
        public async Task<SslCreationResult> AddSslAsync(DomainModel businessName)
        {

            var result = new SslCreationResult()
            {
                IsSuccess = false,
                Message = ""
            };
            
            string subdomainName = businessName.DomainName;

            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                CreateNoWindow = false,
                UseShellExecute = true,
                Verb = "runas",
                //Werkt alleen geen admin rechten
                FileName = _domainRegistrationSettings.wacsFileName,
                //todo get parameters from appsettings.json
                Arguments = "--source iis --host \"" + subdomainName + "\".\"" + _domainRegistrationSettings.mainDomain + "\" --siteid \"" + _domainRegistrationSettings.siteid + "\" --transip-login \"" + _domainRegistrationSettings.transipLogin + "\" --validation transip --transip-privatekeyfile \"" + _domainRegistrationSettings.transipPrivateKeyFile + "\" --installation iis"
            };

            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    await exeProcess.WaitForExitAsync();
                }
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
