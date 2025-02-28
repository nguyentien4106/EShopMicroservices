using Basket.API.Data;
using Basket.API.Models;
using FluentValidation;

namespace Basket.API.Basket.GetBasket;

public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;


public record GetBasketResult(ShoppingCart Cart);


public class GetBasketQueryValidation : AbstractValidator<GetBasketQuery>
{
    public GetBasketQueryValidation()
    {
        RuleFor(q => q.UserName).NotEmpty();
    }
}

public class GetBasketHandler(IBasketRepository repository) : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        return new(await repository.GetBasket(query.UserName));
    }
}
