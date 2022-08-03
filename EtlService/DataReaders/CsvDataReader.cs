using CsvHelper;
using CsvHelper.Configuration;
using EtlService.Models;
using System.Globalization;

namespace EtlService.DataReaders
{
    internal class CsvDataReader : DataReader
    {
        public CsvDataReader(FileStream fileStream) : base(fileStream)
        {
        }
        protected override CsvConfiguration GetCsvConfiguration()
        {
            return new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                TrimOptions = TrimOptions.Trim,
                BadDataFound = null,
            };
        }
        
    }
}
