using System.ComponentModel.DataAnnotations;

namespace portalPoC.lib.Models
{
    public class DomainModel
    {
        [Required]
        public string? DomainName { get; set; }
    }


    public class Rootobject
    {
        public DnsEntry dnsEntry { get; set; }
    }

    public class DnsEntry
    {
        [Required]
        public string? name { get; set; }
        public int expire { get; set; }
        public string type { get; set; }
        public string content { get; set; }
    }

}
