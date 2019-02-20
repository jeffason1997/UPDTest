using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

namespace UPDTest

{
    class Program
    {
        static BitmapShowForm form;
        private static string ip = "192.168.178.50";
        private static int port = 8080;
        static List<IPAddress> addresses = new List<IPAddress>();

        static void Main(string[] args)
        {
            Thread thread = UDPClientThread();
            //Thread thread = UPDServerThread();
            thread.Start();
            Console.ReadKey();
            Console.WriteLine("\nthread stopped");
            thread.Abort();
            Console.ReadKey();
        }

        static Thread UDPClientThread()
        {
            return new Thread(() => { StartClient(); });
        }

        static Thread UPDServerThread()
        {
            form = new BitmapShowForm();
            new Thread(() => { form.ShowDialog(); }).Start();
            return new Thread(() => { StartServer(); });
        }


        static void StartClient()
        {
            Camera cam = new Camera();
            UdpClient udpServer = new UdpClient(port);
            var remoteEP = new IPEndPoint(IPAddress.Parse(ip), port);
            while (true)
            {
                byte[] pic = cam.getBitArray();
                udpServer.Send(pic, pic.Length, remoteEP);
                Thread.Sleep(15);
            }
        }

        static void StartServer()
        {
            UdpClient server = new UdpClient(port);
            server.Client.ReceiveTimeout = 300;
            var remoteEP = new IPEndPoint(IPAddress.Any, port);
            while (true)
            {
                try
                {
                    var data = server.Receive(ref remoteEP);
                    Bitmap bmp;
                    using (var ms = new MemoryStream(data))
                    {
                        bmp = new Bitmap(ms);
                        form.ShowBitmap(bmp, 0);
                    }
                }
                catch (SocketException e)
                {
                    //Dit betekent dat er een timeout heeft plaats gevonden. 
                    //Dit heeft geen invloed op de rest van de werking van de server.
                }
            }
        }
    }
}
