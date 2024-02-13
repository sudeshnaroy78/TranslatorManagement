using System;
using System.Collections.Generic;
using TranslationManagement.Api.Model;

namespace TranslationManagement.Api.Service
{
	public interface IJobAssignmentService
	{

		public bool JobAssignToCertifiedTranslator(int JobId, int translaotorId);
		public List<TranslationJob> GetAllJobAssignedToTranslator(int translatorId);


    }
}

