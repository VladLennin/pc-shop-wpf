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

namespace КР2семестр
{
    /// <summary>
    /// Логика взаимодействия для AccountAssambly.xaml
    /// </summary>
    public partial class AccountAssambly : Page
    {
        List<Component> components;
        public AccountAssambly(List<Component> components)
        {
            InitializeComponent();
           
            this.components = components;
            assamblyList.ItemsSource = components;
        }
        public AccountAssambly()
        {
            InitializeComponent();

            border1.Child= new TextBlock() { Text = "Your dont have assemblies :(", Foreground = Brushes.LightGray, FontSize = 55,Margin = new Thickness(0,100, 0 , 0), HorizontalAlignment = HorizontalAlignment.Center };
        }
    }
}
