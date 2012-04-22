using System;
using Microsoft.SPOT;
using GT = Gadgeteer;

namespace GadgeteerApp1
{
    public class Debug2
    {
        private Gadgeteer.Modules.Seeed.OledDisplay oled;
        private GT.Modules.Seeed.CellularRadio radio;
        private uint oledH;
        private uint oledW;

        private string previousDebugMsg, currentDebugMsg, oldestDebugMsg;
        GT.Modules.Seeed.CellularRadio.SignalStrengthType signalStrength = new GT.Modules.Seeed.CellularRadio.SignalStrengthType();
        private GT.Timer signalStrengthCheck;

        private static Debug2 instance;

        public static Debug2 Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Debug2();
                }
                return Instance;
            }

        }

        public Debug2()
        {
            this.oledH = oled.Height;
            this.oledW = oled.Width;
            currentDebugMsg = ""; previousDebugMsg = ""; oldestDebugMsg="";
            signalStrength = GT.Modules.Seeed.CellularRadio.SignalStrengthType.Error;

            signalStrengthCheck = new GT.Timer(10000);
            signalStrengthCheck.Tick += new GT.Timer.TickEventHandler(signalStrengthCheck_Tick);
            radio.SignalStrengthRetrieved += new GT.Modules.Seeed.CellularRadio.SignalStrengthRetrievedHandler(radio_SignalStrengthRetrieved);

        }

        public void setOled(Gadgeteer.Modules.Seeed.OledDisplay oled)
        {
            this.oled = oled;
        }

        public void setCellRadio(Gadgeteer.Modules.Seeed.CellularRadio radio)
        {
            this.radio = radio;
        }

        void radio_SignalStrengthRetrieved(GT.Modules.Seeed.CellularRadio sender, GT.Modules.Seeed.CellularRadio.SignalStrengthType signalStrength)
        {
            this.signalStrength = signalStrength;
            if (signalStrengthCheck.IsRunning)
            {
                DrawSignalStrength();
            }
        }

        void signalStrengthCheck_Tick(GT.Timer timer)
        {
            if (radio.RetrieveSignalStrength() == GT.Modules.Seeed.CellularRadio.ReturnedState.ModuleIsOff)
            {
                Print("Cell Module is off, disabling signal strength indicator");
                signalStrengthCheck.Stop();
            }
        }

        public void Print(String s)
        {
            Debug.Print(s);
            oldestDebugMsg = previousDebugMsg;
            previousDebugMsg = currentDebugMsg;
            currentDebugMsg = s;
            RedrawDebugs();

        }

        public void RedrawDebugs()
        {
            oled.SimpleGraphics.Clear();
            oled.SimpleGraphics.DisplayTextInRectangle(oldestDebugMsg, 0, 3, oledW, oledH / 3, GT.Color.Red, Resources.GetFont(Resources.FontResources.small));
            oled.SimpleGraphics.DisplayTextInRectangle(previousDebugMsg, 0, 2 + (oledH / 3), oledW, (2 / 3) * oledH, GT.Color.Red, Resources.GetFont(Resources.FontResources.small));
            oled.SimpleGraphics.DisplayTextInRectangle(currentDebugMsg, 0, 2 + (2 * oledH / 3), oledW, oledH, GT.Color.Red, Resources.GetFont(Resources.FontResources.small));
        }

        public void DrawSignalStrength() 
        {
            oled.SimpleGraphics.DisplayRectangle(GT.Color.Black, 0, GT.Color.Black, 0, 0, oledW, 5);
            switch (signalStrength)
            {
                case GT.Modules.Seeed.CellularRadio.SignalStrengthType.VeryStrong:
                    oled.SimpleGraphics.DisplayRectangle(GT.Color.Black, 1, GT.Color.Orange, 0, 0, oledW, 5);
                    goto case GT.Modules.Seeed.CellularRadio.SignalStrengthType.Strong;
                case GT.Modules.Seeed.CellularRadio.SignalStrengthType.Strong:
                    oled.SimpleGraphics.DisplayRectangle(GT.Color.Black, 1, GT.Color.Orange, 0, 0, (3 / 4) * oledW, 5);
                    goto case GT.Modules.Seeed.CellularRadio.SignalStrengthType.Weak;
                case GT.Modules.Seeed.CellularRadio.SignalStrengthType.Weak:
                    oled.SimpleGraphics.DisplayRectangle(GT.Color.Black, 1, GT.Color.Orange, 0, 0, (2 / 4) * oledW, 5);
                    goto case GT.Modules.Seeed.CellularRadio.SignalStrengthType.VeryWeak;
                case GT.Modules.Seeed.CellularRadio.SignalStrengthType.VeryWeak:
                    oled.SimpleGraphics.DisplayRectangle(GT.Color.Black, 1, GT.Color.Orange, 0, 0, (2 / 4) * oledW, 5);
                    break;
                case GT.Modules.Seeed.CellularRadio.SignalStrengthType.Error:
                    oled.SimpleGraphics.DisplayTextInRectangle("Error", 0, 0, oledW, 5, GT.Color.White, Resources.GetFont(Resources.FontResources.small));
                    break;
                case GT.Modules.Seeed.CellularRadio.SignalStrengthType.Unknown:
                    oled.SimpleGraphics.DisplayTextInRectangle("Unknown Strength", 0, 0, oledW, 5, GT.Color.White, Resources.GetFont(Resources.FontResources.small));
                    break;
            } 
        }

        public void EnableSignalStrengthIndicator()
        {
            signalStrengthCheck.Start();            
        }

        public void DisableSignalStrengthIndicator()
        {
            signalStrengthCheck.Stop();
        }


        
    }
}
