namespace CityVizorImport
{
    public class MergeProcessor : IProcessor
    {
        private readonly System.Dynamic.ExpandoObject inputRecord;
        public MergeProcessor(System.Dynamic.ExpandoObject inputRecord)
        {
            this.inputRecord = inputRecord;
        }

        public string Key => "merge";

        public object Process(ColumnDefinition column)
        {
            var names = column.Source.Split(',');
            var values = new List<string>();
            foreach (var name in names)
            {
                values.Add(inputRecord.First(x => x.Key == name).Value as string);
            }
            return string.Join("", values);
        }
    }
}