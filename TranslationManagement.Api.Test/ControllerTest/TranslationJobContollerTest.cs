using System.Linq;
using Microsoft.Extensions.Logging;
using Moq;
using TranslationManagement.Api.Controllers;
using TranslationManagement.Api.Model;
using TranslationManagement.Api.Service;
namespace TranslationManagement.Api.Test.ControllerTest
{
	public class TranslationJobContollerTest
	{
		private readonly Mock<ILogger<TranslationJobController>> _logger;
		private readonly Mock<ITranslationJobService> _mockService;
		private readonly TranslationJobController _translationJobController;
		public TranslationJobContollerTest()
		{
			_mockService = new Mock<ITranslationJobService>();
			_logger = new Mock<ILogger<TranslationJobController>>();
			_translationJobController = new TranslationJobController(_mockService.Object, _logger.Object);
        }
		[Fact]
		public void GetJobTest()
		{
			//arrange
			TranslationJob[] Job = new TranslationJob[0];
			//act
			var testJob = _translationJobController.GetJobs();
			//assert
			Assert.Equal(Job,testJob);
        }
		[Fact]
		public void CreatJobTest()
		{
			//arrange
			TranslationJob Job = new TranslationJob {
				Id=1,
				CustomerName = "TEST_Customer",
				Status="New",
				OriginalContent="abc",
				TranslatedContent="abcd",
				Price=1
			};
            //act
            var testJob = _translationJobController.CreateJob(Job);
            //assert
            Assert.False(testJob);

        }
	}
}

