using System.Text.RegularExpressions;

namespace CityVizorImport
{
    public class IcoProcessor : IProcessor
    {
        private readonly System.Dynamic.ExpandoObject inputRecord;
        public IcoProcessor(System.Dynamic.ExpandoObject inputRecord)
        {
            this.inputRecord = inputRecord;
        }

        public string Key => "ico";

        public object Process(ColumnDefinition column)
        {
            var sourceValue = inputRecord.First(x => x.Key == column.Source).Value as string;
            if (string.IsNullOrEmpty(sourceValue)){
                return string.Empty;
            }
            return Regex.Replace(sourceValue, @"[^0-9]+", "");
        }
    }
}