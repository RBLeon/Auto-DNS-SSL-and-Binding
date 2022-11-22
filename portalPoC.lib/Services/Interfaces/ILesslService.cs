using portalPoC.lib.Models;

namespace portalPoC.lib.Services.Interfaces
{
    public interface ILesslService
    {
        Task<SslCreationResult> AddSslAsync(DomainModel businessName);
    }
}
