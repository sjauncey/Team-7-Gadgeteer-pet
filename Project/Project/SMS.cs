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

        public void smsHandler(CellularRadio sender, CellularRadio.Sms message)
        {
            if (message.TelephoneNumber != "0")
            {
                char[] delims = { ' ', ',', '.', ':', '\t', '\n' };
                string[] words = message.TextMessage.ToUpper().Split(delims);

                Queue instructions = new Queue();
                int i = 0;
                //Using i+1 here to ensure we don't get an error when we look at words[i+1]
                while (i + 1 < words.Length)
                {
                    try
                    {
                        switch (words[i])
                        {
                            case "FORWARD":
                                instructions.Enqueue(new SPORK(Instruction.FORWARD, Convert.ToInt16(words[i + 1])));
                                i += 2;
                                break;
                            case "LEFT":
                                 instructions.Enqueue(new SPORK(Instruction.LEFT, Convert.ToInt16(words[i + 1])));
                                i += 2;
                                break;
                            case "RIGHT":
                                 instructions.Enqueue(new SPORK(Instruction.RIGHT, Convert.ToInt16(words[i + 1])));
                                i += 2;
                                break;
                            case "FW":
                                words[i] = "FORWARD";
                                break;
                            case "L":
                                words[i] = "LEFT";
                                break;
                            case "R":
                                words[i] = "RIGHT";
                                break;
                            default:
                                throw new Exception();

                        }
                    }
                    catch (Exception e)
                    {
                        Debug2.Instance.Print("Error Parsing Commands. Error in: '" + words[i+1] + "' after '" + words[i]);
                    }
                }

                if (i < words.Length)
                {
                    Debug2.Instance.Print("Error Parsing Commands. Error in: '" + words[i] + "'");
                }

                program.addSPORKS(instructions);
            }
        }
    }

}

