using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using LabProgramowanie_II.Models;
using LabProgramowanie_II.RPN;

namespace LabProgramowanie_II.Filters{

    public class ExceptionHandlerFilters : IExceptionFilter{

        public void OnException(ExceptionContext context){
            ObjectResult result;
            if(context.Exception is RPNException)
                result = new BadRequestObjectResult(new ErrorResponse(context.Exception));
            else
                result = new ObjectResult(new ErrorResponse("Nienzany błąd serwera spróbuj ponownie później")){
                    StatusCode = 500 
                };
            context.Result = result;
        }

    }

}