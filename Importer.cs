using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using log4net;
using log4net.Core;

namespace CityVizorImport
{
    public class Importer
    {
        private readonly Configuration _config;
        private readonly ILog _logger;

        public Importer(Configuration config, ILog logger)
        {
            _config = config;
            _logger = logger;
        }

        public void Import()
        {
            _logger.Debug("Importing data...");
            var inputRecords = ReadRecords();
            var outputRecords = ConvertRecords(inputRecords);
            WriteRecords(outputRecords);
            _logger.Debug("Done.");
        }

        private void WriteRecords(List<OutputRecord> records)
        {
            var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",

            };
            using (var writer = new StreamWriter(_config.OutputFileName))
            using (var csv = new CsvWriter(writer, csvConfiguration))
            {
                csv.WriteRecords(records);
            }
        }

        private List<OutputRecord> ConvertRecords(List<dynamic> inputRecords)
        {
            var result = new List<OutputRecord>();
            foreach (var inputRecord in inputRecords)
            {
                OutputRecord record = ConvertRecord(inputRecord);
                AnonymizeRecord(record);
                if (!string.IsNullOrEmpty(record.paragraph))
                    result.Add(record);
            }
            return result;
        }

        private void AnonymizeRecord(OutputRecord record)
        {
            if (!string.IsNullOrEmpty(record.counterpartyId) && record.counterpartyId.Contains(".")){
                record.counterpartyId = "";
                record.counterpartyName = "Soukromá osoba";
                record.description  = "Anonymizováno";
            }
        }

        private OutputRecord ConvertRecord(dynamic inputRecord)
        {
            var processors = new List<IProcessor>{
                new DefaultProcessor(inputRecord),
                new IcoProcessor(inputRecord),
                new MergeProcessor(inputRecord),
                new DateProcessor(inputRecord),
                new DecimalProcessor(inputRecord),
            };

            var result = new OutputRecord { type = _config.DocumentType };
            foreach (var column in _config.Columns)
            {
                var procesor = GetProcessorForColumn(processors, column);

                var sourceValue = procesor.Process(column);
                var destinationProperty = typeof(OutputRecord).GetProperty(column.Destination);
                destinationProperty.SetValue(result, sourceValue);
            }
            return result;
        }

        private IProcessor GetProcessorForColumn(List<IProcessor> processors, ColumnDefinition column)
        {
            var result = processors.FirstOrDefault(x => x.Key == column.Type);
            if (result == null)
                _logger.Error($"No processor found for column {column.Source} with type {column.Type}");
            return result;
        }

        private List<dynamic> ReadRecords()
        {
            _logger.Debug("Reading records...");
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using var reader = new StreamReader(_config.InputFileName, Encoding.GetEncoding("Windows-1250"));

            var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true,
                Encoding = Encoding.GetEncoding("Windows-1250"),
            };
            using var csv = new CsvReader(reader, csvConfiguration);
            return csv.GetRecords<dynamic>().ToList();
        }


    }
}