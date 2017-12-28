using System;
using System.Collections.Generic;
using MicroPipes.Schema;
using MicroPipes.Schema.Green;

namespace SyntaxTests
{
    class Program
    {
        static void Main(string[] args)
        {
            BasicValue basic = 5;
            switch (basic)
            {
                case BasicValue.BoolValue b:
                    break;
                case BasicValue.DTValue dt:
                    break;
                case BasicValue.DTOValue dto:
                    break;
                case BasicValue.FloatValue f:
                    break;
                case BasicValue.IdValue id:
                    break;
                case BasicValue.OrdinalValue ordinal:
                    break;
                case BasicValue.StringValue s:
                    break;
                case BasicValue.TSValue ts:
                    break;
                case BasicValue.UuidValue uuid:
                    break;
            }

            Literal arr = new Literal[] {5, "try", true};
            Literal map = new Dictionary<string, Literal>
            {
                {"1", 5},
                {"3", true},
                {"fg", "Hellow"}
            };
        }
    }
}
