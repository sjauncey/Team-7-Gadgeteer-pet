using System;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Microsoft.SPOT;

namespace GadgeteerApp1
{
    class ImageRec {
        GTM.GHIElectronics.Camera camera;
        public ImageRec(GTM.GHIElectronics.Camera camera) { 

            GT.Timer timer = new GT.Timer(1000);
            timer.Tick += new GT.Timer.TickEventHandler(timer_Tick);

            this.camera = camera;
            camera.PictureCaptured += new GTM.GHIElectronics.Camera.PictureCapturedEventHandler(camera_PictureCaptured);

        }

        void camera_PictureCaptured(GTM.GHIElectronics.Camera sender, GT.Picture picture)
        {
            Bitmap bmp = picture.MakeBitmap();

            
            if (method1(bmp)) { Debug2.Instance.Print("method1 says yes"); }
            if (method2(bmp)) { Debug2.Instance.Print("method2 says yes"); }


        }

        bool method1(Bitmap bmp) {
            // test one pixel in each top corner only
            if (isBlack(bmp.GetPixel(1, 1)) && isBlack(bmp.GetPixel(159, 1)))
            { return true; }
            else { return false; }
        }

        bool method2(Bitmap bmp)
        {
            // test a group of four pixels in each top corner and the top centre (and average each set)
            Single[] left = new Single[3]; Single[] right = new Single[3]; Single[] centre = new Single[3];

            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < 2; j++) {
                    // sum reds
                    left[0] = ((GT.Color)(bmp.GetPixel(1 + i, 1 + j))).R;
                    centre[0] = ((GT.Color)(bmp.GetPixel(79 + i, 1 + j))).R;
                    right[0] = ((GT.Color)(bmp.GetPixel(159 + i, 1 + j))).R;

                    // sum greens
                    left[1] = ((GT.Color)(bmp.GetPixel(1 + i, 1 + j))).G;
                    centre[1] = ((GT.Color)(bmp.GetPixel(79 + i, 1 + j))).G;
                    right[1] = ((GT.Color)(bmp.GetPixel(159 + i, 1 + j))).G;

                    // sum blues
                    left[2] = ((GT.Color)(bmp.GetPixel(1 + i, 1 + j))).B;
                    centre[2] = ((GT.Color)(bmp.GetPixel(79 + i, 1 + j))).B;
                    right[2] = ((GT.Color)(bmp.GetPixel(159 + i, 1 + j))).B;

                }
              }

            // average
            for (int k = 0; k < 3; k++) {
                left[k] = left[k] / 3;
                centre[k] = centre[k] / 3;
                right[k] = right[k] / 3;
            }

            if (isBlack(left) && isBlack(centre) && isBlack(right)) { return true; } else { return false; }
                                  
        }


        bool isBlack(GT.Color col) {
            if (col.B < 20 && col.G < 20 && col.R < 20) {
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

        void timer_Tick(GT.Timer timer)
        {
            camera.TakePicture();
        }
    }
}
