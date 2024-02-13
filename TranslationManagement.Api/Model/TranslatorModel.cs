using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TranslationManagement.Api.Model
{
	public class TranslatorModel
	{
     
        public int Id { get; set; }
        public string Name { get; set; }
        public string HourlyRate { get; set; }
        public string Status { get; set; }
        public string CreditCardNumber { get; set; }
      
       
    }
}

