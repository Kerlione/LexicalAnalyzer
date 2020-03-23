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
        public List<string> Strings { get; set; }
        public List<DecompositionTableEntry> CommonSymbolTable { get; set; }
        public ParsingResult()
        {
            Keywords = new List<string>();
            Identifiers = new List<string>();
            DecimalNumbers = new List<string>();
            Delimiters = new List<string>();
            Strings = new List<string>();
            CommonSymbolTable = new List<DecompositionTableEntry>();
        }
        public void AddLexemLog(Tuple<State, string> value)
        {
            int position = 0;
            switch (value.Item1)
            {
                case (State.Identifier):
                    {
                        position = Identifiers.IndexOf(value.Item2);
                        if(position == -1)
                        {
                            Identifiers.Add(value.Item2);
                            position = Identifiers.IndexOf(value.Item2);
                        }
                        break;
                    }
                case (State.Keyword):
                    {
                        position = Keywords.IndexOf(value.Item2);
                        if (position == -1)
                        {
                            Keywords.Add(value.Item2);
                            position = Keywords.IndexOf(value.Item2);
                        }
                        break;
                    }
                case (State.DecimalNumber):
                    {
                        position = DecimalNumbers.IndexOf(value.Item2);
                        if (position == -1)
                        {
                            DecimalNumbers.Add(value.Item2);
                            position = DecimalNumbers.IndexOf(value.Item2);
                        }
                        break;
                    }
                case (State.Delimiter):
                    {
                        position = Delimiters.IndexOf(value.Item2);
                        if (position == -1)
                        {
                            Delimiters.Add(value.Item2);
                            position = Delimiters.IndexOf(value.Item2);
                        }
                        break;
                    }
                case (State.DoubleDelimiter):
                    {
                        position = Delimiters.IndexOf(value.Item2);
                        if (position == -1)
                        {
                            Delimiters.Add(value.Item2);
                            position = Delimiters.IndexOf(value.Item2);
                        }
                        break;
                    }
                case (State.String):
                    {
                        position = Strings.IndexOf(value.Item2);
                        if(position == -1)
                        {
                            Strings.Add(value.Item2);
                            position = Strings.IndexOf(value.Item2);
                        }
                        break;
                    }
                default:
                    {
                        Console.WriteLine($"{value} Not found");
                        break;
                    }
            }
            var entry = new DecompositionTableEntry
            {
                Table = value.Item1,
                Lexem = value.Item2,
                Position = position
            };
            CommonSymbolTable.Add(entry);
        }
        private int _currentElementId = 0;
        public DecompositionTableEntry Next()
        {
            if (_currentElementId < CommonSymbolTable.Count)
            {
                var returnedEntry = CommonSymbolTable[_currentElementId];
                _currentElementId += 1;
                return returnedEntry;
            }
            else
            {
                return new DecompositionTableEntry
                {
                    Table = State.Space,
                    Lexem = "\0",
                    Position = 0
                };
            }
        }
    }
}
