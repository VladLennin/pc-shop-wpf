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
    /// Логика взаимодействия для EnterPage.xaml
    /// </summary>

    public partial class EnterPage : Page
    {


       
        private OleDbConnection dbConnection;
        MainWindow window;
        List<Component> Components;
        public EnterPage(MainWindow window, OleDbConnection dbConnection, List<Component> components)
        {
            this.window = window;
            InitializeComponent();
            this.Components = components;
            this.dbConnection = dbConnection;
          
        }
        

        private void RegisterPage_Click(object sender, RoutedEventArgs e)
        {
            dbConnection.Close();
            NavigationService.Navigate(new RegisterPage(window, dbConnection,Components));
        }

        private void EnterAccount_Click(object sender, RoutedEventArgs e)
        {
            var user = User.FillUserFromBD(EnterLogin.Text, EnterPassword.Password, dbConnection);
            if (user != null)
            {
                MainMenu mainMenu = new MainMenu(user, dbConnection,Components);
                mainMenu.Show();
                window.Close();
            }
            else
            {
                MessageBoxNew message = new MessageBoxNew("Uncorrect data");
                message.Show();
                EnterLogin.Text = "";
                EnterPassword.Password = "";
            }

        }



       
        private void EnterLogin_KeyDown(object sender, KeyEventArgs e)
        {
            EnterLogin.BorderBrush = Brushes.LawnGreen;
            MaterialDesignThemes.Wpf.HintAssist.SetHint(EnterLogin, "Enter login");
        }

        private void EnterPassword_KeyDown(object sender, KeyEventArgs e)
        {
            EnterPassword.BorderBrush = Brushes.LawnGreen;
            MaterialDesignThemes.Wpf.HintAssist.SetHint(EnterPassword, "Enter pass");
        }
    }
}
