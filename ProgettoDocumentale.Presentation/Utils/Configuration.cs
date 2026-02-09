using System.Configuration;
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
        public string UploadsRootPhysical => HostingEnvironment.MapPath(UploadsRootVirtual);
    }
}