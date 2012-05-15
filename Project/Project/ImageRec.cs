using System;
using System.Threading;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;
using Microsoft.SPOT;

namespace GadgeteerApp1
{
    class ImageRec
    {
        GTM.GHIElectronics.Camera camera;
        Bitmap stream = new Bitmap(160, 120);
        GT.Timer tmr;
        Program program;

        public ImageRec(GTM.GHIElectronics.Camera camera, Program program)
        {
            this.program = program;
            camera.CurrentPictureResolution = GTM.GHIElectronics.Camera.PictureResolution.Resolution160x120;
            camera.BitmapStreamed += new Camera.BitmapStreamedEventHandler(camera_BitmapStreamed);

            this.camera = camera;
            Thread.Sleep(1000);
            camera.StartStreamingBitmaps(stream);

            tmr = new GT.Timer(100);
            tmr.Tick += new GT.Timer.TickEventHandler(test);
        }

        public void startContinuousChecking()
        {
            tmr.Start();
        }
        public void stopContinuousChecking()
        {
            tmr.Stop();
        }

        public void testCurrentLocation(){
            test(null);
        }


        void camera_BitmapStreamed(Camera sender, Bitmap bitmap)
        {
            //Debug2.Instance.displayImage(stream);
        }

        void test(GT.Timer timer)
        {
            switch (testForWall(stream))
            {
                case 1:
                    program.aboveWall();
                    break;
                case 0:
                    program.aboveSpace();
                    break;
                case 2:
                    program.aboveTarget();
                    break;
            }
        }

        // tests for walls and the finish cell - returns 0 for clear, 1 for wall, 2 for finish cell
        int ttestForWall(Bitmap bmp)
        {
            // test a group of four pixels in each top corner and the top centre (and average each set)
            Single[] left = new Single[3]; Single[] right = new Single[3]; Single[] centre = new Single[3];
            GT.Color leftPixel, centrePixel, rightPixel;

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++) {
                    //pixel indexing starts at 0, up to 119
                    leftPixel = bmp.GetPixel(10+j,0+i);
                    centrePixel = bmp.GetPixel(1+j,59+i);
                    rightPixel = bmp.GetPixel(10+j,118+i);
                    
                    // sum reds
                    left[0] += leftPixel.R;
                    centre[0] += centrePixel.R;
                    right[0] += rightPixel.R;

                    // sum greens
                    left[1] += leftPixel.G;
                    centre[1] += centrePixel.G;
                    right[1] += rightPixel.G;
                    
                    // sum blues
                    left[2] += leftPixel.B;
                    centre[2] += centrePixel.B;
                    right[2] += rightPixel.B;

                }
            }

            // average
            for (int k = 0; k < 3; k++)
            {
                left[k] = left[k] / 4;
                centre[k] = centre[k] / 4;
                right[k] = right[k] / 4;

            }
           // Debug2.Instance.Print(left[0].ToString() + " " + left[1].ToString() + " " + left[2].ToString());
            Debug2.Instance.Print(centre[0].ToString() + " " + centre[1].ToString() + " " + centre[2].ToString());
           // Debug2.Instance.Print(right[0].ToString() + " " + right[1].ToString() + " " + right[2].ToString());
            if (/*isBlue(left) && isBlue(right)*/ isBlue(centre)) { return 1; }
            else if (/*isRed(left) && isRed(right) &&*/ isRed(centre)) { return 2; }
            else { return 0; }

        }

        // tests for walls and the finish cell - returns 0 for clear, 1 for wall, 2 for finish cell
        int testForWall(Bitmap bmp)
        {
            Single[] centre = new Single[3];
            GT.Color centrePixel;

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {

                    centrePixel = bmp.GetPixel(58 + j, 58 + i);

                    centre[0] += centrePixel.R;

                    centre[1] += centrePixel.G;

                    centre[2] += centrePixel.B;


                }
            }

            // average
            for (int k = 0; k < 3; k++)
            {
                centre[k] = centre[k] / 25;

            }
            Debug2.Instance.Print(centre[0].ToString() + " " + centre[1].ToString() + " " + centre[2].ToString());
            if (isBlack(centre)) { return 1; }
            if (isRed(centre)) { return 2; }
            else { return 0; }

        }

        bool isGreen(Single[] cols)
        {
            if (cols[0] < 100 && cols[1] < 100 && cols[2] > 200) { return true; }
            else { return false; }
        }

        bool isBlue(Single[] cols)
        {
            if (cols[2] > 160 && cols[2] - cols[1] > 30 && cols[2] - cols[0] > 30) { return true; }
            else { return false; }
        }

        bool isRed(Single[] cols)
        {

            //if (cols[0] > 215 && cols[1] < 180 && cols[2] < 180) { return true; }
            //30+ difference between red each of the other colors should give a strong indication of a red hint
            if (cols[0] > 160 && cols[0] - cols[1] > 30 && cols[0] - cols[2] > 30) { return true; }
            else { return false; }
        }

        bool isBlack(Single[] cols)
        {
            //Debug2.Instance.Print(cols[0].ToString() + " " + cols[1].ToString() + " " + cols[2].ToString());
            if (cols[0] < 120 && cols[1] < 120 && cols[2] < 120)
            {
                return true;
            }
            else { return false; }
        }

    }
}
