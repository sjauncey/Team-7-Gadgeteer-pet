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
        private Gyro gyro;
        private GT.Timer runTimer;
        private double offset;
        private double objective;
        private double turned;
        private Program model;
        private int timeUnit;

        public MovementController(Relays relayBoard, Gyro gyroBoard, Program passedModel, int timeUnit)
        {
            model = passedModel;
            relays = relayBoard;
            gyro = gyroBoard;
            this.timeUnit = timeUnit;
            runTimer = new GT.Timer(timeUnit, GT.Timer.BehaviorType.RunOnce);
            runTimer.Tick += new GT.Timer.TickEventHandler(runTimer_Tick);
            gyro.ContinuousMeasurementInterval = new System.TimeSpan(0, 0, 0, 0, 100);
            gyro.Calibrate();
            gyro.MeasurementComplete += new Gyro.MeasurementCompleteEventHandler(gyro_MeasurementComplete);
        }


        public void advance(int distance)
        {
            Debug2.Instance.Print("advancing " + distance.ToString());
            if (runTimer.IsRunning)
            {
                //TODO: Error
            }
            else
            {
                stop();
                runTimer.Interval = new System.TimeSpan(0, 0, 0, 0, distance * timeUnit);
                runTimer.Restart();
                relays.Relay1 = true;
                relays.Relay3 = true;
            }
        }

        public void rotateRight()
        {
            stop();
            Debug2.Instance.Print("rotate right");
            objective = (90 + offset);
            turned = 0;
            gyro.StartContinuousMeasurements();
            relays.Relay1 = true;
            relays.Relay4 = true;
        }

        public void rotateLeft()
        {
            stop();
            Debug2.Instance.Print("rotate left");
            objective = (-90 + offset);
            turned = 0;
            gyro.StartContinuousMeasurements();
            relays.Relay2 = true;
            relays.Relay3 = true;
        }


        void gyro_MeasurementComplete(Gyro sender, Gyro.SensorData sensorData)
        {
            turned += (sensorData.Z / 10);
            if (((objective < 0) && (turned < objective)) || ((objective > 0) && (turned > objective)))
            {
                stop();
                offset = objective - turned;
                gyro.StopContinuousMeasurements();
                if (offset < -5)
                {
                    offset += 90;
                    rotateLeft();
                }
                else if (offset > 5)
                {
                    offset -= 90;
                    rotateRight();
                }
                else
                {
                    model.movementFinished();
                }

            }
        }

        public void allStop()
        {
            stop();
            runTimer.Stop();
        }

        private void runTimer_Tick(GT.Timer timer)
        {
            Debug2.Instance.Print("advance finished");
            stop();
            model.movementFinished();
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
