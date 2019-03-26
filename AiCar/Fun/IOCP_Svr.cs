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
    class IOCP_Svr
    {
        private Socket s_Server;

        private string strVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        /// <summary>
        /// 通讯SAEA池
        /// </summary>
        public SAEAPool saeaPool_Receive;


        /// <summary>
        /// 侦听客户端
        /// </summary>
        public void   ListenClient(int _iPort = 2018, int iClientMaxCount = 30000)
        {
            try
            {
                
                //int iClientMaxCount = 30000; //最大客户端数量
                s_Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                s_Server.Bind(new IPEndPoint(IPAddress.Any, _iPort));
                s_Server.Listen(iClientMaxCount);
                int ibufferSize = 1024; //每个缓冲区大小
                BufferManager bufferManager = new BufferManager(ibufferSize * iClientMaxCount, ibufferSize);
                saeaPool_Receive = new SAEAPool(iClientMaxCount);
                for (int i = 0; i < iClientMaxCount; i++) //填充SocketAsyncEventArgs池
                {
                    SocketAsyncEventArgs saea_New = new SocketAsyncEventArgs();
                    saea_New.Completed += new EventHandler<SocketAsyncEventArgs>(OnIOCompleted);
                    bufferManager.SetBuffer(saea_New);
                    
                    SocketAsyncEventArgs saea_Send = new SocketAsyncEventArgs();
                    saea_Send.Completed += new EventHandler<SocketAsyncEventArgs>(OnIOCompleted);

                    IOCP_Svr_client userToken = new IOCP_Svr_client();
                    userToken.SAEA_Send = saea_Send;
                    userToken.clear();

                    saea_New.UserToken = userToken;
                    saeaPool_Receive.Add(saea_New);
                }


                Thread tCheckClientHeartbeat = new Thread(CheckClientHeartbeat);//心跳检测进程
                tCheckClientHeartbeat.IsBackground = true;
                tCheckClientHeartbeat.Start();
                ShowMsg("监听端口 [" + _iPort.ToString() + "] 成功", ConsoleColor.DarkGreen);
                StartAccept(null);


            }
            catch(Exception ex)
            {
                //s_Udp_Client = null;
                ShowMsg("监听端口 [" + _iPort.ToString() + "] 失败", ConsoleColor.DarkRed);
                ShowMsg(ex.ToString(), ConsoleColor.DarkRed);
            }
        }
       

        #region 服务器传输部分
        /// <summary>
        /// 接受来自客户机的连接请求操作
        /// </summary>
        private void StartAccept(SocketAsyncEventArgs saea_Accept)
        {
            if (saea_Accept == null)
            {
                saea_Accept = new SocketAsyncEventArgs();
                saea_Accept.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
            }
            else
                saea_Accept.AcceptSocket = null;  //重用前进行对象清理

            if (!s_Server.AcceptAsync(saea_Accept))
                ProcessAccept(saea_Accept);
        }

        /// <summary>
        /// 连接完成异步操作回调
        /// </summary>
        private void OnAcceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        /// <summary>
        /// 接收或发送完成异步操作回调
        /// </summary>
        private void OnIOCompleted(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend(e);
                    break;
            }
        }

        /// <summary>
        /// 异步发送操作完成后调用该方法
        /// </summary>
        private void ProcessSend(SocketAsyncEventArgs e)
        {

        }


        /// <summary>
        /// 异步连接操作完成后调用该方法
        /// </summary>
        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            Socket s = e.AcceptSocket;
            if (s != null && s.Connected)
            {
                try
                {
                    string sClientIP = ((IPEndPoint)s.RemoteEndPoint).Address.ToString();
                    //((IPEndPoint)s.RemoteEndPoint).Port.ToString();
                    int port = ((IPEndPoint)s.RemoteEndPoint).Port;
                    ShowMsg("[" + sClientIP + ":" + port.ToString() + "] 连接", ConsoleColor.DarkGreen);
                    SocketAsyncEventArgs saea_Receive = saeaPool_Receive.Pull();
                    if (saea_Receive != null)
                    {
                        ShowMsg("当前在线数量：" + saeaPool_Receive.GetUsedSAEACount(), ConsoleColor.Yellow);
                        IOCP_Svr_client userToken = (IOCP_Svr_client)saea_Receive.UserToken;
                        //userToken.s_Udp_Client = s_Udp_Client;
                        userToken.S = s;
                        userToken.psUserInfo.Port = port;
                        //userToken.HeartbeatTime = DateTime.Now;
                        userToken.psUserInfo.sIP = sClientIP;
                        if (!userToken.S.ReceiveAsync(saea_Receive))
                            ProcessReceive(saea_Receive);

                        
                    }
                    else
                    {
                        s.Close();
                        ShowMsg("[" + sClientIP + ":" + port + "]连接已经达到上限，不允许再连接");
                    }
                }
                catch { }
            }
            StartAccept(e);
        }

        /// <summary>
        /// 异步接收操作完成后调用该方法
        /// </summary>
        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            try
            {
                if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
                {
                    IOCP_Svr_client userToken = (IOCP_Svr_client)e.UserToken;
                    try
                    {
                        byte[] abFactReceive = new byte[e.BytesTransferred];
                        Array.Copy(e.Buffer, e.Offset, abFactReceive, 0, e.BytesTransferred);
                        userToken.RecvData(abFactReceive, e.BytesTransferred);
                    }
                    catch (Exception ex) { ShowMsg(ex.ToString(),ConsoleColor.Red); }
                    finally
                    {
                        if (!userToken.S.ReceiveAsync(e))
                            ProcessReceive(e);
                    }
                }
                else
                    CloseClientSocket(e);
            }
            catch { }
        }

        /// <summary>
        /// Socket 断开处理
        /// </summary>
        public void CloseClientSocket(SocketAsyncEventArgs saea)
        {
            try
            {
                IOCP_Svr_client userToken = (IOCP_Svr_client)saea.UserToken;
                if (!saeaPool_Receive.Push(saea))
                    return;
                if (userToken.S != null)
                {
                    if (userToken.S.Connected)
                    {
                        try
                        {
                            userToken.S.Shutdown(SocketShutdown.Both);
                        }
                        catch { }
                        string sClientIP = ((IPEndPoint)userToken.S.RemoteEndPoint).Address.ToString();
                    }
                    userToken.S.Close();
                    userToken.clear();
                }
                ShowMsg("当前在线数量：" + saeaPool_Receive.GetUsedSAEACount(), ConsoleColor.Yellow);
            }
            catch { }
        }

        /// <summary>
        /// 客户端心跳检测
        /// </summary>
        private void CheckClientHeartbeat()
        {
            long _lCheckHeartBeat    = DateTime.Now.Ticks / 10000000;
            long _lUpdateStationInfo = DateTime.Now.Ticks / 10000000;
            while (true)
            {
                Thread.Sleep(10000);
                try
                {
                    //int iCheckInterval = 180 * 1000; //180秒检测间隔
                    //Thread.Sleep(iCheckInterval);
                    #region 检测心跳
                    int iCheckInterval = (int)(DateTime.Now.Ticks / 10000000 - _lCheckHeartBeat); //180秒检测间隔
                    if (iCheckInterval >= 90)
                    {
                        _lCheckHeartBeat = DateTime.Now.Ticks / 10000000;
                        List<SocketAsyncEventArgs> lUserdSAEA = saeaPool_Receive.GetUsedSAEA();
                        if (lUserdSAEA != null && lUserdSAEA.Count > 0)
                        {
                            foreach (SocketAsyncEventArgs saea in lUserdSAEA)
                            {
                                IOCP_Svr_client userToken = (IOCP_Svr_client)saea.UserToken;
                                if (userToken.checkHeartbeatTime(iCheckInterval) < 0)
                                {
                                    if (userToken.S != null)
                                    {
                                        try
                                        {
                                            string sClientIP = ((IPEndPoint)userToken.S.RemoteEndPoint).Address.ToString();
                                        }
                                        catch { }

                                        userToken.close(); //服务端主动关闭心跳超时连接，在此关闭连接，会触发OnIOCompleted回调  
                                        //userToken.clear();
                                    }
                                }
                            }
                        }
                    }
                    
                    #endregion


                }
                catch { }
            }
        }

        public void SendData2AllClient(byte[] data,int len)
        {
            try
            {
                List<SocketAsyncEventArgs> lUserdSAEA = saeaPool_Receive.GetUsedSAEA();
                if (lUserdSAEA != null && lUserdSAEA.Count > 0)
                {
                    foreach (SocketAsyncEventArgs saea in lUserdSAEA)
                    {
                        IOCP_Svr_client userToken = (IOCP_Svr_client)saea.UserToken;
                        userToken.SendData2Client(data, len);
                    }
                }
            }
            catch { }

        }
        #endregion
        private void  ShowMsg(string strData, ConsoleColor cclolor = ConsoleColor.DarkCyan)
        {
            strData = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff  ") + strData + "";

            Console.ForegroundColor = cclolor;
            Console.WriteLine(strData);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
