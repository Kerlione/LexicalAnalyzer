using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzer.BL.FSM
{
    public class DecompositionTableEntry
    {
        public State Table { get; set; }
        public int Position { get; set; }
        public string Lexem { get; set; }
    }
}
