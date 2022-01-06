using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HamCat
{
    public class CatYaesu : CatSerialBase
    {

        public CatYaesu(string config): base (config)
        {
        }

        protected override void DataReceived(byte[] buff)
        {
            foreach (byte b in buff)
                Console.Write(string.Format("{0:X}", b));
            Console.WriteLine();
        }

        private void SetValue(string p, int startIndex, int length, Values values, out int swr)
        {
            if (int.TryParse(p.Substring(startIndex, length), out swr))
                onChange(values, p);
        }

        public override void TX()
        {
            ser.WriteArray(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x08});
            Console.WriteLine("TX");
        }

        public override void RX()
        {
            ser.WriteArray(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x88 });
            Console.WriteLine("RX");
        }

        public override void LowPower()
        {
            Console.WriteLine("LowPower - NOT AVAILABLE");
        }

        public override void HighPower()
        {
            Console.WriteLine("HighPower - Not implemented");
        }

        public override void SetMode(Modes mode)
        {
            byte[] buff = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x07 };
            switch (mode)
            {
                case Modes.LSB:
                    buff[0] = 0x00; break;
                case Modes.USB:
                    buff[0] = 0x01; break;
                case Modes.CW:
                    buff[0] = 0x02; break;
                case Modes.AM:
                    buff[0] = 0x04; break;
                case Modes.FM:
                    buff[0] = 0x08; break;
                case Modes.DIG:
                    buff[0] = 0x0A; break;
            }
            ser.Write(buff, 0, buff.Length);
            Console.WriteLine("SetMode");
        }

        public override void DisableATU()
        {
            Console.WriteLine("DisableATU - NOT AVAILABLE");
        }

        public override void SetFrequency(int freq)
        {
            byte[] buff = ToBcd(freq / 10);
            Array.Resize(ref buff, 5);
            buff[4] = 0x01;
            ser.Write(buff, 0, buff.Length);
            Console.WriteLine("SetFrequency");
        }

        public override void AskSwr()
        {
            // http://www.ft897.com/CAT_Commands
            ser.WriteArray(new byte[] { 0x00, 0x00, 0x00, 0x00, 0xBD });
            Console.WriteLine("AskSwr");
        }

        protected byte[] ToBcd(int value)
        {
            if (value < 0 || value > 99999999)
                throw new ArgumentOutOfRangeException("BCD value out of range");
            byte[] ret = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                ret[3 - i] = (byte)(value % 10);
                value /= 10;
                ret[3 - i] |= (byte)((value % 10) << 4);
                value /= 10;
            }
            return ret;
        }

        public override void AskFrequency()
        {
            Console.WriteLine("AskFrequency - Not implemented");
        }

        public override void OpenSettings()
        {
            throw new NotImplementedException();
        }

    }
}
