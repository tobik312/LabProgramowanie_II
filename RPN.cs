using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace LabProgramowanie_II
{
    class RPN{

        private static Dictionary<string,int> priorityWeight = new Dictionary<string, int>(){
            {"abs",4},{"cos",4},{"exp",4},{"log",4},{"sin",4},{"sqrt",4},{"tan",4},{"tanh",4},{"acos",4},{"asin",4},{"atan",4},
            {"^",3},
            {"*",2},{"/",2},
            {"+",1},{"-",1},
            {"(",0}
        };
        
        private String formula;

        public RPN(string formula){
            this.formula = '('+Regex.Replace(formula,@"\s+","").ToLower()+')';
        }
        public string[] convert(){
            int startBrackets = this.formula.Split("(").Length -1;
            int endBrackets = this.formula.Split(")").Length -1;

            if(startBrackets!=endBrackets) return null; //TODO add throw ...

            Stack<string> stack = new Stack<string>();
            Queue<string> queue = new Queue<string>();

            MatchCollection tokens = Regex.Matches(this.formula,@"\(|\)|\^|\*|\/|\+|\-|(abs)|(cos)|(exp)|(log)|(sin)|(sqrt)|(tan)|(cosh)|(sinh)|(tanh)|(acos)|(asin)|(atan)|([a-z])|((\d*)(\.)?(\d+))");
            foreach(Match match in tokens){
                string token = match.Value;
                if(token=="(")
                    stack.Push(token);
                else if(token==")"){
                    while(stack.Peek()!="(")
                        queue.Enqueue(stack.Pop());
                    stack.Pop();
                }else if(priorityWeight.ContainsKey(token)){
                    while(stack.Count>0 && priorityWeight[token]<=priorityWeight[stack.Peek()])
                        queue.Enqueue(stack.Pop());
                    stack.Push(token);
                }else if(Regex.IsMatch(token,@"[a-z]|((\d*)(\.)?(\d+))")){ //TODO check difference between var and number and ask user about value
                    queue.Enqueue(token);
                }
            }
            while(stack.Count>0) queue.Enqueue(stack.Pop());
            
            return queue.ToArray();
        }

    }
}