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
namespace КР2семестр
{
    /// <summary>
    /// Логика взаимодействия для AccountPage.xaml
    /// </summary>
    public partial class AccountPage : Page
    {
        User user;
        OleDbConnection dbConnection;
        Order currentOrder;
        Assembly currentAssemble;
        List<Component> components;


        TextBox newLogin = new TextBox();
        TextBox newPass = new TextBox();
        TextBox newNumber = new TextBox();
        TextBox newMail = new TextBox();
        TextBox newName = new TextBox();
        TextBox newSurname = new TextBox();

        public AccountPage(User user, OleDbConnection dbConnection,List<Component> components)
        {
            InitializeComponent();
            this.components = components;
            for (int i = 0; i < user.Assemblies.Count; i++)
            {
                string[] temp = user.Assemblies[i].Time.Split(' ');
                user.Assemblies[i].Time = temp[0];
            }


            assemblieList.ItemsSource = user.Assemblies;
            if (user.Assemblies.Count != 0)
            {
                assemblyFrame.Content = new AccountAssambly(user.Assemblies[0].ConvertAssemblyToListComponents(user.Assemblies[0]));
                currentAssemble = user.Assemblies[0];
            }
            else
                assemblyFrame.Content = new AccountAssambly();


            if (user.Orders.Count != 0)
            {
                orderFrame.Content = new OrderPage(user.Orders[0].Products);
                currentOrder = user.Orders[0];
            }
            this.user = user;
            this.dbConnection = dbConnection;

            loginLabel.Content = user.Login;
            passLabel.Content = user.Password;
            numberLabel.Content = user.Number;
            mailLabel.Content = user.Mail;
            nameLabel.Content = user.Name;
            surnameLabel.Content = user.Surname;

            PrintOrders(user.Orders);
            PrintStatistic(user.Orders);

        }

