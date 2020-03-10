using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HamCat
{
    public abstract class CatSerialBase : CatBase
    {

        protected Serial ser = null;
        public Serial Serial
        { get { return ser; } }

        protected abstract void DataReceived(byte[] buff);

        public CatSerialBase(string config)
        {
            ser = new Serial();
            ser.ConfigurePort(config);
            ser.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(ser_DataReceived);
        }

        public override void Open()
        {
            try
            {
                if (ser != null)
                    ser.Open();
            }
            catch (Exception ex)
            {
                onError(ex.Message);
            }
        }

        public override void Close()
        {
            if (ser != null)
                ser.Close();
        }

        void ser_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            int n = ser.BytesToRead;
            byte[] buff = new byte[n];
            ser.Read(buff, 0, n);
            onDataReceived(buff);
            DataReceived(buff);
        }


        public override void SendRaw(byte[] buff)
        {
            ser.WriteArray(buff);
        }

        public override void SendRaw(string text)
        {
            ser.Write(text);
        }


    }
}
