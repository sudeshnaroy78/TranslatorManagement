using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TranslationManagement.Api.Data;
using TranslationManagement.Api.Model;

namespace TranslationManagement.Api.Service
{
	public class TranslationJobService:ITranslationJobService
	{

		private readonly AppDbContext _appDbContext;

        
        public TranslationJobService(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}

		public TranslationJob[] GetJobs()
		{
            var res= _appDbContext.TranslationJobs.ToArray();
            return res;
		}

        public List<TranslationJob> GetJobsbyCustomerName(string customerName)
        {

            return _appDbContext.TranslationJobs.Where(x => x.CustomerName ==  customerName).ToList();

                }

        public bool CreateJob(TranslationJob job)
		{
			_appDbContext.TranslationJobs.Add(job);
			return _appDbContext.SaveChanges() > 0?true:false;
		}
		public string UpdateJobStatus(int jobId, int translatorId, string newStatus = "")
		{
            
            if (typeof(JobStatuses).GetProperties().Count(prop => prop.Name == newStatus) == 0)
            {
                return "invalid status";
            }

            var job = _appDbContext.TranslationJobs.Single(j => j.Id == jobId);
            bool isInvalidStatusChange = (job.Status == JobStatuses.New && newStatus == JobStatuses.Completed) ||
                                        job.Status == JobStatuses.Completed || newStatus == JobStatuses.New;
            if (isInvalidStatusChange)
            {
                return "invalid status change";
            }

            job.Status = newStatus;
            _appDbContext.SaveChanges();
            return "updated";
        }
    }
}

