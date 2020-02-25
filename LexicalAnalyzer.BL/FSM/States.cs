using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer.BL.FSM
{
    public enum States
    {
        Letter,
        Digit,
        Delimiter,
        ComplexDelimiter,
        Symbol,
        Eof,
        Error
    }
}
