using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AiCar
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 页面定义
        bool    isfullscreen = true;
        private AiCar.Pages.Page_Home             page_Home           = new AiCar.Pages.Page_Home();            //主页
        private AiCar.Pages.Page_DeviceInfo page_DeviceInfo           = new AiCar.Pages.Page_DeviceInfo();      //传感器信息
        private AiCar.Pages.Page_Setting      page_Setting            = new AiCar.Pages.Page_Setting();         //主机参数设置
        private AiCar.Pages.Page_Analogsignal page_Analogsignal       = new AiCar.Pages.Page_Analogsignal();    //模拟灯光训练
        private AiCar.Pages.Page_collect_map_km2 page_collect_map_km2 = new AiCar.Pages.Page_collect_map_km2();    //科目二地图采集
        private AiCar.Pages.Page_km3_menu page_km3_menu               = new AiCar.Pages.Page_km3_menu();//科目三路线选择
        private AiCar.Pages.Page_km2_menu page_km2_menu               = new AiCar.Pages.Page_km2_menu();//科目二路线选择
        private string strNow_Page_Name = "HOME";
        #endregion

        #region 定义各类通信参数
        Serial io_com = new Serial();
        NtripClient net_ntripclient = new NtripClient();
        Judge xl_ks_judge = new Judge();//评判
        #endregion

        #region 业务处理函数
        //设置界面数据交互
        private void SettingSendCMDData(byte[] b_cmd)
        {
            if(io_com.Isopen())
                io_com.Send(b_cmd);
            else ShowMessage("数据口未打开，无法请求");
        }



        //回调差分数据
        private void UpdateNtripSvrRTCM3Data(byte[] rtcmdata, int len)
        {
            if (len > 0)
            {
                io_com.Send_DiffData(rtcmdata);

                page_Home.Ntrip_Message(len.ToString());
            }
        }
        //消息回调
        private void SubWindow_updateMainwindowMessage(string sContent, System_Message_Info itype = 0)
        {
            Dispatcher.BeginInvoke(new Action(delegate
            {

                switch (itype)
                {
                    case System_Message_Info.notification:
                        ShowMessage(sContent);
                        break;
                    case System_Message_Info.ksxt://KSXT信息报文                    
                        dispose_ksxt(sContent);
                        break;
                    case System_Message_Info.PowerOnSelfTest://开机自检，异常消息打印提醒                  
                        ShowMessage(sContent);
                        break;
                    case System_Message_Info.getradionum://获取电台通道号
                        page_Setting?.getradio(sContent);
                        break;
                    case System_Message_Info.gettran://获取当前数据传输模式
                        page_Setting?.gettranmodel(sContent);
                        break;
                    case System_Message_Info.setiohearttime://暂停发送心跳信息
                        io_com.setCheckHeartTime(Convert.ToInt32(sContent));
                        break;
                    case System_Message_Info.getiojx://获取IO极性
                        page_Setting?.ShowDeveiceInfo_IOJX(sContent);
                        break;
                    case System_Message_Info.updatefile://升级固件
                        page_Setting?.SendAppFile();
                        break;
                    default:
                        break;
                }

            }));



        }

        private void dispose_ksxt(string message)
        {
            if (strNow_Page_Name == "HOME")
                page_Home?.Aly_KSXT(message);
            else if (strNow_Page_Name == "DEVICEINFO")
                page_DeviceInfo?.Aly_KSXT(message);
            else if (strNow_Page_Name == "SETTING")
                page_Setting?.Aly_KSXT(message);
            else if (strNow_Page_Name == "ANALOGSIGNAL")
                page_Analogsignal?.Aly_KSXT(message);
            else if (strNow_Page_Name == "COLLECT_MAP_KM2")
                page_collect_map_km2?.Aly_KSXT(message);//
            else if (strNow_Page_Name == "KM3_MENU")
                page_km3_menu?.Aly_KSXT(message);
            else if (strNow_Page_Name == "KM2_MENU")
                page_km2_menu?.Aly_KSXT(message);

            //将数据传入底层解析库
            xl_ks_judge.Add_KSXT_Data(message.Split('*')[0]);

//             int count = 0;
//             double[] point_x = new double[39];
//             double[] point_y = new double[39];
//             xl_ks_judge.Get_Car_Point_Sensor_Judge(point_x, point_y,ref count);
//             ShowMessage(count.ToString() + "---" + point_x[0].ToString("0.00") + "---" + point_y[0].ToString("0.00"));


            net_ntripclient.Update_KSXT_info(message);
        }

        public void ShowMessage(string message)
        {
            if (strNow_Page_Name == "SETTING")
            {
                page_Home?.ShowMessage(message);
                page_Setting?.ShowMessage(message);
            }                
            else 
                page_Home?.ShowMessage(message);        
        }
        #endregion
        
        public MainWindow()
        {
            InitializeComponent();
        }

        public void ShowPage(string strpagename)
        {
            strpagename = strpagename.ToUpper();
            strNow_Page_Name = strpagename;

            try
            {
                switch(strpagename)
                {
                    case "HOME":
                        this.frmLayout.Content = page_Home;                        
                        break;
                    case "DEVICEINFO":
                        this.frmLayout.Content = page_DeviceInfo;
                        break;
                    case "SETTING":
                        this.frmLayout.Content = page_Setting;
                        break;
                    case "ANALOGSIGNAL":
                        this.frmLayout.Content = page_Analogsignal;
                        break;
                    case "COLLECT_MAP_KM2":
                        this.frmLayout.Content = page_collect_map_km2;
                        break;
                    case "KM3_MENU":
                        this.frmLayout.Content = page_km3_menu;
                        break;
                    case "KM2_MENU":
                        this.frmLayout.Content = page_km2_menu;
                        break;
                }
                //string path = "AiCar.Pages.Page_Home";// "AppDemo.Pages." + btn.Tag.ToString(); KM2_MENU
                //Type type = Type.GetType(path);
                //object obj = type.Assembly.CreateInstance(path);
                ////Type t = System.Reflection.Assembly.GetExecutingAssembly().GetType(path);
                //Page p = obj as Page;
                //this.frmLayout.Content = p;
                //PropertyInfo[] infos = type.GetProperties();
                //foreach (PropertyInfo info in infos)
                //{
                //    if (info.Name == "ParentWin")
                //    {
                //        info.SetValue(p, this, null);
                //        break;
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ShowMiniWindow()
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.frmLayout.Content = page_Home;

            #region 全屏
            if(isfullscreen)
            {
                this.WindowState = System.Windows.WindowState.Normal;
                this.WindowStyle = System.Windows.WindowStyle.None;
                this.ResizeMode  = System.Windows.ResizeMode.NoResize;
                this.Topmost     = false;

                this.Left = 0.0;
                this.Top = 0.0;
                //this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
                //this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;

                Rect rc = SystemParameters.WorkArea;//获取工作区大小--不覆盖任务栏
                this.Width = rc.Width;
                this.Height = rc.Height;

                //Mouse.OverrideCursor = Cursors.None;  隐藏光标---正式发布版本隐藏光标，看起来更像平板
            }
            
            #endregion


            #region 处理子类各类事件：消息回调
            io_com.UpdateMessage           += SubWindow_updateMainwindowMessage;
            net_ntripclient.UpdateMessage  += SubWindow_updateMainwindowMessage;
            net_ntripclient.UpdateRtcmData += UpdateNtripSvrRTCM3Data;
            page_Setting._SendCMDData      += SettingSendCMDData;
            page_Setting.UpdateMessage     += SubWindow_updateMainwindowMessage; 

            page_Home.ParentWin       = this;
            page_DeviceInfo.ParentWin = this;
            page_Setting.ParentWin    = this;
            page_Analogsignal.ParentWin = this;
            page_collect_map_km2.ParentWin = this;
            page_km3_menu.ParentWin = this;
            page_km2_menu.ParentWin = this;
            #endregion


            #region 自适应各控件大小
            page_Home.Width  = this.ActualWidth;
            page_Home.Height = this.ActualHeight;

            page_DeviceInfo.Width = this.ActualWidth;
            page_DeviceInfo.Height = this.ActualHeight;
            #endregion

//            Helper.PlayerText2MP3("欢迎使用 AI CAR 智慧学车系统");
//             if (io_com.Opencom())
//             {
//                 ShowMessage("串口打开成功");
//             }
//             else
//             {
//                 ShowMessage("串口打开失败");
//             }
        }
    }
}
