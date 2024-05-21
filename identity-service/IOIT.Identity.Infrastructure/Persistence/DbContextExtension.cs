using IOIT.Identity.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace IOIT.Identity.Infrastructure.Persistence
{
    public static class DbContextExtension
    {
        public static async Task<int> ExecuteNonQuery(this DbContext context, string storeProcedure, params AppSpParameter[] parameters)
        {
            using var connection = context.Database.GetDbConnection();
            var connectionState = connection.State;
            if (connection.State != ConnectionState.Open)
                connection.Open();

            using var cmd = connection.CreateCommand();
            cmd.CommandText = storeProcedure;
            cmd.SetParams(parameters);
            cmd.CommandType = CommandType.StoredProcedure;

            return await cmd.ExecuteNonQueryAsync();
        }
        public static async Task<IList<TResult>> GetFromSqlAsync<TResult>(this DbContext context, string storeProcedure, params AppSpParameter[] parameters) where TResult : new()
        {
            var results = new List<TResult>();
            using (var connection = context.Database.GetDbConnection())
            {
                var connectionState = connection.State;
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using var cmd = connection.CreateCommand();
                cmd.CommandText = storeProcedure;
                cmd.SetParams(parameters);
                cmd.CommandType = CommandType.StoredProcedure;

                using var reader = await cmd.ExecuteReaderAsync();
                while (reader.Read())
                {
                    results.Add(reader.MapToList<TResult>());
                }
            }

            return results;
        }
        private static void SetParams(this DbCommand cmd, params AppSpParameter[] parameters)
        {
            if (parameters == null) return;

            foreach (var param in parameters)
            {
                var par = new SqlParameter(param.Name, param.Value);

                if (!string.IsNullOrEmpty(param.TypeName))
                {
                    par.SqlDbType = SqlDbType.Structured;
                    par.TypeName = param.TypeName;
                }
                if (param.Value == null && par.SqlDbType != SqlDbType.Structured)
                {
                    par.Value = DBNull.Value;
                }

                cmd.Parameters.Add(par);
            }
        }
        private static TResult MapToList<TResult>(this DbDataReader dr) where TResult : new()
        {
            var result = new TResult();
            var properties = TypeDescriptor.GetProperties(typeof(TResult));

            for (var i = 0; i < dr.FieldCount; i++)
            {
                var fieldName = dr.GetName(i);
                var property = properties.Find(fieldName, true);

                if (property == null) continue;
                var value = dr.GetValue(i);
                property.SetValue(result, value == DBNull.Value ? null : value);
            }

            return result;
        }
    }
}
