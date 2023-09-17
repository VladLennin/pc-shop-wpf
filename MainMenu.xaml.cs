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
using System.Data.OleDb;
namespace КР2семестр
{
    /// <summary>
    /// Логика взаимодействия для MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        int checkForButtonColor = 0;
        List<Button> buttonsInMenu = new List<Button>();
        OleDbConnection dbConnection;
        User user;
        List<User> users;
        List<Component> components;
       
        public MainMenu(User user,OleDbConnection dbConnection,List<Component>components)
        {
            InitializeComponent();
            this.components = components;
            MenuFrame.Content = new MainPage();
            buttonsInMenu.Add(mainButton);
            buttonsInMenu.Add(accountButton);
            buttonsInMenu.Add(shopButton);
            buttonsInMenu.Add(basketButton);
            buttonsInMenu.Add(constructButton);
            ChangeButtonColor();
            menuBorder.MaxHeight = 210;
            menuBorder.MinHeight = 210;




            if (user.Login == "admin")
            {
                Button adminButton = new Button();
                menuPanel.Children.Add(adminButton);
                adminButton.Background = null;
                adminButton.Foreground = Brushes.Black;
                adminButton.BorderBrush = null;
                adminButton.Margin = new Thickness(15, 5, 10, 5);
                adminButton.Content = "ADMIN";
                adminButton.FontFamily = new FontFamily("Bauhaus 93");
                adminButton.FontSize = 16;
                adminButton.Click += AdminButton_Click;
                Image image = new Image();
                buttonsInMenu.Add(adminButton);


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
                path += $"Icon\\adminImage.png";
                image.Source = (ImageSource)new ImageSourceConverter().ConvertFromString(path);


                image.Height = 30;
                image.Width = 30;
                image.Margin = new Thickness(0, 5, 23, 5);
                menuPanelIcon.Children.Add(image);
                menuBorder.MaxHeight = 250;
                menuBorder.MinHeight = 250;
                users = new List<User>();
                User.ReadAllUsersFromBD(dbConnection, users);
                for (int i = 0; i < users.Count; i++)
                {
                    user.ReadOrdersFromBD(users[i], dbConnection);
                }

            }
            else users = new List<User>();

            this.user = user;
            this.dbConnection = dbConnection;

             user.ReadOrdersFromBD(user, dbConnection);           
            user.ReadAssembliesFromBD(user, dbConnection,components);
            user.Basket = User.FillBasketFromBD(dbConnection, user);

        }

        public static  void SelectComponentsFromBD(OleDbConnection dbConnection,List<Component> components)
        {
            components.Clear();

           
            CPU cpu = new CPU();cpu.FillComponentsFromBD(components, dbConnection);
            GPU gpu = new GPU();gpu.FillComponentsFromBD(components, dbConnection);
            Motherboard motherboard = new Motherboard();motherboard.FillComponentsFromBD(components, dbConnection);
            RAM ram = new RAM();ram.FillComponentsFromBD(components, dbConnection);
            PowerSupply powerSupply = new PowerSupply();powerSupply.FillComponentsFromBD(components, dbConnection);
            HDD hdd = new HDD();hdd.FillComponentsFromBD(components, dbConnection);
            SSD ssd = new SSD();ssd.FillComponentsFromBD(components, dbConnection);
            Case case1 = new Case();case1.FillComponentsFromBD(components, dbConnection);
            CoolingSystem cs = new CoolingSystem();cs.FillComponentsFromBD(components, dbConnection);

          
        }


        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            MenuFrame.Content = new AdminPage(dbConnection,components,users);
            checkForButtonColor = 5;
            ChangeButtonColor();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();    
            this.Close();
        }

        private void mainButton_Click(object sender, RoutedEventArgs e)
        {
            MenuFrame.Content = new MainPage();
            checkForButtonColor = 0;
            ChangeButtonColor();
        }

        private void accountButton_Click(object sender, RoutedEventArgs e)
        {

            dbConnection.Close();
            MenuFrame.Content = new AccountPage(user,dbConnection,components);
            checkForButtonColor = 1;
            ChangeButtonColor();
        }

        private void shopButton_Click(object sender, RoutedEventArgs e)
        {

            MenuFrame.Content = new ShopPage(MenuFrame,user,components,dbConnection);
            checkForButtonColor = 2;
            ChangeButtonColor();
        }

        public void basketButton_Click(object sender, RoutedEventArgs e)
        {

            MenuFrame.Content = new BasketPage(user,dbConnection,components,users);
            checkForButtonColor = 3;
            ChangeButtonColor();
        }

        private void constructButton_Click(object sender, RoutedEventArgs e)
        {

            MenuFrame.Content = new ConstructPage(components,MenuFrame,user,dbConnection);
            checkForButtonColor = 4;
            ChangeButtonColor();
        }

        public void ChangeButtonColor()
        {
            for (int i = 0; i < buttonsInMenu.Count; i++)
            {
                buttonsInMenu[i].Background = null;
            }
            buttonsInMenu[checkForButtonColor].Background = Brushes.DarkGray;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
