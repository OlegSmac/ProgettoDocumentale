using System;
using System.IO;
using ProgettoDocumentale.Application.Common.Interfaces;

namespace ProgettoDocumentale.API.Utils
{
    public class Configuration : Application.Common.Interfaces.IConfiguration
    {
        public Configuration(Microsoft.Extensions.Configuration.IConfiguration configuration, IWebHostEnvironment env)
        {
            var uploadsFolder = configuration["FileStorage:UploadsFolder"] ?? "Uploads";
            UploadsRootPhysical = Path.Combine(env.ContentRootPath, uploadsFolder);

            Directory.CreateDirectory(UploadsRootPhysical);
        }

        public string FrontEndDateTimeFormat => "not used";
        public string FrontEndDateFormat => "not used";
        public string FrontEndDateDataTableFormat => "not used";
        public string FrontEndDateTimeFormatForBackend => "not used";

        public string UploadsRootVirtual => "not used";
        public string UploadsRootPhysical { get; }
    }
}