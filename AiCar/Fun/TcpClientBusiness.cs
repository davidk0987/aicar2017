using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AiCar
{
    public enum TcpClientMessage
    {
        conected=0,
        diconected,
        datatran,
        cannot_connect
    }
    public class TcpClientBusiness
    {
        #region    自定义参数

        public delegate void UpdateAsynConnect(IAsyncResult iar); //消息回调主线程
        public event UpdateAsynConnect UpdateAC     = null;
        public event UpdateAsynConnect UpdateACRecv = null;
        //public void AsynReceiveData(IAsyncResult iar)


        public delegate void UpdateData2top_out(byte[] b_data, int len, TcpClientMessage type); //消息回调主线程
        public event UpdateData2top_out Updatetopout = null;
        /// <summary>
        /// Tcp客户端模型
        /// </summary>
        private TcpClient tcpClient= null;
        /// <summary>
        /// 网络访问数据流
        /// </summary>
        private NetworkStream networkStream;

        /// <summary>
        /// 返回数据
        /// </summary>
        //public List<byte[]> ResponseBytes = new List<byte[]>();

        /// <summary>
        /// 远程服务IP地址
        /// </summary>
        private string RemoteIp = string.Empty;
        /// <summary>
        /// 远程服务IP地址对应端口
        /// </summary>
        private int RemotePort = -1;

        /// <summary>
        /// 是否连接
        /// </summary>
        private bool IsConnected = false;

        private const int recvdatacount_max = 1024;
        //private byte[] TempBytes = new byte[recvdatacount_max];
        
        #endregion

        public TcpClientBusiness()
        {
            //开始连接
            UpdateAC     += AsynConnect;
            UpdateACRecv += AsynReceiveData;
        }

        public string GetDomain()
        {
            return RemoteIp;
        }



        public bool IsConnect()
        {
            return IsConnected;
        }
        /// <summary>
        /// 打开TCP连接
        /// </summary>
        public void ConnectToServer(string sIP, int iPort)
        {
            if (IsConnected==true) return;
            try
            {
                RemoteIp   = sIP;
                RemotePort = iPort;
                //初始化TCP客户端对象
                tcpClient  = new TcpClient();                
                tcpClient.BeginConnect(RemoteIp, RemotePort, new AsyncCallback(UpdateAC), tcpClient);
            }
            catch (Exception )
            {
                Updatetopout?.Invoke(null, 0, TcpClientMessage.cannot_connect);
            }
        }

        public void closeclient()
        {
            if (IsConnected == false) return;

            if(tcpClient!=null)
            {
                networkStream.Close();
                tcpClient.Close();
                tcpClient = null;
                IsConnected = false;
                Updatetopout?.Invoke(null, 0, TcpClientMessage.diconected);
            }
        }
        /// <summary>
        /// 异步连接
        /// </summary>
        /// <param name="iar"></param>
        public void AsynConnect(IAsyncResult iar)
        {
            TcpClient t = (TcpClient)iar.AsyncState;
            try
            {
                if (t.Connected)
                {
                    //连接成功
                    tcpClient.EndConnect(iar);
                    //连接成功标志
                    IsConnected      = true;
                    networkStream    = tcpClient.GetStream();

                    //开始异步读取返回数据

                    byte[] TempBytes = new byte[recvdatacount_max];
                    networkStream.BeginRead(TempBytes, 0, recvdatacount_max, new AsyncCallback(UpdateACRecv), TempBytes);

                    Updatetopout?.Invoke(null, 0, TcpClientMessage.conected);
                }
                else
                {
                    tcpClient.Close();
                    tcpClient = null;
                }
                   
            }
            catch (Exception )
            {
                //VerficationOperate.WriteTextLogs("TcpClientBusiness", "AsynConnect|异常消息：" + ex.Message.ToString());
            }
        }

        /// <summary>
        /// 发送数据
        /// <param name="SendBytes">需要发送的数据</param>
        /// </summary>
        public void SendData(byte[] SendBytes,int len)
        {
            try
            {
                if (networkStream.CanWrite && SendBytes != null && len > 0)
                {
                    //发送数据
                    networkStream.Write(SendBytes, 0, len);
                    networkStream.Flush();
                }
            }
            catch (Exception )
            {
                if (tcpClient != null)
                {
                    networkStream.Close();
                    tcpClient.Close();
                    tcpClient = null;
                    //关闭连接后马上更新连接状态标志
                    IsConnected = false;

                    Updatetopout?.Invoke(null, 0, TcpClientMessage.diconected);
                }
            }
        }

        /// <summary>
        /// 异步接受数据
        /// </summary>
        /// <param name="iar"></param>
        public void AsynReceiveData(IAsyncResult iar)
        {
            byte[] CurrentBytes = (byte[])iar.AsyncState;
            try
            {
                //结束了本次数据接收
                int num = networkStream.EndRead(iar);
                //Array.Copy((byte[])iar.AsyncState, TempBytes,num);
                //这里展示结果为InfoModel的CurrBytes属性，将返回的数据添加至返回数据容器中
                Updatetopout?.Invoke(CurrentBytes, num, TcpClientMessage.datatran);

                byte[] TempBytes = new byte[recvdatacount_max];
                networkStream.BeginRead(TempBytes, 0, recvdatacount_max, new AsyncCallback(UpdateACRecv), TempBytes);
            }
            catch (Exception )
            {
                //VerficationOperate.WriteTextLogs("TcpClientBusiness", "AsynReceiveData|异常消息：" + ex.Message.ToString());
            }
        }

    }
}
