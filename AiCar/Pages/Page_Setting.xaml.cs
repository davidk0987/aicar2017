using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AiCar.Pages
{
    /// <summary>
    /// Page_Setting.xaml 的交互逻辑
    /// </summary>
    public partial class Page_Setting : BasePage
    {
        public delegate void SettingSendCMDData(byte[] b_cmd);
        public event SettingSendCMDData _SendCMDData = null;

        public delegate void UpdateMainwindowMessage(string labelContent, System_Message_Info itype);
        public event UpdateMainwindowMessage UpdateMessage = null;


        public Page_Setting()
        {
            InitializeComponent();
        }

        public void getradio(string sradio)
        {
            lab_getradio.Content = "当前通道 " + sradio;
            if(Convert.ToInt32(sradio)>-1&& Convert.ToInt32(sradio) < radioSource.Items.Count) radioSource.SelectedIndex = Convert.ToInt32(sradio);

        }
        public void gettranmodel(string sradio)
        {
            lab_gettranmodel.Content = (sradio == "0" ? "内置电台" : "外置电台/网络");
            if (Convert.ToInt32(sradio) > -1 && Convert.ToInt32(sradio) < tranSource.Items.Count) tranSource.SelectedIndex = Convert.ToInt32(sradio);
        }

        public void ShowDeveiceInfo_IOJX(string strData)
        {
            chb_s_ss.IsChecked = strData.Substring(31, 1) == "1" ? true : false;

            chb_s_cm.IsChecked = strData.Substring(30, 1) == "1" ? true : false;
            //29为转速保留，直接跳过
            chb_s_aqd.IsChecked = strData.Substring(28, 1) == "1" ? true : false;

            chb_s_xh.IsChecked = strData.Substring(27, 1) == "1" ? true : false;

            chb_s_dh.IsChecked = strData.Substring(26, 1) == "1" ? true : false;

            chb_s_kd.IsChecked = strData.Substring(25, 1) == "1" ? true : false;

            chb_s_iozw.IsChecked = strData.Substring(24, 1) == "1" ? true : false;

            chb_s_iozzx.IsChecked = strData.Substring(23, 1) == "1" ? true : false;

            chb_s_ioyzx.IsChecked = strData.Substring(22, 1) == "1" ? true : false;

            chb_s_iolb.IsChecked = strData.Substring(21, 1) == "1" ? true : false;

            chb_s_ioys.IsChecked = strData.Substring(20, 1) == "1" ? true : false;

            chb_s_ioygd.IsChecked = strData.Substring(19, 1) == "1" ? true : false;

            chb_s_iojgd.IsChecked = strData.Substring(18, 1) == "1" ? true : false;

            chb_s_ioskd.IsChecked = strData.Substring(17, 1) == "1" ? true : false;

            chb_s_iowd.IsChecked = strData.Substring(16, 1) == "1" ? true : false;

            chb_s_ioscd.IsChecked = strData.Substring(15, 1) == "1" ? true : false;

            chb_s_iofs.IsChecked = strData.Substring(14, 1) == "1" ? true : false;

            chb_s_iodhj.IsChecked = strData.Substring(13, 1) == "1" ? true : false;

            chb_s_iohsj.IsChecked = strData.Substring(12, 1) == "1" ? true : false;

            chb_s_iozwtz.IsChecked = strData.Substring(11, 1) == "1" ? true : false;

            chb_s_iorczq.IsChecked = strData.Substring(10, 1) == "1" ? true : false;

            chb_s_iorcyq.IsChecked = strData.Substring(9, 1) == "1" ? true : false;

            chb_s_iorczh.IsChecked = strData.Substring(8, 1) == "1" ? true : false;

            chb_s_iorcyh.IsChecked = strData.Substring(7, 1) == "1" ? true : false;

            chb_s_ioybp.IsChecked = strData.Substring(6, 1) == "1" ? true : false;

            chb_s_iodcd.IsChecked = strData.Substring(5, 1) == "1" ? true : false;

            chb_s_iolh.IsChecked = strData.Substring(4, 1) == "1" ? true : false;
        }

        private string SetDeveiceInfo_IOJX()
        {
            string strIOJX = "";

            //strIOJX = Helper.RepairZero(strIOJX, 32);

            strIOJX += "1111";

            strIOJX += chb_s_iolh.IsChecked == true ? "1" : "0";//离合
            strIOJX += chb_s_iodcd.IsChecked == true ? "1" : "0";//倒车灯
            strIOJX += chb_s_ioybp.IsChecked == true ? "1" : "0";//摸仪表
            strIOJX += chb_s_iorcyh.IsChecked == true ? "1" : "0";//绕车右后
            strIOJX += chb_s_iorczh.IsChecked == true ? "1" : "0";//绕车左后
            strIOJX += chb_s_iorcyq.IsChecked == true ? "1" : "0";//绕车右前
            strIOJX += chb_s_iorczq.IsChecked == true ? "1" : "0";//绕车左前
            strIOJX += chb_s_iozwtz.IsChecked == true ? "1" : "0";//调整座位
            strIOJX += chb_s_iohsj.IsChecked == true ? "1" : "0";//后视镜
            strIOJX += chb_s_iodhj.IsChecked == true ? "1" : "0";//倒后镜
            strIOJX += chb_s_iofs.IsChecked == true ? "1" : "0";//副刹
            strIOJX += chb_s_ioscd.IsChecked == true ? "1" : "0";//刹车灯
            strIOJX += chb_s_iowd.IsChecked == true ? "1" : "0";//雾灯
            strIOJX += chb_s_ioskd.IsChecked == true ? "1" : "0";//示宽灯
            strIOJX += chb_s_iojgd.IsChecked == true ? "1" : "0";//近光灯
            strIOJX += chb_s_ioygd.IsChecked == true ? "1" : "0";//远光灯
            strIOJX += chb_s_ioys.IsChecked == true ? "1" : "0";//雨刷
            strIOJX += chb_s_iolb.IsChecked == true ? "1" : "0";//喇叭
            strIOJX += chb_s_ioyzx.IsChecked == true ? "1" : "0";//右转向
            strIOJX += chb_s_iozzx.IsChecked == true ? "1" : "0";//左转向
            strIOJX += chb_s_iozw.IsChecked == true ? "1" : "0";//io指纹
            strIOJX += chb_s_kd.IsChecked == true ? "1" : "0";//空档
            strIOJX += chb_s_dh.IsChecked == true ? "1" : "0";//点火
            strIOJX += chb_s_xh.IsChecked == true ? "1" : "0";//熄火
            strIOJX += chb_s_aqd.IsChecked == true ? "1" : "0";//安全带
            strIOJX += "1";//29为转速保留，直接跳过
            strIOJX += chb_s_cm.IsChecked == true ? "1" : "0";//车门
            strIOJX += chb_s_ss.IsChecked == true ? "1" : "0";//手刹

            return strIOJX;
        }


        private void Menu_Click(object sender, MouseButtonEventArgs e)
        {
            Image btn = sender as Image;
            if (btn.Tag.ToString() == "image_home")
            {
                ParentWin.ShowPage("HOME");
            }
            else if (btn.Tag.ToString() == "image_zcm")
            {
                image_zcm.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_zcm_on.png"));
                canvas_zcminfo.Visibility = Visibility.Visible;

                image_tran.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_tran.png")); 
                canvas_traninfo.Visibility = Visibility.Collapsed;


                image_datas.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_datas.png"));
                canvas_datas.Visibility = Visibility.Collapsed;

                image_io_obd_jx.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_io_obd_jx.png"));
                canvas_io_obd_jx.Visibility = Visibility.Collapsed;

                image_obd_use.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_obd_use.png"));
                canvas_obd_use.Visibility = Visibility.Collapsed;

                image_updata.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_update.png"));
                canvas_update.Visibility = Visibility.Collapsed;
            }
            else if (btn.Tag.ToString() == "image_tran")
            {
                image_zcm.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_zcm.png"));
                canvas_zcminfo.Visibility = Visibility.Collapsed;

                image_tran.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_tran_on.png"));
                canvas_traninfo.Visibility = Visibility.Visible;

                image_datas.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_datas.png"));
                canvas_datas.Visibility = Visibility.Collapsed;

                image_io_obd_jx.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_io_obd_jx.png"));
                canvas_io_obd_jx.Visibility = Visibility.Collapsed;

                image_obd_use.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_obd_use.png"));
                canvas_obd_use.Visibility = Visibility.Collapsed;

                image_updata.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_update.png"));
                canvas_update.Visibility = Visibility.Collapsed;
            }
            else if (btn.Tag.ToString() == "image_datas")
            {
                image_zcm.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_zcm.png"));
                canvas_zcminfo.Visibility = Visibility.Collapsed;

                image_tran.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_tran.png"));
                canvas_traninfo.Visibility = Visibility.Collapsed;

                image_datas.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_datas_on.png"));
                canvas_datas.Visibility = Visibility.Visible;

                image_io_obd_jx.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_io_obd_jx.png"));
                canvas_io_obd_jx.Visibility = Visibility.Collapsed;

                image_obd_use.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_obd_use.png"));
                canvas_obd_use.Visibility = Visibility.Collapsed;

                image_updata.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_update.png"));
                canvas_update.Visibility = Visibility.Collapsed;
            }
            else if (btn.Tag.ToString() == "image_io_obd_jx")
            {
                image_zcm.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_zcm.png"));
                canvas_zcminfo.Visibility = Visibility.Collapsed;

                image_tran.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_tran.png"));
                canvas_traninfo.Visibility = Visibility.Collapsed;

                image_datas.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_datas.png"));
                canvas_datas.Visibility = Visibility.Collapsed;

                image_io_obd_jx.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_io_obd_jx_on.png"));
                canvas_io_obd_jx.Visibility = Visibility.Visible;

                image_obd_use.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_obd_use.png"));
                canvas_obd_use.Visibility = Visibility.Collapsed;

                image_updata.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_update.png"));
                canvas_update.Visibility = Visibility.Collapsed;
            }
            else if (btn.Tag.ToString() == "image_obd_use")
            {
                image_zcm.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_zcm.png"));
                canvas_zcminfo.Visibility = Visibility.Collapsed;

                image_tran.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_tran.png"));
                canvas_traninfo.Visibility = Visibility.Collapsed;

                image_datas.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_datas.png"));
                canvas_datas.Visibility = Visibility.Collapsed;

                image_io_obd_jx.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_io_obd_jx.png"));
                canvas_io_obd_jx.Visibility = Visibility.Collapsed;

                image_obd_use.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_obd_use_on.png"));
                canvas_obd_use.Visibility = Visibility.Visible;

                image_updata.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_update.png"));
                canvas_update.Visibility = Visibility.Collapsed;
            }
            else if (btn.Tag.ToString() == "image_updata")
            {
                image_zcm.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_zcm.png"));
                canvas_zcminfo.Visibility = Visibility.Collapsed;

                image_tran.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_tran.png"));
                canvas_traninfo.Visibility = Visibility.Collapsed;

                image_datas.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_datas.png"));
                canvas_datas.Visibility = Visibility.Collapsed;

                image_io_obd_jx.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_io_obd_jx.png"));
                canvas_io_obd_jx.Visibility = Visibility.Collapsed;

                image_obd_use.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_obd_use.png"));
                canvas_obd_use.Visibility = Visibility.Collapsed;

                image_updata.Source = new BitmapImage(new Uri("pack://application:,,,/images/setting_update_on.png"));
                canvas_update.Visibility = Visibility.Visible;
            }
        }
        
        private void KeyBoard_Click(object sender, MouseButtonEventArgs e)
        {
            Image btn = sender as Image;
            KeyBoard_Show(btn.Tag.ToString());

        }

        private void SendCMDData(byte[] d)
        {
            _SendCMDData?.Invoke(d);
        }

        private void KeyBoard_Show(string skey)
        {
            
            skey.ToUpper();
            if (skey.ToUpper() == "BACK")
            {
                if (lab_keyboard_zcm.Content.ToString() != "")
                {
                    string str_back = lab_keyboard_zcm.Content.ToString();
                    lab_keyboard_zcm.Content = str_back.Substring(0, str_back.Length - 1);
                }
                return;
            }

            if (skey.ToUpper() == "OK")
            {
                string str0503 = lab_keyboard_zcm.Content.ToString().Replace("-", "");
                if (str0503.Length != 16)
                {
                    ShowMessage("输入授权码不正确，未能授权");
                    return;
                }

                if (Regex.IsMatch(str0503, @"^[\d|A-F]*$") == false)
                {
                    ShowMessage("授权码必须为0-F之间的字符串");
                    return;
                }

                byte[] b_0503 = Device_CMD.Set_Device_CMD_0x0503(str0503);
                ShowMessage("正在发送授权码.....");
                if (b_0503.Length > 0) SendCMDData(b_0503);
                return;
            }

            if (lab_keyboard_zcm.Content.ToString().Length > 18) return;


            string str = lab_keyboard_zcm.Content.ToString().Replace("-", "");

            

            string strNew = "";
            for (int i = 0; i < str.Length; i++)
            {
                strNew += str.Substring(i, 1);
                if ((i + 1) % 4 == 0)
                {
                    strNew += "-";
                }
            }


            lab_keyboard_zcm.Content = strNew + skey;
        }

        public void ShowMessage(string strData)
        {

            //strData = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss   ") + strData;
            //             Dispatcher.BeginInvoke(new Action(delegate
            //             {
            //                 system_messageinfo.Content = strData;
            //             }));

            system_messageinfo.Content = strData;
        }

        public void Aly_KSXT(string strData)
        {
            string[] strArray = strData.Split('*')[0].Split(',');

            if (strArray[1].Length >= 17)
            {
                string gpgdate = strArray[1].Substring(0, 4) + "-" + strArray[1].Substring(4, 2) + "-" + strArray[1].Substring(6, 2) + " " + strArray[1].Substring(8, 2) + ":" + strArray[1].Substring(10, 2) + ":" + strArray[1].Substring(12, 5);
                DateTime gpstime = Convert.ToDateTime(gpgdate).AddHours(8);
                lab_datetime.Content = gpstime.ToString("yyyy年MM月dd日  ") + gpstime.ToString("HH:mm:ss  ") + Helper.Helper_GetDayName(gpstime.DayOfWeek);
            }

            lab_zcm_sn.Content = "设备编号：" + strArray[28];
            lab_zcm_zcm.Content = "授权期至：" + strArray[30];

            #region IO 极性显示
            //lab_s_ss.Content = /*"手    刹："+ */getSerontype(strArray[22].Substring(1, 1) ,strArray[20].Substring(1, 1),strArray[21].Substring(1, 1),"拉起","放下");
            getSerontype("lab_s_ss", strArray[20].Substring(1, 1),  "拉起", "放下");
            //lab_s_cm.Content = /*"车    门：" + */getSerontype(strArray[22].Substring(2, 1), strArray[20].Substring(2, 1), strArray[21].Substring(2, 1), "关门", "开门");
            getSerontype("lab_s_cm", strArray[20].Substring(2, 1), "关门", "开门");
            //3为转速定义，略过
            //lab_s_aqd.Content = /*"安全带：" + */getSerontype(strArray[22].Substring(4, 1), strArray[20].Substring(4, 1), strArray[21].Substring(4, 1), "系上", "解开");
            getSerontype("lab_s_aqd", strArray[20].Substring(4, 1), "系上", "解开");
            //lab_s_aqd.Foreground = "#ffffff";
            //lab_s_xh.Content = /*"熄    火：" + */getSerontype(strArray[22].Substring(5, 1), strArray[20].Substring(5, 1), strArray[21].Substring(5, 1), "熄火", "着车");
            getSerontype("lab_s_xh", strArray[20].Substring(5, 1), "熄火", "着车");
            //lab_s_dh.Content = /*"点    火：" + */getSerontype(strArray[22].Substring(6, 1), strArray[20].Substring(6, 1), strArray[21].Substring(6, 1), "点火", "常态");
            getSerontype("lab_s_dh", strArray[20].Substring(6, 1), "点火", "常态");
            //lab_s_kd.Content = /*"空    档：" + */getSerontype(strArray[22].Substring(7, 1), strArray[20].Substring(7, 1), strArray[21].Substring(7, 1), "空档", "挂档");
            getSerontype("lab_s_kd", strArray[20].Substring(7, 1), "空档", "挂档");
            //lab_s_iozw.Content = /*"IO指纹：" + */getSerontype(strArray[22].Substring(8, 1), strArray[20].Substring(8, 1), strArray[21].Substring(8, 1), "触摸", "常态");
            getSerontype("lab_s_iozw", strArray[20].Substring(8, 1),  "按指纹", "未按指纹");
            //lab_s_iozzx.Content = /*"左转向：" + */getSerontype(strArray[22].Substring(9, 1), strArray[20].Substring(9, 1), strArray[21].Substring(9, 1), "打开", "关闭");
            getSerontype("lab_s_iozzx", strArray[20].Substring(9, 1), "亮起", "未亮");
            //lab_s_ioyzx.Content = /*"右转向：" + */getSerontype(strArray[22].Substring(10, 1), strArray[20].Substring(10, 1), strArray[21].Substring(10, 1), "打开", "关闭");
            getSerontype("lab_s_ioyzx", strArray[20].Substring(10, 1), "亮起", "未亮");
            //lab_s_iolb.Content = /*"喇    叭：" + */getSerontype(strArray[22].Substring(11, 1), strArray[20].Substring(11, 1), strArray[21].Substring(11, 1), "按下", "松开");
            getSerontype("lab_s_iolb", strArray[20].Substring(11, 1), "鸣笛", "未鸣笛");
            //lab_s_ioys.Content = /*"雨    刷：" + */getSerontype(strArray[22].Substring(12, 1), strArray[20].Substring(12, 1), strArray[21].Substring(12, 1), "打开", "关闭");
            getSerontype("lab_s_ioys", strArray[20].Substring(12, 1), "打开", "关闭");
            //lab_s_ioygd.Content = /*"远光灯：" + */getSerontype(strArray[22].Substring(13, 1), strArray[20].Substring(13, 1), strArray[21].Substring(13, 1), "打开", "关闭");
            getSerontype("lab_s_ioygd", strArray[20].Substring(13, 1), "亮起", "未亮");
            //lab_s_iojgd.Content = /*"近光灯：" + */getSerontype(strArray[22].Substring(14, 1), strArray[20].Substring(14, 1), strArray[21].Substring(14, 1), "打开", "关闭");
            getSerontype("lab_s_iojgd",  strArray[20].Substring(14, 1), "亮起", "未亮");
            //lab_s_ioskd.Content = /*"示廓灯：" + */getSerontype(strArray[22].Substring(15, 1), strArray[20].Substring(15, 1), strArray[21].Substring(15, 1), "打开", "关闭");
            getSerontype("lab_s_ioskd", strArray[20].Substring(15, 1),  "亮起", "未亮");
            //lab_s_iowd.Content = /*"雾    灯：" + */getSerontype(strArray[22].Substring(16, 1), strArray[20].Substring(16, 1), strArray[21].Substring(16, 1), "打开", "关闭");
            getSerontype("lab_s_iowd",  strArray[20].Substring(16, 1), "亮起", "未亮");
            //lab_s_ioscd.Content = /*"刹    车：" + */getSerontype(strArray[22].Substring(17, 1), strArray[20].Substring(17, 1), strArray[21].Substring(17, 1), "踩下", "松开");
            getSerontype("lab_s_ioscd", strArray[20].Substring(17, 1), "踩下", "松开");
            //lab_s_iofs.Content = /*"副    刹：" + */getSerontype(strArray[22].Substring(18, 1), strArray[20].Substring(18, 1), strArray[21].Substring(18, 1), "踩下", "松开");
            getSerontype("lab_s_iofs", strArray[20].Substring(18, 1),  "踩下", "松开");
            //lab_s_iodhj.Content = /*"倒后镜：" + */getSerontype(strArray[22].Substring(19, 1), strArray[20].Substring(19, 1), strArray[21].Substring(19, 1), "触摸", "离开");
            getSerontype("lab_s_iodhj",  strArray[20].Substring(19, 1), "触摸", "未碰触");
            //lab_s_iohsj.Content = /*"后视镜：" + */getSerontype(strArray[22].Substring(20, 1), strArray[20].Substring(20, 1), strArray[21].Substring(20, 1), "触摸", "离开");
            getSerontype("lab_s_iohsj",  strArray[20].Substring(20, 1),  "触摸", "未碰触");
            //lab_s_iozwtz.Content = /*"座    位：" + */getSerontype(strArray[22].Substring(21, 1), strArray[20].Substring(21, 1), strArray[21].Substring(21, 1), "调整", "常态");
            getSerontype("lab_s_iozwtz",  strArray[20].Substring(21, 1),  "调整", "未调整");
            //lab_s_iorczq.Content = /*"绕左前：" + */getSerontype(strArray[22].Substring(22, 1), strArray[20].Substring(22, 1), strArray[21].Substring(22, 1), "经过", "离开");
            getSerontype("lab_s_iorczq",  strArray[20].Substring(22, 1),  "经过", "离开");
            //lab_s_iorcyq.Content = /*"绕右前：" + */getSerontype(strArray[22].Substring(23, 1), strArray[20].Substring(23, 1), strArray[21].Substring(23, 1), "经过", "离开");
            getSerontype("lab_s_iorcyq", strArray[20].Substring(23, 1),  "经过", "离开");
            //lab_s_iorczh.Content = /*"绕左后：" + */getSerontype(strArray[22].Substring(24, 1), strArray[20].Substring(24, 1), strArray[21].Substring(24, 1), "经过", "离开");
            getSerontype("lab_s_iorczh", strArray[20].Substring(24, 1),  "经过", "离开");
            //lab_s_iorcyh.Content = /*"绕右后：" + */getSerontype(strArray[22].Substring(25, 1), strArray[20].Substring(25, 1), strArray[21].Substring(25, 1), "经过", "离开");
            getSerontype("lab_s_iorcyh", strArray[20].Substring(25, 1),  "经过", "离开");
            //lab_s_ioybp.Content = /*"仪表盘：" + */getSerontype(strArray[22].Substring(26, 1), strArray[20].Substring(26, 1), strArray[21].Substring(26, 1), "查看", "常态");
            getSerontype("lab_s_ioybp", strArray[20].Substring(26, 1),  "触摸", "未碰触");
            //lab_s_iodcd.Content = /*"倒车灯：" + */getSerontype(strArray[22].Substring(27, 1), strArray[20].Substring(27, 1), strArray[21].Substring(27, 1), "倒车", "前进");
            getSerontype("lab_s_iodcd", strArray[20].Substring(27, 1),  "亮起", "未亮");
            //lab_s_iolh.Content = /*"离    合：" + */getSerontype(strArray[22].Substring(28, 1), strArray[20].Substring(28, 1), strArray[21].Substring(28, 1), "踩下", "松开");
            getSerontype("lab_s_iolh", strArray[20].Substring(28, 1), "踩下", "松开");
            #endregion

            #region OBD使用状态显示
            //lab_o_ss.Content = /*"手    刹："+ */getSerontype(strArray[22].Substring(1, 1) ,strArray[21].Substring(1, 1),strArray[21].Substring(1, 1),"拉起","放下");
            getSerontype("lab_o_ss", strArray[21].Substring(1, 1), "拉起", "放下");
            //lab_o_cm.Content = /*"车    门：" + */getSerontype(strArray[22].Substring(2, 1), strArray[21].Substring(2, 1), strArray[21].Substring(2, 1), "关门", "开门");
            getSerontype("lab_o_cm", strArray[21].Substring(2, 1), "关门", "开门");
            //3为转速定义，略过
            //lab_o_aqd.Content = /*"安全带：" + */getSerontype(strArray[22].Substring(4, 1), strArray[21].Substring(4, 1), strArray[21].Substring(4, 1), "系上", "解开");
            getSerontype("lab_o_aqd", strArray[21].Substring(4, 1), "系上", "解开");
            //lab_o_aqd.Foreground = "#ffffff";
            //lab_o_xh.Content = /*"熄    火：" + */getSerontype(strArray[22].Substring(5, 1), strArray[21].Substring(5, 1), strArray[21].Substring(5, 1), "熄火", "着车");
            getSerontype("lab_o_xh", strArray[21].Substring(5, 1), "熄火", "着车");
            //lab_o_dh.Content = /*"点    火：" + */getSerontype(strArray[22].Substring(6, 1), strArray[21].Substring(6, 1), strArray[21].Substring(6, 1), "点火", "常态");
            getSerontype("lab_o_dh", strArray[21].Substring(6, 1), "点火", "常态");
            //lab_o_kd.Content = /*"空    档：" + */getSerontype(strArray[22].Substring(7, 1), strArray[21].Substring(7, 1), strArray[21].Substring(7, 1), "空档", "挂档");
            getSerontype("lab_o_kd", strArray[21].Substring(7, 1), "空档", "挂档");
            //lab_o_iozw.Content = /*"IO指纹：" + */getSerontype(strArray[22].Substring(8, 1), strArray[21].Substring(8, 1), strArray[21].Substring(8, 1), "触摸", "常态");
            getSerontype("lab_o_iozw", strArray[21].Substring(8, 1), "按指纹", "未按指纹");
            //lab_o_iozzx.Content = /*"左转向：" + */getSerontype(strArray[22].Substring(9, 1), strArray[21].Substring(9, 1), strArray[21].Substring(9, 1), "打开", "关闭");
            getSerontype("lab_o_iozzx", strArray[21].Substring(9, 1), "亮起", "未亮");
            //lab_o_ioyzx.Content = /*"右转向：" + */getSerontype(strArray[22].Substring(10, 1), strArray[21].Substring(10, 1), strArray[21].Substring(10, 1), "打开", "关闭");
            getSerontype("lab_o_ioyzx", strArray[21].Substring(10, 1), "亮起", "未亮");
            //lab_o_iolb.Content = /*"喇    叭：" + */getSerontype(strArray[22].Substring(11, 1), strArray[21].Substring(11, 1), strArray[21].Substring(11, 1), "按下", "松开");
            getSerontype("lab_o_iolb", strArray[21].Substring(11, 1), "鸣笛", "未鸣笛");
            //lab_o_ioys.Content = /*"雨    刷：" + */getSerontype(strArray[22].Substring(12, 1), strArray[21].Substring(12, 1), strArray[21].Substring(12, 1), "打开", "关闭");
            getSerontype("lab_o_ioys", strArray[21].Substring(12, 1), "打开", "关闭");
            //lab_o_ioygd.Content = /*"远光灯：" + */getSerontype(strArray[22].Substring(13, 1), strArray[21].Substring(13, 1), strArray[21].Substring(13, 1), "打开", "关闭");
            getSerontype("lab_o_ioygd", strArray[21].Substring(13, 1), "亮起", "未亮");
            //lab_o_iojgd.Content = /*"近光灯：" + */getSerontype(strArray[22].Substring(14, 1), strArray[21].Substring(14, 1), strArray[21].Substring(14, 1), "打开", "关闭");
            getSerontype("lab_o_iojgd", strArray[21].Substring(14, 1), "亮起", "未亮");
            //lab_o_ioskd.Content = /*"示廓灯：" + */getSerontype(strArray[22].Substring(15, 1), strArray[21].Substring(15, 1), strArray[21].Substring(15, 1), "打开", "关闭");
            getSerontype("lab_o_ioskd", strArray[21].Substring(15, 1), "亮起", "未亮");
            //lab_o_iowd.Content = /*"雾    灯：" + */getSerontype(strArray[22].Substring(16, 1), strArray[21].Substring(16, 1), strArray[21].Substring(16, 1), "打开", "关闭");
            getSerontype("lab_o_iowd", strArray[21].Substring(16, 1), "亮起", "未亮");
            //lab_o_ioscd.Content = /*"刹    车：" + */getSerontype(strArray[22].Substring(17, 1), strArray[21].Substring(17, 1), strArray[21].Substring(17, 1), "踩下", "松开");
            getSerontype("lab_o_ioscd", strArray[21].Substring(17, 1), "踩下", "松开");
            //lab_o_iofs.Content = /*"副    刹：" + */getSerontype(strArray[22].Substring(18, 1), strArray[21].Substring(18, 1), strArray[21].Substring(18, 1), "踩下", "松开");
            getSerontype("lab_o_iofs", strArray[21].Substring(18, 1), "踩下", "松开");
            //lab_o_iodhj.Content = /*"倒后镜：" + */getSerontype(strArray[22].Substring(19, 1), strArray[21].Substring(19, 1), strArray[21].Substring(19, 1), "触摸", "离开");
            getSerontype("lab_o_iodhj", strArray[21].Substring(19, 1), "触摸", "未碰触");
            //lab_o_iohsj.Content = /*"后视镜：" + */getSerontype(strArray[22].Substring(20, 1), strArray[21].Substring(20, 1), strArray[21].Substring(20, 1), "触摸", "离开");
            getSerontype("lab_o_iohsj", strArray[21].Substring(20, 1), "触摸", "未碰触");
            //lab_o_iozwtz.Content = /*"座    位：" + */getSerontype(strArray[22].Substring(21, 1), strArray[21].Substring(21, 1), strArray[21].Substring(21, 1), "调整", "常态");
            getSerontype("lab_o_iozwtz", strArray[21].Substring(21, 1), "调整", "未调整");
            //lab_o_iorczq.Content = /*"绕左前：" + */getSerontype(strArray[22].Substring(22, 1), strArray[21].Substring(22, 1), strArray[21].Substring(22, 1), "经过", "离开");
            getSerontype("lab_o_iorczq", strArray[21].Substring(22, 1), "经过", "离开");
            //lab_o_iorcyq.Content = /*"绕右前：" + */getSerontype(strArray[22].Substring(23, 1), strArray[21].Substring(23, 1), strArray[21].Substring(23, 1), "经过", "离开");
            getSerontype("lab_o_iorcyq", strArray[21].Substring(23, 1), "经过", "离开");
            //lab_o_iorczh.Content = /*"绕左后：" + */getSerontype(strArray[22].Substring(24, 1), strArray[21].Substring(24, 1), strArray[21].Substring(24, 1), "经过", "离开");
            getSerontype("lab_o_iorczh", strArray[21].Substring(24, 1), "经过", "离开");
            //lab_o_iorcyh.Content = /*"绕右后：" + */getSerontype(strArray[22].Substring(25, 1), strArray[21].Substring(25, 1), strArray[21].Substring(25, 1), "经过", "离开");
            getSerontype("lab_o_iorcyh", strArray[21].Substring(25, 1), "经过", "离开");
            //lab_o_ioybp.Content = /*"仪表盘：" + */getSerontype(strArray[22].Substring(26, 1), strArray[21].Substring(26, 1), strArray[21].Substring(26, 1), "查看", "常态");
            getSerontype("lab_o_ioybp", strArray[21].Substring(26, 1), "触摸", "未碰触");
            //lab_o_iodcd.Content = /*"倒车灯：" + */getSerontype(strArray[22].Substring(27, 1), strArray[21].Substring(27, 1), strArray[21].Substring(27, 1), "倒车", "前进");
            getSerontype("lab_o_iodcd", strArray[21].Substring(27, 1), "亮起", "未亮");
            //lab_o_iolh.Content = /*"离    合：" + */getSerontype(strArray[22].Substring(28, 1), strArray[21].Substring(28, 1), strArray[21].Substring(28, 1), "踩下", "松开");
            getSerontype("lab_o_iolh", strArray[21].Substring(28, 1), "踩下", "松开");
            #endregion

        }

        private void getSerontype(string strlab_naem, string xh, string strzt_on, string strzt_out)
        {
            object obj = this.GetType().GetField(strlab_naem, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase).GetValue(this);

            Label p = obj as Label;


            if (xh == "1")
            {
                p.Content = strzt_on + "  I";
                Color color = (Color)ColorConverter.ConvertFromString("#FFf26b29");
                p.Foreground = new SolidColorBrush(color);
            }
            else
            {
                p.Content = strzt_out + "  I";
                Color color = (Color)ColorConverter.ConvertFromString("#FF359542");
                p.Foreground = new SolidColorBrush(color);
            }


        }


        private void lab_btn_Click(object sender, MouseButtonEventArgs e)
        {
            Label btn = sender as Label;
            if (btn.Tag.ToString() == "lab_btn_getradio")
            {
                byte[] b_030C = Device_CMD.Get_Device_CMD_0x030C();
                ShowMessage("正在获取当前电台通道号.....");
                if (b_030C.Length > 0) SendCMDData(b_030C);
            }
            else if (btn.Tag.ToString() == "lab_btn_gettranmodel")
            {
                byte[] b = Device_CMD.Get_Device_CMD_0x0903();
                ShowMessage("正在获取当前数据传输模式.....");
                if (b.Length > 0) SendCMDData(b);
            }
            else if (btn.Tag.ToString() == "lab_btn_setradionum")
            {
                //ShowMessage("", setiohearttime)
                UpdateMessage?.Invoke("-30", System_Message_Info.setiohearttime);
                
                byte[] b = Device_CMD.Set_Device_CMD_0x030A(radioSource.SelectedIndex);
                ShowMessage("正在更改当前电台通道 [" + radioSource.Text + "].....");
                if (b.Length > 0) SendCMDData(b);

            }
            else if (btn.Tag.ToString() == "lab_btn_settrantype") 
            {
                byte[] b = Device_CMD.Set_Device_CMD_0x0905(tranSource.SelectedIndex);
                ShowMessage("正在更改当前差分数据传输模式 [" + tranSource.Text + "].....");
                if (b.Length > 0) SendCMDData(b);
            }
            else if (btn.Tag.ToString() == "lab_btn_io_getjx")
            {
                byte[] b = Device_CMD.Get_Device_CMD_0x0601();
                ShowMessage("正在获取IO极性.....");
                if (b.Length > 0) SendCMDData(b);
            }
            else if (btn.Tag.ToString() == "lab_btn_io_setjx")
            {
                //byte[] b = Device_CMD.Get_Device_CMD_0x0601();
                string strIOJX = SetDeveiceInfo_IOJX();

                byte[] b = Device_CMD.Set_Device_CMD_0x0603(strIOJX);
                ShowMessage("正在设置IO极性.....");
                if (b.Length > 0) SendCMDData(b);
            }
            else if(btn.Tag.ToString()== "lab_btn_selectfilepath")//选择bin文件
            {
                var openFileDialog = new Microsoft.Win32.OpenFileDialog()
                {
                    Filter = "Excel Files (*.bin)|*.bin"
                };
                var result = openFileDialog.ShowDialog();
                if (result == true)
                {
                    lab_update_local.Content = openFileDialog.FileName;
                }
            }

            else if (btn.Tag.ToString() == "lab_btn_updatefile")//选择bin文件
            {
                if(lab_update_local.Content.ToString() == "")
                {
                    ShowMessage("请先选择文件.....");
                    return;
                }
                Read_binFile();
            }
        }

        private int pAppFileCount_Now = 0;
        private int pAppFileCount = 0;
        private int pAppFileLen = 0;
        private byte[] pAppFileData = new byte[1024];
        private void Read_binFile()
        {
            UpdateMessage?.Invoke("-300", System_Message_Info.setiohearttime);//暂停发送心跳

            string filepath_app = lab_update_local.Content.ToString();
            FileStream fs_app   = new FileStream(filepath_app, FileMode.Open);// 读取方式打开，得到流
            fs_app.Seek(0, SeekOrigin.Begin);          // 定位到第0个字节
            pAppFileLen = (int)fs_app.Length;
            byte[] app_file = new byte[pAppFileLen];
            fs_app.Read(app_file, 0, pAppFileLen);     // 开始读取，读取的内容放到datas数组里，0是从第一个开始放，datas.length是最多允许放多少个
            fs_app.Close();
            pAppFileData = app_file;
            for (int i = 0; i < pAppFileLen; i++)
            {
                pAppFileData[i] = (byte)Helper.invers(pAppFileData[i]);
            }

            pAppFileCount_Now = 0;
            pAppFileCount = pAppFileLen / 1024;
            //int amod = len % 200;
            if (pAppFileLen % 1024 > 1)
            {
                pAppFileCount += 1;
            }

            //ShowMessage(pAppFileCount.ToString()+" -- "+ pAppFileLen.ToString());
            //return;
            SendCMDData(Device_CMD.Set_Device_CMD_0x0104());//重启主机
            System.Threading.Thread.Sleep(300);
            SendCMDData(Device_CMD.Set_Device_CMD_0x0300(pAppFileCount, pAppFileLen));//开始升级固件
            System.Threading.Thread.Sleep(600);
        }

        public void SendAppFile()
        {
            if (pAppFileCount_Now < 0)
            {
                return;
            }

            if (pAppFileCount_Now == 0)
            {
                ShowMessage("校验成功，开始升级，请勿断开电源....");
            }

            pAppFileCount_Now++;

            System.Threading.Thread.Sleep(200);

            if (pAppFileCount_Now == pAppFileCount + 1)
            {
                //发送完毕

                SendCMDData(Device_CMD.Set_Device_CMD_0x0302());
                ShowMessage("升级完成");

                pAppFileCount_Now = -1;
                pAppFileLen = 0;
                pAppFileCount = 0;
                System.Threading.Thread.Sleep(200);

                UpdateMessage?.Invoke("-10", System_Message_Info.setiohearttime);//5秒后发送心跳唤醒主机
                return;
            }

            SendCMDData(Device_CMD.Get_Device_CMD_0x0301(pAppFileData,pAppFileCount, pAppFileLen, pAppFileCount_Now));//发送文件数据包
            double per = Convert.ToDouble(pAppFileCount_Now) / Convert.ToDouble(pAppFileCount) * 100;
            ShowMessage("升级进度：" + per.ToString("0.00") + "%");
            //Device_CMD.Get_Device_CMD_0x0301();
        }

        private void saveconfig_Click(object sender, MouseButtonEventArgs e)
        {
            string strfilepaht = AppDomain.CurrentDomain.BaseDirectory + "\\config\\config.cfg";

            Helper.IniWriteValue("ntrip", "isuse", (isusentrip.IsChecked==true?"1":"0"), strfilepaht);
            Helper.IniWriteValue("ntrip", "ip", textbox_ntrip_svr_name.Text, strfilepaht);
            Helper.IniWriteValue("ntrip", "basesn", textbox_ntrip_moution.Text, strfilepaht);
            Helper.IniWriteValue("serial", "com", dataserialnum.Text, strfilepaht);


            ShowMessage("保存配置成功");
        }

        private void loadconfig()
        {
            string strfilepaht = AppDomain.CurrentDomain.BaseDirectory + "\\config\\config.cfg";

            isusentrip.IsChecked        = Helper.IniReadValue("ntrip", "isuse", strfilepaht) == "1" ? true : false;
            textbox_ntrip_svr_name.Text = Helper.IniReadValue("ntrip", "ip",  strfilepaht);
            textbox_ntrip_moution.Text  = Helper.IniReadValue("ntrip", "basesn", strfilepaht);
            dataserialnum.Text          =  Helper.IniReadValue("serial", "com", strfilepaht);
        }

        private void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            loadconfig();
        }

        private void Textbox_ntrip_svr_name_GotFocus(object sender, RoutedEventArgs e)
        {
            //Process.Start(Environment.GetEnvironmentVariable("systemdrive")+@"\WINDOWS\system32\osk.exe");
            Helper.popupkeyboard();
        }

        private void Textbox_ntrip_moution_GotFocus(object sender, RoutedEventArgs e)
        {
            Helper.popupkeyboard(); //Process.Start(Environment.GetEnvironmentVariable("systemdrive") + @"\WINDOWS\system32\osk.exe");
        }


        ////////////////////////////////////////////////////////
    }
}
