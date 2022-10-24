using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using portalPoC.lib.Models;

namespace portalPoC.lib.Services.Interfaces
{
    public interface IDomainService
    {
        Task<DomainCreationResult> CreateDomainAsync(DomainModel bedrijfsnaam);
    }
}
