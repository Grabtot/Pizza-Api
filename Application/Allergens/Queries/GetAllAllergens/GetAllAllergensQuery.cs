using ErrorOr;
using MediatR;
using PizzaApi.Domain.Ingredients.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApi.Application.Allergens.Queries.GetAllAllergens
{
    public class GetAllAllergensQuery : IRequest<ErrorOr<List<Allergen>>>
    {
    }
}
