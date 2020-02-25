using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LexicalAnalyzer.BL;
using System.Xml.Serialization;

namespace LexicalAnalyzer.BL.Helpers
{
    public class LanguageDefinitionLoader : IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private TextReader Reader { get; set; }

        public LanguageDefinitionLoader(string path)
        {
            if (!File.Exists(path))
            {
                var message = $"File '{path}' is not found";
                Logger.Error(message);
                throw new FileNotFoundException(message);
            }
            Reader = new StreamReader(path);
        }

        public Language Load()
        {
            var serializer = new XmlSerializer(typeof(Language));
            var language = (Language)serializer.Deserialize(Reader);
            return language;
        }

        public void Dispose()
        {
            Reader.Close();
            Reader.Dispose();
        }
    }
}
