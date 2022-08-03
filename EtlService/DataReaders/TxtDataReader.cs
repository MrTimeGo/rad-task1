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
        public TxtDataReader(FileStream fileStream) : base(fileStream)
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
