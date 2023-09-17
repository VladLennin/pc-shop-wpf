using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
namespace КР2семестр
{
    /// <summary>
    /// Логика взаимодействия для BasketPage.xaml
    /// </summary>
    public partial class BasketPage : Page
    {
        User user;
        OleDbConnection dbConnection;
        List<User> users;
        ObservableCollection<Component> basket = new ObservableCollection<Component>();
        List<Component> components;
        public BasketPage(User user, OleDbConnection dbConnection, List<Component> components,List<User>users)
        {
            InitializeComponent();
            this.users = users;
            departmentDelivery.IsChecked = true;
            PaymentOnDelivery.IsChecked = true;
            this.user = user;
            this.dbConnection = dbConnection;
            this.components = components;
            if (user.Name != "***")
                nameBox.Text = user.Name;
            if (user.Surname != "***")
                surnameBox.Text = user.Surname;


            basketList.ItemsSource = basket;
            for (int i = 0; i < user.Basket.Count; i++)
            {
                basket.Add(user.Basket[i]);
            }

            OrderCost();

            if (basket.Count == 0)
            {
                basketPanel.Children.Add(new TextBlock() { Text = "Your basket is empty :(", Foreground = Brushes.LightGray, FontSize = 55 , HorizontalAlignment = HorizontalAlignment.Center});
            }
        }
        public void OrderCost()
        {
            double ordercost=0;
            for (int i = 0; i < user.Basket.Count; i++)
            {
                ordercost += user.Basket[i].Cost * user.Basket[i].Count;
            }
            orderCost.Text = Convert.ToString(ordercost) + " uah";
        }
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            StackPanel stackPanel1 = (StackPanel)button.Parent;
            Grid grid = (Grid)stackPanel1.Parent;
            StackPanel stackPanel2 = (StackPanel)grid.Children[0];
            string type = ((TextBlock)stackPanel2.Children[1]).Text;
            string producer = ((TextBlock)stackPanel2.Children[2]).Text;
            string model = ((TextBlock)stackPanel2.Children[3]).Text;
            Component component = new CPU();
            for (int i = 0; i < user.Basket.Count; i++)
            {
                if(user.Basket[i].Type.ToString() == type&& user.Basket[i].Producer==producer&& user.Basket[i].Model==model)
                {
                     component = user.Basket[i];
                }
            }
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i].Type.ToString() == type && components[i].Producer == producer && components[i].Model == model)
                {
                    components[i].Count = components[i].Count + component.Count;
                }
            } 
            basket.Remove(component);
            dbConnection.Open();
            string query = $"DELETE FROM basket WHERE productCode ={component.ProductCode} and userCode = {user.UserCode}";
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            dbCommand.ExecuteNonQuery();
            query = $"SELECT [count] FROM products WHERE productCode = {component.ProductCode}";
            dbCommand = new OleDbCommand(query, dbConnection);
            OleDbDataReader dbReader = dbCommand.ExecuteReader();
            int tempCount = 0;
            while(dbReader.Read())
            {
                tempCount = (int)dbReader["count"];
            }
            tempCount = tempCount + component.Count;
            query = $"UPDATE products SET [count] = {tempCount} WHERE productCode = {component.ProductCode}";
            dbCommand = new OleDbCommand(query, dbConnection);
            dbCommand.ExecuteNonQuery();
            user.Basket.Remove(component);
            dbConnection.Close();
            OrderCost();
            if(basket.Count==0)
            {
                basketPanel.Children.Add(new TextBlock() { Text = "Your basket is empty :(", Foreground = Brushes.LightGray,FontSize = 55, HorizontalAlignment = HorizontalAlignment.Center });
            }
        }

        private void minusButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            StackPanel stackPanel1 = (StackPanel)button.Parent;
            Grid grid = (Grid)stackPanel1.Parent;
            StackPanel stackPanel2 = (StackPanel)grid.Children[0];
            string type = ((TextBlock)stackPanel2.Children[1]).Text;
            string producer = ((TextBlock)stackPanel2.Children[2]).Text;
            string model = ((TextBlock)stackPanel2.Children[3]).Text;
            Component component = new CPU();
            bool check = false;

            string query = " ";
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            dbConnection.Open();
            for (int i = 0; i < user.Basket.Count; i++)
            {
                if (user.Basket[i].Type.ToString() == type && user.Basket[i].Producer == producer && user.Basket[i].Model == model)
                {
                    if (user.Basket[i].Count > 1)
                    {
                         user.Basket[i].Count--;
                        query = $"UPDATE basket SET [count] = {user.Basket[i].Count} WHERE productCode ={user.Basket[i].ProductCode} and userCode = {user.UserCode}";
                        dbCommand = new OleDbCommand(query, dbConnection);
                        dbCommand.ExecuteNonQuery();
                        check = true;
                        basket.Clear();
                        for (int h = 0; h < user.Basket.Count; h++)
                        {
                            basket.Add(user.Basket[h]);
                        }
                    }
                }
            }

            for (int i = 0; i < components.Count; i++)
            {
                if (components[i].Type.ToString() == type && components[i].Producer == producer && components[i].Model == model && check)
                {
                    components[i].Count++;

                    query = $"UPDATE products SET [count] = {components[i].Count}  WHERE productCode = {components[i].ProductCode}";
                    dbCommand = new OleDbCommand(query, dbConnection);
                    dbCommand.ExecuteNonQuery();
                }
            }           
               dbConnection.Close();
            OrderCost();
        }



      
        private void plusButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            StackPanel stackPanel1 = (StackPanel)button.Parent;
            Grid grid = (Grid)stackPanel1.Parent;
            StackPanel stackPanel2 = (StackPanel)grid.Children[0];
            string type = ((TextBlock)stackPanel2.Children[1]).Text;
            string producer = ((TextBlock)stackPanel2.Children[2]).Text;
            string model = ((TextBlock)stackPanel2.Children[3]).Text;
            bool check = false;


            string query = " ";
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            dbConnection.Open();
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i].Type.ToString() == type && components[i].Producer == producer && components[i].Model == model )
                {
                    if (components[i].Count > 0)
                    {
                        components[i].Count--;

                        query = $"UPDATE products SET [count] = {components[i].Count}  WHERE productCode = {components[i].ProductCode}";
                        dbCommand = new OleDbCommand(query, dbConnection);
                        dbCommand.ExecuteNonQuery();
                        check = true;
                        break;
                    }
                }
            }
            for (int i = 0; i < user.Basket.Count; i++)
            {
                
                if (user.Basket[i].Type.ToString() == type && user.Basket[i].Producer == producer && user.Basket[i].Model == model && check)
                {
                    
                        user.Basket[i].Count++;
                        query = $"UPDATE basket SET [count] = {user.Basket[i].Count} WHERE productCode ={user.Basket[i].ProductCode} and userCode = {user.UserCode}";
                        dbCommand = new OleDbCommand(query, dbConnection);
                        dbCommand.ExecuteNonQuery();
                    basket.Clear();
                    for (int h = 0; h < user.Basket.Count; h++)
                    {
                        basket.Add(user.Basket[h]);
                    }
                    break;
                }
            }

            dbConnection.Close();
            OrderCost();
        }

        private void createOrder_Click(object sender, RoutedEventArgs e)
        {
            string typeOfDelivery = "";
            string typeOfPaymant = " ";
            if (courierDelivery.IsChecked == true)
                typeOfDelivery = "Courier";
            else if (departmentDelivery.IsChecked == true)
                typeOfDelivery = "Department";

            if (PayByPurse.IsChecked == true)
                typeOfPaymant = "By purse";
            else if (PaymentOnDelivery.IsChecked == true)
                typeOfPaymant = "Pay by delivery";
            Order order = new Order(nameBox.Text, surnameBox.Text, addresBox.Text, user.Basket, typeOfDelivery,typeOfPaymant,orderCost.Text);
 
            
            User.WriteOrderToBD(dbConnection, order, user);
            user.ReadOrdersFromBD(user, dbConnection);
            if(user.Name == "admin"&&user.Surname =="admin")
            {
                for (int i = 0; i < users.Count; i++)
                {
                    users[i].ReadOrdersFromBD(users[i], dbConnection);
                }
            }
            basket.Clear();
            basketPanel.Children.Add(new TextBlock() { Text = "Your basket is empty :(", Foreground = Brushes.LightGray, FontSize = 55, HorizontalAlignment = HorizontalAlignment.Center });
            nameBox.Text = "";
            surnameBox.Text = "";
            addresBox.Text = "";

            
        }

        private void courierDelivery_Checked(object sender, RoutedEventArgs e)
        {
            departmentDelivery.IsChecked = false;
          
            
        }

        private void departmentDelivery_Checked(object sender, RoutedEventArgs e)
        {
            courierDelivery.IsChecked = false;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
