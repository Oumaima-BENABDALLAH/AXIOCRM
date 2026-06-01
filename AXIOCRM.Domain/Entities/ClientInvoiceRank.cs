using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXIOCRM.Domain.Entities
{
    public  class ClientInvoiceRank
    {
        public string InvoiceNumber { get; set; } = string.Empty;

        public DateTime IssueDate { get; set; }

        public decimal Total { get; set; }

        public long RangFacture { get; set; }
    }
}
