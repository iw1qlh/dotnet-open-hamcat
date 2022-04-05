using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HamCat
{
    public class CatKenwood : CatSerialBase
    {
        private class ModeConvert
        {
            public int KenwoodMode;
            public string KenwoodCmd;
            public Modes Mode;
        }

        static readonly ModeConvert[] KenwoodModes = {
            new ModeConvert{Mode= Modes.LSB, KenwoodCmd = "MD1;", KenwoodMode = 1 },
            new ModeConvert{Mode= Modes.USB, KenwoodCmd = "MD2;", KenwoodMode = 2 },
            new ModeConvert{Mode= Modes.CW, KenwoodCmd = "MD3;", KenwoodMode = 3 },
            new ModeConvert{Mode= Modes.FM, KenwoodCmd = "MD4;", KenwoodMode = 4 },
            new ModeConvert{Mode= Modes.AM, KenwoodCmd = "MD5;", KenwoodMode = 5 },
            new ModeConvert{Mode= Modes.DIG, KenwoodCmd = "MD6;", KenwoodMode = 6 }  // FSK
        };

        public CatKenwood(string config) : base(config)
        {
            ser.RtsEnable = true;
        }

        protected override void DataReceived(byte[] buff)
        {
            string rxSer = Encoding.ASCII.GetString(buff);
            Console.WriteLine(rxSer);
            string[] data = rxSer.Split(';');
            foreach (string rx in data)
            {
                if ((rx == "?") || (rx == "E") || (rx == "O"))
                    onError(rx);
                else if (rx.StartsWith("RM1"))
                    SetSwr(rx, 3, 4);
                else if (rx.StartsWith("RM2"))
                    SetValue(rx, 3, 4, Values.COMP, ref comp);
                else if (rx.StartsWith("RM3"))
                    SetValue(rx, 3, 4, Values.ALC, ref alc);
                else if (rx.StartsWith("FA"))
                    SetValue(rx, 2, 11, Values.FREQ, ref freq);
                else if (rx.StartsWith("IF"))
                {
                    SetValue(rx, 2, 11, Values.FREQ, ref freq);
                    SetRxTx(rx, 28);
                    SetMode(rx, 29);
                }
            }
        }

        private void SetRxTx(string p, int startIndex)
        {
            int n;
            bool nRxTx;

            if (int.TryParse(p[startIndex].ToString(), out n))
            {
                nRxTx = (n == 1);
                if (nRxTx != tx)
                {
                    tx = nRxTx;
                    onChange(Values.RXTX, p);
                }
            }
        }


        private void SetMode(string p, int startIndex)
        {
            int nMode;

            if (int.TryParse(p[startIndex].ToString(), out nMode))
            {
                ModeConvert mc = KenwoodModes.FirstOrDefault(w => w.KenwoodMode == nMode);
                if ((mc != null) && (mc.Mode != mode))
                {
                    mode = mc.Mode;
                    onChange(Values.MODE, p);
                }
            }
        }

        private void SetSwr(string p, int startIndex, int length)
        {
            if (!tx)
                return;

            int n;
            double nSwr;
            if (int.TryParse(p.Substring(startIndex, length), out n))
            {
                nSwr = Math.Pow(10, n / 28.0);
                if (nSwr != swr)
                {
                    swr = nSwr;
                    onChange(Values.SWR, p);
                }
            }
        }

        private void SetValue(string p, int startIndex, int length, Values values, ref int value)
        {
            int nValue;
            if (int.TryParse(p.Substring(startIndex, length), out nValue) && (nValue != value))
            {
                value = nValue;
                onChange(values, p);
            }
        }

        private void SetValue(string p, int startIndex, int length, Values values, ref long value)
        {
            long nValue;
            if (long.TryParse(p.Substring(startIndex, length), out nValue) && (nValue != value))
            {
                value = nValue;
                onChange(values, p);
            }
        }


        public override void TX()
        {
            ser.Write("TX1;");
            Console.WriteLine("TX");
        }

        public override void RX()
        {
            ser.Write("RX;");
            Console.WriteLine("RX");
        }

        public override void LowPower()
        {
            ser.Write("PC005;");
            Console.WriteLine("LowPower");
        }

        public override void HighPower()
        {
            ser.Write("PC200;");
            Console.WriteLine("HighPower");
        }


        public override void SetMode(Modes mode)
        {
            ModeConvert mc = KenwoodModes.FirstOrDefault(w => w.Mode == mode);
            if (mc != null)
            {
                ser.Write(mc.KenwoodCmd);
                Console.WriteLine("SetMode");
            }
        }

        public override void DisableATU()
        {
            ser.Write("AC000;");
            Console.WriteLine("DisableATU");
        }

        public override void SetFrequency(long freq)
        {
            ser.Write(string.Format("FA{0:D11};", freq));
            Console.WriteLine("SetFrequency");
        }

        public override void AskSwr()
        {
            ser.Write("RM;");
            Console.WriteLine("AskSwr");
        }

        public override void AskFrequency()
        {
            ser.Write("IF;");
            Console.WriteLine("AskFrequency");
        }

        public override void OpenSettings()
        {
            throw new NotImplementedException();
        }


    }
}
