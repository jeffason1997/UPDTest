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
        static List<IPAddress> addresses = new List<IPAddress>();

        static void Main(string[] args)
        {


            //Thread thread = startSenderThread("192.168.43.88", 8080);
            form = new BitmapShowForm();
            new Thread(() => { form.ShowDialog(); }).Start();

            Thread thread = StartUPDServer(8080);
            thread.Start();
            Console.ReadKey();
            Console.WriteLine("\nthread stopped");
            thread.Abort();
            Console.ReadKey();
        }

        static Thread startSenderThread(string ip, int port)
        {
            return new Thread(() => { sender(ip, port); });
        }

        static Thread StartUPDServer(int port)
        {
            return new Thread(() => { receiver(port); });
        }


        static void sender(string ip, int port)
        {
            Camera cam = new Camera();
            UdpClient udpServer = new UdpClient(port);

            while (true)
            {
                var remoteEP = new IPEndPoint(IPAddress.Parse(ip), port);
                byte[] pic = cam.getBitArray();
                udpServer.Send(pic, pic.Length, remoteEP); // if data is received reply letting the client know that we got his data    
                Thread.Sleep(100);
            }
        }

        static void receiver(int port)
        {
            UdpClient server = new UdpClient(port);
            server.Client.ReceiveTimeout = 50;
            while (true)
            {
                var remoteEP = new IPEndPoint(IPAddress.Any, port);
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
            //Console.WriteLine("Data received");
            if(!addresses.Contains(ipAddres))
            {
                addresses.Add(ipAddres);
            }
            Bitmap bmp;
            using (var ms = new MemoryStream(data))
            {
                bmp = new Bitmap(ms);
                int index = addresses.FindIndex(a => a.Equals(ipAddres));
                form.ShowBitmap(bmp, index);
            }
        }

    }


}
