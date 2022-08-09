using EtlService.Configuration;
using EtlService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtlService
{
    internal class MetaFileProcessor
    {
        private readonly IConfiguration configuration;

        public MetaFileProcessor(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void DumpToMetaFile(DateTime folderDateTime, MetaFileModel model)
        {
            var invalidFilesLine = model.InvalidFiles.Any() ? model.InvalidFiles.Aggregate((f1, f2) => f1 + ", " + f2) : "";
            string[] fileLines = new string[]
            {
                $"parsed_files: {model.ParsedFiles}",
                $"parsed_lines: {model.ParsedLines}",
                $"found_errors: {model.FoundErrors}",
                $"invalid_files: {invalidFilesLine}"
            };
            var folderPath = configuration.GetOutputFolderPath() + "/" + folderDateTime.ToString("MM-dd-yyyy");
            if (!Directory.Exists(folderPath))
            {
                return;
            }
            var metaFilePath = folderPath + "/" + "meta.log";
            using var writer = new StreamWriter(metaFilePath);
            foreach (var line in fileLines)
            {
                writer.WriteLine(line);
            }
        }
    }
}
