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

            Debug.Print("Program Started");
            smsController = new SMS(this);
            movementController = new MovementController();

            camera.CurrentPictureResolution = Camera.PictureResolution.Resolution160x120;

            cellularRadio.PowerOn();
            cellularRadio.SmsReceived += smsController.smsHandler;
        }

        internal void addSPORKS(Queue sporks)
        {
            foreach (SPORK s in sporks)
            {
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
