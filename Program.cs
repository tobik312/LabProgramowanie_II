using System.Text.RegularExpressions;
using System;

namespace LabProgramowanie_II
{
    class Program{
        static void Main(string[] args){
            try{
                RPN rpnObj = new RPN(args[0]+"{x:"+args[1]+"}");
                foreach(string token in rpnObj.getTokens()) Console.Write("{0} ",token);
                Console.Write("\n");
                foreach(string token in rpnObj.getPosfixSyntax()) Console.Write("{0} ",token);
                Console.Write("\n");
                Console.WriteLine(rpnObj.getValue());
                rpnObj.setVariable("x",new EquationVariable(RPN.parseDouble(args[2]),RPN.parseDouble(args[3]),Int32.Parse(args[4])));
                rpnObj.getValues();
            }catch(Exception e){
                Console.WriteLine(e.Message);
            }
        }
    }
}
