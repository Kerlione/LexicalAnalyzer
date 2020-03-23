using LexicalAnalyzer.BL.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer.BL.Syntax
{
    public class SyntaxValidator
    {
        private DecompositionTableEntry NS { get; set; }
        private ParsingResult ParsingResult { get; set; }
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
                CheckExpression();
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
                    CheckTerminal();
                }
            }
            if (NS.Lexem.Equals(">"))
            {
                Scan();
                CheckExpression();
            }
        }


        public void CheckTerminal()
        {
            if (NS.Lexem.Equals("("))
            {
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
            if (NS.Table.Equals(State.Identifier) || NS.Table.Equals(State.Keyword) || NS.Table.Equals(State.DecimalNumber))
            {
                Scan();
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
