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

        private DateTime today = DateTime.Today;

        private MetaFileModel metaFile = new MetaFileModel();

        public FlowController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void StartService(CancellationToken cancellationToken)
        {
            List<Task> tasks = new List<Task>();
            while (!cancellationToken.IsCancellationRequested)
            {
                tasks.Clear();
                try
                {
                    foreach (var filePath in Directory.GetFiles(configuration.GetSourceFolderPath(), "*.txt"))
                    {
                        //tasks.Add(Task.Run(() => ProcessFile(filePath, ExtractTxt), cancellationToken));
                        ProcessFile(filePath, ExtractTxt);
                    }

                    foreach (var filePath in Directory.GetFiles(configuration.GetSourceFolderPath(), "*.csv"))
                    {
                        //tasks.Add(Task.Run(() => ProcessFile(filePath, ExtractCsv), cancellationToken));
                        ProcessFile(filePath, ExtractCsv);
                    }
                    Task.WaitAll(tasks.ToArray(), cancellationToken);
                    DumpToMetaFileIfDateChanged();
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        private void DumpToMetaFileIfDateChanged()
        {
            if(DateTime.Today != today)
            {
                MetaFileProcessor metaFileProcessor = new MetaFileProcessor(configuration);
                metaFileProcessor.DumpToMetaFile(today, metaFile);
                metaFile = new MetaFileModel();
                today = DateTime.Today;
            }
        }

        private void ProcessFile(string filePath, Func<string, List<RawData>> extractMethod)
        {
            var rawData = extractMethod(filePath);
            var transformedData = DataTransformer.Transform(rawData);
            var saver = new DataSaver(configuration);
            saver.SaveData(transformedData);

            File.Delete(filePath);
        }

        private void GetMetaDataFromProcessedFile(DataReader reader, string filePath)
        {
            metaFile.ParsedFiles += 1;
            metaFile.ParsedLines += reader.AllLines;
            metaFile.FoundErrors += reader.InvalidLinesNumber;
            if(reader.InvalidLinesNumber != 0)
            {
                metaFile.InvalidFiles.Add(filePath);
            }
        }

        private List<RawData> ExtractTxt(string filePath)
        {
            DataReader reader = new TxtDataReader(filePath);
            var data = reader.ReadRawData();
            GetMetaDataFromProcessedFile(reader, filePath);
            return data;
        }

        private List<RawData> ExtractCsv(string filePath)
        {
            DataReader reader = new CsvDataReader(filePath);
            var data =  reader.ReadRawData();
            GetMetaDataFromProcessedFile(reader, filePath);
            return data;
        }
    }
}
