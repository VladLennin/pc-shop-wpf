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
    /// Логика взаимодействия для CaseCreator.xaml
    /// </summary>
    public partial class CaseCreator : Page
    {
        OleDbConnection dbConnection;
        List<Component> components;
        List<string> sizes = new List<string>();
        public CaseCreator(List<Component> components,OleDbConnection dbConnection)
        {
            InitializeComponent();
            this.dbConnection = dbConnection;
            this.components = components;
            for (int i = 0; i < 4; i++)
            {
                sizeMotherboardBox.Items.Add((MotherboardSizes)i);
            }
            for (int i = 0; i < 5; i++)
            {
                typeBox.Items.Add((TypeCases)i);
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
            string model = " ";
            string producer = " ";
            double cost = 0;
            int count = 0;
            string image = " ";

            string typeCase = " ";
            int count3_5 = 0;
            int count2_5 = 0;
            string type = "Case";
            bool check = false;
            try
            {
                if (sizes.Count > 0)
                {
                    model = modelBox.Text;
                    producer = producerBox.Text;
                    cost = Convert.ToDouble(costBox.Text);
                    count = Convert.ToInt32(countBox.Text);
                    image = imagePath.Text;
                    typeCase = typeBox.SelectedItem.ToString();
                    count3_5 = int.Parse(countSlots3_5.Text);
                    count2_5 = int.Parse(countSlots2_5.Text);
                    check = true;
                }
                else
                    throw new Exception("Sizes is empty!");
            }
            catch
            {

                MessageBoxNew message = new MessageBoxNew("Some field entered uncorectly!");
                message.Show();
            }
            if (check)
            {
                bool check2 = Case.AddCaseToBD(model,producer,cost,image,count,type,sizes,typeCase,count3_5,count2_5,components,dbConnection);
                if (check2)
                {
                    costBox.Text = "";
                    producerBox.Text = "";
                    modelBox.Text = "";
                    countBox.Text = "";
                    imagePath.Text = "";
                    sizes.Clear();
                    typeBox.SelectedIndex = -1;
                    countSlots2_5.Text = "";
                    countSlots3_5.Text = "";

                    MessageBoxNew message = new MessageBoxNew("Case succesfull added!");
                    message.Show();
                }
            }
        }

        private void addSize_Click(object sender, RoutedEventArgs e)
        {
            bool check = true;
            for (int i = 0; i < sizes.Count; i++)
            {
                if(sizes[i] == sizeMotherboardBox.SelectedItem.ToString())
                {
                    check = false;
                }
            }
            if (check)
            {
                if (sizeMotherboardBox.SelectedIndex != -1)
                {
                    sizes.Add(sizeMotherboardBox.SelectedItem.ToString());
                    sizesBox.Text += sizeMotherboardBox.SelectedItem.ToString() + ", ";
                    sizeMotherboardBox.SelectedIndex = -1;
                }
            }
        }
    }
}
