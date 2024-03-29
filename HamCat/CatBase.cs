﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static HamCat.CatBase;

namespace HamCat
{
    public abstract class CatBase
    {

        public delegate void OnErrorEventHandler(string text);
        public event OnErrorEventHandler OnError;

        public delegate void OnDataReceivedEventHandler(byte[] buff);
        public event OnDataReceivedEventHandler OnDataReceived;

        public enum Values { SWR, COMP, ALC, FREQ, MODE, RXTX };
        protected double swr;
        protected long freq;
        protected int comp, alc;
        protected Modes mode;
        protected bool tx;

        public enum Modes { LSB, USB, CW, AM, FM, DIG, UNKNOW };
        public Modes Mode { get { return mode; }  set { SetMode(value); } }

        public double Swr { get { return swr; } }
        public int Comp { get { return comp; } }
        public int Alc { get { return alc; } }
        public long Freq { get { return freq; } set { SetFrequency(value); } }
        public bool Tx { get { return tx; } set { RxTx(value); } }

        public delegate void OnChangeEventHandler(Values value, string text);
        public event OnChangeEventHandler OnChange;

        public abstract void TX();
        public abstract void RX();
        public abstract void LowPower();
        public abstract void HighPower();
        public abstract void DisableATU();

        public abstract void SetFrequency(long freq);
        public abstract void SetMode(Modes mode);

        public abstract void AskSwr();
        public abstract void AskFrequency();

        public abstract void SendRaw(byte[] buff);
        public abstract void SendRaw(string text);

        public abstract void Open();
        public abstract void Close();

        public abstract void OpenSettings();

        public void RxTx(bool tx)
        {
            if (tx)
                TX();
            else
                RX();
        }

        protected void onError(string text)
        {
            if (OnError != null)
                OnError(text);
        }

        protected void onChange(Values value, string text)
        {
            if (OnChange != null)
                OnChange(value, text);
        }

        protected void onDataReceived(byte[] buff)
        {
            if (OnDataReceived != null)
                OnDataReceived(buff);
        }

    }

    public static class CatBaseExtensions
    {
        public static string ToText(this Modes mode)
        {
            if (mode == Modes.LSB)
                return "LSB";
            if (mode == Modes.USB)
                return "USB";
            if (mode == Modes.CW)
                return "CW";
            if (mode == Modes.FM)
                return "FM";
            if (mode == Modes.AM)
                return "AM";
            return null;
        }

    }

}
