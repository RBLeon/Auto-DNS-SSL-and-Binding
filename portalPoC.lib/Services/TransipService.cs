using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using portalPoC.lib.Models;
using portalPoC.lib.Services.Interfaces;

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
        private readonly string _url = "https://api.transip.nl/v6/domains/auxil-portaal.nl/dns";
        private readonly string _stringIp = "127.0.0.1";
        private readonly string _accessToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImp0aSI6IkJybzdNJmI4R3RDJUNnV2dXOGF3In0.eyJpc3MiOiJhcGkudHJhbnNpcC5ubCIsImF1ZCI6ImFwaS50cmFuc2lwLm5sIiwianRpIjoiQnJvN00mYjhHdEMlQ2dXZ1c4YXciLCJpYXQiOjE2NjYzNjQyMjAsIm5iZiI6MTY2NjM2NDIyMCwiZXhwIjoxNjY5MDQ2MjIwLCJjaWQiOiIyMDA2MjYyODUiLCJybyI6ZmFsc2UsImdrIjp0cnVlLCJrdiI6ZmFsc2V9.nOYVasYL8roDo9OkPGr9s-Enc6tgE65fu-9iIITeMRNnEK6W-duRCSB9pyBQSLUNDHmlZAcQSWPLA0Gq52DtI371GbdXQQsSlx6GxOM6tDYHij9HA6Mrar_QTnicctlv_bZJ2Q-vgUfScgheaJLZothDk14Eyj3lHzyvpwRzjg1I7CbEUje1lFM8pKaXm0v_EjlO007utEk-yj1BiSC0DRBD8QvZYga9uAEmgyk3Vi8wiF2cqlxWeJVdzwOezJ0_5_T38j4DxzZMZk4jL5SJpphTBW226oi4ofbyxa2nuluFEgQMD4j1fovw80LYec42_B8vZmm1JTxOb7l4TuekjQ";

        public async Task<DnsEntryResult> AddDnsEntryAsync(DomainModel bedrijfsnaam)
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
                        name = bedrijfsnaam.DomainName,
                        expire = 86400,
                        type = "A",
                        content = _stringIp
                    }
                };


                //use factory IHttpClientFactory and .CreateClient() in production
                HttpClient client = new();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", $"{_accessToken}");

                var jsonContent = JsonConvert.SerializeObject(dnsEntry);
                var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                contentString.Headers.ContentType = new
                    MediaTypeHeaderValue("application/json");

                var response = await client.PostAsync(_url, contentString);
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
