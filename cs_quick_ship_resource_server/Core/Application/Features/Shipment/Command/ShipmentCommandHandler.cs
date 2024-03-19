using Application.Abstractions;
using Domain.Entities;
using MediatR;

namespace Application.Features.Shipment.Command
{
    public class ShipmentCommandHandler : IRequestHandler<ShipmentCommand, Shipments>
    {
        private readonly IShipmentRepository _applicationDbContext;
        public ShipmentCommandHandler(IShipmentRepository applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Shipments> Handle(ShipmentCommand request, CancellationToken cancellationToken)
        {
            var model = new Shipments()
            {
                AddressFrom = request.AddressFrom,
                AddressTo = request.AddressTo,
                CarrierId = request.CarrierId,
            };
            return await _applicationDbContext.AddAsyncAndGet(model);
        }
    }
}
