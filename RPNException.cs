using System;

namespace LabProgramowanie_II
{

    class RPNException : Exception{

        public RPNException(){}

        public RPNException(string message): base(message){}

    }

}