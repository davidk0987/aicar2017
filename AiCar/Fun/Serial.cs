using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

//以主机通讯的类
namespace AiCar
{
    class Serial
    {

        public delegate void UpdateMainwindowMessage(string labelContent, System_Message_Info itype);
        public event UpdateMainwindowMessage UpdateMessage = null;


        private SerialPort     io_serial = new SerialPort();
        private IOCP_Svr    net_svr_0006 = new IOCP_Svr();  //8110
        private IOCP_Svr    net_svr_ksxt = new IOCP_Svr();  //8111
        private TcpClientBusiness net_cli_0006 = new TcpClientBusiness();  //127.0.0.1   8002
        
        private string str_com_num = "";
        ByteBuffer pBuffer        = new ByteBuffer();
        Thread    tCheckHeartbeat = null;  //心跳线程

        DispatcherTimer _timer = new DispatcherTimer();
        private int icheckheart = 10;
        StringBuilder strbuild_KSXTData = new StringBuilder();
        StringBuilder strbuild_KSXTData_old = new StringBuilder();

        DateTime saveKSXTData_GpsDate;
        string strSN = "";

        List<Gps> APoints = new List<Gps>();



        public Serial()
        {
            _timer.Interval = new TimeSpan(0, 0, 1);   //间隔1秒
            _timer.Tick += new EventHandler(Timer_Tick);
            _timer.Start();

            net_svr_0006.ListenClient(8110, 3);
            net_svr_ksxt.ListenClient(8111, 3);

        }

        ~Serial()
        {
            Closecom();
        }

        private void SetGpsCoord(double lat,double lng)
        {
            Gps _gps = new Gps();
            _gps.setWgLat(lat);
            _gps.setWgLon(lng);
            APoints.Add(_gps);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            //dTimer.Stop();//先停止定时器的作用

            try
            {
                net_cli_0806.ConnectToServer("127.0.0.1", 8002);//发送0806协议

                try
                {
                    if (icheckheart < 1) icheckheart++;

                    string strcomnum = getSerialCom();
                    if(strcomnum != str_com_num)
                    {
                        if (io_serial.IsOpen) io_serial.Close();
                        str_com_num = strcomnum;
                    }


                    if (io_serial.IsOpen && icheckheart>0)
                    {
                        Send(System.Text.ASCIIEncoding.Default.GetBytes("#cfg,heartbeat,5\r\n"));
                    }
                    else
                    {
                        if(str_com_num!="")
                        {
                            Opencom(str_com_num);
                        }
                    }
                }
                catch { }
            }
            catch { }
        }

        public bool Isopen()
        {
            return io_serial.IsOpen;
        }

        public void setCheckHeartTime(int i)
        {
            icheckheart = i;
        }
        //打开串口
        public bool Opencom(string strcom="")
        {
            if (strcom == "")
            {
                strcom = getSerialCom();
                
            }

            if (strcom == "") return false;
            try
            {
                //gpscom_serial = comboBox1.Text.ToUpper();
                //gpscom_baud = 115200;
                
                if (!io_serial.IsOpen)
                {
                    io_serial.BaudRate = 115200;
                    io_serial.PortName = strcom;
                    io_serial.DataBits = 8;


                    io_serial.Open();

                    if (io_serial.IsOpen)
                    {
                        ShowMessage("打开车载终端串口[" + strcom + "]成功");
                        if(tCheckHeartbeat==null)
                        {
                            //tCheckHeartbeat = new Thread(CheckHeartbeat);//心跳检测进程
                            //tCheckHeartbeat.IsBackground = true;
                            //tCheckHeartbeat.Start();
                            //ShowMessage("sdfsdfdfdf");
                            io_serial.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.OnDataReceived);

                            

                            if(pBuffer.ReadConfig_bool())
                            {
                                ShowMessage("读取配置文件成功");
                            }
                            else
                            {
                                ShowMessage("读取配置文件失败");
                            }
                        }
                        else
                        {
#pragma warning disable CS0618 // 类型或成员已过时
                            tCheckHeartbeat.Resume();
#pragma warning restore CS0618 // 类型或成员已过时

                        }
                        return true;
                    }
                    else
                    {
                        ShowMessage("打开车载终端串口[" + strcom + "]失败");
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                ShowMessage("串口打开失败，可能被其它程序占用，请检查。");
                return false;
            }
        }

        //关闭串口
        public void Closecom()
        {
            try
            {
                io_serial.Close();
                //AddContent("关闭GPS数据串口");
                ShowMessage("关闭车载终端串口");

                if (tCheckHeartbeat != null)
                {
#pragma warning disable CS0618 // 类型或成员已过时
                    tCheckHeartbeat.Suspend();
#pragma warning restore CS0618 // 类型或成员已过时
                }
            }
            catch {
                //io_serial(ex.ToString());
            }

        }

        public void Send(byte[] data)
        {
            if(io_serial.IsOpen)
            {
                io_serial.Write(data,0,data.Length);
            }
            
        }

        public void Send_DiffData(byte[] data)
        {
            if (io_serial.IsOpen && icheckheart > 0)
            {
                io_serial.Write(data, 0, data.Length);
            }

        }

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                byte[] ReDatas = new byte[io_serial.BytesToRead];
                io_serial.Read(ReDatas, 0, ReDatas.Length);  //读取数据  
                                                             //System.Text.ASCIIEncoding.Default.GetString(ReDatas);

                net_svr_ksxt.SendData2AllClient(ReDatas, io_serial.BytesToRead); //将数据转发出去


                pBuffer.Buff_Append(ReDatas);
                AlyData();
            }
            catch { }
            
        }

