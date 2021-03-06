using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace LabProgramowanie_II.RPN{
    delegate EquationVariable VariableAsk(string variableName);
    class RPN{
        public event VariableAsk onVariableAsk;
        private static Dictionary<string,int> priorityWeight = new Dictionary<string, int>(){
            {"abs",4},{"cos",4},{"exp",4},{"log",4},{"sin",4},{"sqrt",4},{"tan",4},{"tanh",4},{"sinh",4},{"acos",4},{"asin",4},{"atan",4},
            {"^",3},
            {"*",2},{"/",2},
            {"+",1},{"-",1},
            {"(",0}
        };
        
        public string formula {get;}
        private Dictionary<string,EquationVariable> variables = new Dictionary<string,EquationVariable>(){
            {"pi",Math.PI},{"e",Math.E}
        };
        
        private static Regex operandReg = new Regex(@"([a-z]+)|(-?(\d*)(\.)?(\d+))");
        private static Regex variableBracketsReg = new Regex(@"(\{(.*?)\})$");

        public static double parseDouble(string number){
            try{
                return Double.Parse(number,NumberStyles.Float,CultureInfo.CreateSpecificCulture("en-GB"));
            }catch{ 
                throw new RPNException("Podano błędą wartość liczbową: "+number);
            }
        }

        public RPN(string formula){
            if(formula==null) throw new RPNException("Nie podano wszystkich wymaganych wartości!");
            formula = Regex.Replace(formula,@"\s+","").ToLower();
            formula = formula.Replace("--","+");
            Match variablesBrackets = variableBracketsReg.Match(formula);
            this.formula = "("+(variablesBrackets.Success ? formula.Substring(0,variablesBrackets.Index) : formula)+")";

            if(variablesBrackets.Success)
                foreach(string varPair in EquationVariable.getPairs(variablesBrackets.Value)){
                    string varName;
                    EquationVariable var = EquationVariable.getFromString(varPair,out varName);
                    variables.Add(varName,var);
                }

            if(!isInfixNotation()) throw new RPNException("Podano błędą formułę!");
        }

        public void setVariable(string name,EquationVariable variable){
            if(variables.ContainsKey(name)) variables.Remove(name);
            variables.Add(name,variable);
        }

        public static bool isOperator(string token){
            if(priorityWeight.ContainsKey(token))
                if(priorityWeight[token]%4 == 0)
                    return false;
            if(operandReg.IsMatch(token)) return false;
            return true;
        }
        
        public static bool isFunction(string token){
            if(priorityWeight.ContainsKey(token))
                if(priorityWeight[token]==4) return true;
            return false;
        }
        public bool isInfixNotation(){
            Stack<string> equation = new Stack<string>();
            Stack<string> stack = new Stack<string>();
            int bracketsCounter = 0;
            foreach(string token in this.getTokens()){
                if(token=="("){
                    bracketsCounter++;
                    stack.Push(token);
                }else if(token==")"){
                    if(bracketsCounter<=0) return false; 
                    while(stack.Peek()!="(")
                        equation.Push(stack.Pop());
                    stack.Pop();
                    stack.Push("()");
                    bracketsCounter--;
                    int syntax = 0;
                    while(equation.Count>0){
                        string equationToken = equation.Pop();
                        if(equationToken=="()"){
                            syntax = 1;
                            continue;
                        }
                        if(syntax==1){
                            if(operandReg.IsMatch(equationToken)) return false;
                            if(!priorityWeight.ContainsKey(equationToken)) return false;
                            if(priorityWeight[equationToken]%4 == 0) return false;
                        }else{
                            if(priorityWeight.ContainsKey(equationToken)){
                                if(priorityWeight[equationToken]%4 != 0) return false;
                            }
                            if(!operandReg.IsMatch(equationToken)) return false;
                        }
                        syntax++;
                        syntax %= 2;
                    }
                    if(syntax==0) return false;
                }else stack.Push(token);
            }
            if(bracketsCounter!=0) return false;
            return true;
        }

        public string[] getTokens(){
            MatchCollection matchesTokens = Regex.Matches(this.formula,@"\(|\)|\^|\*|\/|\+|\-|(abs)|(cos)|(exp)|(log)|(sinh)|(sin)|(sqrt)|(tanh)|(tan)|(cosh)|(acos)|(asin)|(atan)|([a-z]+)|((\d*)(\.)?(\d+))");
            if(matchesTokens==null) return null;
            Queue<string> tokensQueue = new Queue<string>();
            for(int i=0;i<matchesTokens.Count;i++){
                if(matchesTokens[i].Value=="-"){
                    string before = matchesTokens[i-1].Value;
                    string after = matchesTokens[i+1].Value;
                    if(!(before!="(" && !isOperator(before) && !isOperator(after)))
                        if(!isOperator(after) && (before=="*" || before!=")" || before=="(")){
                            tokensQueue.Enqueue("(");
                            tokensQueue.Enqueue("-1");
                            tokensQueue.Enqueue("*");
                            tokensQueue.Enqueue("1");
                            tokensQueue.Enqueue(")");
                            tokensQueue.Enqueue("*");
                            continue;
                        }
                }
                tokensQueue.Enqueue(matchesTokens[i].Value);
            } 
            return tokensQueue.ToArray();
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
                }else if(operandReg.IsMatch(token)){
                    queue.Enqueue(token);
                }
            }
            while(stack.Count>0) queue.Enqueue(stack.Pop());
            
            return queue.ToArray();
        }
        public double getValue(){
            Stack<double> stack = new Stack<double>();
            foreach(string token in this.getPosfixSyntax()){
                if(Regex.IsMatch(token,@"(\-?(\d*)(\.)?(\d+))"))
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
                    }else{
                        double b = stack.Pop();
                        if(token=="+") a += b;
                        else if(token=="-") a = b-a;
                        else if(token=="*") a *= b;
                        else if(token=="/") a = b/a;
                        else if(token=="^") a = Math.Pow(b,a);
                    }
                    stack.Push(a);
                }else if(Regex.IsMatch(token,@"[a-z]+")){
                    if(variables.ContainsKey(token))
                        stack.Push(variables[token].getValue());
                    else{
                        if(onVariableAsk!=null){
                            variables.Add(token,this.onVariableAsk(token));
                            stack.Push(variables[token].getValue());
                        }else throw new RPNException("Nie ma zdarzenia obsługującego nieznaną zmienną.");
                    } 
                }
            }
            if(stack.Count>0) return stack.Pop();
            else throw new RPNException("Nieznany błąd obliczeń!");
        }

        public RPNResult[] getValues(){
            if(!variables.ContainsKey("x")) throw new RPNException("Nie zdefiniowano zmiennej x.");
            RPNResult[] results = new RPNResult[variables["x"].steps];
            for(int i=0;i<variables["x"].steps;i++){
                results[i] = new RPNResult();
                results[i].x = variables["x"].getValue();
                results[i].y = this.getValue();
                variables["x"].next();
            }
            variables["x"].reset();
            return results;
        }

    }
}