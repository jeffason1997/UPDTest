using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;


namespace UPDTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Hello World!");
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }


    public class UdpState
    {
        public UdpClient client;
        public IPEndPoint endpoint;
        public UdpState(UdpClient c, IPEndPoint iep)
        {
            this.client = c;
            this.endpoint = iep;
        }
    }
    public class UdpCom
    {
        public UdpState uState;
        public IPAddress address;
        public int port;
        public IPEndPoint uEndpoint;
        public bool receiveFlag;
        public List<byte[]> rxBytesBuffer;

        public UdpCom(IPAddress address, int port)
        {
            uEndpoint = new IPEndPoint(address, port);
            // uClient = new UdpClient (uEndpoint);
            uState = new UdpState(new UdpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), port)), uEndpoint);
            receiveFlag = false;
            uState.client.BeginReceive(new AsyncCallback(RxCallback), uState);
            rxBytesBuffer = new List<byte[]>();
        }

        public void RxCallback(IAsyncResult result)
        {
            UdpState rState = (UdpState)result.AsyncState;
            UdpClient rClient = ((UdpState)result.AsyncState).client;
            IPEndPoint rEndPoint = ((UdpState)result.AsyncState).endpoint;
            byte[] rxBytes = rClient.EndReceive(result, ref rEndPoint);
            rxBytesBuffer.Add(rxBytes);
            Console.WriteLine("Received Bytes ___________________________");
            Console.WriteLine(rxBytes.ToString());
            receiveFlag = true;
            rClient.BeginReceive(new AsyncCallback(RxCallback), result.AsyncState);
        }
    }
}
