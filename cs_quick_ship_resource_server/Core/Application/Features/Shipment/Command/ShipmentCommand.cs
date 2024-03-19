using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Shipment.Command
{
    public record ShipmentCommand
    (
        string AddressFrom,
        string AddressTo,
        int CarrierId
    ) : IRequest<Shipments>;
}
