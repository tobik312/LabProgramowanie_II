using System.Text.RegularExpressions;
using System;

namespace LabProgramowanie_II
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test RPN functions");

            RPN rpnObj = new RPN("2*x+z {x:2}");
            
            rpnObj.onVaribleAsk += ((varibleName)=>{
                Console.WriteLine("Put \"{0}\" value:",varibleName);
                double varValue = Double.Parse(Console.ReadLine());
                return new EquationVariable(varValue);
            });
           /* foreach(var t in rpnObj.getTokens()){
                    Console.Write("{0} ",t);
            }*/
            Console.WriteLine("\nValue: {0}",rpnObj.getValue());
        }
    }
}
