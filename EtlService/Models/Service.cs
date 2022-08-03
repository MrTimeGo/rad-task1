using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtlService.Models
{
    internal class Service
    {
        public string Name { get; set; } = string.Empty;
        public List<Payer> Payers { get; set; } = new List<Payer>();
        public decimal Total { get; set; }
    }
}
