using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restaurants.Queries.EmailRestaurantExist
{
    public class EmailRestaurantExistQuery(string email) : IRequest<bool>
    {
        public string Email { get; } = email;
    }
}
