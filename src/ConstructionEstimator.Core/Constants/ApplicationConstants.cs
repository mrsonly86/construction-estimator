namespace ConstructionEstimator.Core.Constants;

public static class ApplicationConstants
{
    public const string ApplicationName = "Construction Estimator";
    public const string DatabaseName = "ConstructionEstimator.db";
    public const string ConfigFileName = "appsettings.json";
    public const string LogFileName = "logs/construction-estimator-.log";
    
    public static class Currencies
    {
        public const string VND = "VND";
        public const string USD = "USD";
        public const string EUR = "EUR";
    }
    
    public static class DefaultValues
    {
        public const decimal DefaultProfitPercentage = 10m;
        public const decimal DefaultContingencyPercentage = 5m;
        public const decimal DefaultProductivityFactor = 1.0m;
        public const string DefaultCurrency = Currencies.VND;
    }
    
    public static class ValidationRules
    {
        public const int MaxNameLength = 200;
        public const int MaxDescriptionLength = 1000;
        public const int MaxCodeLength = 50;
        public const int MaxPhoneLength = 50;
        public const int MaxEmailLength = 100;
        public const int MaxAddressLength = 500;
        
        public const decimal MinQuantity = 0.0001m;
        public const decimal MaxQuantity = 999999.9999m;
        public const decimal MinPrice = 0.01m;
        public const decimal MaxPrice = 999999999.99m;
    }
    
    public static class FileExtensions
    {
        public const string Excel = ".xlsx";
        public const string Pdf = ".pdf";
        public const string Word = ".docx";
        public const string Csv = ".csv";
        public const string Json = ".json";
        public const string Xml = ".xml";
    }
    
    public static class ExportTemplates
    {
        public const string DetailedEstimate = "DetailedEstimate";
        public const string SummaryReport = "SummaryReport";
        public const string MaterialList = "MaterialList";
        public const string LaborAnalysis = "LaborAnalysis";
        public const string CostBreakdown = "CostBreakdown";
    }
}