using System;
using Microsoft.SPOT;
using GT = Gadgeteer;

namespace ScreenDebugTest
{
    public class Debug2
    {
        private Gadgeteer.Modules.Seeed.OledDisplay oled;
        private uint oledH;
        private uint oledW;

        private string prev, prev1;

        public Debug2(Gadgeteer.Modules.Seeed.OledDisplay oled)
        {
            this.oled = oled;
            this.oledH = oled.Height;
            this.oledW = oled.Width;
            prev1 = ""; prev = "";
        }

        public void Print(String s)
        {
            oled.SimpleGraphics.Clear();
            oled.SimpleGraphics.DisplayTextInRectangle(prev, 0, 0, oledW, oledH/3, GT.Color.Red, Resources.GetFont(Resources.FontResources.small));
            prev = prev1;
            oled.SimpleGraphics.DisplayTextInRectangle(prev1, 0,2+(oledH/3), oledW, (2/3)*oledH, GT.Color.Red, Resources.GetFont(Resources.FontResources.small));
            prev1 = s;
            oled.SimpleGraphics.DisplayTextInRectangle(s, 0,2+(2*oledH/3), oledW, oledH, GT.Color.Red, Resources.GetFont(Resources.FontResources.small));
            Debug.Print(s);
        }
    }
}
