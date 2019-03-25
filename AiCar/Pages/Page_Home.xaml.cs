using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Page_Home.xaml 的交互逻辑
    /// </summary>
    public partial class Page_Home : BasePage
    {
        bool bfirst = true;
        public Page_Home()
        {
            InitializeComponent();
        }

        private void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (bfirst)
            {
                #region 控件自适应处理
                //wrappanel_menu.ActualWidth;//
                double dMenu_height =  stackpanel_menu_1.ActualHeight / 6;
                stackpanel_menu_1_1.Height = dMenu_height;
                stackpanel_menu_1_2.Height = dMenu_height;
                stackpanel_menu_1_3.Height = dMenu_height;
                stackpanel_menu_1_4.Height = dMenu_height;
                double dMenu_Width = (stackpanel_menu_1_1.ActualWidth-20) / 3;
                menu_km2.Width = menu_exit.Width = menu_km3.Width = dMenu_Width;
                menu_km2.Height = menu_exit.Height = menu_km3.Height = dMenu_height;

                menu_km3_1.Width = dMenu_Width;
                menu_km3_1.Height = dMenu_height;


                menu_collectmap_km2.Width = dMenu_Width;
                menu_collectmap_km2.Height = dMenu_height;

                menu_collectmap_km3.Width = dMenu_Width;
                menu_collectmap_km3.Height = dMenu_height;

                menu_setting.Width = dMenu_Width;
                menu_setting.Height = dMenu_height;

                menu_deviceinfo.Width = dMenu_Width;
                menu_deviceinfo.Height = dMenu_height;

                menu_analogsignal.Width = dMenu_Width;
                menu_analogsignal.Height = dMenu_height;
                #endregion

                #region 控件信息初始化
                //编译时间作为版本号
                version.Content = "Ver 1.0." + System.IO.File.GetLastWriteTime(this.GetType().Assembly.Location).ToString("yyMMddHHmmss");// System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                sys_date.Content = DateTime.Now.ToString("yyyy年MM月dd日  ") + Helper.Helper_GetDayName(DateTime.Now.DayOfWeek);
                sys_date_time.Content = DateTime.Now.ToString("HH:mm:ss.fff");
                //image_wxqr.Source = Helper.Helper_BitmapToBitmapImage(Properties.Resources.wx_qr);
                #endregion

                bfirst = false;
            }


            
            
           
        }
        private void Menu_Click(object sender, MouseButtonEventArgs e)
        {
            Label btn = sender as Label;
            if (btn.Tag != null)
            {
                if(btn.Tag.ToString() == "menu_exit")
                {
                    Application.Current.Shutdown();
                    //ParentWin.ShowMessage("12345678888");
                }
                else if (btn.Tag.ToString() == "menu_km2")
                {
                    ParentWin.ShowPage("KM2_MENU");


                    //ParentWin.ShowPage("KM2");
                    //ParentWin.ShowMessage("12345678888");
                    //string strfilepaht = AppDomain.CurrentDomain.BaseDirectory + "\\config\\config.cfg";
                    //string strexepath = AppDomain.CurrentDomain.BaseDirectory  + Helper.IniReadValue("external", "km2", strfilepaht);
                    //try
                    //{
                    //    System.Diagnostics.Process.Start(strexepath);　　　　//调用该命令，在程序启动时打开Excel程序
                    //}
                    //catch { ShowMessage("未找到程序"); }
                    
                }
                else if (btn.Tag.ToString() == "menu_km3")
                {
                    ParentWin.ShowPage("KM3_MENU");
                    //ParentWin.ShowMessage("12345678888");
//                     string strfilepaht = AppDomain.CurrentDomain.BaseDirectory + "\\config\\config.cfg";
//                     string strexepath = AppDomain.CurrentDomain.BaseDirectory + Helper.IniReadValue("external", "km3", strfilepaht);
//                     try
//                     {
//                         System.Diagnostics.Process.Start(strexepath);　　　　//调用该命令，在程序启动时打开Excel程序
//                     }
//                     catch { ShowMessage("未找到程序"); }
                }
                else if (btn.Tag.ToString() == "menu_km3_1")
                {
                    //ParentWin.ShowPage("KM3");
                    //ParentWin.ShowMessage("12345678888");
                    string strfilepaht = AppDomain.CurrentDomain.BaseDirectory + "\\config\\config.cfg";
                    string strexepath = AppDomain.CurrentDomain.BaseDirectory + Helper.IniReadValue("external", "km3_1", strfilepaht);
                    try
                    {
                        System.Diagnostics.Process.Start(strexepath);　　　　//调用该命令，在程序启动时打开Excel程序
                    }
                    catch { ShowMessage("未找到程序"); }
                }
                else if (btn.Tag.ToString() == "menu_deviceinfo")
                {
                    ParentWin.ShowPage("DEVICEINFO");
                }
                else if (btn.Tag.ToString() == "menu_setting")
                {
                    ParentWin.ShowPage("SETTING");
                }
                else if (btn.Tag.ToString() == "menu_analogsignal")
                {
                    //ParentWin.ShowPage("ANALOGSIGNAL");

                    string strexepath = AppDomain.CurrentDomain.BaseDirectory + "\\km3_map\\map.exe";
                    try
                    {
                        System.Diagnostics.Process.Start(strexepath);    //调用该命令，在程序启动时打开Excel程序
                    }
                    catch { ShowMessage("未找到程序"); }
                }
                else if(btn.Tag.ToString() == "menu_collectmap_km3")
                {
                    //弹出消息框
                    //Page_Home_Menu_Map box = new Page_Home_Menu_Map();
                    //box.ShowDialog(ParentWin);
                    //this._loading.Visibility = Visibility.Visible;

                    //                     string strfilepaht = AppDomain.CurrentDomain.BaseDirectory + "\\config\\config.cfg";
                    string strexepath = AppDomain.CurrentDomain.BaseDirectory + "\\km3_map\\map.exe";
                    try
                    {
                        System.Diagnostics.Process.Start(strexepath);    //调用该命令，在程序启动时打开Excel程序
                    }
                    catch { ShowMessage("未找到程序"); }
                }
                else if (btn.Tag.ToString() == "menu_collectmap_km2")
                {
                    //ParentWin.ShowPage("COLLECT_MAP_KM2");
                    //弹出消息框
                    //Page_Home_Menu_Map box = new Page_Home_Menu_Map();
                    //box.ShowDialog(ParentWin);
                    //this._loading.Visibility = Visibility.Visible;

                    //                     string strfilepaht = AppDomain.CurrentDomain.BaseDirectory + "\\config\\config.cfg";
                    string strexepath = AppDomain.CurrentDomain.BaseDirectory + "\\km2_map\\map.exe";
                    try
                    {
                        System.Diagnostics.Process.Start(strexepath);    //调用该命令，在程序启动时打开Excel程序
                    }
                    catch { ShowMessage("未找到程序"); }
                }




                //try
                //{
                //    string path = "AppDemo.Pages." + btn.Tag.ToString();
                //    Type type = Type.GetType(path);
                //    object obj = type.Assembly.CreateInstance(path);
                //    //Type t = System.Reflection.Assembly.GetExecutingAssembly().GetType(path);
                //    Page p = obj as Page;
                //    this.frmLayout.Content = p;
                //    PropertyInfo[] infos = type.GetProperties();
                //    foreach (PropertyInfo info in infos)
                //    {
                //        if (info.Name == "ParentWin")
                //        {
                //            info.SetValue(p, this, null);
                //            break;
                //        }
                //    }
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message);
                //}

            }
        }
        public void Ntrip_Message(string message)
        {
            Dispatcher.BeginInvoke(new Action(delegate
            {
                sys_deviceinfo_rtcm3.Content = "网络差分：" + message + " Byte";

            }));
        }
        public void Aly_KSXT(string strData)
        {
            string[] strArray = strData.Split('*')[0].Split(',');

            if (strArray.Count() < 44) return;

            try
            {
                if (strArray[1].Length >= 17)
                {
                    string gpgdate = strArray[1].Substring(0, 4) + "-" + strArray[1].Substring(4, 2) + "-" + strArray[1].Substring(6, 2) + " " + strArray[1].Substring(8, 2) + ":" + strArray[1].Substring(10, 2) + ":" + strArray[1].Substring(12, 5);
                    DateTime gpstime = Convert.ToDateTime(gpgdate).AddHours(8);
                    sys_date_time.Content = gpstime.ToString("HH:mm:ss.ff");
                    sys_date.Content = gpstime.ToString("yyyy年MM月dd日  ") + Helper.Helper_GetDayName(gpstime.DayOfWeek);
                }

                sys_deviceinfo_gpsstatus.Content = string.Format("定位状态：前（{0}）- 后（{1}）", Helper.Helper_GetGpsStatus(strArray[11]), Helper.Helper_GetGpsStatus(strArray[10]));
                sys_deviceinfo_gpsstarcount.Content = string.Format("卫星数量：前（{0}）   - 后（{1}）", strArray[12].PadLeft(2, '0'), strArray[13].PadLeft(2, '0'));
                sys_deviceinfo_sn.Content = "设备编号：" + strArray[28];
                sys_deviceinfo_limidate.Content = "授权期至：" + strArray[30];

                sys_deviceinfo_basesn.Content = "设备编号：" + strArray[44];
                sys_deviceinfo_base_limidate.Content = "授权期至：" + strArray[45];

                double ddxzb = strArray[14] == "" ? 0.0 : Convert.ToDouble(strArray[14]);
                double dbxzb = strArray[15] == "" ? 0.0 : Convert.ToDouble(strArray[15]);

                sys_deviceinfo_basejuli.Content = "基站距离：" + System.Math.Sqrt(ddxzb * ddxzb + dbxzb * dbxzb).ToString("f3");
            }
            catch { }
            
        }
        public void ShowMessage(string strData)
        {

            //strData = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss   ") + strData;
            system_messageinfo.Content = strData;

        }
    }
}
