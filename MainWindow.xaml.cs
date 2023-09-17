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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static string connectString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=CW2.mdb";
        private OleDbConnection dbConnection;
        List<Component> components = new List<Component>();
        
        public MainWindow()
        {
            InitializeComponent();
            dbConnection = new OleDbConnection(connectString);           
             LoadAllDirectory();
          

            mainFrame.Content = new EnterPage(this, dbConnection,components);
         
        }

        public void LoadAllDirectory()
        {
            MainMenu.SelectComponentsFromBD(dbConnection, components);

            for (int i = 0; i < components.Count; i++)
            {
                components[0].FillFeedbacksFromDB(components[i], dbConnection);
            }           
            for (int i = 0; i < components.Count; i++)
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
                path += $"Resources\\{components[i].Image}";
                components[i].Image = path;
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            dbConnection.Close();
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
