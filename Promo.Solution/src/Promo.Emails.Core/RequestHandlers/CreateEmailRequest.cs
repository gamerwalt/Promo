using MediatR;
using Promo.Core.Models;
using Promo.Emails.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Emails.Core.RequestHandlers
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

    public class CreateEmailRequestHandler : AsyncRequestHandler<CreateEmailRequest>
    {
        private readonly IMailRepository _repository;

        public CreateEmailRequestHandler(IMailRepository repository)
        {
            _repository = repository;
        }

        protected async override Task Handle(CreateEmailRequest request, CancellationToken cancellationToken)
        {
            await _repository.CreateAsync(request.Email);
        }
    }
}
