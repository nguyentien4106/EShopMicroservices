using BuildingBlocks.Pagination;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.Queries.GetOrders;

public class GetOrdersHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrdersQuery, GetOrdersResult>
{
    public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;

        var totalCount = await dbContext.Orders.LongCountAsync(cancellationToken);
        
        var orders = await dbContext.Orders
            .Include(item => item.OrderItems)
            .OrderBy(item => item.OrderName.Value)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return new GetOrdersResult(new PaginatedResult<OrderDto>(pageIndex, pageSize, pageIndex, orders.ToOrderDtoList()));
    }
}