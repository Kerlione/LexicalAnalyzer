using LexicalAnalyzer.BL;
using LexicalAnalyzer.BL.FSM;
using LexicalAnalyzer.Tests.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer.Tests
{
    class Program
    {
        public static void Main()
        {
            var language = Languages.Pascal;
            var filePath = "pascalDefinition.xml";
            language.Save(filePath);
            var loadedLanguage = Language.Load(filePath);
            Console.WriteLine($"Check if serialized and stored languages are equal: {language.Equals(loadedLanguage)}");
            var file = @"TestData\test_code.pas";
            var fsm = new StateMachine(filePath);
            var result = fsm.Process(file);
            Console.ReadLine();
        }
    }
}
