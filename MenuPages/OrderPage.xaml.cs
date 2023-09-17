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
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        public OrderPage(List<Component> orderProducts)
        {
            InitializeComponent();
            ordersList.Items.Clear();
            for (int i = 0; i < orderProducts.Count; i++)
            {
                if (!orderProducts[i].Image.Contains("Resources")) 
                {
                    string path = Environment.CurrentDirectory;
                    string[] temp = path.Split('\\');
                    temp[temp.Length - 1] = "";
                    temp[temp.Length - 2] = "";
                    path = "";
                    for (int f = 0; f < temp.Length; f++)
                    {
                        if (temp[f] != "")
                            path += temp[f] + "\\";
                    }
                    path += $"Resources\\{orderProducts[i].Image}";
                    orderProducts[i].Image = path;
                }
            }
            
            ordersList.ItemsSource = orderProducts;
        }
    }
}
