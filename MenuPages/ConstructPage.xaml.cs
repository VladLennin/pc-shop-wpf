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
namespace КР2семестр
{
    /// <summary>
    /// Логика взаимодействия для ConstructPage.xaml
    /// </summary>
    public partial class ConstructPage : Page
    {
        bool checkMB = false;
        Assembly assembly = new Assembly();
        List<Component> components;
        Frame menuFrame;
        User user;
        OleDbConnection dbConnection;
        public ConstructPage(List<Component> components, Frame menuFrame, User user, OleDbConnection dbConnection)
        {
            this.components = components;
            this.dbConnection = dbConnection;
            this.user = user;
            InitializeComponent();
            var list = components.FindAll(x => x.Type == ComponentType.Motherboard);
            this.menuFrame = menuFrame;
            assemblyFrame.Content = new AssemblyPage(list, menuFrame, user, dbConnection, this);
        }


        private void MotherBoard_button_Click(object sender, RoutedEventArgs e)
        {
            var list = components.FindAll(x => x.Type == ComponentType.Motherboard);
            assemblyFrame.Content = new AssemblyPage(list, menuFrame, user, dbConnection, this);
        }
        private void CPU_button_Click(object sender, RoutedEventArgs e)
        {
            var list = components.FindAll(x => x.Type == ComponentType.CPU);
            assemblyFrame.Content = new AssemblyPage(list, menuFrame, user, dbConnection, this);
        }

        private void GPU_button_Click(object sender, RoutedEventArgs e)
        {
            var list = components.FindAll(x => x.Type == ComponentType.GPU);
            assemblyFrame.Content = new AssemblyPage(list, menuFrame, user, dbConnection, this);
        }

        private void RAM_button_Click(object sender, RoutedEventArgs e)
        {
            var list = components.FindAll(x => x.Type == ComponentType.RAM);
            assemblyFrame.Content = new AssemblyPage(list, menuFrame, user, dbConnection, this);
        }

        private void PowerSupply_button_Click(object sender, RoutedEventArgs e)
        {
            var list = components.FindAll(x => x.Type == ComponentType.PowerSupply);
            assemblyFrame.Content = new AssemblyPage(list, menuFrame, user, dbConnection, this);
        }

        private void CoolingSystem_button_Click(object sender, RoutedEventArgs e)
        {
            var list = components.FindAll(x => x.Type == ComponentType.CoolingSystem);
            assemblyFrame.Content = new AssemblyPage(list, menuFrame, user, dbConnection, this);
        }

        private void HDD_button_Click(object sender, RoutedEventArgs e)
        {
            var list = components.FindAll(x => x.Type == ComponentType.HDD);
            assemblyFrame.Content = new AssemblyPage(list, menuFrame, user, dbConnection, this);
        }

        private void SSD_button_Click(object sender, RoutedEventArgs e)
        {
            var list = components.FindAll(x => x.Type == ComponentType.SSD);
            assemblyFrame.Content = new AssemblyPage(list, menuFrame, user, dbConnection, this);
        }

        private void Case_button_Click(object sender, RoutedEventArgs e)
        {
            var list = components.FindAll(x => x.Type == ComponentType.Case);
            assemblyFrame.Content = new AssemblyPage(list, menuFrame, user, dbConnection, this);
        }


        public void FillComponentCell(Component component)
        {
            if (component.Type == ComponentType.Case)
            {


                ImageBrush brush = new ImageBrush();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(component.Image, UriKind.Absolute);
                bitmap.EndInit();
                brush.Stretch = Stretch.Uniform;
                brush.ImageSource = bitmap;
                Case_button.Background = brush;
                Case_button.Opacity = 1;
                assembly.Case = (Case)component;

            }
            else if (component.Type == ComponentType.CPU)
            {


                ImageBrush brush = new ImageBrush();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(component.Image, UriKind.Absolute);
                bitmap.EndInit();
                brush.Stretch = Stretch.Uniform;
                brush.ImageSource = bitmap;
               CPU_button.Background = brush;
                CPU_button.Opacity = 1;
                assembly.Cpu = (CPU)component;
            }
            else if (component.Type == ComponentType.CoolingSystem)
            {


                ImageBrush brush = new ImageBrush();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(component.Image, UriKind.Absolute);
                bitmap.EndInit();
                brush.Stretch = Stretch.Uniform;
                brush.ImageSource = bitmap;
               CoolingSystem_button.Background = brush;
                CoolingSystem_button.Opacity = 1;
                assembly.CoolingSystem = (CoolingSystem)component;
            }
            else if (component.Type == ComponentType.GPU)
            {


                ImageBrush brush = new ImageBrush();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(component.Image, UriKind.Absolute);
                bitmap.EndInit();
                brush.Stretch = Stretch.Uniform;
                brush.ImageSource = bitmap;
                GPU_button.Background = brush;
                GPU_button.Opacity = 1;
                assembly.Gpu = (GPU)component;

            }
            else if (component.Type == ComponentType.HDD)
            {


                ImageBrush brush = new ImageBrush();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(component.Image, UriKind.Absolute);
                bitmap.EndInit();
                brush.Stretch = Stretch.Uniform;
                brush.ImageSource = bitmap;
                HDD_button.Background = brush;
                HDD_button.Opacity = 1;
                assembly.HDD1 = (HDD)component;

            }
            else if (component.Type == ComponentType.Motherboard)
            {


                ImageBrush brush = new ImageBrush();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(component.Image, UriKind.Absolute);
                bitmap.EndInit();
                brush.Stretch = Stretch.Uniform;
                brush.ImageSource = bitmap;
                MotherBoard_button.Background = brush;
                MotherBoard_button.Opacity = 1;
                assembly.Motherboard = (Motherboard)component;
            }
            else if (component.Type == ComponentType.PowerSupply)
            {


                ImageBrush brush = new ImageBrush();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(component.Image, UriKind.Absolute);
                bitmap.EndInit();
                brush.Stretch = Stretch.Uniform;
                brush.ImageSource = bitmap;
                PowerSupply_button.Background = brush;
                PowerSupply_button.Opacity = 1;
                assembly.PowerSupply = (PowerSupply)component;
            }
            else if (component.Type == ComponentType.RAM)
            {


                ImageBrush brush = new ImageBrush();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(component.Image, UriKind.Absolute);
                bitmap.EndInit();
                brush.Stretch = Stretch.Uniform;
                brush.ImageSource = bitmap;
                RAM_button.Background = brush;
                RAM_button.Opacity = 1;
                assembly.RAM1 = (RAM)component;
            }
            else if (component.Type == ComponentType.SSD)
            {


                ImageBrush brush = new ImageBrush();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(component.Image, UriKind.Absolute);
                bitmap.EndInit();
                brush.Stretch = Stretch.Uniform;
                brush.ImageSource = bitmap;
                SSD_button.Background = brush;
                SSD_button.Opacity = 1;
                assembly.SSD1 = (SSD)component;
            }

        }

        private void svaeAssemblyButton_Click(object sender, RoutedEventArgs e)
        {
            assembly.CalculateCost(assembly);
            if (nameAssembly.Text == "")
                assembly.Name = "Assembly";
            else assembly.Name = nameAssembly.Text;
            assembly.AuthorLogin = user.Login;
            
            assembly.CheckAssembly(assembly,menuFrame,assemblyFrame,user,dbConnection);
        }
    }
}
