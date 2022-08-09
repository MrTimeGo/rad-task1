using CsvHelper;
using CsvHelper.Configuration;
using EtlService.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtlService.DataReaders
{
    internal abstract class DataReader
    {
        public int AllLines { get; set; }
        public int InvalidLinesNumber { get; set; }

        protected readonly string filePath;

        protected DataReader(string filePath)
        {
            this.filePath = filePath;
        }

        public List<RawData> ReadRawData()
        {
            var config = GetCsvConfiguration();

            List<RawData> data = new List<RawData>();

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);
            if (config.HasHeaderRecord)
            {
                csv.Read();
                csv.ReadHeader();
            }
            while (csv.Read())
            {
                try
                {
                    var record = new RawData()
                    {
                        FirstName = csv.GetField<string>(0),
                        LastName = csv.GetField<string>(1),
                        Address = csv.GetField<string>(2),
                        Payment = csv.GetField<decimal>(3),
                        Date = DateTime.ParseExact(csv.GetField<string>(4), "yyyy-dd-MM", CultureInfo.InvariantCulture),
                        AccontNumber = csv.GetField<long>(5),
                        Service = csv.GetField<string>(6),
                    };
                    if (!record.IsValid())
                    {
                        InvalidLinesNumber++;
                        continue;
                    }
                    data.Add(record);
                }
                catch
                {
                    InvalidLinesNumber++;
                }
                AllLines++;
            }
            return data;
        }
        protected abstract CsvConfiguration GetCsvConfiguration();
    }
}
