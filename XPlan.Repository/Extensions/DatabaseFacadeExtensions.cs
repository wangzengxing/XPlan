using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XPlan.Repository.EntityFrameworkCore.Extensions
{
    public static class DatabaseFacadeExtensions
    {
        public static List<TResult> FromSql<TResult>(this DatabaseFacade facade, string sql, params object[] parameters)
            where TResult : class, new()
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentException(nameof(sql));
            }

            using (var connection = facade.GetDbConnection())
            {
                var command = connection.CreateCommand();
                command.CommandText = sql;
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    var data = new List<TResult>();

                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            var result = new TResult();

                            var propertyInfos = typeof(TResult).GetProperties();
                            foreach (var property in propertyInfos)
                            {
                                try
                                {
                                    var propertyValue = Convert.ChangeType(reader[property.Name], property.PropertyType);
                                    property.SetValue(result, propertyValue);
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception($"unknow field `{property.Name}` find.", ex);
                                }
                            }

                            data.Add(result);
                        }
                    }

                    return data;
                }
            }
        }

        public static Task<List<TResult>> FromSqlAsync<TResult>(this DatabaseFacade facade, string sql, params object[] parameters)
            where TResult : class, new()
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentException(nameof(sql));
            }

            return Task.Run(() =>
            {
                using (var connection = facade.GetDbConnection())
                {
                    var command = connection.CreateCommand();
                    command.CommandText = sql;
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        var data = new List<TResult>();

                        while (reader.Read())
                        {
                            if (reader.HasRows)
                            {
                                var result = new TResult();

                                var propertyInfos = typeof(TResult).GetProperties();
                                foreach (var property in propertyInfos)
                                {
                                    try
                                    {
                                        var propertyValue = Convert.ChangeType(reader[property.Name], property.PropertyType);
                                        property.SetValue(result, propertyValue);
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception($"unknow field `{property.Name}` find.", ex);
                                    }
                                }

                                data.Add(result);
                            }
                        }

                        return data;
                    }
                }
            });
        }
    }
}
