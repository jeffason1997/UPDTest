﻿using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace UPDTest
{
    class Camera
    {
        public byte[] getBitArray()
        {
            Webcam camera = new Webcam(new Size(320, 240), 30);
            Image captured_image = null;
            int counter = 0;
            try
            {
                camera.Start();
                Application.DoEvents();
                do
                {
                    Thread.Sleep(10);
                    captured_image = camera.currentImage;
                    if (captured_image == null)
                    {
                        throw new Exception("Device time-out");
                    }
                    else
                    {
                        byte[] map = null;
                        //make bitmap
                        return map;
                    }
                    counter++;
                } while (captured_image == null && counter <= 10000);
                camera.Stop();

            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Error code 1 : Please specify two valid file-paths");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error code 2 : " + ex.Message);
            }
            finally
            {
                Application.Exit();
            }
            return null;
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
}
