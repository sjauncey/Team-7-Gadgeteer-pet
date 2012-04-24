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

        private Bitmap imageUp, imageLeft, imageRight, imageDown;

        private static Debug2 instance;
        private bool oledDebugEnabled;

        public static Debug2 Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Debug2();
                }
                return instance;
            }

        }

        public Debug2()
        {

            currentDebugMsg = ""; previousDebugMsg = ""; oldestDebugMsg="";
            signalStrength = GT.Modules.Seeed.CellularRadio.SignalStrengthType.Error;

            signalStrengthCheck = new GT.Timer(10000);
            signalStrengthCheck.Tick += new GT.Timer.TickEventHandler(signalStrengthCheck_Tick);

            DisableScreenDebug();

            imageUp = Resources.GetBitmap(Resources.BitmapResources.up);
            imageLeft = Resources.GetBitmap(Resources.BitmapResources.left);
            imageRight = Resources.GetBitmap(Resources.BitmapResources.right);
            imageDown = Resources.GetBitmap(Resources.BitmapResources.down);

        }

        public void displayImage(Bitmap bmp) {
            oled.SimpleGraphics.DisplayImage(bmp, 0, 0);
        }

        private void DisableScreenDebug()
        {
            oledDebugEnabled = false;
        }

        public void setOled(Gadgeteer.Modules.Seeed.OledDisplay oled)
        {
            this.oled = oled;
            this.oledH = oled.Height;
            this.oledW = oled.Width;
            Print("Screen is "+oledW+"X"+oledH);
        }

        public void setCellRadio(Gadgeteer.Modules.Seeed.CellularRadio radio)
        {
            this.radio = radio;
            radio.SignalStrengthRetrieved += new GT.Modules.Seeed.CellularRadio.SignalStrengthRetrievedHandler(radio_SignalStrengthRetrieved);
        }

        void radio_SignalStrengthRetrieved(GT.Modules.Seeed.CellularRadio sender, GT.Modules.Seeed.CellularRadio.SignalStrengthType signalStrength)
        {
            this.signalStrength = signalStrength;
            if (signalStrengthCheck.IsRunning)
            {
                Print("Singal strength is " + signalStrength);
            }
        }

        void signalStrengthCheck_Tick(GT.Timer timer)
        {
            if (radio.RetrieveSignalStrength() == GT.Modules.Seeed.CellularRadio.ReturnedState.ModuleIsOff)
            {
               // Print("Cell Module is off, disabling signal strength indicator");
               // signalStrengthCheck.Stop();
            }
        }

        public void Print(String s)
        {
            Debug.Print(s);
            oldestDebugMsg = previousDebugMsg;
            previousDebugMsg = currentDebugMsg;
            currentDebugMsg = s;
            if (oledDebugEnabled)
            {
                RedrawDebugs();
            }

        }

        private void RedrawDebugs()
        {
            oled.SimpleGraphics.Clear();
            oled.SimpleGraphics.DisplayTextInRectangle(oldestDebugMsg, 0, 3, oledW, oledH / 3, GT.Color.Red, Resources.GetFont(Resources.FontResources.small));
            oled.SimpleGraphics.DisplayTextInRectangle(previousDebugMsg, 0, 2 + (oledH / 3), oledW, (2 / 3) * oledH, GT.Color.Red, Resources.GetFont(Resources.FontResources.small));
            oled.SimpleGraphics.DisplayTextInRectangle(currentDebugMsg, 0, 2 + (2 * oledH / 3), oledW, oledH, GT.Color.Red, Resources.GetFont(Resources.FontResources.small));
            DrawSignalStrength();
        }

        public void EnableScreenDebug()
        {
            oledDebugEnabled = true;
        }

        public void DrawSignalStrength() 
        {
            oled.SimpleGraphics.DisplayRectangle(GT.Color.Black, 0, GT.Color.Black, 87, 0, 40, 5);
            switch (signalStrength)
            {
                case GT.Modules.Seeed.CellularRadio.SignalStrengthType.VeryStrong:
                    oled.SimpleGraphics.DisplayRectangle(GT.Color.Black, 1, GT.Color.Orange, 87, 0, 40, 5);
                    goto case GT.Modules.Seeed.CellularRadio.SignalStrengthType.Strong;
                case GT.Modules.Seeed.CellularRadio.SignalStrengthType.Strong:
                    oled.SimpleGraphics.DisplayRectangle(GT.Color.Black, 1, GT.Color.Orange, 97, 0, 30, 5);
                    goto case GT.Modules.Seeed.CellularRadio.SignalStrengthType.Weak;
                case GT.Modules.Seeed.CellularRadio.SignalStrengthType.Weak:
                    oled.SimpleGraphics.DisplayRectangle(GT.Color.Black, 1, GT.Color.Orange, 107, 0, 20, 5);
                    goto case GT.Modules.Seeed.CellularRadio.SignalStrengthType.VeryWeak;
                case GT.Modules.Seeed.CellularRadio.SignalStrengthType.VeryWeak:
                    oled.SimpleGraphics.DisplayRectangle(GT.Color.Black, 1, GT.Color.Orange, 117, 0, 10, 5);
                    break;
                case GT.Modules.Seeed.CellularRadio.SignalStrengthType.Error:
                    oled.SimpleGraphics.DisplayRectangle(GT.Color.Black, 1, GT.Color.Red, 87, 0, 40, 5);
                    break;
                case GT.Modules.Seeed.CellularRadio.SignalStrengthType.Unknown:
                    oled.SimpleGraphics.DisplayRectangle(GT.Color.Black, 1, GT.Color.LightGray, 87, 0, 40, 5);
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
            oled.SimpleGraphics.DisplayRectangle(GT.Color.Black, 0, GT.Color.Black, 87, 0, 40, 10);
        }

        public void showUp()
        {
            oled.SimpleGraphics.Clear();
            oled.SimpleGraphics.DisplayImage(imageUp, 14, 14);
        }

        public void showLeft()
        {
            oled.SimpleGraphics.Clear();
            oled.SimpleGraphics.DisplayImage(imageLeft, 14, 14);
        }

        public void showRight()
        {
            oled.SimpleGraphics.Clear();
            oled.SimpleGraphics.DisplayImage(imageRight, 14, 14);
        }

        public void showDown()
        {
            oled.SimpleGraphics.Clear();
            oled.SimpleGraphics.DisplayImage(imageDown, 14, 14);
        }


        
    }
}
