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
    /// Page_Home_Menu_Map.xaml 的交互逻辑
    /// </summary>
    public partial class Page_Home_Menu_Map : UserControl
    {
        public Page_Home_Menu_Map()
        {
            InitializeComponent();
        }

        private void Viewbox_Loaded(object sender, RoutedEventArgs e)
        {
            //stackpanel_menu.Width = this.Width;
            //stackpanel_menu.Height = this.Height;
        }
    }
}
