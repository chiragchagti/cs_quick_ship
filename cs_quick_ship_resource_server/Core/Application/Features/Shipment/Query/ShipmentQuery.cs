using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Shipment.Query
{
    public record ShipmentQuery() : IRequest<IEnumerable<Shipments>>;
}
