using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using portalPoC.lib.Models;
using portalPoC.lib.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace portalPoC.lib.Services
{
    public class DnsEntryResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }

    public class TransipService : ITransipService
    {
        private readonly DomainRegistrationSettings _domainRegistrationSettings;

        public TransipService(IOptions<DomainRegistrationSettings> domainRegistrationSettings)
        {
            _domainRegistrationSettings = domainRegistrationSettings.Value;
        }

        public async Task<DnsEntryResult> AddDnsEntryAsync(DomainModel businessName)
        {
            var result = new DnsEntryResult()
            {
                IsSuccess = false,
                Message = "Unknown problem occurred"
            };

            try
            {
                var dnsEntry = new Rootobject
                {
                    dnsEntry = new DnsEntry()
                    {
                        name = businessName.DomainName,
                        expire = 86400,
                        type = "A",
                        content = _domainRegistrationSettings.stringIp
                    }
                };


                //use factory IHttpClientFactory and .CreateClient() in production
                HttpClient client = new();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", _domainRegistrationSettings.accessToken);

                var jsonContent = JsonConvert.SerializeObject(dnsEntry);
                var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                contentString.Headers.ContentType = new
                    MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(_domainRegistrationSettings.url, contentString);
                result.IsSuccess = response.IsSuccessStatusCode;
                result.Message = response.IsSuccessStatusCode ? "OK" : response.ReasonPhrase;
                result.StatusCode = response.StatusCode;
            }
            catch (Exception e)
            {
                result.Message = $"Exception occurred during creation of entry: {e.Message}";
            }

            return result;
        }
    }
}
