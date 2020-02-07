namespace OMP.Dtos
{
    public class CustomAppSettings
    {
        public string CultureInfo { get; set; }

        public string DateTimeFormat { get; set; }

        public string DecimalSeparator { get; set; }

        public long ShowFilterOnMoreThan { get; set; }

        public bool Utc { get; set; }

        public string SheetConfigPath { get; set; }

        public ExcelManagement ExcelManagement { get; set; }
    }

    public class ExcelManagement
    {
        public string SheetConfigPath { get; set; }
    }
}
