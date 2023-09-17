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
using System.Windows.Forms;
using System.IO;

namespace КР2семестр
{
    /// <summary>
    /// Логика взаимодействия для RAMcreator.xaml
    /// </summary>
    public partial class RAMcreator : Page
    {
        OleDbConnection dbConnection;
        List<Component> components;
        public RAMcreator(OleDbConnection dbConnection,List<Component>components)
        {
            this.dbConnection = dbConnection;
            this.components = components;

            InitializeComponent();
            for (int i = 0; i < 4; i++)
            {
                genBox.Items.Add(((RAMgen)i).ToString());
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
            int count = 0;
            double cost = 0;
            string producer = " ";
            string image = imagePath.Text;
            string type = "RAM";

            double frequency = 0;
            double power = 0;
            string ramGEN = " ";
            int capacity = 0;
            int countModules = 0;
            bool check = false;
            try
            {
                model = modelBox.Text;
                count = Convert.ToInt32(countBox.Text);
                cost = Convert.ToDouble(costBox.Text);
                producer = producerBox.Text;
                image = imagePath.Text;
                type = "RAM";

                frequency = Convert.ToDouble(frequencyBox.Text);
                power = Convert.ToDouble(powerBox.Text);
                ramGEN = genBox.SelectedItem.ToString();
                capacity = Convert.ToInt32(capacityBox.Text);
                countModules = Convert.ToInt32(modulesBox.Text);

                check = true;
            }
            catch
            {
                MessageBoxNew message = new MessageBoxNew("Some field entered uncorectly!");
                message.Show();
            }
            if(check)
            {
                


                bool check2 =  RAM.AddRAMtoBD(model, count, cost, producer, image, type, frequency, power, ramGEN, capacity, countModules, dbConnection, components);
                if(check2)
                {
                    modelBox.Text = "";
                    countBox.Text = "";
                    costBox.Text = "";
                    producerBox.Text = "";
                    imagePath.Text = "";
                    frequencyBox.Text = "";
                    powerBox.Text = "";
                    genBox.SelectedIndex = -1;
                    capacityBox.Text = "";
                    modulesBox.Text = "";
                    MessageBoxNew message = new MessageBoxNew("RAM succesfull added!");
                    message.Show();
                }
            }

        }
    }
}
