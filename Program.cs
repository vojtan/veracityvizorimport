// See https://aka.ms/new-console-template for more information
using log4net;
using Newtonsoft.Json;

namespace CityVizorImport
{
    class Program
    {
        static void Main(string[] args)
        {
            var log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);
            Configuration config;
            using (StreamReader file = File.OpenText(@"config.json"))
                config = JsonConvert.DeserializeObject<Configuration>(file.ReadToEnd());
            if (config == null)
            {
                log.Error("Configuration file not found.");
                return;
            }
            var importer = new Importer(config, log);
            importer.Import();
        }
    }
}