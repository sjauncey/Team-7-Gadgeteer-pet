﻿using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Touch;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.Seeed;
using Gadgeteer.Modules.GHIElectronics;

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
        public Instruction instruction;
        public int parameter;

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
                    if(param < 180){
                        instruction = Instruction.RIGHT;
                        parameter = param;
                    } else {
                        instruction = Instruction.LEFT;
                        parameter = 360-param;
                    }
                    break;
                case Instruction.LEFT:
                    param = param % 360;
                    if(param < 180){
                        instruction = Instruction.LEFT;
                        parameter = param;
                    } else {
                        instruction = Instruction.RIGHT;
                        parameter = 360-param;
                    }
                    break;    
                default:
                    throw new NotSupportedException();
            }
        }
    }

    public partial class Program
    {
        // This method is run when the mainboard is powered up or reset.   
        void ProgramStarted()
        {
            /*******************************************************************************************
            Modules added in the Program.gadgeteer designer view are used by typing 
            their name followed by a period, e.g.  button.  or  camera.
            
            Many modules generate useful events. Type +=<tab><tab> to add a handler to an event, e.g.:
                button.ButtonPressed +=<tab><tab>
            
            If you want to do something periodically, use a GT.Timer and handle its Tick event, e.g.:
                GT.Timer timer = new GT.Timer(1000); // every second (1000ms)
                timer.Tick +=<tab><tab>
                timer.Start();
            *******************************************************************************************/

            // Use Debug.Print to show messages in Visual Studio's "Output" window during debugging.
            Debug.Print("Program Started");

            cellularRadio.SmsReceived += new CellularRadio.SmsReceivedHandler(SMS.smsHandler());
        }

        
    }
}
