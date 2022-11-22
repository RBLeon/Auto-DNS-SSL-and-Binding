using portalPoC.lib.Models;

namespace portalPoC.lib.Services.Interfaces
{
    public interface IBindingService
    {
        Task<BindingCreationResult> AddBindingAsync(DomainModel businessName);
    }
}
