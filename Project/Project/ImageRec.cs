using System;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Microsoft.SPOT;

namespace GadgeteerApp1
{
    class ImageRec {
        const int TIMERINTERVAL = 1000;
        GTM.GHIElectronics.Camera camera;
        public ImageRec(GTM.GHIElectronics.Camera camera) { 

            GT.Timer timer = new GT.Timer(TIMERINTERVAL);
            timer.Tick += new GT.Timer.TickEventHandler(timer_Tick);

            this.camera = camera;
            camera.PictureCaptured += new GTM.GHIElectronics.Camera.PictureCapturedEventHandler(camera_PictureCaptured);

        }

        void camera_PictureCaptured(GTM.GHIElectronics.Camera sender, GT.Picture picture)
        {
            Bitmap bmp = picture.MakeBitmap();


            if (bmp.GetPixel(1, 1) == GT.Color.Black && bmp.GetPixel(159,1) == GT.Color.Black)
            {
                // do something
            }
        }

        void timer_Tick(GT.Timer timer)
        {
            camera.TakePicture();
        }
    }
}
