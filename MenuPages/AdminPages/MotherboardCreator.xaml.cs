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

namespace КР2семестр
{
    /// <summary>
    /// Логика взаимодействия для MotherboardCreator.xaml
    /// </summary>
    public partial class MotherboardCreator : Page
    {
        OleDbConnection dbConnection;
        List<Component> components;
        public MotherboardCreator(OleDbConnection dbConnection,List<Component>components)
        {
            this.components = components;
            this.dbConnection = dbConnection;
            InitializeComponent();
            for (int i = 0; i < 7; i++)
            {
               socketBox.Items.Add((( Sockets)i).ToString());
            }
            for (int i = 0; i < 5; i++)
            {
                producerBox.Items.Add(((MotherboardProducer)i).ToString());
            }
            forCPUbox.Items.Add(CPUproducers.AMD);
            forCPUbox.Items.Add(CPUproducers.Intel);

            for (int i = 0; i < 4; i++)
            {
                sizeBox.Items.Add(((MotherboardSizes)i).ToString());
            }
            for (int i = 0; i < 4; i++)
            {
                RAMgenBox.Items.Add(((RAMgen)i).ToString());
            }

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string costTemp = " "; 
            string image = " ";
            string producer = " ";
            int count = 0;
            string model = " ";

            string socket = " ";
            string forCpu = " ";
            string size = " ";
            string ramGen = " ";
            string chipset = " ";
            int countRam = 0;
            int countChannelRam = 0;
            int maxCapacityRAM = 0;
            int maxFrequency = 0;
            string ports = " ";
            double cost = 0;
            bool check = true;
            try
            {
                costTemp = costBox.Text;
                 cost = Convert.ToDouble(costTemp);
                image = imagePath.Text;
                producer = producerBox.SelectedItem.ToString();
                count = Convert.ToInt32(countBox.Text);
                model = modelBox.Text;
                socket = socketBox.SelectedItem.ToString();
                forCpu = forCPUbox.SelectedItem.ToString();
                size = sizeBox.SelectedItem.ToString();
                ramGen = RAMgenBox.SelectedItem.ToString();
                chipset = chipsetBox.Text;
                countRam = Convert.ToInt32(countRAMbox.Text);
                countChannelRam = Convert.ToInt32(countChannelRAMbox.Text);
                maxCapacityRAM = Convert.ToInt32(maxCapacityRAMbox.Text);
                maxFrequency = Convert.ToInt32(maxFrequencyRAMbox.Text);
                ports = portsTextBlock.Text;
            }
            catch 
            {
                MessageBoxNew message = new MessageBoxNew("Someone field empty or uncorrectlu entered!");
                message.Show();
                check = false;
               
            }
            if (check)
            {
                int x = Motherboard.AddMotherboardToBD(cost, image, producer, count, model, socket, forCpu, size, ramGen, chipset, countRam, countChannelRam, maxCapacityRAM, maxFrequency, ports, dbConnection, components);
                if (x == 1)
                {
                    costBox.Text = "";
                    producerBox.SelectedIndex = -1;
                    modelBox.Text = "";
                    countBox.Text = "";
                    imagePath.Text = "";

                    socketBox.SelectedIndex = -1;
                    forCPUbox.SelectedIndex = -1;
                    sizeBox.SelectedIndex = -1;
                    RAMgenBox.SelectedIndex = -1;
                    chipsetBox.Text = "";
                    countRAMbox.Text = "";
                    countChannelRAMbox.Text = "";
                    maxCapacityRAMbox.Text = "";
                    maxFrequencyRAMbox.Text = "";
                    portsTextBlock.Text = "";
                    MessageBoxNew message = new MessageBoxNew("Motherboard succesful added!");
                    message.Show();
                }
                else if (x == -1)
                {
                    MessageBoxNew message = new MessageBoxNew("Product with this model already exist!");
                    message.Show();
                }

            }



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
    }
}
