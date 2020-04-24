using System;

using Microsoft.AspNetCore.Mvc;

using LabProgramowanie_II.Models;
using LabProgramowanie_II.RPN;
using LabProgramowanie_II.Filters;

namespace LabProgramowanie_I.Controllers{

    [Route("api/[controller]")]
    [TypeFilter(typeof(ExceptionHandlerFilters))]
    [Produces("application/json")]
    [ApiController]
    public class CalculateController: ControllerBase{

        public EquationVariable onVarAsk(string varName) => throw new RPNException("Podano błędą formułę!");

        [HttpGet]
        public IActionResult GetCalculateX(string formula,string x){
            double xVal = double.NaN;
            if(x!=null) xVal = RPN.parseDouble(x); 
            RPN equation = new RPN(formula);
            equation.onVariableAsk+=onVarAsk;
            equation.setVariable("x",xVal);
            double value = equation.getValue();
            return Ok(new SuccessResponse<double>(value));
        }

        [HttpGet]
        [Route("xy")]
        public IActionResult GetCalculateXY(string formula,string from,string to,string n){
            double fromVal = double.NaN,toVal = double.NaN;
            int nVal = 0;
            if(from!=null) fromVal = RPN.parseDouble(from);
            if(to!=null) toVal = RPN.parseDouble(to);
            if(n!=null)  
                if(!Int32.TryParse(n,out nVal)) throw new RPNException("Podano błędną wartość liczbową: "+n);
                
            RPN equation = new RPN(formula);
            equation.onVariableAsk+=onVarAsk;
            equation.setVariable("x",new EquationVariable(fromVal,toVal,nVal));
            RPNResult[] results = equation.getValues();
            return Ok(new SuccessResponse<RPNResult[]>(results));
        }


    }

}