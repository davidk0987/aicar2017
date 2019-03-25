using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AiCar
{
    internal class IOCP_Svr_client
    {
        public struct UserInfo
        {
            public string sIP { get; set; }
            public int    Port { get; set; }
        };

        
        /// <summary>
        /// 用于发送数据的SocketAsyncEventArgs
        /// </summary>
        public SocketAsyncEventArgs SAEA_Send;
        //public Socket s_Udp_Client = null;   //UDP客户端，将数据转发出去


        /// <summary>
        /// 连接套接字
        /// </summary>
        public Socket S;
        public UserInfo psUserInfo = new UserInfo();


//         private int isConnect = -1;       //通知上层断开连接
//         private long lnetwork_flow = 0;   //数据流量
        /// <summary>
        /// 最新一次心跳时间
        /// </summary>
        private DateTime HeartbeatTime;

        #region 全局调用函数
        public int checkHeartbeatTime(int iCheckInterval)
        {
            return HeartbeatTime.AddSeconds(iCheckInterval).CompareTo(DateTime.Now);
        }

        public void SendData2Client(byte[] data,int len)
        {
            S.Send(data, len,SocketFlags.None);
        }

        public void close()
        {
            if (S != null)
            {
                if (S.Connected)
                {
                    try
                    {
                        S.Shutdown(SocketShutdown.Both);
                    }
                    catch { }
                    //string sClientIP = ((IPEndPoint)S.RemoteEndPoint).Address.ToString();
                    //ShowMsg(psUserInfo.sJSH + " [" + psUserInfo.sIP + "] 断开网络连接", ConsoleColor.Red);
                }
                S.Close();
                clear();
                //SAEA_Send.Completed
            }
        }
        public void clear()
        {
            //pBuffer_Diff.clearbuff();
            //pBuffer_Inork.clearbuff();
            //pBuffer_Ntrip.clearbuff();
            //DiffData_CMD_List.Clear();
            psUserInfo.sIP  = "";
            psUserInfo.Port = 0;
            
        }
            
        #endregion

        #region 公有函数
        public void RecvData(byte[] bdata,int len)
        {
            HeartbeatTime = DateTime.Now;
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////

    }
}
