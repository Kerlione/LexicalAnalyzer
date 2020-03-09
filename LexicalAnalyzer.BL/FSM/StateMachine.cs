using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace LexicalAnalyzer.BL.FSM
{
    public class StateMachine
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private State currentState;
        public State CurrentState
        {
            get
            {
                return currentState;
            }
            set
            {
                if (!CurrentState.Equals(State.Space))
                    PreviousState = CurrentState;
                currentState = value;
            }
        }

        public State PreviousState { get; set; }

        public List<string> Logs { get; set; }

        public Language Language { get; set; }

        private bool LexemEnded { get; set; }

        public StateMachine()
        {

        }

        public StateMachine(string languageDefinitionPath)
        {
            Language = Language.Load(languageDefinitionPath);
        }

        public ParsingResult Process(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Logger.Error($"File {filePath} doesn't exist");
                throw new FileNotFoundException(filePath);
            }
            using (var file = File.OpenRead(filePath))
            {
                var result = new ParsingResult();
                Logger.Info($"Reading content of {filePath}");
                using (var reader = new StreamReader(file))
                {
                    var lexemBuffer = "";
                    while (!reader.EndOfStream)
                    {
                        char currentSymbol = (char)reader.Read();
                        if (!Char.IsWhiteSpace(currentSymbol))
                        {
                            var processingResult = Process(currentSymbol, reader);
                            if (processingResult.Item1.Equals(State.Error))
                            {
                                Console.WriteLine($"Unsupported character '{currentSymbol}' detected at position {file.Position}");
                                throw new Exception($"Unsupported character '{currentSymbol}' detected at position {file.Position}");
                            }
                            else{
                                result.AddLexemLog(processingResult);
                            }
                        }                
                    }
                }
                return result;
            }
        }
        private bool CheckIfLexemEnded()
        {
            return CurrentState.Equals(State.Delimiter) || CurrentState.Equals(State.DoubleDelimiter);
        }
        private Tuple<State, string> Process(char currentSymbol, StreamReader reader)
        {
            if (Char.IsLetter(currentSymbol))
            {
                var identifier = "" + currentSymbol;
                if (!reader.EndOfStream)
                {
                    char character = (char)reader.Peek();
                    while (Char.IsLetter(character) || Char.IsDigit(character))
                    {
                        character = (char)reader.Read();
                        if (Language.AllowedSymbols.Contains(character))
                        {
                            identifier += character;
                            character = (char)reader.Peek();
                        }
                        else
                            throw new InvalidDataException($"Symbol '{character}' is not allowed!");
                    }
                    var actualLexemType = Language.Keywords.Contains(identifier) ? State.Keyword : State.Identifier; 
                    return new Tuple<State, string>(actualLexemType, identifier);
                }
            }

            if (Char.IsDigit(currentSymbol))
            {
                var decimalNumber = "" + currentSymbol;
                if (!reader.EndOfStream)
                {
                    char character = (char)reader.Peek();
                    while (Char.IsDigit(character))
                    {
                        reader.Read();
                        if (Language.Digits.Contains(character))
                        {
                            decimalNumber += character;
                            character = (char)reader.Peek();
                        }
                        else
                            throw new InvalidDataException($"Symbol '{character}' is not allowed!");
                    }
                    return new Tuple<State, string>(State.DecimalNumber, decimalNumber);
                }
            }

            if (currentSymbol.Equals(':'))
            {
                var complexDelimiter = "" + currentSymbol;
                if (!reader.EndOfStream)
                {
                    char character = (char)reader.Peek();
                    if (character.Equals('='))
                    {
                        reader.Read();
                        complexDelimiter += character;
                        return new Tuple<State, string>(State.DoubleDelimiter, complexDelimiter);
                    }
                    else
                    {
                        if(Char.IsLetter(character) || Char.IsDigit(character) || Char.IsWhiteSpace(character))
                        {
                            return new Tuple<State, string>(State.Delimiter, complexDelimiter);
                        }
                        else
                        {
                            throw new InvalidDataException($"Symbol '{character}' is not allowed after {complexDelimiter}!");
                        }
                    }
                }
            }
            if (currentSymbol.Equals('<'))
            {
                var complexDelimiter = "" + currentSymbol;
                if (!reader.EndOfStream)
                {
                    char character = (char)reader.Peek();
                    if (character.Equals('>'))
                    {
                        reader.Read();
                        complexDelimiter += character;
                        return new Tuple<State, string>(State.DoubleDelimiter, complexDelimiter);
                    }
                    else
                    {
                        if (Char.IsLetter(character) || Char.IsDigit(character) || Char.IsWhiteSpace(character))
                        {
                            return new Tuple<State, string>(State.Delimiter, complexDelimiter);
                        }
                        else
                        {
                            throw new InvalidDataException($"Symbol '{character}' is not allowed after {complexDelimiter}!");
                        }
                    }
                }
            }
            if (Language.Delimiters.Contains(currentSymbol))
            {
                CurrentState = State.Delimiter;
                return new Tuple<State, string>(State.Delimiter, "" + currentSymbol);
            }

            if (currentSymbol.Equals('\''))
            {
                CurrentState = State.String;
                var data = "";
                if (!reader.EndOfStream)
                {
                    char character = (char)reader.Read();
                    while (!character.Equals('\''))
                    {
                        data += character;
                        character = (char)reader.Read();
                    }
                    return new Tuple<State, string>(State.String, data);
                }
            }
            if (Char.IsWhiteSpace(currentSymbol))
            {
                return new Tuple<State, string>(State.Space, "" + currentSymbol);
            }
            return new Tuple<State, string>(State.Error, "" + currentSymbol);
        }

        private void DetectLexem(string lexem, ParsingResult result)
        {
            switch (PreviousState)
            {
                case State.Identifier:
                    {
                        if (Language.Keywords.Any(x => String.Compare(x, lexem, true) == 0))
                        {
                            if (!result.Keywords.Contains(lexem))
                                result.Keywords.Add(lexem);
                        }
                        else
                        {
                            if (!result.Identifiers.Contains(lexem))
                                result.Identifiers.Add(lexem);
                        }
                        break;
                    }
                case State.DecimalNumber:
                    {
                        if (!result.DecimalNumbers.Contains(lexem))
                            result.DecimalNumbers.Add(lexem);
                        break;
                    }
                case State.Delimiter:
                    {
                        if (!result.Delimiters.Contains(lexem))
                            result.Delimiters.Add(lexem);
                        break;
                    }
            }
            LexemEnded = false;
        }

        private State CheckState()
        {
            return State.Error;
        }
    }
}
