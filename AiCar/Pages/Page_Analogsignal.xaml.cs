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
    /// Page_Analogsignal.xaml 的交互逻辑
    /// </summary>
    public partial class Page_Analogsignal : BasePage
    {
        bool bfirst = true;
        public Page_Analogsignal()
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
            string[] strArray = strData.Split('*')[0].Split(',');

            if (strArray[1].Length >= 17)
            {
                string gpgdate = strArray[1].Substring(0, 4) + "-" + strArray[1].Substring(4, 2) + "-" + strArray[1].Substring(6, 2) + " " + strArray[1].Substring(8, 2) + ":" + strArray[1].Substring(10, 2) + ":" + strArray[1].Substring(12, 5);
                DateTime gpstime = Convert.ToDateTime(gpgdate).AddHours(8);
                lab_datetime.Content = gpstime.ToString("yyyy年MM月dd日  ") + gpstime.ToString("HH:mm:ss  ") + Helper.Helper_GetDayName(gpstime.DayOfWeek);
            }
        }

        private void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            if (bfirst)
            {
                //double dheight = canvas_gpsinfo.ActualHeight;
                //double dweight = canvas_gpsinfo.ActualWidth;

                //wrapPanel_menu.Height = dheight;
                //wrapPanel_menu.Width = dweight;

                #region 控件自适应处理
                for (int i=0;i<30;i++)
                    AddWarpPanel_Menu("menu_1_"+i.ToString(),"menu_1_" + i.ToString(),i.ToString());
                #endregion

                bfirst = false;
            }
        }


        private void AddWarpPanel_Menu(string strname,string tagname,string lab_text)
        {
            //Uri uri        = new Uri(@"/images/light_btn.png", UriKind.Relative);
            Uri uri = new Uri("pack://application:,,,/images/light_btn.png", UriKind.Absolute);
            ImageBrush ib  = new ImageBrush();
            ib.ImageSource = new BitmapImage(uri);
            Color color = (Color)ColorConverter.ConvertFromString("#FFf26b29");

            Label lab  = new Label();
            lab.Width  = 64;
            lab.Height = 66;
            //img.Source = new BitmapImage(uri);
            lab.Background                 = ib;
            lab.Content                    = lab_text;
            lab.HorizontalContentAlignment = HorizontalAlignment.Center;
            lab.VerticalContentAlignment   = VerticalAlignment.Center;

            lab.Margin = new Thickness(0,0,20,20);
            lab.Foreground = new SolidColorBrush(color);
            lab.Tag = tagname;

            //lab.MouseLeftButtonUp += Menu_XL_Click;

            wrapPanel_menu.Children.Add(lab);
            wrapPanel_menu.RegisterName(strname, lab);//注册名字，以便以后使用
        }



    }
}
