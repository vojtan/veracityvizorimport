namespace CityVizorImport
{
    public class Configuration
    {
        public string? InputFileName { get; set; }
        public string? OutputFileName { get; set; }
        public List<ColumnDefinition>? Columns { get; set; }
        public string? DocumentType { get; set; }
    }
}