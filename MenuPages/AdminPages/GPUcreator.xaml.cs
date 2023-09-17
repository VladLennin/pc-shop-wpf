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
using System.IO;
using System.Data.OleDb;
namespace КР2семестр
{
    /// <summary>
    /// Логика взаимодействия для GPUcreator.xaml
    /// </summary>
    public partial class GPUcreator : Page
    {
        OleDbConnection dbConnection;
        List<Component> components;
        public GPUcreator(OleDbConnection dbConnection,List<Component>components)
        {
            InitializeComponent();
            this.dbConnection = dbConnection;
            this.components = components;

            for (int i = 0; i < 2; i++)
            {
                familyGPUbox.Items.Add(((GPUfamily)i).ToString());
            }
            for (int i = 0; i < 7; i++)
            {
                memoryGenBox.Items.Add(((GPUgen)i).ToString());
            }
            for (int i = 0; i < 9; i++)
            {
                producerBox.Items.Add(((GPUproducers)i).ToString());
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string costTemp = costBox.Text;
            string image = " ";
            string producer = " ";
            int count = 0;
            string model = " ";

            string gpu_family = " ";
            int memory_capacity = 0;
            int digit_capacity = 0;
            string memory_gen = " ";
            int pass_mark = 0;
            string ports = " ";

            double cost = 0;
            string temp = costTemp.Replace('.', ',');

            bool check = true;
            try
            {
                cost = Convert.ToDouble(temp);
                image = imagePath.Text;
                producer = (string)producerBox.SelectedItem;
                count = Convert.ToInt32(countBox.Text);
                model = modelBox.Text;


                gpu_family = familyGPUbox.SelectedItem.ToString();
                memory_capacity = Convert.ToInt32(memoryCapacityBox.Text);
                digit_capacity = Convert.ToInt32(digitCapacityBox.Text);
                memory_gen = memoryGenBox.SelectedItem.ToString();
                pass_mark = Convert.ToInt32(passMarkBox.Text);
                ports = portsTextBlock.Text;
            }
            catch
            {
                MessageBoxNew message = new MessageBoxNew("Someone field empty or \n uncorrectly entered!");
                message.Show();
                check = false;
            }

            int x = GPU.AddGPUtoBD(check, cost, image, producer, count, model, gpu_family, memory_capacity, digit_capacity, memory_gen, pass_mark, ports, components, dbConnection);

            if (x == 1)
            {
                MessageBoxNew message = new MessageBoxNew("GPU succesful added!");
                message.Show();
                costBox.Text = "";
                producerBox.SelectedIndex = -1;
                modelBox.Text = "";
                countBox.Text = "";
                imagePath.Text = "";

                familyGPUbox.SelectedIndex = -1;
                memoryCapacityBox.Text = "";
                digitCapacityBox.Text = "";
                memoryGenBox.SelectedIndex = -1;
                passMarkBox.Text = "";
                portsTextBlock.Text = "";
            }
            else if (x == -1)
            {
                MessageBoxNew message = new MessageBoxNew("This model already exist!");
                message.Show();
            }

            MainMenu.SelectComponentsFromBD(dbConnection, components);
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

        private void addPort_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Port port = new Port(portsBox.Text, Convert.ToInt32(countPorts.Text));
                portsTextBlock.Text += port.ToString() + ";\n";
                portsBox.Text = "";
                countPorts.Text = "";
            }
            catch
            {
                MessageBoxNew message = new MessageBoxNew("Field entered uncorrectly!");
                message.Show();
            }
        }
    }
}
