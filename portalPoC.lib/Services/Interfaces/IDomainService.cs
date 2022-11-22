using portalPoC.lib.Models;

namespace portalPoC.lib.Services.Interfaces
{
    public interface IDomainService
    {
        Task<DomainCreationResult> CreateDomainAsync(DomainModel businessName);
    }
}
