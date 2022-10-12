using System.ComponentModel.DataAnnotations;

namespace portalPoC.mvc.Models
{
    public class DomainForm
    {
        [Required]
        public string DomainName { get; set; }
    }
}
