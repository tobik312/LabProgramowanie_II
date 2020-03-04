using System;

namespace LabProgramowanie_II
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test RPN functions");

            RPN rpnObj = new RPN("exp(sin(a/30+12.3)*4/x^5.32)");
            foreach(var t in rpnObj.convert())
                Console.Write("{0} ",t);
        }
    }
}
