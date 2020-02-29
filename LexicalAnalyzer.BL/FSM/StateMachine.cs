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
                PreviousState = CurrentState;
                currentState = value;
            }
        }

        public State PreviousState { get; set; }

        public List<string> Logs { get; set; }

        public Language Language { get; set; }

        public StateMachine()
        {

        }

        public StateMachine(string languageDefinitionPath)
        {
            Language = Language.Load(languageDefinitionPath);
        }

        public void Process(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Logger.Error($"File {filePath} doesn't exist");
                throw new FileNotFoundException(filePath);
            }
            using (var file = File.OpenRead(filePath))
            {
                Logger.Info($"Reading content of {filePath}");
                using (var reader = new StreamReader(file))
                {
                    char currentSymbol = ' ';
                    var lexemBuffer = "";
                    while (!reader.EndOfStream)
                    {
                        currentSymbol = (char)reader.Read();
                        Process(currentSymbol);
                        Console.WriteLine($"Current State: {CurrentState}. Current symbol: {currentSymbol}");
                        if (!CurrentState.Equals(State.Space))
                        {
                            Console.WriteLine($"Adding symbol: '{currentSymbol}' into lexem buffer");
                            lexemBuffer += currentSymbol;
                        }
                        else
                        {
                            if (PreviousState.Equals(State.Identifier))
                            {
                                Console.WriteLine($"Lexem detected: '{lexemBuffer}'. Type: {(Language.Keywords.Any(x => String.Compare(x, lexemBuffer, true) == 0) ? State.Keyword : State.Identifier)}. ");
                                lexemBuffer = "";
                            }
                        }
                    }
                }
            }
        }

        private void Process(char currentSymbol)
        {
            if (Language.AllowedSymbols.Contains(currentSymbol))
            {
                CurrentState = State.Identifier;
                return;
            }

            if (Language.Digits.Contains(currentSymbol))
            {
                CurrentState = State.DecimalNumber;
                return;
            }

            if (Language.Delimiters.Contains(currentSymbol))
            {
                CurrentState = State.Delimiter;
                return;
            }

            if (currentSymbol.Equals('\''))
            {
                CurrentState = State.String;
                return;
            }

            if (Language.ComplexDelimiters.Contains(currentSymbol))
            {
                CurrentState = State.DoubleDelimiter;
                return;
            }

            if (currentSymbol.Equals(' ') || currentSymbol.Equals('\t') || currentSymbol.Equals('\n'))
            {
                CurrentState = State.Space;
                return;
            }
            CurrentState = State.Error;
            Logger.Error($"Symbol: {currentSymbol} is invalid for this language definition");
        }



        private State CheckState()
        {
            return State.Error;
        }
    }
}
