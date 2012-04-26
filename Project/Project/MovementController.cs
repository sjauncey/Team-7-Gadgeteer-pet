using System;
using System.Text;

using GT = Gadgeteer;

using Gadgeteer.Modules.Seeed;

/*Offset is calculated so that if the rat is pointing at local north, normalise will return 45 degrees.
 * This gets round the problem of the compass value passing from 360 to 0.
 * 
 * Both NO terminals are connected to the +ve terminal, and we assume that openning the odd relays is "forward".
 * Relays 1&2 control the right relay, 3&4 control the left.
 * */

namespace GadgeteerApp1
{
    class MovementController
    {
        private Relays relays;
        private Compass compass;
        private GT.Timer runTimer;
        private float offset;
        private int objective;
        private Program model;

        public MovementController(Relays relayBoard, Compass compassBoard, Program passedModel){
            model = passedModel;
            relays = relayBoard;
            compass = compassBoard;
            compass.MeasurementComplete +=new Compass.MeasurementCompleteEventHandler(compassContinuousComplete);
        }

        //There's never any harm in having a destrucor.
        //Might get rid of it at some point though
        public ~MovementController(){
            if(relays.Relay1 || relays.Relay2 || relays.Relay3 || relays.Relay4 || runTimer.IsRunning){
            //TODO Some error has occured. Handle properly.
            }
        }

        public void advance(){
            runTimer.Start();
            relays.Relay1 = true;
            relays.Relay3 = true;
        }

        public void rotateRight(){
            objective = (objective + 90) % 360;

            relays.Relay2 = true;
            relays.Relay3 = true;

            compass.StartContinuousMeasurements();
        }

        public void rotateLeft(){
            objective = (objective + 270) % 360; //Can't just minus 90 as % will give -ve answer on -ve input
 
            relays.Relay1 = true;
            relays.Relay4 = true;

            compass.StartContinuousMeasurements();
        }


        public void setRunTime(int milliseconds){
            runTimer = new GT.Timer(milliseconds);
            runTimer.Tick += new GT.Timer.TickEventHandler(runTimer_Tick);
        }

        public void setOffset(){
            compass.MeasurementComplete += new Compass.MeasurementCompleteEventHandler(compassDiscreteComplete);
            compass.RequestMeasurement();
            compass.MeasurementComplete -= compassDiscreteComplete;
        }

        void compassDiscreteComplete(Compass sender, Compass.SensorData sensorData){
            offset = 45 - sensorData.X;
        }

        void compassContinuousComplete(Compass sender, Compass.SensorData sensorData){
            int value = (int)localise(sensorData.X);
            if (Math.Abs(value - objective) < 3){
                stop();
                compass.StopContinuousMeasurements();
                model.movementFinished();
            }
        }

        public void allStop(){
            stop();
            runTimer.Stop();
        }

        private void runTimer_Tick(GT.Timer timer){
            stop();
            runTimer.Stop();
            model.movementFinished();
        }

        private float localise(float measure){
            return (measure + offset) % 360;
        }

        private void stop(){
            relays.Relay1 = false;
            relays.Relay2 = false;
            relays.Relay3 = false;
            relays.Relay4 = false;
        }
    }
}
