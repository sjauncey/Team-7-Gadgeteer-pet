using System;
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

    public partial class Program
    {

        //Queue SPORKQueue = new Queue();
        //MovementController movementController;
        //SMS smsController;
        bool stationary = true;
        Display display;
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
            //smsController = new SMS(this);
            //movementController = new MovementController();
            //cellularRadio.SmsReceived += new CellularRadio.SmsReceivedHandler(smsController.smsHandler);

            display = new Display(oledDisplay);
        }
        /*
        internal void addSPORKS(Queue sporks)
        {
            foreach (SPORK s in sporks)
            {
                 SPORKQueue.Enqueue(s);
            }
        }

        internal void movementFinished()
        {
            if (SPORKQueue.Count != 0)
            {
                //send a new instruction

            }
            else
            {
                stationary = true;
            }
        }*/
    }
}
