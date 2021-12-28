using AutoMapper;
using Discount.Grpc.Repositories;
using Discount.Grpc.Protos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discount.Grpc.Repositories.Interface;
using Grpc.Core;
using Discount.Grpc.Entities;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<DiscountRepository> _log;
        private readonly IDiscountRepository _repo;

        public DiscountService(IMapper mapper, ILogger<DiscountRepository> log, IDiscountRepository repo)
        {
            _mapper = mapper;
            _log = log;
            _repo = repo;
        }

        public async override Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupn = _mapper.Map<Coupon>(request.Coupon);
            await _repo.CreateDiscount(coupn);
            _log.LogInformation("discount created");
            return _mapper.Map<CouponModel>(coupn);

        }

        public async override Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            try
            {
                var coup = await _repo.GetDiscount(request.ProductName);

                if (coup == null)
                {
                    throw new RpcException(new Status(StatusCode.NotFound, $"discount with product not found"));


                }
                var coupModel = _mapper.Map<CouponModel>(coup);
                return coupModel;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async override Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupn = _mapper.Map<Coupon>(request.Coupon);
            await _repo.UpdateDiscount(coupn);
            _log.LogInformation("discount created");
            return _mapper.Map<CouponModel>(coupn);
        }

        public async override Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var del = await _repo.DeleteDiscount(request.ProductName);
            var res = new DeleteDiscountResponse
            {
                Success = del
            };
            return res;
        }
    }
}
