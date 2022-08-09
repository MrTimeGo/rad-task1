using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtlService.Configuration
{
    internal class ConfigurationModel
    {
        public string SourceFolderPath { get; set; } = string.Empty;
        public string OutputFolderPath { get; set; } = string.Empty;
    }
}
