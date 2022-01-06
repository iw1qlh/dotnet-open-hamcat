using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HamCat
{

    // https://github.com/Hamlib/Hamlib/wiki

    // Occorre lanciare prima rigctld con parametri
    // TS-590 --> rigctld -m 2031 -r com3 -s 115200

    public class CatHamlib : CatTcpBase
    {
        public CatHamlib()
        {
            endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4532);
        }

        protected override string GetErrorMessage(Exception ex)
        {
            if (Process.GetProcesses().FirstOrDefault(w => w.ProcessName == "rigctld") == null)
                return "You must run rigctld before!";
            return ex.Message;
        }

        private string Write(string s)
        {
            byte[] sendBytes;

            if (netStream == null)
                return "";

            string returndata = null;

            try
            {

                sendBytes = Encoding.UTF7.GetBytes(s + "\n");
                netStream.Write(sendBytes, 0, sendBytes.Length);
                netStream.Flush();

                DateTime timeOut = DateTime.Now.AddSeconds(1);
                while (timeOut > DateTime.Now)
                {
                    if (netStream.DataAvailable)
                    {
                        int available = tcpClient.Available;
                        byte[] bytes = new byte[available];
                        netStream.Read(bytes, 0, available);
                        returndata = Encoding.UTF8.GetString(bytes);
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                onError(ex.Message);
            }

            return returndata;

        }

        public override void TX()
        { 
            Write("T 1");
        }

        public override void RX()
        {
            Write("T 0");
        }

        public override void LowPower()
        {
            Write("L RFPOWER 0.1");
        }

        public override void HighPower()
        {
            Write("L RFPOWER 1.0");
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
            switch (mode)
            {
                case Modes.LSB:
                    Write("M LSB");
                    break;
                case Modes.USB:
                    Write("M USB");
                    break;
                case Modes.CW:
                    Write("M CW");
                    break;
                case Modes.AM:
                    Write("M AM");
                    break;
                case Modes.FM:
                    Write("M FM");
                    break;
                case Modes.DIG:
                    Write("M PKTLSB");      // PSK ?
                    break;

            }
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
            int nFreq;
            string reply = Write("f");
            if (int.TryParse(reply, out nFreq) && (freq != nFreq))
            {
                freq = nFreq;
                onChange(Values.FREQ, reply);
            }
        }

        public override void OpenSettings()
        {
            throw new NotImplementedException();
        }

    }
}
