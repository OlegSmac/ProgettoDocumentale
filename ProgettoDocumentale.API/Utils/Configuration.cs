namespace ProgettoDocumentale.API.Utils
{
    public class Configuration : Application.Common.Interfaces.IConfiguration
    {
        public Configuration()
        { }

        public string FrontEndDateTimeFormat => "not used";
        public string FrontEndDateFormat => "not used";
        public string FrontEndDateDataTableFormat => "not used";
        public string FrontEndDateTimeFormatForBackend => "not used";

        public string UploadsRootVirtual => "not used";
        public string UploadsRootPhysical => "not used";
    }
}