using EtlService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtlService
{
    internal class DataTransformer
    {
        public static List<TransformedData> Transform(List<RawData> rawData)
        {
            return rawData.GroupBy(d => d.Address.Split(",", StringSplitOptions.RemoveEmptyEntries).First())
                .Select(g => new TransformedData
                {
                    City = g.Key,
                    Services = g.GroupBy(s => s.Service)
                    .Select(g => new Service
                    {
                        Name = g.Key,
                        Payers = g.Select(r => new Payer
                        {
                            Name = (r.FirstName + " " + r.LastName).Trim(),
                            Payment = r.Payment,
                            Date = r.Date,
                            AccountNumber = r.AccontNumber
                        }).ToList(),
                        Total = g.Sum(r => r.Payment),
                    }).ToList(),
                    Total = g.Sum(r => r.Payment)
                }).ToList();
        }
    }
}
