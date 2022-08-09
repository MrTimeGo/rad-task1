using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtlService.DataReaders
{
    internal class TxtDataReader : DataReader
    {
        public TxtDataReader(string filePath) : base(filePath)
        {
        }

        protected override CsvConfiguration GetCsvConfiguration()
        {
            return new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                TrimOptions = TrimOptions.Trim,
                BadDataFound = null,
                HasHeaderRecord = false,
            };
        }
    }
}
