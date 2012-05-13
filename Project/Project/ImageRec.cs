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
            if (method2(stream)) { program.aboveWall(); } else { program.aboveSpace(); }
        }


        void camera_BitmapStreamed(Camera sender, Bitmap bitmap)
        {
            //Debug2.Instance.displayImage(stream);
        }

        void test(GT.Timer timer)
        {
            if (method2(stream)) { program.aboveWall(); } else { program.aboveSpace(); }
        }

        bool method2(Bitmap bmp)
        {
            // test a group of four pixels in each top corner and the top centre (and average each set)
            Single[] left = new Single[3]; Single[] right = new Single[3]; Single[] centre = new Single[3];

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    // sum reds
                    left[0] = ((GT.Color)(bmp.GetPixel(1 + i, 1 + j))).R;
                    centre[0] = ((GT.Color)(bmp.GetPixel(1 + j, 59 + i))).R;
                    right[0] = ((GT.Color)(bmp.GetPixel(1 + j, 119 + i))).R;

                    // sum greens
                    left[1] = ((GT.Color)(bmp.GetPixel(1 + i, 1 + j))).G;
                    centre[1] = ((GT.Color)(bmp.GetPixel(1 + j, 59 + i))).G;
                    right[1] = ((GT.Color)(bmp.GetPixel(1 + j, 119 + i))).G;

                    // sum blues
                    left[2] = ((GT.Color)(bmp.GetPixel(1 + i, 1 + j))).B;
                    centre[2] = ((GT.Color)(bmp.GetPixel(1 + j, 59 + i))).B;
                    right[2] = ((GT.Color)(bmp.GetPixel(1 + j, 119 + i))).B;

                }
            }

            // average
            for (int k = 0; k < 3; k++)
            {
                left[k] = left[k] / 3;
                centre[k] = centre[k] / 3;
                right[k] = right[k] / 3;
            }

            if (isBlack(left) && isBlack(centre) && isBlack(right)) { return true; } else { return false; }

        }


<<<<<<< HEAD
        bool isBlack(GT.Color col) {
            if (col.B < 30 && col.G < 30 && col.R < 30) {
=======
        bool isBlack(GT.Color col)
        {
            if (col.B < 20 && col.G < 20 && col.R < 20)
            {
>>>>>>> 6764872532ff5765022e9ae1fac4b815c4a098b0
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
