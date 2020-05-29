using System;

namespace LabProgramowanie_II.RPN{

    class RPNException : Exception{

        public RPNException(){}

        public RPNException(string message): base(message){}

    }

}