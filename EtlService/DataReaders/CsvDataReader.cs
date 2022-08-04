using CsvHelper;
using CsvHelper.Configuration;
using EtlService.Models;
using System.Globalization;

namespace EtlService.DataReaders
{
    internal class CsvDataReader : DataReader
    {
        public CsvDataReader(string filePath) : base(filePath)
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
