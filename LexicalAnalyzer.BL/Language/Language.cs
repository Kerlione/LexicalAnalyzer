using LexicalAnalyzer.BL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer.BL
{
    [Serializable]
    public class Language
    {
        public List<char> AllowedSymbols { get; set; }

        public List<string> Keywords { get; set; }

        public List<string> Delimiters { get; set; }

        public Language()
        {

        }

        public Language(List<char> allowedSymbols, List<string> keywords, List<string> delimiters)
        {
            AllowedSymbols = allowedSymbols;
            Keywords = keywords;
            Delimiters = delimiters;
        }

        /// <summary>
        /// Load the file with serialized language definition
        /// </summary>
        /// <param name="path">Path to file, including file name</param>
        /// <returns>Language instance with the definition from serialized file</returns>
        public static Language Load(string path)
        {
            var language = new Language();
            using(LanguageDefinitionLoader loader = new LanguageDefinitionLoader(path))
            {
                language = loader.Load();
            }
            return language;
        }
    }
}
