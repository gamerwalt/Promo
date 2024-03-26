using FluentValidation;
using hrvster.Web.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace hrvster.Web.Validation
{
	public class UpdateClientPagesValidator : AbstractValidator<UpdateClientPages>
	{
		private const int FiveMegaBytes = 5 * 1024 * 1024;
		private string[] AllowedExtensions = { ".jpeg", ".JPEG", ".jpg", ".JPG", ".png", ".PNG", ".gif", ".GIF" };
		private string AllowedExtensionString = "(.jpeg, .JPEG, .jpg, .JPG, .png, .PNG, .gif, .GIF)";

		public UpdateClientPagesValidator()
		{
			RuleFor(x => x.LogoLocation)
				.Must(c => BeOfTheAllowedExtension(c))
				.WithMessage($"Your logo image has to be of the allowed extensions. {AllowedExtensionString}")
				.Must(c => BeOfTheAllowedSize(c))
				.WithMessage("Your logo image is too large. It must not be larger than 5MB in size.");

			RuleFor(x => x.FooterLocation)
				.Must(c => BeOfTheAllowedExtension(c))
				.WithMessage($"Your footer image has to be of the allowed extensions. {AllowedExtensionString}")
				.Must(c => BeOfTheAllowedSize(c))
				.WithMessage("Your footer image is too large. It must not be larger than 5MB in size.");

			RuleFor(x => x.HeaderLocation)
				.Must(c => BeOfTheAllowedExtension(c))
				.WithMessage($"Your header image has to be of the allowed extensions. {AllowedExtensionString}")
				.Must(c => BeOfTheAllowedSize(c))
				.WithMessage("Your header image is too large. It must not be larger than 5MB in size.");
		}

		private bool BeOfTheAllowedSize(IFormFile media)
		{
			if (media != null)
				return media.Length <= FiveMegaBytes;

			return true;
		}

		private bool BeOfTheAllowedExtension(IFormFile media)
		{
			if (media != null)
			{
				var extension = Path.GetExtension(media.FileName);

				if (!AllowedExtensions.Contains(extension))
					return false;
			}

			return true;
		}
	}
}
