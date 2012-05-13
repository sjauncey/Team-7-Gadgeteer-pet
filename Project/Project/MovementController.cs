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
        private double offset;
        private double objective;
        private Program model;
        private int timeUnit;
        private double quarter = Math.PI / 2;
        private double whole = Math.PI * 2;

        public MovementController(Relays relayBoard, Compass compassBoard, Program passedModel, int timeUnit){
            model = passedModel;
            relays = relayBoard;
            compass = compassBoard;
            this.timeUnit = timeUnit;
            runTimer = new GT.Timer(timeUnit, GT.Timer.BehaviorType.RunOnce);
            runTimer.Tick += new GT.Timer.TickEventHandler(runTimer_Tick);
            setOffset();
        }

        public void advance(int distance){
            Debug2.Instance.Print("advancing " + distance.ToString());
            if (runTimer.IsRunning)
            {
                //TODO: Error
            }
            else
            {
                stop();
                runTimer.Interval = new System.TimeSpan(0,0,0,0,distance * timeUnit);
                runTimer.Restart();
                relays.Relay1 = true;
                relays.Relay3 = true;
            }
        }

        public void rotateRight()
        {
            stop();
            Debug2.Instance.Print("rotate right");
            objective = (objective + quarter) % whole;

            relays.Relay1 = true;
            relays.Relay4 = true;

            compass.StartContinuousMeasurements();
        }

        public void rotateLeft()
        {
            stop();
            Debug2.Instance.Print("rotate left");
            objective = (objective + 3*quarter) % whole; //Can't just minus 90 as % will give -ve answer on -ve input

            relays.Relay2 = true;
            relays.Relay3 = true;

            compass.StartContinuousMeasurements();
        }

        public void setOffset(){
            compass.MeasurementComplete += new Compass.MeasurementCompleteEventHandler(compassDiscreteComplete);
            compass.RequestMeasurement();
        }

        void compassDiscreteComplete(Compass sender, Compass.SensorData sensorData){
            compass.MeasurementComplete -= compassDiscreteComplete;
            offset = ((quarter/2) - sensorData.Angle) % whole;
            compass.MeasurementComplete += new Compass.MeasurementCompleteEventHandler(compassContinuousComplete);
        }

        void compassContinuousComplete(Compass sender, Compass.SensorData sensorData){
            
            double value = localise(sensorData.Angle);
            Debug2.Instance.Print("value:" + value.ToString() + " objective:"+objective.ToString());
            if ((value - objective) < 0.05 && (objective - value) < 0.05)
            {
                stop();
                compass.StopContinuousMeasurements();
                model.movementFinished();
                Debug2.Instance.Print("turn complete value: " + value.ToString() + " objective:" + objective.ToString());
            }            
        }

        public void allStop(){
            stop();
            runTimer.Stop();
        }

        private void runTimer_Tick(GT.Timer timer){
            Debug2.Instance.Print("advance finished");
            stop();
            model.movementFinished();
        }

        private double localise(double measure){
            return (measure + offset) % whole;
        }

        private void stop(){
            relays.Relay1 = false;
            relays.Relay2 = false;
            relays.Relay3 = false;
            relays.Relay4 = false;
        }
    }
}
