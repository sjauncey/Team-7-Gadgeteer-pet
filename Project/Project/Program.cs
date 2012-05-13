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

    public enum Mode
    {
        Solver,
        Directed
    }

    public partial class Program
    {

        Queue SPORKQueue = new Queue();
        MovementController movementController;
        //SMS smsController;
        ImageRec imageRec;
        bool stationary = true;
        //Debug2 Debug;
        public Mode mode { get; set; }
        MazeSearch ms;

        void ProgramStarted()
        {
            mode = Mode.Solver;
            //Debug = Debug2.Instance;
            Debug2.Instance.setOled(oledDisplay);
            Debug2.Instance.EnableScreenDebug();
            Debug.Print("Program Started");
           // smsController = new SMS(this);
            movementController = new MovementController(relays, compass, this, 1000);
            imageRec = new ImageRec(camera, this);
            ms = new MazeSearch(this);
            //ms.initalStep();
            addSPORK(new SPORK(Instruction.FORWARD,2));
            addSPORK(new SPORK(Instruction.LEFT, 0));
            addSPORK(new SPORK(Instruction.RIGHT, 0));
            addSPORK(new SPORK(Instruction.FORWARD, 3));

        }


        internal void addSPORKS(Queue sporks)
        {
            foreach (SPORK s in sporks)
            {
                addSPORK(s);
            }
            if (stationary)
            {
                movementFinished();
            }
        }
        internal void addSPORK(SPORK s)
        {
            Debug.Print("Enqueuing " + s.ToString());
            SPORKQueue.Enqueue(s);
            if (stationary)
            {
                movementFinished();
            }
        }

        internal void movementFinished()
        {
            Debug2.Instance.Print("getting new instruction");
            if (SPORKQueue.Count != 0)
            {
                stationary = false;
                SPORK inst = (SPORK) SPORKQueue.Dequeue();
                switch (inst.getInstruction())
                {
                    case(Instruction.LEFT):
                        movementController.rotateLeft();
                        break;
                    case(Instruction.RIGHT):
                        movementController.rotateRight();
                        break;
                    case(Instruction.FORWARD):
                        movementController.advance(inst.getParamter());
                        break;
                }

            }
            else
            {
                stationary = true;
                if (mode == Mode.Solver)
                {
                    imageRec.testCurrentLocation();
                }
            }
        }

        internal void aboveSpace()
        {
            ms.nextStep(CellType.Unvisited);
        }

        internal void aboveWall()
        {
            ms.nextStep(CellType.Wall);
        }

        internal void aboveTarget()
        {
            ms.nextStep(CellType.Target);
        }



    }
}
