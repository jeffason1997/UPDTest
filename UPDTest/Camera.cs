using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Drawing;
using System.Timers;

namespace UPDTest
{
    class Camera
    {
        private Webcam camera;
        private bool busy = true;
        public Camera()
        {
            camera = new Webcam(new Size(200, 100), 500000);
            camera.Start();

            Timer aTimer = new Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 400;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (busy)
                busy = false;
            else
                camera.Stop();
        }

        public byte[] getBitArray()
        {
            Image captured_image = null;
            try
            {
                while (captured_image == null) { captured_image = camera.currentImage; }
                busy = true;
                ImageConverter converter = new ImageConverter();
                return (byte[])converter.ConvertTo(captured_image, typeof(byte[]));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error code : " + ex.Message);
                return null;
            }
        }
    }

    class Webcam
    {
        private FilterInfoCollection videoDevices = null; //list of all videosources connected to the pc
        private VideoCaptureDevice videoSource = null; //the selected videosource
        private Size frameSize;
        private int frameRate;

        public Bitmap currentImage; //parameter accessible to outside world to capture the current image

        public Webcam(Size framesize, int framerate)
        {

            this.frameSize = framesize;
            this.frameRate = framerate;
            this.currentImage = null;
        }

        // get the devices names cconnected to the pc
        private FilterInfoCollection getCamList()
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            return videoDevices;
        }

        //start the camera
        public void Start()
        {
            //raise an exception incase no video device is found
            //or else initialise the videosource variable with the harware device
            //and other desired parameters.
            if (getCamList().Count == 0)
            {
                throw new Exception("Video device not found");
            }
            else
            {
                videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
                videoSource.DesiredFrameSize = this.frameSize;
                videoSource.DesiredFrameRate = this.frameRate;
                videoSource.Start();
            }
        }

        //dummy method required for Image.GetThumbnailImage() method
        private bool imageconvertcallback()
        {
            return false;
        }

        //eventhandler if new frame is ready
        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            this.currentImage = (Bitmap)eventArgs.Frame.GetThumbnailImage(frameSize.Width, frameSize.Height,
                new Image.GetThumbnailImageAbort(imageconvertcallback), IntPtr.Zero);
        }

        //close the device safely
        public void Stop()
        {
            if (!(videoSource == null))
            {
                if (videoSource.IsRunning)
                {
                    videoSource.SignalToStop();
                    videoSource = null;
                }
            }
        }
    }
}
