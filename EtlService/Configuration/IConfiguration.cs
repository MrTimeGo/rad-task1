using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtlService.Configuration
{
    internal interface IConfiguration
    {
        void ValidateConfiguration();
        string GetSourceFolderPath();
        string GetOutputFolderPath();
        bool ResetConfiguration();
    }
}
