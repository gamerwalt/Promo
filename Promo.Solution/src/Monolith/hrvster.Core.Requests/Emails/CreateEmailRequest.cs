using hrvster.Models;
using MediatR;

namespace hrvster.Core.Requests.Emails
{
	public class CreateEmailRequest : IRequest
	{
		private Email email;

		public CreateEmailRequest(Email email)
		{
			this.email = email;
		}

		public Email Email
		{
			get { return email; }
		}
	}
}
