using System.Text.RegularExpressions;
using System;

namespace LabProgramowanie_II
{
    class Program{
        static void Main(string[] args){
            try{
                if(args.Length<5) throw new RPNException("Niepodano odpowiedniej ilości zmiennych");
                RPN rpnObj = new RPN(args[0]+"{x:"+args[1]+"}");
                rpnObj.onVariableAsk+=((varName)=>{
                    Console.WriteLine("Podaj wartość {0}",varName);
                    double varValue = RPN.parseDouble(Console.ReadLine());
                    return varValue;
                });
                foreach(string token in rpnObj.getTokens()) Console.Write("{0} ",token);
                Console.Write("\n");
                foreach(string token in rpnObj.getPosfixSyntax()) Console.Write("{0} ",token);
                Console.Write("\n");
                Console.WriteLine(rpnObj.getValue());
                rpnObj.setVariable("x",new EquationVariable(RPN.parseDouble(args[2]),RPN.parseDouble(args[3]),(int) RPN.parseDouble(args[4])));
                rpnObj.getValues();
            }catch(RPNException e){
                Console.WriteLine(e.Message);
            }catch(Exception){
                Console.WriteLine("Wystąpił nieznany błąd.");
            }
        }
    }
}
