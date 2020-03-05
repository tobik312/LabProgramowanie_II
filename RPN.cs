using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace LabProgramowanie_II
{
    delegate EquationVariable VariableAsk(string varibleName);
    class RPN{
        public event VariableAsk onVaribleAsk;
        private static Dictionary<string,int> priorityWeight = new Dictionary<string, int>(){
            {"abs",4},{"cos",4},{"exp",4},{"log",4},{"sin",4},{"sqrt",4},{"tan",4},{"tanh",4},{"acos",4},{"asin",4},{"atan",4},
            {"^",3},
            {"*",2},{"/",2},
            {"+",1},{"-",1},
            {"(",0}
        };
        
        private string formula;
        private Dictionary<string,EquationVariable> variables = new Dictionary<string,EquationVariable>();

        private static string numRegex = @"((\d*)(\.)?(\d+))";

        private static double parseDouble(string number){
            return Double.Parse(number,NumberStyles.Float,CultureInfo.CreateSpecificCulture("en-GB"));
        }

        public RPN(string formula){
            formula = Regex.Replace(formula,@"\s+","").ToLower();
            Match variblesBrackets = Regex.Match(formula,@"(\{(.*?)\})$");
            this.formula = '('+(variblesBrackets.Success ? formula.Substring(0,variblesBrackets.Index) : formula)+')';

            if(variblesBrackets.Success){
                string toParseVaribles = variblesBrackets.Value;
                MatchCollection matchesVars = Regex.Matches(toParseVaribles,@"[a-z]+:{1}");
                for(int i=0;i<matchesVars.Count;i++){
                    int length = (i==matchesVars.Count-1) ? toParseVaribles.Length : matchesVars[i+1].Index;
                    length-=matchesVars[i].Index+1;
                    string[] variblePair = toParseVaribles.Substring(matchesVars[i].Index,length).Split(':');
                    //if(variblePair.Length>2) error
                    EquationVariable variable = null;
                    if(Regex.IsMatch(variblePair[1],@"^((\d*)(\.)?(\d+))$")){
                        variable = new EquationVariable(parseDouble(variblePair[1]));
                    }else if(Regex.IsMatch(variblePair[1],@"^([\<\()])"+numRegex+","+numRegex+","+numRegex+@"([\>\)])$")){
                        string[] splitedValue = Regex.Split(variblePair[1],@"^([\<\()])"+numRegex+","+numRegex+","+numRegex+@"([\>\)])$");
                        IntervalType type;
                        if(splitedValue[0]=="<" && splitedValue[4]==">") type = IntervalType.INCLUDE;
                        else if(splitedValue[0]=="<" && splitedValue[4]==")") type = IntervalType.INCLUDE_LEFT;
                        else if(splitedValue[0]=="(" && splitedValue[4]==">") type = IntervalType.INCLUDE_RIGHT;
                        else type = IntervalType.NO_INCLUDE;
                        variable = new EquationVariable(parseDouble(splitedValue[1]),parseDouble(splitedValue[2]),parseDouble(splitedValue[3]),type);
                    }
                    //}else throw error
                    //if(varibles.ContainsKey(variblePair[1])) error
                    variables.Add(variblePair[0],variable);
                }
            }

        }

        public bool isInfixSyntax(){
            //int startBrackets = this.formula.Split("(").Length -1;
            //int endBrackets = this.formula.Split(")").Length -1;
            //if(startBrackets!=endBrackets) return null; //TODO add throw ...
            //int startBrackets = 0;
            //int endBrackets = 0;
            string[] tokens = this.getTokens();
            for(int i=0;i<tokens.Length;i++){
                string token = tokens[i];
                if(Regex.IsMatch(token,@"([a-z])|((\d*)(\.)?(\d+))")){
                    if(priorityWeight.ContainsKey(tokens[i+1])) return false;
                    if(priorityWeight[tokens[i+1]]!=4) return false;
                }
            }
            return false; //TODO
        }

        public string[] getTokens(){
            MatchCollection matchesTokens = Regex.Matches(this.formula,@"\(|\)|\^|\*|\/|\+|\-|(abs)|(cos)|(exp)|(log)|(sin)|(sqrt)|(tan)|(cosh)|(sinh)|(tanh)|(acos)|(asin)|(atan)|([a-z]+)|((\d*)(\.)?(\d+))");
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
                }else if(Regex.IsMatch(token,@"([a-z])|((\d*)(\.)?(\d+))")){
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
                    stack.Push(parseDouble(token));
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
                }else if(Regex.IsMatch(token,@"[a-z]+")){
                    if(variables.ContainsKey(token))
                        stack.Push(variables[token].from);
                    else{
                        if(onVaribleAsk!=null){
                            variables.Add(token,this.onVaribleAsk(token));
                            stack.Push(variables[token].from);
                        }//else throw error
                    } 
                }
            }
            if(stack.Count>0) return stack.Pop();
            else return Double.NaN; //throw error
        }

    }
}