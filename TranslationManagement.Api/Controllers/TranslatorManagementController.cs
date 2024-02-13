using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TranslationManagement.Api.Model;
using TranslationManagement.Api.Service;

namespace TranslationManagement.Api.Controlers
{  // [Authorize]
    [ApiController]
    [Route("api/TranslatorsManagement/[action]")]
    public class TranslatorManagementController : ControllerBase
    {

        private readonly ITranslationManagementService _translatorManagementService;
        private readonly ILogger<TranslatorManagementController> _logger;
        
        public TranslatorManagementController(ITranslationManagementService translationManagementService, ILogger<TranslatorManagementController> logger)
        {
            _translatorManagementService = translationManagementService;
            _logger = logger;
        }
        /// <summary>
        /// This method to get all list of translator
        /// </summary>
        /// <returns>Array of TranslationJob</returns>
     
        [HttpGet]
        public TranslatorModel[] GetTranslators()
        {
            return _translatorManagementService.GetTranslators();
        }
        /// <summary>
        /// This method to get all list of translator by name
        /// </summary>
        /// <param name="name">string</param>
        /// <returns>Array of TranslationJob</returns>
       
        [HttpGet]
        public TranslatorModel[] GetTranslatorsByName(string name)
        {
            return _translatorManagementService.GetTranslatorsByName(name);
        }
        /// <summary>
        /// This method to add new translator
        /// </summary>
        /// <param name="translator"></param>
        /// <returns>bool</returns>
       
        [HttpPost]
        public bool AddTranslator(TranslatorModel translator)
        {
            return _translatorManagementService.AddTranslator(translator);
        }
        /// <summary>
        /// This method to update translator status by translator id
        /// </summary>
        /// <param name="Translator">int:uniqueId</param>
        /// <param name="newStatus">string</param>
        /// <returns>string</returns>
    
        [HttpPost]
        public string UpdateTranslatorStatus(int Translator, string newStatus = "")
        {
            _logger.LogInformation("User status update request: " + newStatus + " for user " + Translator.ToString());
            return _translatorManagementService.UpdateTranslatorStatus(Translator, newStatus);
        }


    }
}