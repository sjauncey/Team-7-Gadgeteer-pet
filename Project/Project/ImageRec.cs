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
            if (testForWall(stream)==1) { program.aboveWall(); } else { program.aboveSpace(); }
        }


        void camera_BitmapStreamed(Camera sender, Bitmap bitmap)
        {
            //Debug2.Instance.displayImage(stream);
        }

        void test(GT.Timer timer)
        {
            if (testForWall(stream) == 1) { program.aboveWall(); } else { program.aboveSpace(); }
        }

        // tests for walls and the finish cell - returns 0 for clear, 1 for wall, 2 for finish cell
        int testForWall(Bitmap bmp)
        {
            // test a group of four pixels in each top corner and the top centre (and average each set)
            Single[] left = new Single[3]; Single[] right = new Single[3]; Single[] centre = new Single[3];
            GT.Color leftPixel, centrePixel, rightPixel;

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++) {
                    leftPixel = bmp.GetPixel(10+j,1+i);
                    centrePixel = bmp.GetPixel(1+j,59+i);
                    rightPixel = bmp.GetPixel(10+j,119+i);
                    
                    // sum reds
                    left[0] = leftPixel.R;
                    centre[0] = centrePixel.R;
                    right[0] = rightPixel.R;

                    // sum greens
                    left[1] = leftPixel.G;
                    centre[1] = centrePixel.G;
                    right[1] = rightPixel.G;

                    // sum blues
                    left[2] = leftPixel.B;
                    centre[2] = centrePixel.B;
                    right[2] = rightPixel.B;

                }
            }

            // average
            for (int k = 0; k < 3; k++)
            {
                left[k] = left[k] / 3;
                centre[k] = centre[k] / 3;
                right[k] = right[k] / 3;
            }

            if (isBlack(left) && isBlack(centre) && isBlack(right)) { return 1; }
            else if (isGreen(left) && isGreen(centre) && isGreen(right)) { return 2; }
            else { return 0; }

        }

        bool isGreen(Single[] cols)
        {
            if (cols[0] < 100 && cols[1] < 100 && cols[2] > 200) { return true; }
            else { return false; }
        }

        bool isBlack(GT.Color col) {
            if (col.B < 30 && col.G < 30 && col.R < 30) {
                return true;
            }
            else { return false; }
        }

        bool isBlack(Single[] cols)
        {
            if (cols[0] < 20 && cols[1] < 20 && cols[2] < 20)
            {
                return true;
            }
            else { return false; }
        }

    }
}
