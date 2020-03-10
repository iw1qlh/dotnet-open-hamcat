using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace HamCat
{
    public class Serial : SerialPort
    {

        public void ConfigurePort(string config)
        {
            int n;

            string[] p = config.Split(',');
            for (int i = 0; i < p.Length; i++)
            {
                if (i == 0)
                    this.PortName = p[i];
                if ((i == 1) && int.TryParse(p[i], out n))
                    this.BaudRate = n;
                if (i == 2)
                {
                    switch (p[i])
                    {
                        case "N": this.Parity = Parity.None; break;
                        case "E": this.Parity = Parity.Even; break;
                        case "O": this.Parity = Parity.Odd; break;
                        case "M": this.Parity = Parity.Mark; break;
                        case "S": this.Parity = Parity.Space; break;
                    }

                }
                if ((i == 3) && int.TryParse(p[i], out n))
                    this.DataBits = n;
                if (i == 4)
                {
                    switch (p[i])
                    {
                        case "0": this.StopBits = StopBits.None; break;
                        case "1": this.StopBits = StopBits.One; break;
                        case "2": this.StopBits = StopBits.Two; break;
                    }
                }
            }

        }

        public void WriteArray(byte[] buffer)
        {
            Write(buffer, 0, buffer.Length);
        }

        public new void Write(string text)
        {
            if (IsOpen)
                base.Write(text);
        }

    }
}
