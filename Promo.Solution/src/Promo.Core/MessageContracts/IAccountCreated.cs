using System;

namespace Promo.Core.MessageContracts
{
	public interface IAccountCreated
	{
		Guid AccountUid { get; }
		string EmailAddress { get; }
		string PhoneNumber { get; }
		string DisplayName { get; }
	}

	public interface IAccountCreatedAccepted
	{
		Guid AccountUid { get; }
		string EmailAddress { get; }
		string PhoneNumber { get; }
		string DisplayName { get; }
	}

	public interface IAccountCreatedRejected
	{
		Guid AccountUid { get; }
		string EmailAddress { get; }
		string PhoneNumber { get; }
		string DisplayName { get; }
		string Reason { get; }
	}
}
