using System.Configuration;
using System.IO;
using System.Web.Hosting;
using ProgettoDocumentale.Application.Common.Interfaces;

namespace ProgettoDocumentale.Presentation.Utils
{
    public class Configuration : IConfiguration
    {
        public Configuration()
        { }

        public string FrontEndDateTimeFormat => ConfigurationManager.AppSettings["FrontEndDateTimeFormat"] ?? "DD.MM.YYYY HH:mm:ss";
        public string FrontEndDateFormat => ConfigurationManager.AppSettings["FrontEndDateFormat"] ?? "DD.MM.YYYY";
        public string FrontEndDateDataTableFormat => ConfigurationManager.AppSettings["FrontEndDateDataTableFormat"] ?? "dd/MM/yyyy";
        public string FrontEndDateTimeFormatForBackend => ConfigurationManager.AppSettings["FrontEndDateTimeFormatForBackend"] ?? "M/D/YYYY h:mm:ss a";

        public string UploadsRootVirtual => ConfigurationManager.AppSettings["UploadsRoot"];
        public string UploadsRootPhysical
        {
            get
            {
                var uploadsPath = UploadsRootVirtual;                
                if (uploadsPath.StartsWith("~")) return HostingEnvironment.MapPath(uploadsPath);
                
                var appPath = HostingEnvironment.ApplicationPhysicalPath;
                var combinedPath = Path.Combine(appPath, uploadsPath);
                return Path.GetFullPath(combinedPath);
            }
        }
    }
}