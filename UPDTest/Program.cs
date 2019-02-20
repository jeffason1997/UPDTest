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
            Thread thread = startSenderThread();
            //Thread thread = StartUPDServer();
            thread.Start();
            Console.ReadKey();
            Console.WriteLine("\nthread stopped");
            thread.Abort();
            Console.ReadKey();
        }

        static Thread startSenderThread()
        {
            return new Thread(() => { sender(); });
        }

        static Thread StartUPDServer()
        {
            form = new BitmapShowForm();
            new Thread(() => { form.ShowDialog(); }).Start();

            return new Thread(() => { receiver(); });
        }


        static void sender()
        {
            Camera cam = new Camera();
            UdpClient udpServer = new UdpClient(port);
            var remoteEP = new IPEndPoint(IPAddress.Parse(ip), port);
            while (true)
            {
                byte[] pic = cam.getBitArray();
                udpServer.Send(pic, pic.Length, remoteEP);
                //Console.WriteLine("send");
                Thread.Sleep(1);
            }
        }

        static void receiver()
        {
            UdpClient server = new UdpClient(port);
            server.Client.ReceiveTimeout = 300;
            var remoteEP = new IPEndPoint(IPAddress.Any, port);
            while (true)
            {
                try
                {
                    var data = server.Receive(ref remoteEP);
                    ParseServerData(data, remoteEP.Address);
                }
                catch (SocketException e)
                {
                }
            }
        }

        static void ParseServerData(byte[] data, IPAddress ipAddres)
        {
            ////Console.WriteLine("Data received");
            //if(!addresses.Contains(ipAddres))
            //{
            //    addresses.Add(ipAddres);
            //}
            Bitmap bmp;
            using (var ms = new MemoryStream(data))
            {
                bmp = new Bitmap(ms);
                //int index = addresses.FindIndex(a => a.Equals(ipAddres));
                form.ShowBitmap(bmp, 0);
            }
        }

    }


}
