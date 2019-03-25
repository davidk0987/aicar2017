using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;

namespace AiCar
{
    public class NtripClient
    {

        public delegate void UpdateMainwindowMessage(string labelContent, System_Message_Info itype); //消息回调主线程
        public event UpdateMainwindowMessage UpdateMessage = null;

        public delegate void UpdateNtripSvrRTCM3Data(byte[] rtcmdata, int len); //消息回调主线程
        public event UpdateNtripSvrRTCM3Data UpdateRtcmData = null;

        private TcpClientBusiness ntripclinet = new TcpClientBusiness();
        private string strNtripSvrIP   = "";    // 服务器地址
        private int iNtripSvrPort = 2018;       // 服务器端口
        private int iIsuse = 0;                 //是否启用，0为不启用，1为启用
        private string strNtrip_basesn = "";
        private string strNtrip_basesn_old = "";
        string str_KSXT_message = "";   //考试系统信息

        DispatcherTimer _timer = new DispatcherTimer();


        //public event TcpClient_cli.TcpClientDllcallBack TcpClient_OnRecvData_CallBack = null;
        //private TcpClient.TcpClientDllcallBack TcpClient_OnRecvData_CallBack = new TcpClient.TcpClientDllcallBack(TcpClient_OnRecvData);
        public NtripClient()
        {
            getNtripSvrInfo();
            _timer.Interval = new TimeSpan(0, 0, 1);   //间隔1秒
            _timer.Tick += new EventHandler(Timer_Tick);
            _timer.Start();

            ntripclinet.Updatetopout += OnRecvData;
            //IntPtr abc = this.;
        }

        private void OnRecvData(byte[] b_data, int nBufLen, TcpClientMessage type)
        {
            if (type == TcpClientMessage.datatran)
                UpdateRtcmData?.Invoke(b_data, nBufLen);
            else if (type == TcpClientMessage.conected)
            {
                ShowMessage("连接差分服务器成功");
                SendLogin();
            }
            else if (type == TcpClientMessage.diconected)
                ShowMessage("差分服务器主动断开连接");
            else if (type == TcpClientMessage.cannot_connect)
                ShowMessage("连接差分服务器失败"); 
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            //dTimer.Stop();//先停止定时器的作用
            getNtripSvrInfo();

            try
            {
                if (ntripclinet.IsConnect() && iIsuse == 0)
                {
                    ntripclinet.closeclient();
                    ShowMessage("断开差分服务器");
                }

                if (ntripclinet.GetDomain()!= strNtripSvrIP && ntripclinet.IsConnect() && iIsuse == 1)
                {
                    ntripclinet.closeclient();
                    ShowMessage("断开差分服务器");
                }

                if (strNtrip_basesn_old != strNtrip_basesn && ntripclinet.IsConnect() && iIsuse == 1)
                {
                    ntripclinet.closeclient();
                    ShowMessage("断开差分服务器");
                }


                if (ntripclinet.IsConnect() && str_KSXT_message != "")
                {
                    byte[] data = System.Text.ASCIIEncoding.Default.GetBytes(str_KSXT_message + "\r\n");
                    ntripclinet.SendData(data, data.Length);
                    //Send(System.Text.ASCIIEncoding.Default.GetBytes("#cfg,heartbeat,5\r\n"));
                }

                if (!ntripclinet.IsConnect() && iIsuse == 1 && str_KSXT_message != "")
                {
                    
//                     TcpClient_OnRecvData_CallBack += TcpClient_OnRecvData;
//                     ntripclinet.TcpClient_SetCallBack(TcpClient_OnRecvData_CallBack, IntPtr.Zero);
                    
                    ntripclinet.ConnectToServer(strNtripSvrIP, iNtripSvrPort);
                    ShowMessage("开始尝试连接差分服务器");
                }                
            }
            catch { }
        }

        public void Update_KSXT_info(string strData)
        {
            str_KSXT_message = strData;
        }
        

        private void getNtripSvrInfo()
        {
            string strfilepaht = AppDomain.CurrentDomain.BaseDirectory + "\\config\\config.cfg";

            try
            {
                strNtrip_basesn = Helper.IniReadValue("ntrip", "basesn", strfilepaht);
                strNtripSvrIP   = Helper.IniReadValue("ntrip", "ip", strfilepaht);
                iNtripSvrPort   = Convert.ToInt32(Helper.IniReadValue("ntrip", "port", strfilepaht));
                iIsuse          = Convert.ToInt32(Helper.IniReadValue("ntrip", "isuse", strfilepaht));
            }
            catch { }
            
        }

        private void ShowMessage(string strdata, System_Message_Info itype = 0)
        {
            UpdateMessage?.Invoke(strdata, itype);
        }

        private void SendLogin()
        {
            try
            {
                if (ntripclinet.IsConnect() && str_KSXT_message != "" && str_KSXT_message.Split(',').Count() > 28)
                {
                    string strlogin = string.Format("SOURCE {0}:{1}", strNtrip_basesn, str_KSXT_message.Split(',')[28]);

                    byte[] data = System.Text.ASCIIEncoding.Default.GetBytes(strlogin + "\r\n");
                    ntripclinet.SendData(data, data.Length);

                    strNtrip_basesn_old = strNtrip_basesn;
                    //Send(System.Text.ASCIIEncoding.Default.GetBytes("#cfg,heartbeat,5\r\n"));
                }
            }
            catch { }
            
        }
       
    }
}
