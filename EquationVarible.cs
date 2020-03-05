using System;

namespace LabProgramowanie_II
{
    enum IntervalType{
        INCLUDE_LEFT, //<)
        INCLUDE_RIGHT, //<)
        INCLUDE, // <>
        NO_INCLUDE, // (),
        NUMBER
    }
    class EquationVariable{
        
        public IntervalType type {get;set;}
        public double from{get;set;}
        public double to{get;set;}
        public double step {get;set;}
        public EquationVariable(double from,double to,double step,IntervalType type){
            this.from = from;
            this.to = to;
            this.step = step;
            this.type = type;
        }

        public EquationVariable(double number): this(number,number,0.0d,IntervalType.NUMBER){}

    }
}