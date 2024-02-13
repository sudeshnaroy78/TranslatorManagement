using System;
using System.Collections.Generic;
using TranslationManagement.Api.Model;

namespace TranslationManagement.Api.Service
{
	public interface ITranslationJobService
	{
        public TranslationJob[] GetJobs();
        public List<TranslationJob> GetJobsbyCustomerName(string customerName);
        public bool CreateJob(TranslationJob job);
        public string UpdateJobStatus(int jobId, int translatorId, string newStatus = "");

    }
}

