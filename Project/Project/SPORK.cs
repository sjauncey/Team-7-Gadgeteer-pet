using System;
using Microsoft.SPOT;

namespace GadgeteerApp1
{
    enum Instruction
    {
        FORWARD,
        LEFT,
        RIGHT
    }
    struct SPORK
    {

        private Instruction instruction;
        private int parameter;

        public SPORK(Instruction inst, int param)
        {
            switch (inst)
            {
                case Instruction.FORWARD:
                    instruction = inst;
                    parameter = param;
                    break;
                case Instruction.RIGHT:
                    param = param % 360;
                    if (param < 180)
                    {
                        instruction = Instruction.RIGHT;
                        parameter = param;
                    }
                    else
                    {
                        instruction = Instruction.LEFT;
                        parameter = 360 - param;
                    }
                    break;
                case Instruction.LEFT:
                    param = param % 360;
                    if (param < 180)
                    {
                        instruction = Instruction.LEFT;
                        parameter = param;
                    }
                    else
                    {
                        instruction = Instruction.RIGHT;
                        parameter = 360 - param;
                    }
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public Instruction getInstruction()
        {
            return instruction;
        }

        public int getParamter()
        {
            return parameter;
        }

    }
}
