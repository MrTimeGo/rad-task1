using EtlService.Configuration;
using EtlService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EtlService
{
    internal class DataSaver
    {
        private readonly IConfiguration configuration;

        private string folderPath;

        public DataSaver(IConfiguration configuration)
        {
            this.configuration = configuration;
            folderPath = configuration.GetOutputFolderPath() + "/" + DateTime.Now.ToString("MM-dd-yyyy");
        }
        public void SaveData(List<TransformedData> transformedData)
        {
            CreateOutputDirectoryIfDoesntExist();
            string pathToSave = GetNextFilePath();
            SaveData(transformedData, pathToSave);
        }

        private void SaveData(List<TransformedData> data, string path)
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                
            };
            using (var stream = File.OpenWrite(path))
            {
                JsonSerializer.Serialize(stream, data, typeof(List<TransformedData>), options: options);
            };
        }

        private void CreateOutputDirectoryIfDoesntExist()
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        private string GetNextFilePath()
        {
            var filename = "output" + GetNextFileNumber() + ".json";
            return folderPath + "/" + filename;
        }

        private int GetNextFileNumber()
        {
            string[] files = Directory.GetFiles(folderPath, "output*.json");
            int lastFileNumber = files.Select(f => f.Split(new char[] {'/', '\\'}).Last().Replace("output", "").Replace(".json", ""))
                .Where(numStr => int.TryParse(numStr, out int dummy))
                .Select(numStr => int.Parse(numStr))
                .OrderBy(num => num)
                .LastOrDefault();
            return lastFileNumber + 1;
        }
    }
}
