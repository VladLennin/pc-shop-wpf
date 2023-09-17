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
using System.IO;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Text.RegularExpressions;

namespace КР2семестр
{
    /// <summary>
    /// Логика взаимодействия для CPUcreator.xaml
    /// </summary>
    public partial class CPUcreator : Page
    {
        OleDbConnection dbConnection;
        List<Component> components;
        public CPUcreator(OleDbConnection dbConnection,List<Component>components)
        {
            this.dbConnection = dbConnection;
            this.components = components;
            InitializeComponent();

           

            for (int i = 0; i < 7; i++)
            {
                socketBox.Items.Add(((Sockets)i).ToString());
            }
            integrated_graficsBox.Items.Add("Yes");
            integrated_graficsBox.Items.Add("No");
            for (int i = 0; i < 2; i++)
            {
                producerBox.Items.Add(((CPUproducers)i).ToString());
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

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            
            int count = 0;
            double cost = 0;
            int cores = 0;
            int threads = 0;
            double frequency = 0;
            int technical_process = 0;
            int power = 0;
            int passMark = 0;
            string socket = " ";
            string producer = " "; 
            string model = modelBox.Text;
            string coreName = coreNameBox.Text;
            string image = imagePath.Text;
            string integrated_grafics = " ";


            string temp1 = costBox.Text.Replace('.', ',');
            string temp2 = frequencyBox.Text.Replace('.', ',');
            bool check = true;
            try
            {
                count = Convert.ToInt32(countBox.Text);
                cost = Convert.ToDouble(temp1);
                cores = Convert.ToInt32(coresBox.Text);
                threads = Convert.ToInt32(threadsBox.Text);
                frequency = Convert.ToDouble(temp2);
                technical_process = Convert.ToInt32(technical_processBox.Text);
                power = Convert.ToInt32(powerBox.Text);
                passMark = Convert.ToInt32(passMarkBox.Text);
                socket = ((Sockets)socketBox.SelectedIndex).ToString();
                producer = producerBox.SelectedItem.ToString();
                integrated_grafics = integrated_graficsBox.SelectedItem.ToString();
            }
            catch
            {
                check = false;
                MessageBoxNew message = new MessageBoxNew("Someone field entered uncorrectly!");
                message.Show();
            }

            int x = CPU.AddCPUtoBD(check,dbConnection, count, cost, cores, threads, frequency, technical_process, power, passMark, socket, producer, model, coreName, image, integrated_grafics,components);
            dbConnection.Close();
            if (x == 1)
            {
                MessageBoxNew message = new MessageBoxNew("CPU succesful added!");
                message.Show();
                costBox.Text = "";
                producerBox.Text = "";
                modelBox.Text = "";
                countBox.Text = "";
                imagePath.Text = "";
                coresBox.Text = "";
                threadsBox.Text = "";
                frequencyBox.Text = "";
                integrated_graficsBox.SelectedIndex = -1;
                technical_processBox.Text = "";
                socketBox.SelectedIndex = -1;
                passMarkBox.Text = "";
                coreNameBox.Text = "";
                powerBox.Text = "";
            }
            else if(x==-2)
            {
                MessageBoxNew message = new MessageBoxNew("Product with this model already exist!");
                message.Show();
            }        

                 
        }
    }
}
