using System;
using TranslationManagement.Api.Model;

namespace TranslationManagement.Api.Service
{
	public interface ITranslationManagementService
	{
        public TranslatorModel[] GetTranslators();
        public TranslatorModel[] GetTranslatorsByName(string name);
        public bool AddTranslator(TranslatorModel translator);
        public string UpdateTranslatorStatus(int Translator, string newStatus = "");

    }
}

