using MediatR;
using Restaurants.Application.Products.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Products.Commands.Create
{
    public class CreateProductCommand : IRequest<int>
    {
        public CreateProductDto? Product { get; set; }
    }
}
