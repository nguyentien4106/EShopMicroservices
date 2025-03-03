using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService(DiscountContext dbContext, ILogger<DiscountContext> logger) : DiscountProtoService.DiscountProtoServiceBase
{
     public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
     {
          var coupon = await dbContext.Coupons.FirstOrDefaultAsync(item => item.ProductName == request.ProductName);

          if (coupon == null)
          {
          }

          return null;
     }

     public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
     {
          var coupon = request.Coupon.Adapt<Coupon>();
          if (coupon == null)
          {
               throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid argument request"));
          }
          
          dbContext.Coupons.Add(coupon);
          await dbContext.SaveChangesAsync();

          return coupon.Adapt<CouponModel>();
     }

     public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
     {
          var coupon = await dbContext.Coupons.FirstOrDefaultAsync(i => i.ProductName == request.ProductName) ?? Coupon.NoDiscount();

          logger.LogInformation("Discount is retrieved for ProductName : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);
          var model = coupon.Adapt<CouponModel>();
          
          return model;
     }

     public override Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
     {
          return base.UpdateDiscount(request, context);
     }
     
}