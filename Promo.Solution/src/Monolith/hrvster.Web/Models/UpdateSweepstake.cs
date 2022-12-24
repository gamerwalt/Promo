using hrvster.Models.WebModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace hrvster.Web.Models
{
	public class UpdateSweepstake
	{
		public Guid SweepUid { get; set; }
		public Guid CampaignUid { get; set; }
		public string Name { get; set; }
		[BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
		public DateTime StartDate { get; set; }
		[BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
		public DateTime EndDate { get; set; }
		public DateTime DrawDate { get; set; }
	}
}
