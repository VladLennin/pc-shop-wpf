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
using System.Data;

namespace КР2семестр
{
    /// <summary>
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        OleDbConnection dbConnection;
        List<Component> components;
        List<Order> allOrders = new List<Order>();
        List<User> users;
        Order currentOrder;
      
        public AdminPage(OleDbConnection dbConnection, List<Component> components,List<User> users)
        {
            InitializeComponent();
            this.dbConnection = dbConnection;
            this.components = components;
            this.users = users;
            adminPageFrame.Content = new CPUcreator(dbConnection, components);



            for (int i = 0; i < users.Count; i++)
            {
                for (int k = 0; k < users[i].Orders.Count; k++)
                {
                    allOrders.Add(users[i].Orders[k]);
                }
            }
            PrintOrders(allOrders);
            PrintStatistic(allOrders);
            currentOrder = null;
            
        }
        public void PrintOrders(List<Order> orders)
        {
            ordersList.Items.Clear();
            for (int i = 0; i <orders.Count; i++)
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
                    else  if(orders[i].Status == "Sent")
                    {
                        button.Background = Brushes.Blue;
                    }
                    else if(orders[i].Status == "Canceled")
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
        public void PrintStatistic(List<Order>orders)
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
            statisticOrders.Text = $"Sent: {sent}   Canceled: {canceled}  In Processing: {procesing}  Received: {received}";
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Order order = (Order)button.Tag;

            orderFrame.Content = new OrderPage(order.Products);
            currentOrder = order;
        }

        private void CPU_button_Creator_Click(object sender, RoutedEventArgs e)
        {
            adminPageFrame.Content = new CPUcreator(dbConnection,components);
        }

        private void MotherBoard_button_Creator_Click(object sender, RoutedEventArgs e)
        {
            adminPageFrame.Content = new MotherboardCreator(dbConnection,components);
        }

        private void GPU_button_Creator_Click(object sender, RoutedEventArgs e)
        {
            adminPageFrame.Content = new GPUcreator(dbConnection,components);
        }

        private void RAM_button_Creator_Click(object sender, RoutedEventArgs e)
        {
            adminPageFrame.Content = new RAMcreator(dbConnection,components);
        }

        private void PowerSupply_button_Creator_Click(object sender, RoutedEventArgs e)
        {
            adminPageFrame.Content = new PowerSupplyCreator(dbConnection,components);
        }

        private void CoolingSystem_button_Creator_Click(object sender, RoutedEventArgs e)
        {
            adminPageFrame.Content = new CoolingSystemCreator(components,dbConnection);
        }

        private void HDD_button_Creator_Click(object sender, RoutedEventArgs e)
        {
            adminPageFrame.Content = new HDDcreator(components,dbConnection);
        }

        private void SSD_button_Creator_Click(object sender, RoutedEventArgs e)
        {
            adminPageFrame.Content = new SSDcreator(dbConnection,components);
        }

        private void Case_button_Creator_Click(object sender, RoutedEventArgs e)
        {
            adminPageFrame.Content = new CaseCreator(components,dbConnection);
        }

        private void receivedButton_Click(object sender, RoutedEventArgs e)
        {
            var list = allOrders.FindAll(x => x.Status == "Received");
            PrintOrders(list);
        }

        private void sentButton_Click(object sender, RoutedEventArgs e)
        {
            var list = allOrders.FindAll(x => x.Status == "Sent");
            PrintOrders(list);
        }

        private void canceledButton_Click(object sender, RoutedEventArgs e)
        {
            var list = allOrders.FindAll(x => x.Status == "Canceled");
            PrintOrders(list);
        }

        private void processingButton_Click(object sender, RoutedEventArgs e)
        {
            var list = allOrders.FindAll(x => x.Status == "In processing");
            PrintOrders(list);
        }

        private void declineButton_Click(object sender, RoutedEventArgs e)
        {
           
            PrintOrders(allOrders);
        }