        public void PrintStatistic(List<Order> orders)
        {
            int sent = 0;
            int canceled = 0;
            int procesing = 0;
            int received = 0;

            for (int i = 0; i < orders.Count; i++)
            {
                if (orders[i].Status == "Sent")
                    sent++;
                else if (orders[i].Status == "In processing")
                    procesing++;
                else if (orders[i].Status == "Canceled")
                    canceled++;
                else if (orders[i].Status == "Received")
                    received++;
            }
            statisticOrders.VerticalAlignment = VerticalAlignment.Center;
            statisticOrders.FontSize = 15;
            statisticOrders.Text = $"Sent: {sent}   Canceled: {canceled}  In Processing: {procesing}  Received: {received}";
        }
        public void PrintOrders(List<Order> orders)
        {
            ordersList.Items.Clear();
            if (user.Orders.Count == 0)
            {
                ordersGrid.Children.Remove(cancelOrderButton);
                ordersGrid.Children.Remove(sentButton);
                ordersGrid.Children.Remove(receivedButton);
                ordersGrid.Children.Remove(canceledButton);
                ordersGrid.Children.Remove(canceledButton);
                ordersGrid.Children.Remove(processingButton);
                ordersGrid.Children.Remove(declineButton);
                ordersGrid.Children.Remove(ordersList);
                TextBlock txt = new TextBlock() { Text = "You dont have orders :(", Foreground = Brushes.LightGray, FontSize = 55, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
                Grid.SetColumnSpan(txt, 2);
                ordersGrid.Children.Add(txt);
            }
            else
            {
                orders.Sort((y, x) => x.Status.CompareTo(y.Status));
                for (int i = 0; i < orders.Count; i++)
                {
                    Button button = new Button() { Content = $" Order - №{orders[i].OrderCode} Cost: {orders[i].OrderCost} \n Date: {orders[i].DateOrder} \n Status: {orders[i].Status} \n Addres: {orders[i].Addres} \n Recipient: {orders[i].Surname} {orders[i].Name}" };
                    button.Height = 100;
                    button.Tag = orders[i];
                    button.Click += Button_Click;
                    button.BorderBrush = null;
                    button.HorizontalAlignment = HorizontalAlignment.Stretch;
                    button.VerticalContentAlignment = VerticalAlignment.Center;


                    if (orders[i].Status == "In processing")
                    {
                        button.Background = Brushes.DeepSkyBlue;
                    }
                    else if (orders[i].Status == "Sent")
                    {
                        button.Background = Brushes.Blue;
                    }
                    else if (orders[i].Status == "Canceled")
                    {
                        button.Background = Brushes.Red;
                    }
                    else if (orders[i].Status == "Received")
                    {
                        button.Background = Brushes.LawnGreen;
                    }
                    ordersList.Items.Add(button);
                }
            }

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Button button = (Button)sender;
            Order order = (Order)button.Tag;
            currentOrder = order;
            orderFrame.Content = new OrderPage(order.Products);

        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (button.Tag.ToString() == "login")
            {
                loginStackPanel.Children.Remove(newLogin);
                loginStackPanel.Children.Remove(button);
                user.Login = newLogin.Text;
                loginLabel.Content = user.Login;
                loginStackPanel.Children.Add(loginLabel);
                loginStackPanel.Children.Add(loginChange);

                dbConnection.Open();
                string query = $"UPDATE users SET user_login='{newLogin.Text}' WHERE user_login = '{user.Login}'";
                OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
                dbCommand.ExecuteNonQuery();
                dbConnection.Close();
            }
            else if (button.Tag.ToString() == "pass")
            {
                passStackPanel.Children.Remove(newPass);
                passStackPanel.Children.Remove(button);
                user.Password = newPass.Text;
                passLabel.Content = user.Password;
                passStackPanel.Children.Add(passLabel);
                passStackPanel.Children.Add(passChange);

                dbConnection.Open();
                string query = $"UPDATE users SET user_pass='{newPass.Text}' WHERE user_login = '{user.Login}'";
                OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
                dbCommand.ExecuteNonQuery();
                dbConnection.Close();
            }
            else if (button.Tag.ToString() == "number")
            {
                numberStackPanel.Children.Remove(newNumber);
                numberStackPanel.Children.Remove(button);
                user.Number = newNumber.Text;
                numberLabel.Content = user.Number;
                numberStackPanel.Children.Add(numberLabel);
                numberStackPanel.Children.Add(numberChange);

                dbConnection.Open();
                string query = $"UPDATE users SET user_number='{newNumber.Text}' WHERE user_login = '{user.Login}'";
                OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
                dbCommand.ExecuteNonQuery();
                dbConnection.Close();
            }
            else if (button.Tag.ToString() == "mail")
            {
                mailStackPanel.Children.Remove(newMail);
                mailStackPanel.Children.Remove(button);
                user.Mail = newMail.Text;
                mailLabel.Content = user.Mail;
                mailStackPanel.Children.Add(mailLabel);
                mailStackPanel.Children.Add(mailChange);

                dbConnection.Open();
                string query = $"UPDATE users SET user_mail='{newMail.Text}' WHERE user_login = '{user.Login}'";
                OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
                dbCommand.ExecuteNonQuery();
                dbConnection.Close();
            }
            else if (button.Tag.ToString() == "name")
            {
                nameStackPanel.Children.Remove(newName);
                nameStackPanel.Children.Remove(button);
                user.Name = newName.Text;
                nameLabel.Content = user.Name;
                nameStackPanel.Children.Add(nameLabel);
                nameStackPanel.Children.Add(nameChange);

                dbConnection.Open();
                string query = $"UPDATE users SET user_name ='{newName.Text}' WHERE user_login = '{user.Login}'";
                OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
                dbCommand.ExecuteNonQuery();
                dbConnection.Close();
            }
            else if (button.Tag.ToString() == "surname")
            {
                surnameStackPanel.Children.Remove(newSurname);
                surnameStackPanel.Children.Remove(button);
                user.Surname = newSurname.Text;
                surnameLabel.Content = user.Surname;
                surnameStackPanel.Children.Add(surnameLabel);
                surnameStackPanel.Children.Add(surnameChange);

                dbConnection.Open();
                string query = $"UPDATE users SET user_surname ='{newSurname.Text}' WHERE user_login = '{user.Login}'";
                OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
                dbCommand.ExecuteNonQuery();
                dbConnection.Close();
            }
        }

        private void loginChange_Click(object sender, RoutedEventArgs e)
        {
            loginStackPanel.Children.Remove(loginLabel);
            loginStackPanel.Children.Remove(loginChange);
            Button okButton = new Button();

            okButton.Content = "ok";
            okButton.Tag = "login";
            okButton.Margin = new Thickness(10, 0, 0, 0);
            okButton.Background = Brushes.Gray;
            okButton.Foreground = Brushes.Black;
            okButton.BorderBrush = null;
            okButton.Click += OkButton_Click;

            newLogin.Text = user.Login;
            newLogin.Height = 30;
            newLogin.Width = 60;
            newLogin.Margin = new Thickness(5, 5, 0, 0);
            newLogin.BorderBrush = Brushes.Gray;

            loginStackPanel.Children.Add(newLogin);
            loginStackPanel.Children.Add(okButton);

        }


        private void passChange_Click(object sender, RoutedEventArgs e)
        {
            passStackPanel.Children.Remove(passLabel);
            passStackPanel.Children.Remove(passChange);
            Button okButton = new Button();

            okButton.Content = "ok";
            okButton.Tag = "pass";
            okButton.Margin = new Thickness(10, 0, 0, 0);
            okButton.Background = Brushes.Gray;
            okButton.Foreground = Brushes.Black;
            okButton.BorderBrush = null;
            okButton.Click += OkButton_Click;

            newPass.Text = user.Password;
            newPass.Height = 30;
            newPass.Width = 60;
            newPass.Margin = new Thickness(5, 5, 0, 0);
            newPass.BorderBrush = Brushes.Gray;

            passStackPanel.Children.Add(newPass);
            passStackPanel.Children.Add(okButton);
        }

        private void numberChange_Click(object sender, RoutedEventArgs e)
        {
            numberStackPanel.Children.Remove(numberLabel);
            numberStackPanel.Children.Remove(numberChange);
            Button okButton = new Button();

            okButton.Content = "ok";
            okButton.Tag = "number";
            okButton.Margin = new Thickness(10, 0, 0, 0);
            okButton.Background = Brushes.Gray;
            okButton.Foreground = Brushes.Black;
            okButton.BorderBrush = null;
            okButton.Click += OkButton_Click;

            newNumber.Text = user.Number;
            newNumber.Height = 30;
            newNumber.Width = 60;
            newNumber.Margin = new Thickness(5, 5, 0, 0);
            newNumber.BorderBrush = Brushes.Gray;

            numberStackPanel.Children.Add(newNumber);
            numberStackPanel.Children.Add(okButton);
        }

        private void mailChange_Click(object sender, RoutedEventArgs e)
        {
            mailStackPanel.Children.Remove(mailLabel);
            mailStackPanel.Children.Remove(mailChange);
            Button okButton = new Button();

            okButton.Content = "ok";
            okButton.Tag = "mail";
            okButton.Margin = new Thickness(10, 0, 0, 0);
            okButton.Background = Brushes.Gray;
            okButton.Foreground = Brushes.Black;
            okButton.BorderBrush = null;
            okButton.Click += OkButton_Click;

            newMail.Text = user.Mail;
            newMail.Height = 30;
            newMail.Width = 60;
            newMail.Margin = new Thickness(5, 5, 0, 0);
            newMail.BorderBrush = Brushes.Gray;

            mailStackPanel.Children.Add(newMail);
            mailStackPanel.Children.Add(okButton);
        }

        private void nameChange_Click(object sender, RoutedEventArgs e)
        {
            nameStackPanel.Children.Remove(nameLabel);
            nameStackPanel.Children.Remove(nameChange);
            Button okButton = new Button();

            okButton.Content = "ok";
            okButton.Tag = "name";
            okButton.Margin = new Thickness(10, 0, 0, 0);
            okButton.Background = Brushes.Gray;
            okButton.Foreground = Brushes.Black;
            okButton.BorderBrush = null;
            okButton.Click += OkButton_Click;

            newName.Text = user.Name;
            newName.Height = 30;
            newName.Width = 60;
            newName.Margin = new Thickness(5, 5, 0, 0);
            newName.BorderBrush = Brushes.Gray;

            nameStackPanel.Children.Add(newName);
            nameStackPanel.Children.Add(okButton);
        }

        private void surnameChange_Click(object sender, RoutedEventArgs e)
        {
            surnameStackPanel.Children.Remove(surnameLabel);
            surnameStackPanel.Children.Remove(surnameChange);
            Button okButton = new Button();

            okButton.Content = "ok";
            okButton.Tag = "surname";
            okButton.Margin = new Thickness(10, 0, 0, 0);
            okButton.Background = Brushes.Gray;
            okButton.Foreground = Brushes.Black;
            okButton.BorderBrush = null;
            okButton.Click += OkButton_Click;

            newSurname.Text = user.Surname;
            newSurname.Height = 30;
            newSurname.Width = 60;
            newSurname.Margin = new Thickness(5, 5, 0, 0);
            newSurname.BorderBrush = Brushes.Gray;

            surnameStackPanel.Children.Add(newSurname);
            surnameStackPanel.Children.Add(okButton);
        }

        private void cancelOrderButton_Click(object sender, RoutedEventArgs e)
        {
            string query = " ";
            OleDbCommand dbCommand = new OleDbCommand();
            if (currentOrder.Status != "Canceled")
            {
                dbConnection.Open();
                currentOrder.Status = "Canceled";
                for (int i = 0; i < currentOrder.Products.Count; i++)
                {
                    int countTemp = 0;
                    query = $"SELECT [count] FROM products WHERE productCode = {currentOrder.Products[i].ProductCode}";
                    dbCommand = new OleDbCommand(query, dbConnection);
                    OleDbDataReader dbreader = dbCommand.ExecuteReader();
                    while (dbreader.Read())
                    {
                        countTemp = (int)dbreader["count"];
                    }
                    countTemp = countTemp + currentOrder.Products[i].Count;
                    query = $"UPDATE products SET [count] = {countTemp}  WHERE productCode = {currentOrder.Products[i].ProductCode}";
                    dbCommand = new OleDbCommand(query, dbConnection);
                    dbCommand.ExecuteNonQuery();

                }
                query = $"UPDATE orders SET orderStatus = 'Canceled' WHERE orderCode = {currentOrder.OrderCode}";
                dbCommand = new OleDbCommand(query, dbConnection);
                dbCommand.ExecuteNonQuery();

                dbConnection.Close();
                user.ReadOrdersFromBD(user, dbConnection);
                PrintOrders(user.Orders);
                MessageBoxNew message = new MessageBoxNew("Order succesful canceled!");
                message.Show();
                if (user.Orders.Count != 0)
                    orderFrame.Content = new OrderPage(user.Orders[0].Products);
                else orderFrame.Content = null;
            }
        }
        private void receivedButton_Click(object sender, RoutedEventArgs e)
        {
            var list = user.Orders.FindAll(x => x.Status == "Received");
            PrintOrders(list);
        }

        private void sentButton_Click(object sender, RoutedEventArgs e)
        {
            var list = user.Orders.FindAll(x => x.Status == "Sent");
            PrintOrders(list);
        }

        private void canceledButton_Click(object sender, RoutedEventArgs e)
        {
            var list = user.Orders.FindAll(x => x.Status == "Canceled");
            PrintOrders(list);
        }

        private void processingButton_Click(object sender, RoutedEventArgs e)
        {
            var list = user.Orders.FindAll(x => x.Status == "In processing");
            PrintOrders(list);
        }

        private void declineButton_Click(object sender, RoutedEventArgs e)
        {

            PrintOrders(user.Orders);
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Border border = (Border)sender;
            StackPanel panel = (StackPanel)border.Child;
            string nameAssembly = ((TextBlock)panel.Children[1]).Text;
            for (int i = 0; i < user.Assemblies.Count; i++)
            {
                if (user.Assemblies[i].Name == nameAssembly)
                {
                    assemblyFrame.Content = new AccountAssambly(user.Assemblies[i].ConvertAssemblyToListComponents(user.Assemblies[i]));
                    currentAssemble = user.Assemblies[i];
                    break;
                }
            }
        }

        private void buyAssemblyButton_Click(object sender, RoutedEventArgs e)
        {
            currentAssemble.BuyAssemble(components, currentAssemble,user,dbConnection);
        }
    }
}
