using System;

namespace Promo.Core
{
    public interface IDatabase
    {
        string GetConnectionString();
        string GetConnectionName();
    }
}
