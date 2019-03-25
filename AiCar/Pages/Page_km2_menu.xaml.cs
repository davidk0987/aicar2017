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
    /// Page_km2_menu.xaml 的交互逻辑
    /// </summary>
    public partial class Page_km2_menu : BasePage
    {
        struct km2_xl_menu
        {
            public string strxlname { get; set; }
            public string strexepath { get; set; }
        }
        List<km2_xl_menu> list = new List<km2_xl_menu>();

        bool bfirst = true;
        DateTime datatime_onclick = new DateTime();

        public Page_km2_menu()
        {
            InitializeComponent();
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
        }

        private void Menu_Click(object sender, MouseButtonEventArgs e)
        {
            Image btn = sender as Image;
            if (btn.Tag.ToString() == "image_home")
            {
                ParentWin.ShowPage("HOME");
            }
        }


        private void ReadXLConfig()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "config/km2menu.ini";
            StreamReader sr = new StreamReader(path, Encoding.UTF8);  //path为文件路径
            String line;
            int i = 0;
            while ((line = sr.ReadLine()) != null)//按行读取 line为每行的数据
            {
                string[] strxl_array = line.Split('=');
                if (strxl_array.Count() == 2)
                {
                    km2_xl_menu _struct_km3_xl_menu = new km2_xl_menu();
                    _struct_km3_xl_menu.strxlname = strxl_array[0];
                    _struct_km3_xl_menu.strexepath = strxl_array[1];
                    list.Add(_struct_km3_xl_menu);
                    AddWarpPanel_Menu("menu_1_" + i.ToString(), i.ToString(), _struct_km3_xl_menu.strxlname);
                    i++;
                }

            }
        }

        private void AddWarpPanel_Menu(string strname, string tagname, string lab_text)
        {
            //Uri uri        = new Uri(@"/images/light_btn.png", UriKind.Relative);
            Uri uri = new Uri("pack://application:,,,/images/km3_xl_btn.png", UriKind.Absolute);
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(uri);
            Color color = (Color)ColorConverter.ConvertFromString("#fffef4e3");

            Label lab = new Label();
            lab.Width = 64;
            lab.Height = 66;
            //img.Source = new BitmapImage(uri);
            lab.Background = ib;
            lab.Content = lab_text;
            lab.HorizontalContentAlignment = HorizontalAlignment.Center;
            lab.VerticalContentAlignment = VerticalAlignment.Center;

            lab.Margin = new Thickness(0, 0, 20, 20);
            lab.Foreground = new SolidColorBrush(color);
            lab.Tag = tagname;

            lab.MouseLeftButtonUp += Menu_XL_Click;


            wrapPanel_menu.Children.Add(lab);
            wrapPanel_menu.RegisterName(strname, lab);//注册名字，以便以后使用
        }

        private void Menu_XL_Click(object sender, MouseButtonEventArgs e)
        {
            double timespane = (DateTime.Now - datatime_onclick).TotalSeconds;

            if (timespane < 3) return;//如果时间小于3秒，返回

            datatime_onclick = DateTime.Now;


            Label btn = sender as Label;
            if (btn.Tag != null)
            {
                try
                {
                    System.Diagnostics.Process.Start(list[Convert.ToInt32(btn.Tag.ToString())].strexepath);    //调用该命令，在程序启动时打开Excel程序
                }
                catch { ShowMessage("未找到程序"); }
            }

        }

        public void ShowMessage(string strData)
        {
            system_messageinfo.Content = strData;
        }

        private void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (bfirst)
            {
                ReadXLConfig();

                bfirst = false;
            }
        }
    }
}
