using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discount.Grpc.Protos;

namespace Basket.Api.Services
{
    public class DiscountGrpcServices
    {

        private readonly DiscountProtoService.DiscountProtoServiceClient _dps;
        public DiscountGrpcServices(DiscountProtoService.DiscountProtoServiceClient dps)
        {
            _dps = dps;
        }

        public async Task<CouponModel> GetDiscount(string pn)
        {
            var disReq = new GetDiscountRequest { ProductName = pn };
            return await _dps.GetDiscountAsync(disReq);
        }
        
    }
}
