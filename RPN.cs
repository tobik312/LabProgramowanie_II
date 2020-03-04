using System;
using System.Globalization;
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

        public bool isInfixSyntax(){
            //int startBrackets = this.formula.Split("(").Length -1;
            //int endBrackets = this.formula.Split(")").Length -1;
            //if(startBrackets!=endBrackets) return null; //TODO add throw ...
            return false; //TODO
        }

        public string[] getTokens(){
            MatchCollection matchesTokens = Regex.Matches(this.formula,@"\(|\)|\^|\*|\/|\+|\-|(abs)|(cos)|(exp)|(log)|(sin)|(sqrt)|(tan)|(cosh)|(sinh)|(tanh)|(acos)|(asin)|(atan)|([a-z])|((\d*)(\.)?(\d+))");
            if(matchesTokens==null) return null;
            string[] tokens = new string[matchesTokens.Count];
            for(int i=0;i<matchesTokens.Count;i++) tokens[i] = matchesTokens[i].Value;
            return tokens;
        }
        public string[] getPosfixSyntax(){
            Stack<string> stack = new Stack<string>();
            Queue<string> queue = new Queue<string>();

            foreach(string token in this.getTokens()){
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
        public double getValue(){
            Stack<double> stack = new Stack<double>();
            foreach(string token in this.getPosfixSyntax()){
                if(Regex.IsMatch(token,@"((\d*)(\.)?(\d+))"))
                    stack.Push(double.Parse(token,NumberStyles.Float,CultureInfo.CreateSpecificCulture("en-GB")));
                //else if(Regex.IsMatch(token,@"[a-z]"))
                    //TODO custom varibles
                else if(priorityWeight.ContainsKey(token)){
                    double a = stack.Pop();
                    if(priorityWeight[token]==4){
                        if(token=="abs") a = Math.Abs(a);
                        else if(token=="cos") a = Math.Cos(a);
                        else if(token=="exp") a = Math.Exp(a);
                        else if(token=="log") a = Math.Log(a);
                        else if(token=="sin") a = Math.Sin(a);
                        else if(token=="sqrt") a = Math.Sqrt(a);
                        else if(token=="tan") a = Math.Tan(a);
                        else if(token=="cosh") a = Math.Cosh(a);
                        else if(token=="sinh") a = Math.Sinh(a);
                        else if(token=="tanh") a = Math.Tanh(a);
                        else if(token=="acos") a = Math.Acos(a);
                        else if(token=="asin") a = Math.Asin(a);
                        else if(token=="atan") a = Math.Atan(a);
                        //else throw error...
                    }else{
                        double b = stack.Pop();
                        if(token=="+") a += b;
                        else if(token=="-") a = b-a;
                        else if(token=="*") a *= b;
                        else if(token=="/") a = b/a;
                        else if(token=="^") a = Math.Pow(b,a);
                        //else throw error 
                    }
                    stack.Push(a);
                }
            }
            if(stack.Count>0) return stack.Pop();
            else return Double.NaN; //throw error
        }

    }
}