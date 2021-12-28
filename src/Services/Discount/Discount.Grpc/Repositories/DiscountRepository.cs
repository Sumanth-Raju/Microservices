using Dapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Repositories.Interface;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Grpc.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _config;
        public DiscountRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<Coupon> GetDiscount(string ProductName)
        {
            using var conn = new NpgsqlConnection(_config.GetValue<string>("DatabaseSettings:ConnectionString"));
            var coupon = await conn.QueryFirstOrDefaultAsync<Coupon>("select * from Coupon where productname= @ProductName", new { ProductName = ProductName });
            if(coupon == null)
            {
                return new Coupon
                {
                    ProductName = "NoDiscount",
                    Amount = 0,
                    Description = "No Discount availiable for the product"
                };
            }
            return coupon;
        }
        public async  Task<bool> CreateDiscount(Coupon coupon)
        {
            using var conn = new NpgsqlConnection(_config.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected =
                await conn.ExecuteAsync("Insert into Coupon(productname,description,amount) values(@ProductName,@Description,@Amount)",
                new
                {
                    ProductName = coupon.ProductName,
                    Description = coupon.Description,
                    Amount = coupon.Amount
                });
            if(affected == 0)
            {
                return false;
            }
            return true;

        }

        public async Task<bool> DeleteDiscount(string ProductName)
        {
            using var conn = new NpgsqlConnection(_config.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected =
             await conn.ExecuteAsync("Delete from Coupon  where productname=@ProductName",
                new
                {
                    ProductName = ProductName
                });
            if (affected == 0)
            {
                return false;
            }

            return true;
        }

        

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var conn = new NpgsqlConnection(_config.GetValue<string>("DatabaseSettings:ConnectionString"));
            var affected =
             await conn.ExecuteAsync("update Coupon set Productname=@ProductName,description=@Description,amount=@Amount where id=@Id",
                new
                {
                    ProductName = coupon.ProductName,
                    Description = coupon.Description,
                    Amount = coupon.Amount,
                    Id = coupon.Id
                });
            if (affected == 0)
            {
                return false;
            }
                
            return true;
        }
    }
}
