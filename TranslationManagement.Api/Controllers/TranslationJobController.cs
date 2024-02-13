using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using External.ThirdParty.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TranslationManagement.Api.Controlers;
using TranslationManagement.Api.Model;
using TranslationManagement.Api.Service;

namespace TranslationManagement.Api.Controllers
{
   // [Authorize]
    [ApiController]
    [Route("api/jobs/[action]")]
    public class TranslationJobController : ControllerBase
    {

        private readonly ITranslationJobService _translationService;
        private readonly ILogger<TranslationJobController> _logger;

        public TranslationJobController(ITranslationJobService translationService, ILogger<TranslationJobController> logger)
        {
            _translationService = translationService;
            _logger = logger;
        }
        /// <summary>
        /// This method to get all list of jobs 
        /// </summary>
        /// <returns>Array of TranslationJob</returns>
       
        [HttpGet]
        public TranslationJob[] GetJobs()
        {
            try
            {
                return _translationService.GetJobs();
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
           
        }
        /// <summary>
        /// This method to set price for the Job
        /// </summary>
        const double PricePerCharacter = 0.01;
        private void SetPrice(TranslationJob job)
        {
            job.Price = job.OriginalContent.Length * PricePerCharacter;
        }
        [HttpGet]
        public List<TranslationJob> GetJobsbyCustomerName(string customerName)
        {
            return _translationService.GetJobsbyCustomerName(customerName);


        }
        /// <summary>
        /// This method to create Job with TranslationJob object
        /// </summary>
        /// <param name="job">object of TranslationJob</param>
        /// <returns>bool value based on result</returns>
        [HttpPost]
        public bool CreateJob(TranslationJob job)
        {
            try
            {
                job.Status = "New";
                SetPrice(job);
                bool success = _translationService.CreateJob(job);
                // _context.TranslationJobs.Add(job);
                //bool success = _context.SaveChanges() > 0;

                if (success)
                {
                    var notificationSvc = new UnreliableNotificationService();
                    while (!notificationSvc.SendNotification("Job created: " + job.Id).Result)
                    {
                    }

                    _logger.LogInformation("New job notification sent");
                }

                return success;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
        /// <summary>
        /// This method to create Job with input file and customer name
        /// </summary>
        /// <param name="file">file</param>
        /// <param name="customer">string</param>
        /// <returns>bool value based on result</returns>
        /// <exception cref="NotSupportedException">throw exception based on file type: "unsupported file"</exception>
        [HttpPost]
        public bool CreateJobWithFile(IFormFile file, string customer)
        {
            try
            {
                var reader = new StreamReader(file.OpenReadStream());
                string content;
                FileInfo fi = new FileInfo(file.FileName);
                string ext = fi.Extension?? "";
                switch (ext)
                {
                    case ".txt":
                        content = reader.ReadToEnd();
                        break;
                    case ".xml":
                        var xdoc = XDocument.Parse(reader.ReadToEnd());
                        content = xdoc.Root.Element("Content").Value;
                        customer = xdoc.Root.Element("Customer").Value.Trim();
                        break;
                    case ".csv":
                    case ".doc":
                        throw new NotSupportedException("unsupported file");
                    //need to add implementation in future
                    default:
                        throw new NotSupportedException("unsupported file");
                }

                var newJob = new TranslationJob()
                {
                    OriginalContent = content,
                    TranslatedContent = "",
                    CustomerName = customer,
                };

                SetPrice(newJob);

                return CreateJob(newJob);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

        }
        /// <summary>
        /// This method to update Job status by jobId,translatorId
        /// </summary>
        /// <param name="jobId">Int: uniqueId</param>
        /// <param name="translatorId">Int: uniqueId</param>
        /// <param name="newStatus">string</param>
        /// <returns>bool value based on result</returns>
        [HttpPost]
        public string UpdateJobStatus(int jobId, int translatorId, string newStatus = "")
        {
            try
            { 
            _logger.LogInformation("Job status update request received: " + newStatus + " for job " + jobId.ToString() + " by translator " + translatorId);
            _translationService.UpdateJobStatus(jobId, translatorId, newStatus);
            return "updated";
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
        
    }
}