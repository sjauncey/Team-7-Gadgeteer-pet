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
    public enum SPORK
    {
        FORWARD,
        LEFT,
        RIGHT,
        BACKWARD
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
            mode = Mode.Solver;
            Debug2.Instance.setOled(oledDisplay);
            //Debug2.Instance.DisableScreenDebug();
            Debug2.Instance.Print("Program Started");
            smsController = new SMS(this);
            movementController = new MovementController(relays, gyro, this, 150);
            imageRec = new ImageRec(camera, this);
            ms = new MazeSearch(this);
            ms.initalStep();
            //imageRec.startContinuousChecking();
            //addSPORK(new SPORK(Instruction.LEFT, 0));
            //addSPORK(new SPORK(Instruction.FORWARD, 1));
            //addSPORK(new SPORK(Instruction.RIGHT, 0));
            //cheat();
        }

        internal void cheat()
        {
            smsController.msgToSpork("F F R F F R F F R F F R");
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
            if (s == SPORK.RIGHT || s ==SPORK.LEFT)
            {
                SPORKQueue.Enqueue(SPORK.FORWARD);
                SPORKQueue.Enqueue(s);
                SPORKQueue.Enqueue(SPORK.BACKWARD);
            }
            else
            {
                SPORKQueue.Enqueue(s);
            }
            if (stationary)
            {
                movementFinished();
            }
        }

        internal void movementFinished()
        {
            
            if (SPORKQueue.Count != 0)
            {
                Debug2.Instance.Print("getting new instruction");
                stationary = false;
                SPORK inst = (SPORK) SPORKQueue.Dequeue();
                switch (inst)
                {
                    case(SPORK.LEFT):
                        movementController.rotateLeft();
                        break;
                    case(SPORK.RIGHT):
                        movementController.rotateRight();
                        break;
                    case(SPORK.FORWARD):
                        movementController.advance();
                        break;
                    case (SPORK.BACKWARD):
                        movementController.reverse();
                        break;
                }

            }
            else
            {
                Debug2.Instance.Print("No new instruction, testing square");
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
                ms.nextStep(CellType.Unfinished);
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
