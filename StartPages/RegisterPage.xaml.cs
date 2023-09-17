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
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        MainWindow window;
        OleDbConnection dbConnection;
        List<Component> components;
        public RegisterPage(MainWindow _window, OleDbConnection dbConnection,List<Component> components)
        {
            InitializeComponent();
            window = _window;
            this.components = components;
            this.dbConnection = dbConnection;
            dbConnection.Open();
        }

        private void EnterPage_Click(object sender, RoutedEventArgs e)
        {
            dbConnection.Close();
            NavigationService.Navigate(new EnterPage(window, dbConnection,components));
        }

        private void RegNewAccount_Click(object sender, RoutedEventArgs e)
        {
            string query = $"SELECT COUNT(user_login) FROM users WHERE user_login ='{EnterNewLogin.Text}'";
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
           
            if ((int)dbCommand.ExecuteScalar() == 0)
            {
                query = $"SELECT COUNT(user_mail) FROM users WHERE user_mail = '{EnterNewMail.Text}'";
                 dbCommand = new OleDbCommand(query, dbConnection);
                if ((int)dbCommand.ExecuteScalar()==0)
                {
                    if (EnterNewMail.Text.Contains("@")&&EnterNewMail.Text.Contains("."))
                    {
                        if (EnterNewPassword1.Password == EnterNewPassword2.Password)
                        {
                            if (EnterNewPassword1.Password.Length > 8)
                            {
                                string x = "***";
                                query = $"INSERT INTO users ([user_login],[user_pass],[user_mail],[user_number],[user_name],[user_surname],[purse]) VALUES('{EnterNewLogin.Text}','{EnterNewPassword2.Password}','{EnterNewMail.Text}','{x}','{x}','{x}',0)";
                                dbCommand = new OleDbCommand(query, dbConnection);
                                dbCommand.ExecuteNonQuery();
                                MessageBoxNew message = new MessageBoxNew("User succesful created!");
                                message.Show();
                                message.OKbutton.Click += OKbutton_Click;
                            }
                            else
                            {
                                EnterNewPassword1.BorderBrush = Brushes.Red;
                                EnterNewPassword2.BorderBrush = Brushes.Red;
                                PassError.Text = "Passwords is tol low!";
                            }
                        }
                        else
                        {
                            EnterNewPassword1.BorderBrush = Brushes.Red;
                            EnterNewPassword2.BorderBrush = Brushes.Red;
                            PassError.Text = "Passwords is not equals!";
                        }
                    }
                    else
                    {
                        EnterNewMail.BorderBrush = Brushes.Red;
                        MailError.Text = "Mail is not correct!";
                    }
                }
                else
                {
                    EnterNewMail.BorderBrush = Brushes.Red;
                    MailError.Text = "User with this mail already exist!";
                }
            }
            else
            {
                EnterNewLogin.BorderBrush = Brushes.Red;
                LoginError.Text = "User with this login already exist!";               
            }
        }

        private void OKbutton_Click(object sender, RoutedEventArgs e)
        {
            dbConnection.Close();
            NavigationService.Navigate(new EnterPage(window, dbConnection,components));
        }

        private void EnterNewLogin_KeyDown(object sender, KeyEventArgs e)
        {
            EnterNewLogin.BorderBrush = Brushes.LawnGreen;
            LoginError.Text = " ";
        }

        private void EnterNewMail_KeyDown(object sender, KeyEventArgs e)
        {
            EnterNewMail.BorderBrush = Brushes.LawnGreen;
           MailError.Text = " ";
        }

        private void EnterNewPassword1_KeyDown(object sender, KeyEventArgs e)
        {
            EnterNewPassword1.BorderBrush = Brushes.LawnGreen;
            EnterNewPassword2.BorderBrush = Brushes.LawnGreen;
            PassError.Text = " ";
        }

        private void EnterNewPassword2_KeyDown(object sender, KeyEventArgs e)
        {
            EnterNewPassword1.BorderBrush = Brushes.LawnGreen;
            EnterNewPassword2.BorderBrush = Brushes.LawnGreen;
            PassError.Text = " ";
        }
    }
}
