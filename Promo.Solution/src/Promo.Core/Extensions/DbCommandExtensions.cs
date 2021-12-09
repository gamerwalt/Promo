using MySqlConnector;
using System;
using System.Data.Common;

namespace Promo.Core.Extensions
{
    public static class DbCommandExtensions
    {
        public static DbCommand AddParameter<T>(this DbCommand command, string paramName, T value)
        {
            if (string.IsNullOrWhiteSpace(paramName)) throw new ArgumentNullException(nameof(paramName));

            var parameter = new MySqlParameter
            {
                ParameterName = paramName,
                Value = value
            };
            parameter.ResetDbType();

            command.Parameters.Add(parameter);

            return command;
        }
    }
}
