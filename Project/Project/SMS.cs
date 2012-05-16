using System;
using Microsoft.SPOT;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.Seeed;
using Gadgeteer.Modules.GHIElectronics;
using System.Collections;

namespace GadgeteerApp1
{

    class SMS
    {
        Program program;

        public SMS(Program program)
        {
            this.program = program;
        }

        public void msgToSpork(string message)
        {

            char[] delims = { ' ', ',', '.', ':', '\t', '\n' };
            string[] words = message.ToUpper().Split(delims);

            Queue instructions = new Queue();
            int i = 0;
            //Using i+1 here to ensure we don't get an error when we look at words[i+1]
            while (i < words.Length)
            {
                try
                {
                    switch (words[i])
                    {
                        case "FORWARD":
                            instructions.Enqueue(SPORK.FORWARD);
                            i += 1;
                            break;
                        case "LEFT":
                            instructions.Enqueue(SPORK.LEFT);
                            i += 1;
                            break;
                        case "RIGHT":
                            instructions.Enqueue(SPORK.RIGHT);
                            i += 1;
                            break;
                        case "BACKWARD":
                            instructions.Enqueue(SPORK.BACKWARD);
                            i += 1;
                            break;
                        case "FW":
                            goto case "FORWARD";
                        case "F":
                            goto case "FORWARD";
                        case "L":
                            goto case "LEFT";
                        case "R":
                            goto case "RIGHT";
                        case "B":
                            goto case "BACKWARD";
                        default:
                            throw new Exception();

                    }
                }
                catch (Exception e)
                {
                    Debug2.Instance.Print("Error Parsing Commands. Error in: '" + words[i + 1] + "' after '" + words[i]);
                }
            }

            program.addSPORKS(instructions);
        }

    }

}

