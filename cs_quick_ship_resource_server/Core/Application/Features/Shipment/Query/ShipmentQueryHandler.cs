using Application.Abstractions;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Shipment.Query
{
    public class ShipmentQueryHandler : IRequestHandler<ShipmentQuery, IEnumerable<Shipments>>
    {
        private readonly IShipmentRepository _applicationDbContext;
        public ShipmentQueryHandler(IShipmentRepository applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<Shipments>> Handle(ShipmentQuery request, CancellationToken cancellationToken)
        {
            return await _applicationDbContext.GetAllAsync();
        }
    }
}
