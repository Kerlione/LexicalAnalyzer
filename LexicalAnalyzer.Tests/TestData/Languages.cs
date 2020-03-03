using LexicalAnalyzer.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer.Tests.TestData
{
    public class Languages
    {
        public static Language Pascal => new Language()
        {
            AllowedSymbols = new List<char>
            {
                'q','w','e','r','t','y','u','i','o','p','a','s','d','f','g','h','j','k','l','z','x','c','v','b','n','m',
                'Q','W','E','R','T','Y','U','I','O','P','A','S','D','F','G','H','J','K','L','Z','X','C','V','B','N','M',
                '0','1','2','3','4','5','6','7','8','9'
            },
            Delimiters = new List<char>
            {
                '(',':',')','[',']',',','.','>',';','+','<'
            },
            Keywords = new List<string>
            {
                "procedure",
                "Real",
                "var",
                "string",
                "begin",
                "Str",
                "if",
                "then",
                "else",
                "Delete",
                "Length",
                "Error",
                "end"
            },
            Digits = new List<char>
            {
                '0','1','2','3','4','5','6','7','8','9'
            },
            ComplexDelimiters = new List<char>
            {
                '='
            }
        };
    }
}
