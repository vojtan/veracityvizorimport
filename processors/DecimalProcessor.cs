namespace CityVizorImport
{
    public class DecimalProcessor : IProcessor
    {
        private readonly System.Dynamic.ExpandoObject inputRecord;
        public DecimalProcessor(System.Dynamic.ExpandoObject inputRecord)
        {
            this.inputRecord = inputRecord;
        }

        public string Key => "decimal";

        public object Process(ColumnDefinition column)
        {
            var sourceValue = inputRecord.First(x => x.Key == column.Source).Value as string;
            if (!string.IsNullOrEmpty(sourceValue))
            {
                sourceValue = sourceValue.Replace(",", ".");
                return decimal.Parse(sourceValue);
            }
            return null;
        }
    }
}