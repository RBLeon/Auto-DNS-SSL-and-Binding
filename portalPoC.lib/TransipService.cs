using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using portalPoC.lib.Models;

namespace portalPoC.lib
{
    public class DnsEntryResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public HttpStatusCode StatusCode{ get; set; }
    }


    public interface ITransipService
    {
        Task<DnsEntryResult> AddDnsEntryAsync(DomainModel bedrijfsnaam);
    }

    public class TransipService : ITransipService
    {
        private readonly string _url = "https://api.transip.nl/v6/domains/auxil-portaal.nl/dns";
        private readonly string _stringIp = "127.0.0.1";
        private readonly string _accessToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImp0aSI6IjJhaCFkc0tJS3hiUm1tMGxDU2ZIIn0.eyJpc3MiOiJhcGkudHJhbnNpcC5ubCIsImF1ZCI6ImFwaS50cmFuc2lwLm5sIiwianRpIjoiMmFoIWRzS0lLeGJSbW0wbENTZkgiLCJpYXQiOjE2NjMzMjk3MjgsIm5iZiI6MTY2MzMyOTcyOCwiZXhwIjoxNjY1OTIxNzI4LCJjaWQiOiIyMDA2MjYyODUiLCJybyI6ZmFsc2UsImdrIjp0cnVlLCJrdiI6ZmFsc2V9.klQYwWvt6WRQC_Pso0gH4uPE9KDjj2f5SBHb5mp07Jf0NO8PgFYcaGnxr00SipPTTLTb91bCpygoBZaTCIgFG_bx1gqcPu4msgzyvH0eF-Gas673CWHVq5zHOod2MoX0s7u9zmrOQTLGj82YUB6l2t7AK0YeO_34-ivgpzheIEZNGu12v-neL0WriLzLXOjEQMXDE5N46lNjk5jUbYRsHGbgJwvCAaj1KsPHoRzOtDc47R06FbHoX313RSow8EFtNpVlYxkid7OsOlIKF0w9DmZzCfd6AvMchV3a2YpL8vxIlB9ODgj6-jjfk3ps1OUkw6VdtCKaMNp_sukSo1xUWw";

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
