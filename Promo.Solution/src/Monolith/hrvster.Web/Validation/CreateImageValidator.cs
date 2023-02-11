using FluentValidation;
using hrvster.Web.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace hrvster.Web.Validation
{
	public class CreateImageValidator : AbstractValidator<CreateImage>
	{
		private const int FiveMegaBytes = 5 * 1024 * 1024;
		private string[] AllowedExtensions = { ".jpeg", ".JPEG", ".jpg", ".JPG", ".png", ".PNG", ".gif", ".GIF" };

		public CreateImageValidator()
		{
			RuleFor(x => x.Images)
				.NotEmpty()
				.WithMessage("You need to select an image or a set of images.");

			RuleFor(c => c.Images)
				.Must(c => BeOfTheAllowedSize(c))
				.WithMessage("One of the image's size is too large. It has to be less than 5mb in size");

			RuleFor(c => c.Images)
				.Must(c => BeOfTheAllowedExtension(c))
				.WithMessage("One of the image's extension is not allowed.");
		}

		private bool BeOfTheAllowedSize(IFormFile media)
		{
			if (media != null)
				return media.Length <= FiveMegaBytes;

			return true;
		}

		private bool BeOfTheAllowedSize(ICollection<IFormFile> images)
		{
			if(images != null)
			{
				foreach (var image in images)
				{
					if (image != null)
						if (image.Length >= FiveMegaBytes)
							return false;
				}

				return true;
			}			

			return false;
		}

		private bool BeOfTheAllowedExtension(ICollection<IFormFile> images)
		{
			if(images != null)
			{
				foreach (var image in images)
				{
					if (image != null)
					{
						var extension = Path.GetExtension(image.FileName);

						if (!AllowedExtensions.Contains(extension))
							return false;
					}
				}

				return true;
			}

			return false;
		}
	}
}
