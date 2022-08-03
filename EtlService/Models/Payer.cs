using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtlService.Models
{
    internal class Payer
    {
        public string Name { get; set; } = string.Empty;
        public decimal Payment { get; set; }
        public DateOnly Date { get; set; }
        public long AccountNumber { get; set; }
    }
}
