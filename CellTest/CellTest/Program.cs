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
using Gadgeteer.Modules.GHIElectronics;
using Gadgeteer.Modules.Seeed;

namespace GadgeteerApp1
{
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
            cellularRadio.DebugPrintEnabled = true;
            cellularRadio.PowerOn();
            GT.Timer ledtime = new GT.Timer(500);
            ledtime.Tick += new GT.Timer.TickEventHandler(ledtime_Tick);
            ledtime.Start();
            cellularRadio.SmsReceived += new CellularRadio.SmsReceivedHandler(cellularRadio_SmsReceived);
            button.ButtonPressed += new Button.ButtonEventHandler(button_ButtonPressed);
        }

        void button_ButtonPressed(Button sender, Button.ButtonState state)
        {
            cellularRadio.PowerOff();

        }

        void cellularRadio_SmsReceived(CellularRadio sender, CellularRadio.Sms message)
        {
            Debug.Print(message.TextMessage);
        }

        void ledtime_Tick(GT.Timer timer)
        {
            button.ToggleLED();
        }
    }
}
