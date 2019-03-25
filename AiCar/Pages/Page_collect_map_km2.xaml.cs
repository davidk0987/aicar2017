using System;
using System.Collections.Generic;
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
    /// Page_collect_map_km2.xaml 的交互逻辑
    /// </summary>
    public partial class Page_collect_map_km2 : BasePage
    {
        public Page_collect_map_km2()
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
        }

        public void Aly_KSXT(string strData)
        {
            try
            {
                string[] strArray = strData.Split('*')[0].Split(',');

                if (strArray[1].Length >= 17)
                {
                    string gpgdate = strArray[1].Substring(0, 4) + "-" + strArray[1].Substring(4, 2) + "-" + strArray[1].Substring(6, 2) + " " + strArray[1].Substring(8, 2) + ":" + strArray[1].Substring(10, 2) + ":" + strArray[1].Substring(12, 5);
                    DateTime gpstime = Convert.ToDateTime(gpgdate).AddHours(8);
                    lab_datetime.Content = gpstime.ToString("yyyy年MM月dd日  ") + gpstime.ToString("HH:mm:ss  ") + Helper.Helper_GetDayName(gpstime.DayOfWeek);
                }

                lab_star_dingwei_info.Content = string.Format("定位状态：前（{0}）- 后（{1}）", Helper.Helper_GetGpsStatus(strArray[11]), Helper.Helper_GetGpsStatus(strArray[10]));
                lab_star_info.Content = string.Format("卫星数量：前（{0}）   - 后（{1}）", strArray[12].PadLeft(2, '0'), strArray[13].PadLeft(2, '0'));
                lab_horizontal_error.Content = "水平误差：" + strArray[38];

                lab_coorder_east.Content = "东向坐标：" + strArray[14];
                lab_coorder_north.Content = "北向坐标：" + strArray[15];
                lab_coorder_sky.Content = "天向坐标：" + strArray[16];
            }
            catch { }            
        }



    }
}
