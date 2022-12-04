using hrvster.Web.Models;

namespace TestUtilities.Builders.Resources
{
    public class RegisterBuilder
    {
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public bool AcceptedTerms { get; set; }

        public RegisterBuilder(string emailAddress, string userName, string phone, string password)
        {
            EmailAddress = emailAddress;
            UserName = userName;
            PhoneNumber = phone;
            Password = password;
        }

        public RegisterBuilder Create(string emailAddress, string userName, string phone, string password, bool acceptedTerms, string firstName, string lastName)
            => new RegisterBuilder(emailAddress, userName, phone, password)
                .WithFirstName(firstName)
                .WithLastName(lastName)
                .WithAcceptedTerms(acceptedTerms);

        public Register Build()
            => new Register
            {
                EmailAddress = EmailAddress,
                UserName = UserName,
                PhoneNumber = PhoneNumber,
                FirstName = FirstName,
                LastName = LastName,
                Password = Password,
                AcceptedTerms = AcceptedTerms
            };

        public RegisterBuilder WithFirstName(string firstName)
        {
            FirstName = firstName;

            return this;
        }

        public RegisterBuilder WithLastName(string lastName)
        {
            LastName = lastName;

            return this;
        }

        public RegisterBuilder WithAcceptedTerms(bool acceptedTerms)
        {
            AcceptedTerms = acceptedTerms;

            return this;
        }
    }
}
