using System;
using System.Text.RegularExpressions;

namespace LabProgramowanie_II
{
   class EquationVariable{
        
        public bool isNumber {get;} = false;
        private double from;
        private double number;
        private double to;
        public int steps {get;}
        private double step;
        private int iterator;
        public EquationVariable(double from,double to,int steps){
            this.from = Math.Min(from,to);
            this.to = Math.Max(from,to);
            this.steps = steps;
            this.step = (this.to-this.from)/(this.steps-1);
        }

        public EquationVariable(double number){
            this.number = number;
            this.isNumber = true;
            this.steps = 1;
        }

        public static implicit operator EquationVariable(double number){
            return new EquationVariable(number);
        }

        public double getValue(){
            if(isNumber) return number;
            return from+(iterator*step);
        }

        public void next(){
            iterator++;
            iterator%=steps+1;
        }

        public void reset(){
            iterator = 0;
        }

        //x:value, x:(from,to,steps)
        public static EquationVariable getFromString(string variablePairSyntax,out string variableName){
            string[] varPair = variablePairSyntax.Split(':');
            if(varPair.Length>2) throw new Exception("Błędny zapis formuły zmiennej");
            if(!Regex.IsMatch(varPair[0],@"([a-z]+)")) throw new Exception("Błędny zapis formuły zmiennej");
            variableName = varPair[0];
            if(Regex.IsMatch(varPair[1],@"^((\d*)(\.)?(\d+))$")){
                return RPN.parseDouble(varPair[1]);
            }
            Regex syntaxReg = new Regex(@"^\(((\d*)(\.)?(\d+)),((\d*)(\.)?(\d+)),([0-9]+)\)$");
            if(syntaxReg.IsMatch(varPair[1])){
                string[] splitedValue = Regex.Replace(varPair[1],@"[\(\)]","").Split(',');
                return new EquationVariable(RPN.parseDouble(splitedValue[0]),RPN.parseDouble(splitedValue[1]),Int32.Parse(splitedValue[2]));
            }
            //else throw syntaxError
            return null;
        }

        public static EquationVariable getFromString(string variablePairSyntax){
            string none;
            return getFromString(variablePairSyntax,out none);
        }

        public static string[] getPairs(string variablePairsSyntax){
            Match variablesBrackets = new Regex(@"(\{(.*?)\})$").Match(variablePairsSyntax);
            if(variablesBrackets.Success) variablePairsSyntax = variablesBrackets.Value;
            MatchCollection matchesPairs = Regex.Matches(variablePairsSyntax,@"[a-z]+:{1}");
            if(matchesPairs==null) return null;
            string[] pairs = new string[matchesPairs.Count];
            for(int i=0;i<matchesPairs.Count;i++){
                int length = (i==matchesPairs.Count-1) ? variablePairsSyntax.Length : matchesPairs[i+1].Index;
                length-=matchesPairs[i].Index+1;
                pairs[i] = variablePairsSyntax.Substring(matchesPairs[i].Index,length);
            }
            return pairs;
        }

    }
}