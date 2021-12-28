using Discount.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Api.Repositories.Interface
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscount(string ProductName);
        Task<bool> CreateDiscount(Coupon coupon);
        Task<bool> DeleteDiscount(string ProductName);
        Task<bool> UpdateDiscount(Coupon coupon);
    }
}
