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

        Queue SPORKQueue = new Queue();
        MovementController movementController;
        SMS smsController;
        bool stationary = true;
        Debug2 Debug;
        // This method is run when the mainboard is powered up or reset.   
        void ProgramStarted()
        {
            Debug = new Debug2(oledDisplay);
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
            smsController = new SMS(this);
            movementController = new MovementController();
            led.TurnWhite();
            led1.TurnWhite();
            cellularRadio.PowerOn();
           // GT.Timer t = new GT.Timer(40000, GT.Timer.BehaviorType.RunOnce);
           // t.Tick += new GT.Timer.TickEventHandler(t_Tick);
           // cellularRadio.SignalStrengthRetrieved += new CellularRadio.SignalStrengthRetrievedHandler(cellularRadio_SignalStrengthRetrieved);
            cellularRadio.SmsReceived += new CellularRadio.SmsReceivedHandler(cellularRadio_SmsReceived);
        }

        void cellularRadio_SmsReceived(CellularRadio sender, CellularRadio.Sms message)
        {
            Debug.Print(message.TextMessage);
        }

        void cellularRadio_SignalStrengthRetrieved(CellularRadio sender, CellularRadio.SignalStrengthType signalStrength)
        {
            switch (signalStrength)
            {
                case CellularRadio.SignalStrengthType.VeryWeak:
                    Debug.Print("Very Weak");
                    break;
                case CellularRadio.SignalStrengthType.Weak:
                    Debug.Print("Weak");
                    break;
                case CellularRadio.SignalStrengthType.Strong:
                    Debug.Print("Strong");
                    break;
                case CellularRadio.SignalStrengthType.VeryStrong:
                    Debug.Print("Very Strong");
                    break;
                default:
                    Debug.Print("Undefined Strength");
                    break;
            }

            cellularRadio.RetrieveSignalStrength();
        }

        void t_Tick(GT.Timer timer)
        {
            //takes 40 seconds to power up, then more time to actually pick up a signal
            Debug.Print("Asking for a signal strenght");
            cellularRadio.RetrieveSignalStrength();
        }

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
        }
    }
}
