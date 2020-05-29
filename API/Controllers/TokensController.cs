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
    public class TokensController: ControllerBase{

        [HttpGet]
        public IActionResult GetTokens(string formula){
            RPN equation = new RPN(formula);
            equation.isInfixNotation();
            var result = new{
                infix = equation.getTokens(),
                postfix = equation.getPosfixSyntax()
            };
            return Ok(new SuccessResponse<object>(result));
        }

    }
}