using System;
using System.Linq;
using TranslationManagement.Api.Data;
using TranslationManagement.Api.Model;

namespace TranslationManagement.Api.Service
{
	public class TranslationManagementService:ITranslationManagementService
	{
        private AppDbContext _context;
        public TranslationManagementService(AppDbContext appDbContext)
		{
            _context = appDbContext;
		}

        public TranslatorModel[] GetTranslators()
        {
            return _context.Translators.ToArray();
        }

        public TranslatorModel[] GetTranslatorsByName(string name)
        {
            return _context.Translators.Where(t => t.Name == name).ToArray();
        }
        public bool AddTranslator(TranslatorModel translator)
        {
            _context.Translators.Add(translator);
            return _context.SaveChanges() > 0;

        }
        public string UpdateTranslatorStatus(int Translator, string newStatus = "")
        {
            if (TranslatorStatus.TranslatorStatuses.Where(status => status == newStatus).Count() == 0)
            {
                throw new ArgumentException("unknown status");
            }

            var job = _context.Translators.Single(j => j.Id == Translator);
            job.Status = newStatus;
            _context.SaveChanges();

            return "updated";
        }



    }
}

