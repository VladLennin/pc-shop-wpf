using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.OleDb;
namespace КР2семестр
{
    /// <summary>
    /// Логика взаимодействия для CoolingSystemCreator.xaml
    /// </summary>
    public partial class CoolingSystemCreator : Page
    {

        OleDbConnection dbConnection;
        List<Component> components;

        public CoolingSystemCreator(List<Component>components,OleDbConnection dbConnection)
        {
            InitializeComponent();
            this.dbConnection = dbConnection;
            this.components = components;
        }
        private void ImageOpen_Click(object sender, RoutedEventArgs e)
        {

            string filePath = " ";
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;

                }
            }
            imagePath.Text = filePath;
            imagePath.ToolTip = filePath;
            string[] temp = filePath.Split('\\');
            filePath = temp[temp.Length - 1];
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string model = " ";
            string producer = " ";
            double cost = 0;
            int count = 0;
            string image = " ";
            bool compabilityAMD = false;
            bool compabilityIntel = false;
            string type = "CoolingSystem";
            bool check = false;
            try
            {
                if (checkAMD.IsChecked == true && checkIntel.IsChecked == true)
                {
                    model = modelBox.Text;
                    producer = producerBox.Text;
                    cost = Convert.ToDouble(costBox.Text);
                    count = Convert.ToInt32(countBox.Text);
                    image = imagePath.Text;
                    if (checkAMD.IsChecked == true)
                        compabilityAMD = true;
                    if (checkIntel.IsChecked == true)
                        compabilityIntel = true;


                    check = true;
                }
                else
                    throw new Exception("CheckBoxes empty!");
               
            }
            catch
            {

                MessageBoxNew message = new MessageBoxNew("Some field entered uncorectly!");
                message.Show();
            }
            if (check)
            {
                bool check2 = CoolingSystem.AddCoolingSystemToBD(compabilityAMD, compabilityIntel, model, producer, cost, image, count, type, dbConnection, components);
                if (check2)
                {
                    costBox.Text = "";
                    producerBox.Text = "";
                    modelBox.Text = "";
                    countBox.Text = "";
                    imagePath.Text = "";
                    checkAMD.IsChecked = false;
                    checkIntel.IsChecked = false;

                    MessageBoxNew message = new MessageBoxNew("Cooling System succesfull added!");
                    message.Show();
                }
            }
        }
    }
}
