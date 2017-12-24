using System;
using System.Collections.Generic;
using MicroPipes.Schema;
using MicroPipes.Schema.Green;
using MicroPipes.Schema.Literals;

namespace SyntaxTests
{
    class Program
    {
        static void Main(string[] args)
        {
            Basic basic = 5;
            switch (basic)
            {
                case Basic.Bool b:
                    break;
                case Basic.DT dt:
                    break;
                case Basic.DTO dto:
                    break;
                case Basic.Float f:
                    break;
                case Basic.Id id:
                    break;
                case Basic.Ordinal ordinal:
                    break;
                case Basic.String s:
                    break;
                case Basic.TS ts:
                    break;
                case Basic.Uuid uuid:
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
