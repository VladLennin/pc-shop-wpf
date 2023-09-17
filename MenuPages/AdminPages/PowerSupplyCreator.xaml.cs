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
    /// Логика взаимодействия для PowerSupplyCreator.xaml
    /// </summary>
    public partial class PowerSupplyCreator : Page
    {
        OleDbConnection dbConnection;
        List<Component> components;
        public PowerSupplyCreator( OleDbConnection dbConnection,List<Component>components)
        {
            this.dbConnection = dbConnection;
            this.components = components;
            InitializeComponent();
            for (int i = 0; i < 3; i++)
            {
                formBox.Items.Add(((PowerSupplyform)i).ToString());
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

            string model = " ";
            string producer = " ";
            string image = " ";
            int count = 0;
            double cost = 0;

            double power = 0;
            string form = " ";

            string gpuPower = " ";
            string motherBoardPower = " ";
            string cpuPower = " ";
            int countGPUslots = 0;
            int countSATA = 0;
            int countMolex = 0;
            string type = "PowerSupply";
            bool check = false;
            try
            {
                model = modelBox.Text;
                producer = producerBox.Text;
                image = imagePath.Text;
                count = Convert.ToInt32(countBox.Text);
                cost = Convert.ToDouble(costBox.Text);
                power = Convert.ToDouble(powerBox.Text);
                form = formBox.SelectedItem.ToString();
                gpuPower = gpuPowerBox.Text;
                motherBoardPower = motherboardPowerBox.Text;
                cpuPower = cpuPowerBox.Text;
                countGPUslots = Convert.ToInt32(countGPUbox.Text);
                countSATA = Convert.ToInt32(countSATAbox.Text);
                countMolex = Convert.ToInt32(countMolexBox.Text);
                check = true;
            }
            catch
            {

                MessageBoxNew message = new MessageBoxNew("Some field entered uncorectly!");
                message.Show();
            }
            if(check)
            {
              bool check2 =   PowerSupply.AddPowerSupplyToBD(model, cost, image, producer, count,type, power, form, gpuPower, motherBoardPower, cpuPower, countGPUslots, countSATA, countMolex, dbConnection, components);
                if (check2)
                {
                    modelBox.Text = "";
                    countBox.Text = "";
                    costBox.Text = "";
                    producerBox.Text = "";
                    imagePath.Text = "";
                    formBox.SelectedIndex = -1;
                    powerBox.Text = "";
                    gpuPowerBox.Text = "";
                    motherboardPowerBox.Text = "";
                    cpuPowerBox.Text = "";
                    countGPUbox.Text = "";
                    countSATAbox.Text = "";
                    countMolexBox.Text = "";
                    MessageBoxNew message = new MessageBoxNew("PowerSupply succesfull added!");
                    message.Show();

                }
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
    }
}
