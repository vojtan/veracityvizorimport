namespace CityVizorImport
{
    public interface IProcessor
    {
         string Key { get; }
        object Process(ColumnDefinition column);

    }
}