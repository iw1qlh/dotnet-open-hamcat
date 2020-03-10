using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HamCat
{
    public class CatHamlib : CatTcpBase
    {
        public CatHamlib()
        {
            endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4532);
        }

        private string Write(string s)
        {
            string returndata = null;
            byte[] sendBytes;

            sendBytes = Encoding.UTF7.GetBytes(s + "\n");
            netStream.Write(sendBytes, 0, sendBytes.Length);
            netStream.Flush();

            if (netStream.DataAvailable)
            {
                byte[] bytes = new byte[tcpClient.Available];
                netStream.Read(bytes, 0, (int)tcpClient.Available);
                returndata = Encoding.UTF8.GetString(bytes);
            }

            return returndata;

        }

        public override void TX()
        {
            Console.WriteLine("TX - Not implemented");
        }

        public override void RX()
        {
            Console.WriteLine("RX - Not implemented");
        }

        public override void LowPower()
        {
            Console.WriteLine("LowPower - Not implemented");
        }

        public override void DisableATU()
        {
            Console.WriteLine("DisableATU - Not implemented");
        }

        public override void SetFrequency(int freq)
        {
            Write(string.Format("F{0}", freq));
        }

        public override void SetMode(Modes mode)
        {
            Console.WriteLine("SetMode - Not implemented");
        }

        public override void AskSwr()
        {
            Console.WriteLine("AskSwr - Not implemented");
        }

        public override void SendRaw(byte[] buff)
        {
            Console.WriteLine("SendRaw - Not implemented");
        }

        public override void SendRaw(string text)
        {
            Console.WriteLine("SendRaw - Not implemented");
        }

        public override void AskFrequency()
        {
            Console.WriteLine("AskFrequency - Not implemented");
        }
    }
}
