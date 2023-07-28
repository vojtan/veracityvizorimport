using System.Globalization;

namespace CityVizorImport
{
    public class DateProcessor : IProcessor
    {
        private readonly System.Dynamic.ExpandoObject inputRecord;
        public DateProcessor(System.Dynamic.ExpandoObject inputRecord)
        {
            this.inputRecord = inputRecord;
        }

        public string Key => "date";

        public object Process(ColumnDefinition column)
        {
            var sourceValue = inputRecord.First(x => x.Key == column.Source).Value as string;
            if (!string.IsNullOrEmpty(sourceValue))
                return DateTime.ParseExact(sourceValue, "dd.MM.yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            return string.Empty;

        }
    }
}