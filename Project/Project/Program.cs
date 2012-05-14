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
        SMS smsController;
        ImageRec imageRec;
        bool stationary = true;
        public Mode mode { get; set; }
        MazeSearch ms;

        void ProgramStarted()
        {
            mode = Mode.Directed;
            Debug2.Instance.setOled(oledDisplay);
            Debug2.Instance.EnableScreenDebug();
            Debug2.Instance.Print("Program Started");
            smsController = new SMS(this);
            movementController = new MovementController(relays, compass, this, 1000);
            imageRec = new ImageRec(camera, this);
            ms = new MazeSearch(this);
            //ms.initalStep();
            imageRec.startContinuousChecking();
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
            Debug2.Instance.Print("Over white");
            if (mode == Mode.Solver)
            {
                ms.nextStep(CellType.Unvisited);
            }
            else
            {
                //do nothing
            }
        }

        internal void aboveWall()
        {
            Debug2.Instance.Print("Over black");
            if (mode == Mode.Solver)
            {
                ms.nextStep(CellType.Wall);
            }
            else
            {
                //
            }
        }

        internal void aboveTarget()
        {
            Debug2.Instance.Print("Over red");
            if (mode == Mode.Solver)
            {

                ms.nextStep(CellType.Target);
            }
            else
            {

            }
        }




        internal void mazeSolved()
        {
            Debug2.Instance.Print("Solved the maze!");
        }
    }
}
