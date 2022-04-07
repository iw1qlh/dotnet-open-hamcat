using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace HamCat
{
    public class CatOmniRig : CatBase
    {
        const int RIG_NO = 1;

        OmniRig.OmniRigX OmniRigEngine;
        OmniRig.RigX Rig;

        public CatOmniRig()
        {
        }

        public override void Open()
        {
            OmniRigEngine = (OmniRig.OmniRigX)Activator.CreateInstance(Type.GetTypeFromProgID("OmniRig.OmniRigX"));
            // we want OmniRig interface V.1.1 to 1.99
            // as V2.0 will likely be incompatible  with 1.x
            if (OmniRigEngine.InterfaceVersion < 0x101 && OmniRigEngine.InterfaceVersion > 0x299)
            {
                OmniRigEngine = null;
                throw new Exception("OmniRig is not installed or has unsupported version.");
            }

            OmniRigEngine.StatusChange += OmniRigEngine_StatusChange;
            OmniRigEngine.ParamsChange += OmniRigEngine_ParamsChange;           

            Rig = OmniRigEngine.Rig1;
            ReadCat();

        }

        public override void Close()
        {
            
            OmniRigEngine.StatusChange -= OmniRigEngine_StatusChange;
            OmniRigEngine.ParamsChange -= OmniRigEngine_ParamsChange;

            Rig = null;
            Marshal.ReleaseComObject(OmniRigEngine);

        }

        private void ReadCat()
        {
            if (Rig == null)
                return;

            int nFreq = Rig.Freq;
            if (freq != nFreq)
            {
                freq = nFreq;
                onChange(Values.FREQ, Rig.StatusStr);
            }

            Modes nMode = Modes.UNKNOW;
            switch (Rig.Mode)
            {
                case OmniRig.RigParamX.PM_CW_L: nMode = Modes.CW; break;
                case OmniRig.RigParamX.PM_CW_U: nMode = Modes.CW; break;
                case OmniRig.RigParamX.PM_SSB_L: nMode = Modes.LSB; break;
                case OmniRig.RigParamX.PM_SSB_U: nMode = Modes.USB; break;
                case OmniRig.RigParamX.PM_FM: nMode = Modes.FM; break;
                case OmniRig.RigParamX.PM_AM: nMode = Modes.AM; break;
                case OmniRig.RigParamX.PM_DIG_L: nMode = Modes.DIG; break;
                default:
                    Console.WriteLine("OmniRig - Unknow mode");
                    break;
            }
            if (nMode != mode)
            {
                mode = nMode;
                onChange(Values.MODE, Rig.StatusStr);
            }

            bool nTx = (Rig.Tx == OmniRig.RigParamX.PM_TX);
            if (nTx != tx)
            {
                tx = nTx;
                onChange(Values.RXTX, Rig.StatusStr);
            }

        }

        private void OmniRigEngine_ParamsChange(int RigNumber, int Params)
        {
            if (RigNumber == RIG_NO)
            {
                ReadCat();
            }
        }

        private void OmniRigEngine_StatusChange(int RigNumber)
        {
            if (RigNumber == RIG_NO)
            {
                ReadCat();
            }
        }

        public override void OpenSettings()
        {
            if (OmniRigEngine != null)
                OmniRigEngine.DialogVisible = true;
        }

        public override void AskFrequency()
        {
            ReadCat();
        }

        public override void SetFrequency(long freq)
        {
            Rig.Freq = (int)freq;
        }

        public override void RX()
        {
            Rig.Tx = OmniRig.RigParamX.PM_RX;
        }

        public override void TX()
        {
            Rig.Tx = OmniRig.RigParamX.PM_TX;
        }

        public override void SetMode(Modes mode)
        {
            switch (mode)
            {
                case Modes.AM: Rig.Mode = OmniRig.RigParamX.PM_AM; break;
                case Modes.CW: Rig.Mode = OmniRig.RigParamX.PM_CW_U; break;
                case Modes.DIG: Rig.Mode = OmniRig.RigParamX.PM_DIG_L; break;
                case Modes.FM: Rig.Mode = OmniRig.RigParamX.PM_FM; break;
                case Modes.LSB: Rig.Mode = OmniRig.RigParamX.PM_SSB_L; break;
                case Modes.USB: Rig.Mode = OmniRig.RigParamX.PM_SSB_U; break;
            }
        }

        public override void AskSwr()
        {
            throw new NotImplementedException();
        }


        public override void DisableATU()
        {
            throw new NotImplementedException();
        }

        public override void LowPower()
        {
            throw new NotImplementedException();
        }

        public override void HighPower()
        {
            Console.WriteLine("HighPower - Not implemented");
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
