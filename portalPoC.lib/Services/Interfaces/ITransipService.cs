using portalPoC.lib.Models;

namespace portalPoC.lib.Services.Interfaces
{
    public interface ITransipService
    {
        Task<DnsEntryResult> AddDnsEntryAsync(DomainModel businessName);
    }
}
