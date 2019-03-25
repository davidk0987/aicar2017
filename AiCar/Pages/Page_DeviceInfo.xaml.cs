using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AiCar.Pages
{
    /// <summary>
    /// Page_DeviceInfo.xaml 的交互逻辑
    /// </summary>
    public partial class Page_DeviceInfo : BasePage
    {
        public Page_DeviceInfo()
        {
            InitializeComponent();
        }

        private void Menu_Click(object sender, MouseButtonEventArgs e)
        {
            Image btn = sender as Image;
            if (btn.Tag.ToString() == "image_home")
            {
                ParentWin.ShowPage("HOME");
            }
            else if(btn.Tag.ToString() == "image_gps")
            {
                image_gps.Source = new BitmapImage(new Uri("pack://application:,,,/images/deviceinfo_gpsinfo_menu_on.png")); ///new BitmapImage(new Uri(@"/images/deviceinfo_gpsinfo_menu_on.png", UriKind.Relative));
                image_seron.Source = new BitmapImage(new Uri("pack://application:,,,/images/deviceinfo_senorinfo_menu.png")); //new BitmapImage(new Uri(@"/images/deviceinfo_senorinfo_menu.png", UriKind.Relative));
                canvas_gpsinfo.Visibility = Visibility.Visible;
                canvas_seroninfo.Visibility = Visibility.Collapsed;
            }
            else if (btn.Tag.ToString() == "image_seron")
            {
                //image_gps.Source = new BitmapImage(new Uri(@"/images/deviceinfo_gpsinfo_menu.pn", UriKind.Relative));
                //image_seron.Source = new BitmapImage(new Uri(@"/images/deviceinfo_senorinfo_menu_on.pn", UriKind.Relative));
                image_gps.Source = new BitmapImage(new Uri("pack://application:,,,/images/deviceinfo_gpsinfo_menu.png")); ///new BitmapImage(new Uri(@"/images/deviceinfo_gpsinfo_menu_on.png", UriKind.Relative));
                image_seron.Source = new BitmapImage(new Uri("pack://application:,,,/images/deviceinfo_senorinfo_menu_on.png")); //new BitmapImage(new Uri(@"/images/deviceinfo_senorinfo_menu.png", UriKind.Relative));

                canvas_gpsinfo.Visibility = Visibility.Collapsed;
                canvas_seroninfo.Visibility = Visibility.Visible;
            }
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

            lab_gpstime.Content = "GPS时间：" + strArray[1];
            lab_sn.Content = "设备编号：" + strArray[28];
            lab_ver.Content = "固件版本：" + strArray[29];
            lab_limidate.Content = "授权期至：" + strArray[30];
            lab_gnh.Content = "授权编码：" + strArray[31];
            lab_txjl.Content = "天线距离：" + strArray[41];
            lab_lat.Content = "终端纬度：" + strArray[3];
            lab_lng.Content = "终端经度：" + strArray[2];
            lab_alt.Content = "终端海拔：" + strArray[4];
            lab_dxzb.Content = "东向坐标：" + strArray[14];
            lab_bxzb.Content = "北向坐标：" + strArray[15];
            lab_txzb.Content = "天向坐标：" + strArray[16];
            lab_spwc.Content = "水平误差：" + strArray[38];
            lab_gcwc.Content = "高程误差：" + strArray[39];
            lab_cfyc.Content = "差分延迟：" + strArray[40];
            lab_dxsd.Content = "东向速度：" + strArray[17];
            lab_bxsd.Content = "北向速度：" + strArray[18];
            lab_txsd.Content = "天向速度：" + strArray[19];
            lab_dwq.Content = "定位 (前)：" + strArray[11];
            lab_wxq.Content = "卫星 (前)：" + strArray[12];
            lab_pdop.Content = "PDOP    ：" + strArray[36];
            lab_dwh.Content = "定位 (后)：" + strArray[10];
            lab_dwh.Content = "定位 (后)：" + strArray[10];
            lab_wxh.Content = "卫星 (后)：" + strArray[13];
            lab_hdop.Content = "HDOP    ：" + strArray[37];
            lab_fwj.Content = "方位角   ：" + strArray[5];
            lab_fyj.Content = "俯仰角   ：" + strArray[6];
            lab_hgj.Content = "横滚角   ：" + strArray[9];
            lab_fwjwc.Content = "方位角误差：" + strArray[42];
            lab_fyjwc.Content = "俯仰角误差：" + strArray[43];
            lab_gpssd.Content = "GPS速度：" + strArray[8];

            double ddxzb = strArray[14] == "" ? 0.0 : Convert.ToDouble(strArray[14]);
            double dbxzb = strArray[15] == "" ? 0.0 : Convert.ToDouble(strArray[15]);
            lab_zdjjl.Content = "终端间距离：" + System.Math.Sqrt(ddxzb * ddxzb + dbxzb * dbxzb).ToString("f3");


            ////////差分终端信息
            lab_basesn.Content = "设备编号：" + strArray[44];
            lab_base_limidata.Content = "授权期至：" + strArray[45];
            lab_base_gnh.Content = "授权编码：" + strArray[46];

            lab_base_lat.Content = "终端纬度：" + strArray[33];
            lab_base_lng.Content = "终端经度：" + strArray[32];
            lab_base_alt.Content = "终端海拔：" + strArray[34];





            ///////////////////////////传感器
            lab_iozx.Content = "IO    转速：" + strArray[23]; /*+ "转/每分"*/;
            lab_obdzx.Content = "OBD转速：" + strArray[24]; /*+ "转/每分"*/;
            lab_iodw.Content = "IO    档位：" + strArray[20].Substring(0, 1);// + strArray[20] == "" ? "" : strArray[20].Substring(0, 1);
            lab_obddw.Content = "OBD档位：" + strArray[21].Substring(0, 1); ;// + strArray[21] == "" ? "" : strArray[21].Substring(0, 1);
            lab_obdfxpjd.Content = "OBD方向盘角度：" + strArray[27];
            lab_obdyszt.Content = "OBD    钥匙状态：" + strArray[26];
            lab_obdsd.Content = "OBD速度：" + strArray[25];

            //IntPtr hwnd = ((HwndSource)PresentationSource.FromVisual(lab_s_ss)).Handle;
            //Label p = (Label)HwndSource.FromHwnd(hwnd).RootVisual;
            //p.Content = "2222";




            //lab_s_ss.Content = /*"手    刹："+ */getSerontype(strArray[22].Substring(1, 1) ,strArray[20].Substring(1, 1),strArray[21].Substring(1, 1),"拉起","放下");
            getSerontype("lab_s_ss", strArray[22].Substring(1, 1), strArray[20].Substring(1, 1), strArray[21].Substring(1, 1), "拉起", "放下");
            //lab_s_cm.Content = /*"车    门：" + */getSerontype(strArray[22].Substring(2, 1), strArray[20].Substring(2, 1), strArray[21].Substring(2, 1), "关门", "开门");
            getSerontype("lab_s_cm", strArray[22].Substring(2, 1), strArray[20].Substring(2, 1), strArray[21].Substring(2, 1), "关门", "开门");
            //3为转速定义，略过
            //lab_s_aqd.Content = /*"安全带：" + */getSerontype(strArray[22].Substring(4, 1), strArray[20].Substring(4, 1), strArray[21].Substring(4, 1), "系上", "解开");
            getSerontype("lab_s_aqd", strArray[22].Substring(4, 1), strArray[20].Substring(4, 1), strArray[21].Substring(4, 1), "系上", "解开");
            //lab_s_aqd.Foreground = "#ffffff";
            //lab_s_xh.Content = /*"熄    火：" + */getSerontype(strArray[22].Substring(5, 1), strArray[20].Substring(5, 1), strArray[21].Substring(5, 1), "熄火", "着车");
            getSerontype("lab_s_xh", strArray[22].Substring(5, 1), strArray[20].Substring(5, 1), strArray[21].Substring(5, 1), "熄火", "着车");
            //lab_s_dh.Content = /*"点    火：" + */getSerontype(strArray[22].Substring(6, 1), strArray[20].Substring(6, 1), strArray[21].Substring(6, 1), "点火", "常态");
            getSerontype("lab_s_dh", strArray[22].Substring(6, 1), strArray[20].Substring(6, 1), strArray[21].Substring(6, 1), "点火", "常态");
            //lab_s_kd.Content = /*"空    档：" + */getSerontype(strArray[22].Substring(7, 1), strArray[20].Substring(7, 1), strArray[21].Substring(7, 1), "空档", "挂档");
            getSerontype("lab_s_kd", strArray[22].Substring(7, 1), strArray[20].Substring(7, 1), strArray[21].Substring(7, 1), "空档", "挂档");
            //lab_s_iozw.Content = /*"IO指纹：" + */getSerontype(strArray[22].Substring(8, 1), strArray[20].Substring(8, 1), strArray[21].Substring(8, 1), "触摸", "常态");
            getSerontype("lab_s_iozw", strArray[22].Substring(8, 1), strArray[20].Substring(8, 1), strArray[21].Substring(8, 1), "按指纹", "未按指纹");
            //lab_s_iozzx.Content = /*"左转向：" + */getSerontype(strArray[22].Substring(9, 1), strArray[20].Substring(9, 1), strArray[21].Substring(9, 1), "打开", "关闭");
            getSerontype("lab_s_iozzx", strArray[22].Substring(9, 1), strArray[20].Substring(9, 1), strArray[21].Substring(9, 1), "亮起", "未亮");
            //lab_s_ioyzx.Content = /*"右转向：" + */getSerontype(strArray[22].Substring(10, 1), strArray[20].Substring(10, 1), strArray[21].Substring(10, 1), "打开", "关闭");
            getSerontype("lab_s_ioyzx", strArray[22].Substring(10, 1), strArray[20].Substring(10, 1), strArray[21].Substring(10, 1), "亮起", "未亮");
            //lab_s_iolb.Content = /*"喇    叭：" + */getSerontype(strArray[22].Substring(11, 1), strArray[20].Substring(11, 1), strArray[21].Substring(11, 1), "按下", "松开");
            getSerontype("lab_s_iolb", strArray[22].Substring(11, 1), strArray[20].Substring(11, 1), strArray[21].Substring(11, 1), "鸣笛", "未鸣笛");
            //lab_s_ioys.Content = /*"雨    刷：" + */getSerontype(strArray[22].Substring(12, 1), strArray[20].Substring(12, 1), strArray[21].Substring(12, 1), "打开", "关闭");
            getSerontype("lab_s_ioys", strArray[22].Substring(12, 1), strArray[20].Substring(12, 1), strArray[21].Substring(12, 1), "打开", "关闭");
            //lab_s_ioygd.Content = /*"远光灯：" + */getSerontype(strArray[22].Substring(13, 1), strArray[20].Substring(13, 1), strArray[21].Substring(13, 1), "打开", "关闭");
            getSerontype("lab_s_ioygd", strArray[22].Substring(13, 1), strArray[20].Substring(13, 1), strArray[21].Substring(13, 1), "亮起", "未亮");
            //lab_s_iojgd.Content = /*"近光灯：" + */getSerontype(strArray[22].Substring(14, 1), strArray[20].Substring(14, 1), strArray[21].Substring(14, 1), "打开", "关闭");
            getSerontype("lab_s_iojgd", strArray[22].Substring(14, 1), strArray[20].Substring(14, 1), strArray[21].Substring(14, 1), "亮起", "未亮");
            //lab_s_ioskd.Content = /*"示廓灯：" + */getSerontype(strArray[22].Substring(15, 1), strArray[20].Substring(15, 1), strArray[21].Substring(15, 1), "打开", "关闭");
            getSerontype("lab_s_ioskd", strArray[22].Substring(15, 1), strArray[20].Substring(15, 1), strArray[21].Substring(15, 1), "亮起", "未亮");
            //lab_s_iowd.Content = /*"雾    灯：" + */getSerontype(strArray[22].Substring(16, 1), strArray[20].Substring(16, 1), strArray[21].Substring(16, 1), "打开", "关闭");
            getSerontype("lab_s_iowd", strArray[22].Substring(16, 1), strArray[20].Substring(16, 1), strArray[21].Substring(16, 1), "亮起", "未亮");
            //lab_s_ioscd.Content = /*"刹    车：" + */getSerontype(strArray[22].Substring(17, 1), strArray[20].Substring(17, 1), strArray[21].Substring(17, 1), "踩下", "松开");
            getSerontype("lab_s_ioscd", strArray[22].Substring(17, 1), strArray[20].Substring(17, 1), strArray[21].Substring(17, 1), "踩下", "松开");
            //lab_s_iofs.Content = /*"副    刹：" + */getSerontype(strArray[22].Substring(18, 1), strArray[20].Substring(18, 1), strArray[21].Substring(18, 1), "踩下", "松开");
            getSerontype("lab_s_iofs", strArray[22].Substring(18, 1), strArray[20].Substring(18, 1), strArray[21].Substring(18, 1), "踩下", "松开");
            //lab_s_iodhj.Content = /*"倒后镜：" + */getSerontype(strArray[22].Substring(19, 1), strArray[20].Substring(19, 1), strArray[21].Substring(19, 1), "触摸", "离开");
            getSerontype("lab_s_iodhj", strArray[22].Substring(19, 1), strArray[20].Substring(19, 1), strArray[21].Substring(19, 1), "触摸", "未碰触");
            //lab_s_iohsj.Content = /*"后视镜：" + */getSerontype(strArray[22].Substring(20, 1), strArray[20].Substring(20, 1), strArray[21].Substring(20, 1), "触摸", "离开");
            getSerontype("lab_s_iohsj", strArray[22].Substring(20, 1), strArray[20].Substring(20, 1), strArray[21].Substring(20, 1), "触摸", "未碰触");
            //lab_s_iozwtz.Content = /*"座    位：" + */getSerontype(strArray[22].Substring(21, 1), strArray[20].Substring(21, 1), strArray[21].Substring(21, 1), "调整", "常态");
            getSerontype("lab_s_iozwtz", strArray[22].Substring(21, 1), strArray[20].Substring(21, 1), strArray[21].Substring(21, 1), "调整", "未调整");
            //lab_s_iorczq.Content = /*"绕左前：" + */getSerontype(strArray[22].Substring(22, 1), strArray[20].Substring(22, 1), strArray[21].Substring(22, 1), "经过", "离开");
            getSerontype("lab_s_iorczq", strArray[22].Substring(22, 1), strArray[20].Substring(22, 1), strArray[21].Substring(22, 1), "经过", "离开");
            //lab_s_iorcyq.Content = /*"绕右前：" + */getSerontype(strArray[22].Substring(23, 1), strArray[20].Substring(23, 1), strArray[21].Substring(23, 1), "经过", "离开");
            getSerontype("lab_s_iorcyq", strArray[22].Substring(23, 1), strArray[20].Substring(23, 1), strArray[21].Substring(23, 1), "经过", "离开");
            //lab_s_iorczh.Content = /*"绕左后：" + */getSerontype(strArray[22].Substring(24, 1), strArray[20].Substring(24, 1), strArray[21].Substring(24, 1), "经过", "离开");
            getSerontype("lab_s_iorczh", strArray[22].Substring(24, 1), strArray[20].Substring(24, 1), strArray[21].Substring(24, 1), "经过", "离开");
            //lab_s_iorcyh.Content = /*"绕右后：" + */getSerontype(strArray[22].Substring(25, 1), strArray[20].Substring(25, 1), strArray[21].Substring(25, 1), "经过", "离开");
            getSerontype("lab_s_iorcyh", strArray[22].Substring(25, 1), strArray[20].Substring(25, 1), strArray[21].Substring(25, 1), "经过", "离开");
            //lab_s_ioybp.Content = /*"仪表盘：" + */getSerontype(strArray[22].Substring(26, 1), strArray[20].Substring(26, 1), strArray[21].Substring(26, 1), "查看", "常态");
            getSerontype("lab_s_ioybp", strArray[22].Substring(26, 1), strArray[20].Substring(26, 1), strArray[21].Substring(26, 1), "触摸", "未碰触");
            //lab_s_iodcd.Content = /*"倒车灯：" + */getSerontype(strArray[22].Substring(27, 1), strArray[20].Substring(27, 1), strArray[21].Substring(27, 1), "倒车", "前进");
            getSerontype("lab_s_iodcd", strArray[22].Substring(27, 1), strArray[20].Substring(27, 1), strArray[21].Substring(27, 1), "亮起", "未亮");
            //lab_s_iolh.Content = /*"离    合：" + */getSerontype(strArray[22].Substring(28, 1), strArray[20].Substring(28, 1), strArray[21].Substring(28, 1), "踩下", "松开");
            getSerontype("lab_s_iolh", strArray[22].Substring(28, 1), strArray[20].Substring(28, 1), strArray[21].Substring(28, 1), "踩下", "松开");
        }

     
        private string getSerontype(string isuseobd,string io,string obd,string strzt_on, string strzt_out)
        {
            if(isuseobd == "0")//IO信号
            {
                if(io == "")
                {
                    return "";
                }

                return (io=="1"? strzt_on: strzt_out) + "  I";
            }
            else//OBD信号
            {
                if (obd == "")
                {
                    return "";
                }

                return (obd == "1" ? strzt_on : strzt_out) + "  O";
            }
        }

        private void getSerontype(string strlab_naem,string isuseobd, string io, string obd, string strzt_on, string strzt_out)
        {
            object obj = this.GetType().GetField(strlab_naem, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase).GetValue(this);
            
            Label p = obj as Label;
            

            if (isuseobd == "0")//IO信号
            {
                if (io == "")
                {
                    return;
                }

                if(io == "1")
                {
                    p.Content = strzt_on + "  I";
                    Color color = (Color)ColorConverter.ConvertFromString("#FFf26b29");
                    p.Foreground = new SolidColorBrush(color);
                    return;
                }
                else
                {
                    p.Content = strzt_out + "  I";
                    Color color = (Color)ColorConverter.ConvertFromString("#FF359542");
                    p.Foreground = new SolidColorBrush(color);
                    return;
                }
                //return (io == "1" ? strzt_on : strzt_out) + "  I";
            }
            else//OBD信号
            {
                if (obd == "")
                {
                    return;
                }

                if (obd == "1")
                {
                    p.Content = strzt_on + "  0";
                    Color color = (Color)ColorConverter.ConvertFromString("#FFf26b29");
                    p.Foreground = new SolidColorBrush(color);
                    return;
                }
                else
                {
                    p.Content = strzt_out + "  0";
                    Color color = (Color)ColorConverter.ConvertFromString("#FF359542");
                    p.Foreground = new SolidColorBrush(color);
                    return;
                }
            }
        }


        /////////////////////////////////////////////////////////////////////////////
    }
}
