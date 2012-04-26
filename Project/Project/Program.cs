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
            Debug = Debug2.Instance;
            Debug.setCellRadio(cellularRadio);
            Debug.setOled(oledDisplay);
            //Debug.EnableSignalStrengthIndicator(); //Displays the signal strength in the top right hand corner of the screen
            Debug.EnableScreenDebug();

            Debug.Print("Program Started");
            //smsController = new SMS(this);
            movementController = new MovementController();

            camera.CurrentPictureResolution = Camera.PictureResolution.Resolution160x120;

            cellularRadio.PowerOn();
            button1.TurnLEDOn();
            button1.ButtonPressed += new Button.ButtonEventHandler(button1_ButtonPressed);

            //smsController.smsHandler(cellularRadio, new CellularRadio.Sms("07772275081","LEFT 90 FORWARD 20 RIGHT 180", CellularRadio.SmsState.All, new DateTime()));
            //cellularRadio.OperatorRetrieved += new CellularRadio.OperatorRetrievedHandler(cellularRadio_OperatorRetrieved);
            //GT.Timer smscheck = new GT.Timer(10000);
            //smscheck.Tick += new GT.Timer.TickEventHandler(smscheck_Tick);
            //cellularRadio.SmsListRetrieved += new CellularRadio.SmsListRetrievedHandler(cellularRadio_SmsListRetrieved);
            //cellularRadio.SmsReceived += new CellularRadio.SmsReceivedHandler(cellularRadio_SmsReceived);
            //smscheck.Start();


            camera.BitmapStreamed += new Camera.BitmapStreamedEventHandler(camera_BitmapStreamed);
        }

        void camera_BitmapStreamed(Camera sender, Bitmap bitmap)
        {
            throw new NotImplementedException();
        }

        void cellularRadio_SmsReceived(CellularRadio sender, CellularRadio.Sms message)
        {
            Debug.Print("Message Received");
        }

        void button1_ButtonPressed(Button sender, Button.ButtonState state)
        {
            if (button1.IsLedOn)
            {
                button1.TurnLEDOff();
                cellularRadio.PowerOff();
            }
            else
            {
                button1.TurnLEDOn();
                cellularRadio.PowerOn();
            }
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
