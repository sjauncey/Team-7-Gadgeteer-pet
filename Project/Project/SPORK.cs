using System;
using Microsoft.SPOT;

namespace GadgeteerApp1
{
    enum Instruction
    {
        FORWARD ,
        LEFT ,
        RIGHT 
    }
    struct SPORK
    {

        private Instruction instruction;
        private int parameter;

        public SPORK(Instruction inst, int param)
        {
            this.instruction = inst;
            parameter = param;
        }

        public Instruction getInstruction()
        {
            return instruction;
        }

        public int getParamter()
        {
            return parameter;
        }

        public override string ToString()
        {
            switch (instruction)
            {
                case Instruction.FORWARD:
                    return ("FORWARD " + parameter.ToString());
                case Instruction.RIGHT:
                    return ("RIGHT");
                case Instruction.LEFT:
                    return ("LEFT");
                default:
                    return ("Null Value");
            }
        }

    }
}
