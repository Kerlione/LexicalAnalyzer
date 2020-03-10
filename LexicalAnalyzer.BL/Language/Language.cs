using LexicalAnalyzer.BL.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace LexicalAnalyzer.BL
{
    [Serializable]
    public class Language
    {
        public List<char> AllowedSymbols { get; set; }

        public List<string> Keywords { get; set; }

        public List<char> Delimiters { get; set; }

        public List<char> Digits { get; set; }

        public List<string> ComplexDelimiters { get; set; }

        public Language()
        {

        }

        public Language(List<char> allowedSymbols, List<string> keywords, List<char> delimiters, List<string> complexDelimiters)
        {
            AllowedSymbols = allowedSymbols;
            Keywords = keywords;
            Delimiters = delimiters;
            ComplexDelimiters = complexDelimiters;
        }

        /// <summary>
        /// Load the file with serialized language definition
        /// </summary>
        /// <param name="path">Path to file, including file name</param>
        /// <returns>Language instance with the definition from serialized file</returns>
        public static Language Load(string path)
        {
            var language = new Language();
            using (LanguageDefinitionLoader loader = new LanguageDefinitionLoader(path))
            {
                language = loader.Load();
            }
            return language;
        }

        /// <summary>
        /// Save the Language definition into file
        /// </summary>
        /// <param name="fileName">File for storing language</param>
        public void Save(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            using (var writer = new StreamWriter(fileName))
            {
                var serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(writer, this);
                writer.Flush();
            }
        }

        public override bool Equals(object obj)
        {
            var anotherObject = obj as Language;
            return Keywords.SequenceEqual(anotherObject.Keywords) 
                && Delimiters.SequenceEqual(anotherObject.Delimiters)
                && AllowedSymbols.SequenceEqual(anotherObject.AllowedSymbols)
                && Digits.SequenceEqual(anotherObject.Digits)
                && ComplexDelimiters.SequenceEqual(anotherObject.ComplexDelimiters);
        }
    }
}
