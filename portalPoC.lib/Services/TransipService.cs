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

        //todo get values from appsettings.json
        private readonly string _url = "https://api.transip.nl/v6/domains/auxil-portaal.nl/dns";
        private readonly string _stringIp = "127.0.0.1";
        private readonly string _accessToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImp0aSI6InlCYyNtZXdaVkh3RzdMZCZZUzllIn0.eyJpc3MiOiJhcGkudHJhbnNpcC5ubCIsImF1ZCI6ImFwaS50cmFuc2lwLm5sIiwianRpIjoieUJjI21ld1pWSHdHN0xkJllTOWUiLCJpYXQiOjE2Njg2Nzg0NjcsIm5iZiI6MTY2ODY3ODQ2NywiZXhwIjoxNjcxMjcwNDY3LCJjaWQiOiIyMDA2MjYyODUiLCJybyI6ZmFsc2UsImdrIjp0cnVlLCJrdiI6ZmFsc2V9.Y0b9KyWdy-gF30dX3_M1MbxAQmj6Z2MwK_gtOrRGOcDCZ31B4dALM9v2r27rwDVS4I3Pi95hS4qiM3GvhtTrR56-RCwIS4G07ZDFksuTVtccHtRg5JubqNxNgpawHKYIpxjEuRdIITNutkiywLE6XGXY7xW05Y518xqYFpVMD2F_jAwu3D7MddnDcyE_msxbMzn9xllW4mN2e657dtvs6oF3SUYiGrKtnCoJ2fHwHFi8embhleCbBmWM4WjWy83czOvjan_XLJsbshh6MNY7T-D0UKyxYOdPB7YA5ArtLJiJR4n5tCQHc88XJzRZQdhvLhG_eypgHegNkHGudrvW0w";

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
