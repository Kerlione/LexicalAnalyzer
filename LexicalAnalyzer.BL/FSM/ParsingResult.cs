using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer.BL.FSM
{
    public class ParsingResult
    {
        public List<string> Keywords { get; set; }
        public List<string> Identifiers { get; set; }
        public List<string> DecimalNumbers { get; set; }
        public List<string> Delimiters { get; set; }
        public List<DecompositionTableEntry> CommonSymbolTable { get; set; }
        public ParsingResult()
        {
            Keywords = new List<string>();
            Identifiers = new List<string>();
            DecimalNumbers = new List<string>();
            Delimiters = new List<string>();
            CommonSymbolTable = new List<DecompositionTableEntry>();
        }
        public void AddLexemLog(State state, string value)
        {
            int position = 0;
            switch (state)
            {
                case(State.Identifier):
                    {
                        position = Identifiers.IndexOf(value);
                        break;
                    }
                default:
                    {
                        throw new Exception($"{value} Not found");
                    }
            }
            var entry = new DecompositionTableEntry
            {
                Table = state,
                Lexem = value,
                Position = position
            };
            CommonSymbolTable.Add(entry);
        }
    }
}