        private void cancelOrder_Click(object sender, RoutedEventArgs e)
        {
           
                string query = " ";
                OleDbCommand dbCommand = new OleDbCommand();
                if (currentOrder.Status != "Canceled"&& currentOrder.Status != "Received")
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

                  
                 

                    query = $"SELECT userCode FROM orders  WHERE orderCode = {currentOrder.OrderCode}";
                    dbCommand = new OleDbCommand(query, dbConnection);
                    OleDbDataReader dbReader = dbCommand.ExecuteReader();
                      int userCode = 0;
                      while(dbReader.Read())
                      {
                        userCode = (int)dbReader["userCode"];
                      }
                    dbConnection.Close();


                     for (int i = 0; i < users.Count; i++)
                     {
                         if(users[i].UserCode==userCode)
                         {
                           users[i].ReadOrdersFromBD(users[i], dbConnection);
                         }
                     }
                  
                 
                        
                      PrintOrders(allOrders);
                    
                    MessageBoxNew message = new MessageBoxNew("Order succesful canceled!");
                    message.Show();
                  
                        orderFrame.Content = null;
                    
                }
            
        }

        private void sentOrder_Click(object sender, RoutedEventArgs e)
        {
            string query = " ";
            OleDbCommand dbCommand = new OleDbCommand();
            if (currentOrder.Status != "Sent"&&currentOrder.Status!="Received")
            {
                dbConnection.Open();
                currentOrder.Status = "Sent";

                query = $"UPDATE orders SET orderStatus = 'Sent' WHERE orderCode = {currentOrder.OrderCode}";
                dbCommand = new OleDbCommand(query, dbConnection);
                dbCommand.ExecuteNonQuery();


                query = $"SELECT userCode FROM orders  WHERE orderCode = {currentOrder.OrderCode}";
                dbCommand = new OleDbCommand(query, dbConnection);
                OleDbDataReader dbReader = dbCommand.ExecuteReader();
                int userCode = 0;
                while (dbReader.Read())
                {
                    userCode = (int)dbReader["userCode"];
                }
                dbConnection.Close();


                for (int i = 0; i < users.Count; i++)
                {
                    if (users[i].UserCode == userCode)
                    {
                        users[i].ReadOrdersFromBD(users[i], dbConnection);
                    }
                }



                PrintOrders(allOrders);

                MessageBoxNew message = new MessageBoxNew("Order succesful sent!");
                message.Show();

                orderFrame.Content = null;

            }
        }

        private void processingOrder_Click(object sender, RoutedEventArgs e)
        {
            string query = " ";
            OleDbCommand dbCommand = new OleDbCommand();
            if (currentOrder.Status != "In processing")
            {
                dbConnection.Open();
                currentOrder.Status = "In processing";

                query = $"UPDATE orders SET orderStatus = 'In processing' WHERE orderCode = {currentOrder.OrderCode}";
                dbCommand = new OleDbCommand(query, dbConnection);
                dbCommand.ExecuteNonQuery();


                query = $"SELECT userCode FROM orders  WHERE orderCode = {currentOrder.OrderCode}";
                dbCommand = new OleDbCommand(query, dbConnection);
                OleDbDataReader dbReader = dbCommand.ExecuteReader();
                int userCode = 0;
                while (dbReader.Read())
                {
                    userCode = (int)dbReader["userCode"];
                }
                dbConnection.Close();


                for (int i = 0; i < users.Count; i++)
                {
                    if (users[i].UserCode == userCode)
                    {
                        users[i].ReadOrdersFromBD(users[i], dbConnection);
                    }
                }



                PrintOrders(allOrders);

                MessageBoxNew message = new MessageBoxNew("Order succesful processing!");
                message.Show();

                orderFrame.Content = null;

            }
        }

        private void recieveOrder_Click(object sender, RoutedEventArgs e)
        {
            string query = " ";
            OleDbCommand dbCommand = new OleDbCommand();
            if (currentOrder.Status != "Received" && currentOrder.Status != "Received")
            {
                dbConnection.Open();
                currentOrder.Status = "Received";

                query = $"UPDATE orders SET orderStatus = 'Received' WHERE orderCode = {currentOrder.OrderCode}";
                dbCommand = new OleDbCommand(query, dbConnection);
                dbCommand.ExecuteNonQuery();


                query = $"SELECT userCode FROM orders  WHERE orderCode = {currentOrder.OrderCode}";
                dbCommand = new OleDbCommand(query, dbConnection);
                OleDbDataReader dbReader = dbCommand.ExecuteReader();
                int userCode = 0;
                while (dbReader.Read())
                {
                    userCode = (int)dbReader["userCode"];
                }
                dbConnection.Close();


                for (int i = 0; i < users.Count; i++)
                {
                    if (users[i].UserCode == userCode)
                    {
                        users[i].ReadOrdersFromBD(users[i], dbConnection);
                    }
                }



                PrintOrders(allOrders);

                MessageBoxNew message = new MessageBoxNew("Order succesful received!");
                message.Show();

                orderFrame.Content = null;

            }
        }
    }
}
