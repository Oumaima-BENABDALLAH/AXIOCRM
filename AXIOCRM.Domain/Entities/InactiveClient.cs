using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXIOCRM.Domain.Entities
{
    public class InactiveClient
    {
        public int ClientId { get; set; }
        public DateTime DateDerniereFacture { get; set; }
    }
}
