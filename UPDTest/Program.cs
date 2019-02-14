using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace UPDTest

{
    class Program
    {
        static BitmapShowForm form;

        static void Main(string[] args)
        {
            // Thread thread = startSenderThread("192.168.43.88", 8080);
            form = new BitmapShowForm();
            new Thread(() => { form.ShowDialog(); }).Start();

            Thread thread = StartUPDServer();
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

        static Thread StartUPDServer()
        {
            return new Thread(() => { StartServer(); });
        }


        static void sender(string ip, int port)
        {

            UdpClient udpServer = new UdpClient(port);

            while (true)
            {
                var remoteEP = new IPEndPoint(IPAddress.Parse(ip), port);
                string text = "hello";
                byte[] send_buffer = Encoding.ASCII.GetBytes(text);
                udpServer.Send(send_buffer, send_buffer.Length, remoteEP); // if data is received reply letting the client know that we got his data    
                Thread.Sleep(100);
            }
        }

        static void StartServer()
        {
            UdpClient server = new UdpClient(8080);
            server.Client.ReceiveTimeout = 50;
            while (true)
            {
                var remoteEP = new IPEndPoint(IPAddress.Any, 8080);
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
