using System;

namespace Promo.Core
{
    public interface IDatabase
    {
        string GetConnectionString();
		string GetMembershipConnectionString();
		string GetGamesConnectionstring();
		string GetNotificationConnectionString();
		string GetFulfillmentConnectionString();
        string GetConnectionName();
		void DeleteTestData();
	}
}
