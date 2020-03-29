using LexicalAnalyzer.BL.FSM;
using System;

namespace LexicalAnalyzer.BL.Syntax
{
    public class SyntaxValidator
    {
        private DecompositionTableEntry NS { get; set; }
        private ParsingResult ParsingResult { get; set; }
        private int BracketOpened = 0;
        public SyntaxValidator(ParsingResult result)
        {
            ParsingResult = result;
            NS = ParsingResult.Next();
        }
        public void CheckStatement()
        {
            if (NS.Lexem.Equals("if"))
            {
                Scan();
                CheckExpression();
                if (!NS.Lexem.Equals("then"))
                {
                    throw new Exception($"'then' is not found in IF clause");
                }
                else
                {
                    Scan();
                    CheckStatement();
                }
            }
            else
            {
                if (NS.Table.Equals(State.Space))
                {
                    throw new Exception($"No statement found");
                }
                CheckExpression();
                if (NS.Lexem.Equals("then"))
                {
                    throw new Exception($"'then' is found when no IF clause is available");
                }
            }
            
        }

        public void CheckExpression()
        {
            if(NS.Table.Equals(State.Identifier) || NS.Table.Equals(State.Keyword) || NS.Table.Equals(State.DecimalNumber))
            {
                Scan();
                CheckTerminal();
                while (NS.Lexem.Equals("+"))
                {
                    Scan();
                    if (NS.Lexem.Equals("+"))
                        throw new Exception($"Duplicate '+' detected");
                    if (NS.Table.Equals(State.Space))
                        throw new Exception($"No identifier of decimal number found after '+'");
                    CheckTerminal();
                }
            }
            if (NS.Lexem.Equals(">"))
            {
                Scan();
                if(NS.Lexem.Equals(">"))
                    throw new Exception($"Duplicate '>' detected");
                CheckExpression();
            }
        }


        public void CheckTerminal()
        {
            if (NS.Table.Equals(State.Identifier) || NS.Table.Equals(State.DecimalNumber))
            {
                Scan();
            }
            if (NS.Lexem.Equals("("))
            {
                BracketOpened += 1;
                Scan();
                CheckExpression();
                if (!NS.Lexem.Equals(")"))
                {
                    throw new Exception("Missing ')'");
                }
                else
                {
                    Scan();
                }
            }
            if (NS.Lexem.Equals(")"))
            {
                BracketOpened -= 1;
                if (BracketOpened > 1 || BracketOpened < 0)
                    throw new Exception($"Found ')' when no '(' was before");
            }
            
        }

        private void CheckParenthesis()
        {
            if (NS.Table.Equals(State.Delimiter) && NS.Lexem.Equals("("))
            {
                Console.WriteLine($"Scanned '{NS.Lexem}'");
                Scan();
                CheckExpression();
            }
            if (!NS.Lexem.Equals(")"))
            {
                throw new Exception("Missing ')'");
            }
            else
            {
                Console.WriteLine($"Scanned '{NS.Lexem}'");
                Scan();
            }
        }

        private void Scan()
        {
            NS = ParsingResult.Next();
        }
    }
}
