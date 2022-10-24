using portalPoC.lib.Models;
using portalPoC.lib.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        //attributes

        public async Task<SslCreationResult> AddSslAsync(DomainModel bedrijfsnaam)
        {

            var result = new SslCreationResult()
            {
                IsSuccess = false,
                Message = ""
            };
            
            string subdomainName = bedrijfsnaam.DomainName;

            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                CreateNoWindow = false,
                UseShellExecute = true,
                Verb = "runas",
                //Werkt alleen geen admin rechten
                FileName = "wacs.exe",
                Arguments = "--source iis --host \"" + subdomainName + "\".auxil-portaal.nl --siteid 2 --transip-login auxilportaal --validation transip --transip-privatekeyfile C:\\localiis\\transip.key --installation iis"
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
