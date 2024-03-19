using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Shipments
    {
        public int Id { get; set; }
        public string AddressFrom { get; set; }
        public string AddressTo { get; set; }
        public int CarrierId { get; set; }
    }
}
