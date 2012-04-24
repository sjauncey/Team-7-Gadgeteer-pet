using System;
using System.Text;

using GT = Gadgeteer;

using Gadgeteer.Modules.Seeed;

//The NC terminal is linked to the positive terminal

namespace GadgeteerApp1
{
    class MovementController
    {
        private Relays relays;
        private Compass compass;
        private GT.Timer runTimer;
        private float latestPoll;
        private float localNorth, localEast, localSouth, localWest;
        private int cutoff;

        public MovementController(Relays relayBoard, Compass compassBoard){
            relays = relayBoard;
            compass = compassBoard;
        }

        public void advance(){
            runTimer.Start();
            relays.Relay1 = true;
            relays.Relay3 = true;
        }

        public void rotateRight(){

            pollNum = 0;
            while(pollNum < 2){
                compass.RequestMeasurement();
            }

            int goal = ((store[0] + store[1] + store[2]) / 3) + 90;
        }


        public void setRunTime(int milliseconds){
            runTimer = new GT.Timer(milliseconds);
            runTimer.Tick += new GT.Timer.TickEventHandler(runTimer_Tick);
        }

        public void getNorth(){
            float f = pollCompass();
            if (345< f && 355 > f){ cutoff = 0;
        }

        public void allStop(){
            stop();
            runTimer.Stop();
        }

        private void runTimer_Tick(GT.Timer timer){
            stop();
            runTimer.Stop();
        }

        private float pollCompass(int passes){
            float total = 0;

            for(int i=0;i<passes;i++){
                compass.RequestMeasurement();
                total += latestPoll;
            }

            return (total / passes);
        }

        private float pollCompass(){
            compass.RequestMeasurement();
            return latestPoll;
        }


        private void stop()
        {
            relays.Relay1 = false;
            relays.Relay2 = false;
            relays.Relay3 = false;
            relays.Relay4 = false;
        }
    }
}