        private void ShowMessage(string strdata, System_Message_Info itype =0)
        {
            UpdateMessage?.Invoke(strdata,itype);
        }

        private void AlyData_0183(string strData)
        {
            int int_IndexOf_first = strData.LastIndexOf('$');
            int int_IndexOf_last = strData.LastIndexOf('*');

            if (int_IndexOf_first < 0 || int_IndexOf_last < 1) return;


            strData = strData.Substring(int_IndexOf_first, int_IndexOf_last - int_IndexOf_first + 3);
            int_IndexOf_first = strData.LastIndexOf('$');
            int_IndexOf_last  = strData.LastIndexOf('*');
                        

            if (strData.Contains("$KSXT"))
            {
                #region $KSXT
                //解析GPS时间
                string[] strArray = strData.Split(',');

                if (strArray.Count() < 40) return;

                if (strArray[35].ToUpper() != Helper.Helper_StrToMD5(strArray[1] + strArray[28]).ToUpper())
                {
                    ShowMessage("设备校验码失败");
                    return;
                }

                
                try
                {
                    if(strArray[10] == "1"|| strArray[10] == "2")
                    {
                        Gps _gps = new Gps();
                        _gps.setWgLat(Convert.ToDouble(strArray[3]));
                        _gps.setWgLon(Convert.ToDouble(strArray[2]));
                        
                        if(Helper.MBR(_gps,APoints))
                        {
                            strArray[10] = "3";
                            strArray[38] = "0.015";
                            strArray[39] = "0.015";

                            string str_chang_ksxt = "";
                            for(int i=0;i < strArray.Count();i++)
                            {
                                str_chang_ksxt += strArray[i] + ",";                                
                            }
                            str_chang_ksxt = str_chang_ksxt.Substring(0, str_chang_ksxt.Length - 3);
                            string strXoryanzhenBTstring = str_chang_ksxt.Substring(1, str_chang_ksxt.Length - 2);
                            //AddContent(strXoryanzhenBTstring+"--------");

                            int iXoryanzhenBTstring = Helper.Helper_XoryanzhenBTstring(System.Text.ASCIIEncoding.Default.GetBytes(strXoryanzhenBTstring));
                            str_chang_ksxt += iXoryanzhenBTstring.ToString("X2");
                            strData = str_chang_ksxt;
                            //ShowMessage(str_chang_ksxt);
                        }
                    }
                    



                    ShowMessage(strData, System_Message_Info.ksxt);

                    WriteKSXTData(strData + "\r\n");
                }
                catch { }

                #endregion
            }
            else if(strData.Contains("$XTXX"))
            {
                try
                {
                    AlyXTXX(strData);
                    ShowMessage(strData, System_Message_Info.xtxx);
                }
                catch { }
                
            }
        }
        
        private void AlyXTXX(string strData)
        {
            string[] strArray = strData.Split(',');

            if (strArray.Count() < 7) return;

            if(strArray[4]=="0") ShowMessage("终端设备授权过期，请重新授权", System_Message_Info.notification);
            if (strArray[5] == "0") ShowMessage("终端设备GPS模块异常，请联系厂家", System_Message_Info.notification);
        }

        
        
        private void AlyDeviceData(string strData)
        {
            strData = strData.ToUpper();
            ShowMessage(strData, System_Message_Info.PowerOnSelfTest);//开机自检，异常消息打印提醒
        }

