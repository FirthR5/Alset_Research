﻿using System.ComponentModel.DataAnnotations;

namespace Alset_Research.DTO
{
	public class UploadJournalDTO
	{
		// Title
		[StringLength(100, MinimumLength = 5)]
		[Required(ErrorMessage = "Please enter the title of your book")]
		public string Title { get; set; }
		// Description
		[StringLength(500)]
		public string Description { get; set; }
		// File
		[Display(Name = "Upload your book in pdf format")]
		[Required]
		public IFormFile JournalPDF { get; set; }
	}
}
