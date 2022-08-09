using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtlService.Models
{
    internal class RawData
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal Payment { get; set; }
        public DateTime Date { get; set; }
        public long AccontNumber { get; set; }
        public string Service { get; set; } = string.Empty;

        public bool IsValid()
        {
            return !(
                string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName) ||
                string.IsNullOrEmpty(Address) ||
                Payment == 0 ||
                Date == default ||
                AccontNumber == 0 ||
                string.IsNullOrEmpty(Service)
                );
        }
    }
}
