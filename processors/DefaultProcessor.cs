namespace CityVizorImport{
    public class DefaultProcessor : IProcessor
    {
        private readonly System.Dynamic.ExpandoObject inputRecord;
        public DefaultProcessor(System.Dynamic.ExpandoObject inputRecord)
        {
            this.inputRecord = inputRecord;
        }

        public string Key => "default";

        public object Process(ColumnDefinition column)
        {
            return inputRecord.First(x => x.Key == column.Source).Value as string;
        }
    }
}