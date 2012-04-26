using System;
using Microsoft.SPOT;

using Gadgeteer.Modules.Seeed;
using Gadgeteer.Modules.GHIElectronics;

namespace GadgeteerApp1
{
    class Display
    {
        //Note that the screen is 128*128

        private UInt32 defaultX, defaultY, defaultWidth, defaultHeight;
        private Font myFont;
        private OledDisplay screen;
        private Bitmap imageUp, imageLeft, imageRight;

        public Display(OledDisplay display){
            defaultX = 10;
            defaultY = 10;
            defaultWidth = display.Width - 2 * defaultY;
            defaultHeight = display.Height - 2 * defaultX;
            myFont = Resources.GetFont(Resources.FontResources.small);
            screen = display;
            screen.SimpleGraphics.BackgroundColor = Gadgeteer.Color.White;
            imageUp = Resources.GetBitmap(Resources.BitmapResources.up);
            imageLeft = Resources.GetBitmap(Resources.BitmapResources.left);
            imageRight = Resources.GetBitmap(Resources.BitmapResources.right);

        }

        public void showMessage(string message){
            screen.SimpleGraphics.Clear();
            screen.SimpleGraphics.DisplayTextInRectangle(message, defaultX, defaultY, defaultWidth, defaultHeight, Gadgeteer.Color.Black, myFont);
        }

        public void showUp(){
            screen.SimpleGraphics.Clear();
            screen.SimpleGraphics.DisplayImage(imageUp, 14, 14);}

        public void showLeft(){
            screen.SimpleGraphics.Clear();
            screen.SimpleGraphics.DisplayImage(imageLeft, 14, 14); }

        public void showRight(){
            screen.SimpleGraphics.Clear();
            screen.SimpleGraphics.DisplayImage(imageRight, 14, 14); }
    }
        
}
