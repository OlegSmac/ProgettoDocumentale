namespace ProgettoDocumentale.Application.Common.Interfaces
{
    public interface IConfiguration
    {
        string UploadsRootVirtual { get; }
        string UploadsRootPhysical { get; }
        string FrontEndDateTimeFormat { get; }
        string FrontEndDateTimeFormatForBackend { get; }
        string FrontEndDateFormat { get; }
    }
}