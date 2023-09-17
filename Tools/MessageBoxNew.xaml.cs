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
using System.Windows.Shapes;

namespace КР2семестр
{
    /// <summary>
    /// Логика взаимодействия для MessageBoxNew.xaml
    /// </summary>
    public  partial class MessageBoxNew 
    {
        public  MessageBoxNew(string text)
        {
            InitializeComponent();
            MessageText.Text = text;
        }
       
        private void OKbutton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
