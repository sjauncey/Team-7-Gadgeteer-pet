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
        ImageRec imageRec;
        bool stationary = true;
        Debug2 Debug;
        // This method is run when the mainboard is powered up or reset.   

        void ProgramStarted()
        {
            Debug = Debug2.Instance;
            //Debug.setCellRadio(cellularRadio);
            Debug.setOled(oledDisplay);
<<<<<<< HEAD
            //Debug.EnableSignalStrengthIndicator();
            //Debug.EnableScreenDebug();

            oledDisplay.SimpleGraphics.DisplayTextInRectangle("Testing",0,0,100,50,GT.Color.Black,Resources.GetFont(Resources.FontResources.small));

=======
            //Debug.EnableSignalStrengthIndicator(); //Displays the signal strength in the top right hand corner of the screen
            Debug.EnableScreenDebug();
>>>>>>> c4eff3d2276c5bd9d486a9ad91df3fbc16bf6bb3

            Debug.Print("Program Started");
            //smsController = new SMS(this);
            movementController = new MovementController();
            imageRec = new ImageRec(camera, led);

<<<<<<< HEAD
            button1.ButtonPressed += new Button.ButtonEventHandler(button1_ButtonPressed);
            
            /**
            cellularRadio.PowerOn();
            button1.TurnLEDOn();
                    
            

            smsController.smsHandler(cellularRadio, new CellularRadio.Sms("07772275081","LEFT 90 FORWARD 20 RIGHT 180", CellularRadio.SmsState.All, new DateTime()));
=======
            camera.CurrentPictureResolution = Camera.PictureResolution.Resolution160x120;

            cellularRadio.PowerOn();
            button1.TurnLEDOn();
            button1.ButtonPressed += new Button.ButtonEventHandler(button1_ButtonPressed);
>>>>>>> c4eff3d2276c5bd9d486a9ad91df3fbc16bf6bb3

            //smsController.smsHandler(cellularRadio, new CellularRadio.Sms("07772275081","LEFT 90 FORWARD 20 RIGHT 180", CellularRadio.SmsState.All, new DateTime()));
            //cellularRadio.OperatorRetrieved += new CellularRadio.OperatorRetrievedHandler(cellularRadio_OperatorRetrieved);
            //GT.Timer smscheck = new GT.Timer(10000);
            //smscheck.Tick += new GT.Timer.TickEventHandler(smscheck_Tick);
            //cellularRadio.SmsListRetrieved += new CellularRadio.SmsListRetrievedHandler(cellularRadio_SmsListRetrieved);
<<<<<<< HEAD
            cellularRadio.SmsReceived += new CellularRadio.SmsReceivedHandler(cellularRadio_SmsReceived);
            smscheck.Start();
            */

            
=======
            //cellularRadio.SmsReceived += new CellularRadio.SmsReceivedHandler(cellularRadio_SmsReceived);
            //smscheck.Start();
>>>>>>> c4eff3d2276c5bd9d486a9ad91df3fbc16bf6bb3

        }

        void cellularRadio_SmsReceived(CellularRadio sender, CellularRadio.Sms message)
        {
            Debug.Print("Message Received");
        }

        void button1_ButtonPressed(Button sender, Button.ButtonState state)
        {

            /**if (button1.IsLedOn)
            {
                button1.TurnLEDOff();
                cellularRadio.PowerOff();
            }
            else
            {
                button1.TurnLEDOn();
                cellularRadio.PowerOn();
            }
             */
        }

        void cellularRadio_OperatorRetrieved(CellularRadio sender, string operatorName)
        {
            Debug.Print(operatorName);
        }

        void cellularRadio_SmsListRetrieved(CellularRadio sender, ArrayList smsList)
        {
            Debug.Print(smsList.Count + " unread SMS");
           // foreach(Gadgeteer.Modules.Seeed.CellularRadio.Sms s in smsList){
           //     Debug.Print(s.TextMessage);
           //  }
        }

        void smscheck_Tick(GT.Timer timer)
        {
            cellularRadio.RetrieveSmsList(CellularRadio.SmsState.ReceivedUnread);
        }

        internal void addSPORKS(Queue sporks)
        {
            foreach (SPORK s in sporks)
            {
                 Debug.Print("Enqueuing " + s.ToString());
                 SPORKQueue.Enqueue(s);
            }
            if (stationary)
            { 

            }
        }

        internal void movementFinished()
        {
            if (SPORKQueue.Count != 0)
            {
                stationary = false;
                //send a new instruction

            }
            else
            {
                stationary = true;
            }
        }
    }
}
