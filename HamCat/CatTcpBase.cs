﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HamCat
{
    public abstract class CatTcpBase : CatBase
    {

        protected TcpClient tcpClient;
        protected IPEndPoint endPoint;
        protected NetworkStream netStream;

        protected virtual string GetErrorMessage(Exception ex)
        {
            return ex.Message;
        }

        public override void Open()
        {
            try
            {
                if (endPoint != null)
                {
                    tcpClient = new TcpClient();
                    tcpClient.Connect(endPoint);
                    netStream = tcpClient.GetStream();
                }
            }
            catch (Exception ex)
            {
                onError(GetErrorMessage(ex));
            }
        }

        public override void Close()
        {
            if (netStream != null)
                netStream.Close();
            if (tcpClient != null)
                tcpClient.Close();
        }

    }
}
