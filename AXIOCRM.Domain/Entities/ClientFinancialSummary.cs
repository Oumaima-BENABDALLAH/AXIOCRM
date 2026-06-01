using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXIOCRM.Domain.Entities
{
    public  class ClientFinancialSummary
    {
        [Key]
        public int ClientId { get; set; }
        public decimal MaxAmount { get; set; }
        public decimal MinAmount { get; set; }
        public decimal DepenseAmount { get; set; }
        public int NbreInvoice { get; set; }
    }
}
