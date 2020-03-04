using System.Text.RegularExpressions;
using System;

namespace LabProgramowanie_II
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test RPN functions");

            RPN rpnObj = new RPN("exp(sin(5/.03+12.3)*4.0/56.0^5.32)");
            foreach(var t in rpnObj.getPosfixSyntax()){
                    Console.Write("{0} ",t);
            }
            Console.WriteLine("\nValue: {0}",rpnObj.getValue());
        }
    }
}
