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

            
            if (isBlack(bmp.GetPixel(1, 1)) && isBlack(bmp.GetPixel(159, 1)))
            {
                Debug2.Instance.Print("Yes, black");
            }
            else
            {
                Debug2.Instance.Print("No, not black");
            }
        }

        bool isBlack(GT.Color col) {
            if (col.B < 20 && col.G < 20 && col.R < 20) {
                return true;
            }
            else { return false; }
        }

        void timer_Tick(GT.Timer timer)
        {
            camera.TakePicture();
        }
    }
}