        private void AlyData()
        {
            int iLength = pBuffer.Buff_GetBufferLen();

            int iCMD = 0;
            int iIndex = 0;

            while(iLength>0)
            {
                if((iCMD > 0 && iLength < 9)|| iIndex == iLength-1)
                {
                    break;
                }


                if (iCMD == 0)
                {
                    #region 查找数据头
                    byte _bhead = pBuffer.Buff_GetAt(iIndex);
                    if (_bhead == '$')//寻找美元号$
                    {
                        iIndex++;
                        byte b_head = pBuffer.Buff_GetAt(iIndex);
                        if (b_head == 'G' || b_head == 'J' || b_head == 'K' || b_head == 'X')
                        {
                            //G K X J
                            iIndex++;
                            iCMD = 1;  //报文数据为0183格式明文
                        }
                        else
                        {
                            pBuffer.Buff_Delete(iIndex + 1);
                            iIndex = 0;
                        }
                    }
                    else if (_bhead == '#')//寻找#开机自检
                    {
                        iIndex++;
                        byte b_head = pBuffer.Buff_GetAt(iIndex);
                        if (b_head == '#')
                        {
                            iIndex++;
                            iCMD = 2;//主机开机输出报文
                        }
                        else
                        {
                            pBuffer.Buff_Delete(iIndex + 1);
                            iIndex = 0;
                        }

                    }
                    else if (_bhead == 0xbb)
                    {
                        iIndex++;
                        byte b_head = pBuffer.Buff_GetAt(iIndex);
                        if (b_head == 0xfb)
                        {
                            iIndex++;
                            iCMD = 3;//16进制命令
                        }
                        else
                        {
                            pBuffer.Buff_Delete(iIndex + 1);
                            iIndex = 0;
                        }
                    }
                    else
                    {
                        pBuffer.Buff_Delete(iIndex + 1);
                        iIndex = 0;
                    }
                    #endregion
                }
                else if (iCMD == 1)
                {
                    #region 0183格式
                    try
                    {
                        //开始查找\n 
                        if (pBuffer.Buff_GetAt(iIndex) == 0x0D)
                        {
                            iIndex++;
                            if (pBuffer.Buff_GetAt(iIndex) == 0x0A)
                            {
                                //找到换行符
                                iIndex++;
                                //b_buff.setReaderIndex(0 - iIndex);
                                byte[] ReDatas = new byte[iIndex];// b_buff.readBytes(iIndex);
                                pBuffer.Buff_Read(ReDatas, iIndex);

                                AlyData_0183(System.Text.ASCIIEncoding.Default.GetString(ReDatas));

                                iCMD = 0;
                                iIndex = 0;
                            }
                        }
                        else
                        {
                            if (iIndex > 1024)
                            {
                                iCMD = 0;
                                iIndex = 0;
                                pBuffer.Buff_ClearBuffer();
                            }
                            else
                            {
                                iIndex++;
                            }

                        }

                    }
                    catch
                    {
                        iCMD = 0;
                        iIndex = 0;
                        pBuffer.Buff_ClearBuffer();
                    }

                    #endregion
                }
                else if (iCMD == 2)
                {
                    try
                    {
                        //开始查找\n 
                        if (pBuffer.Buff_GetAt(iIndex) == 0x0D)
                        {
                            iIndex++;
                            if (pBuffer.Buff_GetAt(iIndex) == 0x0A)
                            {
                                //找到换行符
                                iIndex++;
                                //b_buff.setReaderIndex(0 - iIndex);
                                byte[] ReDatas = new byte[iIndex];// b_buff.readBytes(iIndex);
                                pBuffer.Buff_Read(ReDatas, iIndex);

                                AlyDeviceData(System.Text.ASCIIEncoding.Default.GetString(ReDatas));

                                iCMD = 0;
                                iIndex = 0;
                            }
                        }
                        else
                        {
                            if (iIndex > 1024)
                            {
                                iCMD = 0;
                                iIndex = 0;
                                pBuffer.Buff_ClearBuffer();
                            }
                            else
                            {
                                iIndex++;
                            }

                        }

                    }
                    catch
                    {
                        iCMD = 0;
                        iIndex = 0;
                        pBuffer.Buff_ClearBuffer();
                    }
                }
                else if (iCMD == 3)
                {
                    byte[] b_CMD = new byte[2];
                    b_CMD[0] = pBuffer.Buff_GetAt(iIndex);
                    iIndex++;
                    b_CMD[1] = pBuffer.Buff_GetAt(iIndex);
                    iIndex++;

                    int icmd = Device_CMD.CheckCMDHead(b_CMD);
                    int iLen = 0;
                    if (icmd>0)
                    {
                        byte[] b_lenght = new byte[4];// b_buff.readBytes(4);
                        for (int i = 0; i < 4; i++)
                        {
                            b_lenght[i] = (byte)Helper.rx_invork(pBuffer.Buff_GetAt(iIndex));
                            iIndex++;
                        }

                        iLen = BitConverter.ToInt16(b_lenght, 2);

                        if (iLength < iLen + iIndex)
                        {
                            break;
                        }
                    }
                    else
                    {
                        pBuffer.Buff_Delete(iIndex);
                        iCMD = 0;
                        iIndex = 0;
                        icmd = 0;
                    }
                }

                iLength = pBuffer.Buff_GetBufferLen();
            }
        }
        
        private string getSerialCom()
        {
            string strfilepaht = AppDomain.CurrentDomain.BaseDirectory + "\\config\\config.cfg";
            
            return Helper.IniReadValue("serial", "com", strfilepaht);
        }
    }
}
