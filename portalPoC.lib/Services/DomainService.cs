using portalPoC.lib.Models;
using portalPoC.lib.Services.Interfaces;
using System.Configuration;
using Microsoft.Web.Administration;

namespace portalPoC.lib.Services
{
    public class DomainCreationResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }


    

    public class DomainService : IDomainService
    {
        private readonly ITransipService _transipService;
        private readonly ILesslService _lesslService;
        private readonly IBindingService _bindingService;

        public DomainService(ITransipService transipService, ILesslService lesslService, IBindingService bindingService)
        {
            _transipService = transipService;
            _lesslService = lesslService;
            _bindingService = bindingService;
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
                    result.Message = "DNS registration successful";
                }
                else
                {
                    Console.WriteLine(transipResult.Message);
                    result.Message = $"DNS registration failed with errorcode: {transipResult.Message}";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            try
            {
                var bindingResult = await _bindingService.AddBindingAsync(bedrijfsnaam);
                result.IsSuccess = bindingResult.IsSuccess;

                if (bindingResult.IsSuccess)
                {
                    Console.WriteLine($"Status code: {bindingResult.PoShError}");
                    result.Message += ", Binding creation successful";
                }
                else
                {
                    Console.WriteLine(bindingResult.Message);
                    result.Message = $"DNS registration succesful, Binding creation failed with errorcode: {bindingResult.Message}";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            try
            {
                var lesslResult = await _lesslService.AddSslAsync(bedrijfsnaam);
                result.IsSuccess = lesslResult.IsSuccess;

                if (lesslResult.IsSuccess)
                {
                    Console.WriteLine($"Status code: {lesslResult.IsSuccess}"); //hier hoort de output van wacs window te komen, hiermee wachten tot service geïmplementeerd is.
                    result.Message += " & SSL request and installation successful";
                }
                else
                {
                    Console.WriteLine(lesslResult.Message);
                    result.Message = $"DNS registration succesful, Binding creation succesful, SSL failed with errorcode: {lesslResult.Message}";

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

