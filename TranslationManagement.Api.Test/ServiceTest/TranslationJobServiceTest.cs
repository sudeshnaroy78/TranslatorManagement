using System;
using TranslationManagement.Api.Service;
using TranslationManagement.Api.Model;
using TranslationManagement.Api.Data;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using System.Linq.Expressions;
using System.Xml;
using System.Net.Sockets;

namespace TranslationManagement.Api.Test.ServiceTest
{
    public class TranslationJobServiceTest
    {

        /// <summary>
        /// Test case to call Service call method
        /// </summary>

        [Fact]
        public void GetAllTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "JobDataBase")
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new AppDbContext(options))
            {
                context.TranslationJobs.Add(new TranslationJob
                {
                    Id = 1,
                    CustomerName = "test",
                    OriginalContent = "test",
                    TranslatedContent = "test1",
                    Price = 1,
                    Status = "New"
                });
                context.TranslationJobs.Add(new TranslationJob {

                    Id = 2,
                    CustomerName = "test",
                    OriginalContent = "test",
                    TranslatedContent = "test1",
                    Price = 1,
                    Status = "New"
                });
               
                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = new AppDbContext(options))
            {
                var serviceResponse = new TranslationJobService(context);
              
                TranslationJob[] jobs = serviceResponse.GetJobs();

                Assert.Equal(2, jobs.ToList().Count);
            }
        }

    }


}

