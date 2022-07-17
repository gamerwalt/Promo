using hrvster.Core.Requests.Emails;
using hrvster.Infrastructure;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Email
{
	public class CreateEmail : AsyncRequestHandler<CreateEmailRequest>
	{
		private readonly IMailRepository _repository;

		public CreateEmail(IMailRepository repository)
		{
			_repository = repository;
		}

		protected async override Task Handle(CreateEmailRequest request, CancellationToken cancellationToken)
		{
			await _repository.CreateAsync(request.Email);
		}
	}
}
