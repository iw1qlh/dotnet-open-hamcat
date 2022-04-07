using AnyRigLibrary;
using AnyRigLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamCat
{
    public class CatAnyRig : CatBase
    {
        const int RIG_NO = 0;

        IAnyRigEngine engine;
        RigCore rig;

        public CatAnyRig()
        {
            engine = new AnyRigEngine();
            IRigCore irig = engine.GetRig(0);

            if (irig != null)
            {
                rig = irig as RigCore;
                rig.NotifyChanges = (rx, changed) => OnChanges(rx, changed);
            }

        }

        private void ReadCat()
        {
            if (rig == null)
                return;

            long nFreq = rig.Freq;
            if (freq != nFreq)
            {
                freq = nFreq;
                onChange(Values.FREQ, rig.StatusStr);
            }

            Modes nMode = Modes.UNKNOW;
            switch (rig.Mode)
            {
                case RigParam.CW: nMode = Modes.CW; break;
                case RigParam.CWR: nMode = Modes.CW; break;
                case RigParam.LSB: nMode = Modes.LSB; break;
                case RigParam.USB: nMode = Modes.USB; break;
                case RigParam.FM: nMode = Modes.FM; break;
                case RigParam.AM: nMode = Modes.AM; break;
                default:
                    Console.WriteLine("AnyRig - Unknow mode");
                    break;
            }
            if (nMode != mode)
            {
                mode = nMode;
                onChange(Values.MODE, rig.StatusStr);
            }

            bool nTx = rig.Tx.GetValueOrDefault();
            if (nTx != tx)
            {
                tx = nTx;
                onChange(Values.RXTX, rig.StatusStr);
            }

        }

        private void OnChanges(int rx, RigParam[] changed)
        {
            if (rx == RIG_NO)
                ReadCat();
        }


        public override void Open()
        {
            rig?.Start();
        }

        public override void Close()
        {
            rig?.Stop();
        }

        public override void AskFrequency()
        {
            ReadCat();
        }

        public override void SetFrequency(long freq)
        {
            rig.Freq = freq;
        }

        public override void SetMode(Modes mode)
        {
            switch (mode)
            {
                case Modes.AM: rig.Mode = RigParam.AM; break;
                case Modes.CW: rig.Mode = RigParam.CW; break;
                case Modes.DIG: rig.Mode = RigParam.DIG; break;
                case Modes.FM: rig.Mode = RigParam.FM; break;
                case Modes.LSB: rig.Mode = RigParam.LSB; break;
                case Modes.USB: rig.Mode = RigParam.USB; break;
            }
        }

        public override void RX()
        {
            rig.Tx = false;
        }

        public override void TX()
        {
            rig.Tx = true;
        }

        public override void AskSwr()
        {
            throw new NotImplementedException();
        }


        public override void DisableATU()
        {
            throw new NotImplementedException();
        }

        public override void HighPower()
        {
            throw new NotImplementedException();
        }

        public override void LowPower()
        {
            throw new NotImplementedException();
        }


        public override void OpenSettings()
        {
            engine.OpenSettings();
        }

        public void DisableOnAir()
        {
            rig.DisableOnAir();
        }

        public override void SendRaw(byte[] buff)
        {
            throw new NotImplementedException();
        }

        public override void SendRaw(string text)
        {
            throw new NotImplementedException();
        }

    }
}
