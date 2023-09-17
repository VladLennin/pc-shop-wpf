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
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;

namespace КР2семестр
{
    /// <summary>
    /// Логика взаимодействия для ShopPage.xaml
    /// </summary>
    public partial class ShopPage : Page
    {
        List<Component> components;
      
        Frame menuFrame;
        User user;
        OleDbConnection dbConnection;
        ObservableCollection<Component> product = new ObservableCollection<Component>();
        List<Component> tempComponents = new List<Component>();
        public ShopPage(Frame menuFrame, User user, List<Component> components,OleDbConnection dbConnection)
        {
            InitializeComponent();       
            this.menuFrame = menuFrame;
            this.user = user;
            this.components = components;
            this.dbConnection = dbConnection;
            for (int i = 0; i < components.Count; i++)
            {
                tempComponents.Add(components[i]);
            }
            FillObserv();

         
            ShopList.ItemsSource = product; 
            
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Border border = (Border)sender;
            StackPanel stackPanel = (StackPanel)border.Child;
            TextBlock textBlock1 = (TextBlock)stackPanel.Children[1];
            TextBlock textBlock2 = (TextBlock)stackPanel.Children[2];
            TextBlock textBlock3 = (TextBlock)stackPanel.Children[3];
            string text = textBlock1.Text + " " + textBlock2.Text + " " + textBlock3.Text;
            for (int i = 0; i < components.Count; i++)
            {
                Component component = components[i];
                string text2 = component.Type + " " + component.Producer + " " + component.Model;
                if (text2 == text)
                {                 
                    menuFrame.Content = new ComponentPage(component,menuFrame,user,dbConnection);
                    break;
                }
            }
        } 
        public void FillObserv()
        {
            product.Clear();
            for (int i = 0; i < tempComponents.Count; i++)
            {
                product.Add(tempComponents[i]);
            }
        }

        #region Filters
        private void CPU_button_filter_Click(object sender, RoutedEventArgs e)
        {
            tempComponents = components.FindAll(x => x.Type == ComponentType.CPU);
            FillObserv();
            ShopList.ItemsSource = product;
        }

        private void MotherBoard_button_filter_Click(object sender, RoutedEventArgs e)
        {
            tempComponents = components.FindAll(x => x.Type == ComponentType.Motherboard);
            FillObserv();
            ShopList.ItemsSource = product;
        }

        private void GPU_button_filter_Click(object sender, RoutedEventArgs e)
        {
            tempComponents = components.FindAll(x => x.Type == ComponentType.GPU);
            FillObserv();
            ShopList.ItemsSource = product;
        }

        private void RAM_button_filter_Click(object sender, RoutedEventArgs e)
        {
            tempComponents = components.FindAll(x => x.Type == ComponentType.RAM);
            FillObserv();
            ShopList.ItemsSource = product;
        }

        private void PowerSupply_button_filter_Click(object sender, RoutedEventArgs e)
        {
            tempComponents = components.FindAll(x => x.Type == ComponentType.PowerSupply);
            FillObserv();
            ShopList.ItemsSource = product;
        }

        private void CoolingSystem_button_filter_Click(object sender, RoutedEventArgs e)
        {
            tempComponents = components.FindAll(x => x.Type == ComponentType.CoolingSystem);
            FillObserv();
            ShopList.ItemsSource = product;
        }

        private void HDD_button_filter_Click(object sender, RoutedEventArgs e)
        {
            tempComponents = components.FindAll(x => x.Type == ComponentType.HDD);
            FillObserv();
            ShopList.ItemsSource = product;
        }

        private void SSD_button_filter_Click(object sender, RoutedEventArgs e)
        {
            tempComponents = components.FindAll(x => x.Type == ComponentType.SSD);
            FillObserv();
            ShopList.ItemsSource = product;
        }

        private void Case_button_filter_Click(object sender, RoutedEventArgs e)
        {
            tempComponents = components.FindAll(x => x.Type == ComponentType.Case);
            FillObserv();
            ShopList.ItemsSource = product;
        }

        private void Repeat_button_filter_Click(object sender, RoutedEventArgs e)
        {
            tempComponents = components;
            FillObserv();
            ShopList.ItemsSource = product;
        }
        #endregion

        private void sortbyRaytingUp_Click(object sender, RoutedEventArgs e)
        {
            tempComponents.Sort((x, y) => y.Rating.CompareTo(x.Rating));
            FillObserv();
            ShopList.ItemsSource = product;
        }

        private void sortbyRaytingDown_Click(object sender, RoutedEventArgs e)
        {
            tempComponents.Sort((x, y) => x.Rating.CompareTo(y.Rating));
            FillObserv();
            ShopList.ItemsSource = product;
        }

        private void sortbyCostUp_Click(object sender, RoutedEventArgs e)
        {
            tempComponents.Sort((x, y) => y.Cost.CompareTo(x.Cost));
            FillObserv();
            ShopList.ItemsSource = product;
        }

        private void sortbycostDown_Click(object sender, RoutedEventArgs e)
        {
            tempComponents.Sort((x, y) => x.Cost.CompareTo(y.Cost));
            FillObserv();
            ShopList.ItemsSource = product;
        }

        private void sortByType_Click(object sender, RoutedEventArgs e)
        {
            tempComponents.Sort((x, y) => x.Type.CompareTo(y.Type));
            FillObserv();
            ShopList.ItemsSource = product;
        }

      

        private void searchBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string search = textBox.Text;
            tempComponents = components.FindAll(x =>x.Model.ToUpper().Contains(search.ToUpper()) || x.Producer.ToUpper().Contains(search.ToUpper()) || x.Type.ToString().ToUpper().Contains(search.ToUpper())); 
            FillObserv();
            ShopList.ItemsSource = product;
        }
    }
}
