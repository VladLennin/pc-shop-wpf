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
    /// Логика взаимодействия для HDDcreator.xaml
    /// </summary>
    public partial class HDDcreator : Page
    {
        List<Component> components;
        OleDbConnection dbConnection;
        public HDDcreator(List<Component> components, OleDbConnection dbConnection)
        {
            InitializeComponent();
            speedRotateBox.Items.Add(HDDspeedRotate.speed_5400);
            speedRotateBox.Items.Add(HDDspeedRotate.speed_7200);

            this.components = components;
            this.dbConnection = dbConnection;

            
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

            int memory_capacity = 0;
            string form_factor = " ";
            string connectPort = " ";
            int speedWrite = 0;
            int speedRead = 0;
            string speedRotate = " ";
            string type = "HDD";
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
                speedRotate = speedRotateBox.SelectedItem.ToString();
                check = true;
            }
            catch
            {

                MessageBoxNew message = new MessageBoxNew("Some field entered uncorectly!");
                message.Show();
            }
            if (check)
            {
               bool check2 = HDD.AddHDDtoBD(model, cost, image, producer, count, type,memory_capacity,form_factor, connectPort,speedWrite,speedRead,speedRotate,components, dbConnection);
                if(check2)
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
                    speedRotateBox.SelectedIndex = -1;
                    MessageBoxNew message = new MessageBoxNew("HDD succesfull added!");
                    message.Show();
                }
            }
        }
    }
}
