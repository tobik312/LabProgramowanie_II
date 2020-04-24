using System;

namespace LabProgramowanie_II.Models{

    public class ErrorResponse : ApiResponse{

        public string message {set;get;}

        public ErrorResponse(Exception exception){
            this.status = "error";
            this.message = exception.Message;
        }

        public ErrorResponse(string errorMessage){
            this.status = "error";
            this.message = errorMessage;
        }

    }

}