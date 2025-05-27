namespace SAMMAI.Authentication.Utility.SettingsFiles
{
    public class ProjectSettings
    {
        public required string RecordRequestLogPathFolder { get; set; }
        public required string SerilogLogPathFolder { get; set; }
        public required string LogTraceabiltyPathFolder { get; set; }
        public required SAMMAIMicroservices SAMMAIMicroservices { get; set; }
        public bool EnableSwagger { get; set; }
        public required string PathBase { get; set; }
    }

    #region SAMMAIMicroservices
    public class SAMMAIMicroservices
    {
        public required DataBase DataBase { get; set; }
        public required Authentication Authentication { get; set; }
    }

    public class DataBase
    {
        public required string BaseRoute { get; set; }
        public required string InsertRecordRequestRequest { get; set; }
        public required string UpdateRecordRequestResponse { get; set; }
        public required string InsertLogTraceability { get; set; }
    }

    public class Authentication
    {
        public required string BaseRoute { get; set; }
    }
    #endregion
}
