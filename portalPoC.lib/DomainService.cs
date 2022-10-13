using portalPoC.lib.Models;

namespace portalPoC.lib
{
    public class DomainCreationResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }


    public interface IDomainService
    {
        Task<DomainCreationResult> CreateDomainAsync(DomainModel bedrijfsnaam);
    }

    public class DomainService : IDomainService
    {
        private readonly ITransipService _transipService;

        public DomainService(ITransipService transipService)
        {
            _transipService = transipService;
        }
        public async Task<DomainCreationResult> CreateDomainAsync(DomainModel bedrijfsnaam)
        {
            var result = new DomainCreationResult()
            {
                IsSuccess = false,
                Message = "Unknown problem occurred"
            };

            try
            {
                var transipResult = await _transipService.AddDnsEntryAsync(bedrijfsnaam);
                result.IsSuccess = transipResult.IsSuccess;

                if (transipResult.IsSuccess)
                {
                    Console.WriteLine($"Status code: {transipResult.StatusCode}");
                    result.Message = "registration successful";
                }
                else
                {
                    Console.WriteLine(transipResult.Message);
                    result.Message = $"registration failed with errorcode: {transipResult.Message}";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


            return result;
        }
    }
}
