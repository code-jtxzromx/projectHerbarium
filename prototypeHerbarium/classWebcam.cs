using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using WebCam_Capture;

namespace prototypeHerbarium
{
    class WebCam
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr handle);
        public static IntPtr ptr;

        private WebCamCapture camera;
        private Image _FrameImage;
        int frames = 30;

        public void InitializeCamera(ref Image PictureBox)
        {
            camera = new WebCamCapture();
            camera.FrameNumber = ((ulong)(0ul));
            camera.TimeToCapture_milliseconds = frames;
            camera.ImageCaptured += new WebCamCapture.WebCamEventHandler(webcam_ImageCaptured);
            _FrameImage = PictureBox;
        }

        void webcam_ImageCaptured(object source, WebcamEventArgs e)
        {
            System.Drawing.Bitmap image = e.WebCamImage as System.Drawing.Bitmap;
            ptr = image.GetHbitmap();
            _FrameImage.Source = Imaging.CreateBitmapSourceFromHBitmap(ptr, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            DeleteObject(ptr);
        }

        public void Start()
        {
            camera.TimeToCapture_milliseconds = frames;
            camera.Start(0);
        }

        public void Continue()
        {
            camera.TimeToCapture_milliseconds = frames;
            camera.Start(this.camera.FrameNumber);
        }

        public void Stop() => camera.Stop();
        public void ResolutionSetting() => camera.Config();
        public void AdvanceSetting() => camera.Config2();

    }
}
