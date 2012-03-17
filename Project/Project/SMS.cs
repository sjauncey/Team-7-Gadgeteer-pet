using System;
using Microsoft.SPOT;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.Seeed;
using Gadgeteer.Modules.GHIElectronics;

namespace GadgeteerApp1
{
    class SMS
    {
        public void smsHandler(CellularRadio sender, CellularRadio.Sms message)
        {
            if (message.TelephoneNumber != "0")
            {
                char[] delims = { ' ', ',', '.', ':', '\t', '\n' };
                string[] words = message.TextMessage.ToUpper().Split(delims);

                SPORK[] instructions = new SPORK[words.Length / 2];
                int i = 0;
                //Using i+1 here to ensure we don't get an error when we look at words[i+1]
                while (i+1 < words.Length)
                {
                    try
                    {
                        switch (words[i])
                        {
                            case "FORWARD":
                                instructions[i / 2] = new SPORK(Instruction.FORWARD, Convert.ToInt16(words[i + 1]));
                                i += 2;
                                break;
                            case "LEFT":
                                instructions[i / 2] = new SPORK(Instruction.LEFT, Convert.ToInt16(words[i + 1]));
                                i += 2;
                                break;
                            case "RIGHT":
                                instructions[i / 2] = new SPORK(Instruction.RIGHT, Convert.ToInt16(words[i + 1]));
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
                                throw new NotSupportedException();

                        }
                    }
                    catch (Exception e)
                    {
                        //we better throw and error here, or whatever is the most efficient way to deal with errors.
                        //Would a custom error reporting module be best? Can print to the oled?
                        //error is in words[i+1]
                    }
                }

                if (i < words.Length)
                {
                    //Then we stopped before looking at the last word. Better throw and error.
                    throw new NotSupportedException();
                    //We need a better way of handling badly formatted messages.
                    //Replace errors with local handling, displaying messages on the screen for example.
                    //words[i] will contain the faulty word.
                }
            }
        }
    }

}

