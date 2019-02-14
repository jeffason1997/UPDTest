using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UPDTest

{
    class Program
    {
        static BitmapShowForm form;
        private static string ip = "192.168.43.88";
        private static int port = 8080;

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

            while (true)
            {
                var remoteEP = new IPEndPoint(IPAddress.Parse(ip), port);
                byte[] pic = cam.getBitArray();
                udpServer.Send(pic, pic.Length, remoteEP);
                Console.WriteLine("send");
                Thread.Sleep(1);
            }

        }

        static void receiver()
        {
            UdpClient server = new UdpClient(port);
            server.Client.ReceiveTimeout = 50;
            while (true)
            {
                var remoteEP = new IPEndPoint(IPAddress.Any, port);
                try
                {
                    var data = server.Receive(ref remoteEP);
                    ParseServerData(data);
                }
                catch (SocketException e)
                {
                }
            }
        }

        static void ParseServerData(byte[] data)
        {
            //Console.WriteLine(Encoding.ASCII.GetString(data));
            Bitmap bmp;
            using (var ms = new MemoryStream(data))
            {
                bmp = new Bitmap(ms);
                form.ShowBitmap(bmp);
            }
        }

    }


}
