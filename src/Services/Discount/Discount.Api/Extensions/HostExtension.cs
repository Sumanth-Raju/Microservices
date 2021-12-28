using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Api.Extensions
{
    public static class HostExtension
    {
        public static IHost MigrateDatabase<Tcontext>(this IHost host,int? retry =0)
        {
            int retryforAvailiablity = retry.Value;
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<Tcontext>>();

                try
                {
                    logger.LogInformation("migration pgsql db");
                    using var connection = new NpgsqlConnection(
                        configuration.GetValue<string>("DatabaseSettings:ConnectionString")
                        );
                    connection.Open();
                    using var command = new NpgsqlCommand
                    {
                        Connection = connection
                    };

                    command.CommandText = "DROP TABLE IF EXISTS COUPON";
                    command.ExecuteNonQuery();
                    command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, 
                                                                ProductName VARCHAR(24) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT)";
                    command.ExecuteNonQuery();


                    command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Apple', 'IPhone Discount', 15);";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Samsung', 'Samsung Discount', 10);";
                    command.ExecuteNonQuery();

                    logger.LogInformation("migrated pgsql db");
                }
                catch(Exception e)
                {
                    logger.LogInformation("Error" + retryforAvailiablity);
                    if(retryforAvailiablity < 50)
                    {
                        retryforAvailiablity++;
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabase<Tcontext>(host, retryforAvailiablity);
                    }
                }
            }
            return host;
        }
    }
}
