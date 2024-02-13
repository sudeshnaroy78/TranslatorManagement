using System;
using System.Collections.Generic;
using System.Linq;
using TranslationManagement.Api.Data;
using TranslationManagement.Api.Model;

namespace TranslationManagement.Api.Service
{
    public class JobAssignmentService : IJobAssignmentService
    {
        private AppDbContext _context;
        public JobAssignmentService(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public bool JobAssignToCertifiedTranslator(int jobId, int translaotorId)
        {

            var translator = _context.Translators.Where( x => x.Status == "Certified").Single(x => x.Id == translaotorId);
           
            var job = _context.TranslationJobs.Single(x => x.Id == jobId);
            if (translator != null)
                job.TranslatorId = translator.Id;
            else
                throw new NullReferenceException("Translator not found with Certified Status");

            _context.TranslationJobs.Update(job);
            return _context.SaveChanges() > 0;
        }
        public List<TranslationJob> GetAllJobAssignedToTranslator(int translatorId)
        {
            
            var jobs = _context.TranslationJobs.Where(x => x.TranslatorId == translatorId).ToList();
            return jobs;
        }
        
    }
}

