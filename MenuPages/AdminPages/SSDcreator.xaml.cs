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
    /// Логика взаимодействия для SSDcreator.xaml
    /// </summary>
    public partial class SSDcreator : Page
    {
        OleDbConnection dbConnection;
        List<Component> components;
        public SSDcreator(OleDbConnection dbConnection,List<Component> components)
        {
            InitializeComponent();
            this.dbConnection = dbConnection;
            this.components = components;
            for (int i = 0; i < 6; i++)
            {
                typeSSDbox.Items.Add((TypeSSD)i);
            }
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
            string[] temp = filePath.Split('\\');
            filePath = temp[temp.Length - 1];
            imagePath.Text = filePath;
            imagePath.ToolTip = filePath;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string model = " ";
            string producer = " ";
            double cost = 0;
            int count = 0;
            string image = " ";

            int memory_capacity = 0;
            string form_factor = " ";
            string connectPort = " ";
            int speedWrite = 0;
            int speedRead = 0;
            string typeSSD = " ";
            string type = "SSD";
            bool check = false;
            try
            {
                model = modelBox.Text;
                producer = producerBox.Text;
                cost = Convert.ToDouble(costBox.Text);
                count = Convert.ToInt32(countBox.Text);
                image = imagePath.Text;
                memory_capacity = Convert.ToInt32(memoryCapacityBox.Text);
                form_factor = formBox.Text;
                connectPort = connectPowrBox.Text;
                speedWrite = Convert.ToInt32(writeBox.Text);
                speedRead = Convert.ToInt32(readBox.Text);
                typeSSD = typeSSDbox.SelectedItem.ToString();
                check = true;
            }
            catch
            {

                MessageBoxNew message = new MessageBoxNew("Some field entered uncorectly!");
                message.Show();
            }
            if (check)
            {
                bool check2 = SSD.AddSSDtoBD(model, cost, image, producer, count, type, memory_capacity, form_factor, connectPort, speedWrite, speedRead, typeSSD, components, dbConnection);
                if (check2)
                {
                    costBox.Text = "";
                    producerBox.Text = "";
                    modelBox.Text = "";
                    countBox.Text = "";
                    imagePath.Text = "";
                    memoryCapacityBox.Text = "";
                    formBox.Text = "";
                    connectPowrBox.Text = "";
                    writeBox.Text = "";
                    readBox.Text = "";
                    typeSSDbox.SelectedIndex = -1;
                    MessageBoxNew message = new MessageBoxNew("SSD succesfull added!");
                    message.Show();
                }
            }
        }
    }
}
