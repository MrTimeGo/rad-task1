using EtlService.Configuration;
using EtlService.DataReaders;
using EtlService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtlService
{
    internal class FlowController
    {
        private readonly IConfiguration configuration;

        public FlowController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void StartService(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                foreach(var filePath in Directory.GetFiles(configuration.GetSourceFolderPath()))
                {
                    ProcessFile(filePath);
                }
            }
        }

        private void ProcessFile(string filePath)
        {
            using(var stream = File.OpenRead(filePath))
            {
                DataReader reader = new CsvDataReader(stream);
                var data = reader.ReadRawData();
                Console.WriteLine(reader.InvalidLinesNumber);
            }
            File.Delete(filePath);
        }

        
    }
}
