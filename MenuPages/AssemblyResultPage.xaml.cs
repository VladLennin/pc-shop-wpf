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

namespace КР2семестр
{
    /// <summary>
    /// Логика взаимодействия для AssemblyResultPage.xaml
    /// </summary>
    public partial class AssemblyResultPage : Page
    {

        Frame menuFrame;
        public AssemblyResultPage(List<string> errors, Assembly assembly,Frame menuFrame)
        {
            this.menuFrame = menuFrame;
            InitializeComponent();
            for (int i = 0; i < errors.Count; i++)
            {
                errorsPanel.Children.Add(new TextBlock() { Text = errors[i], Foreground = Brushes.Red, FontSize = 25 });
            }
            if (assembly != null)
            {
                List<Component> componentList = new List<Component>();
                if (assembly.Motherboard != null)
                    componentList.Add(assembly.Motherboard);

                if (assembly.Cpu != null)
                    componentList.Add(assembly.Cpu);
                if (assembly.Gpu != null)
                    componentList.Add(assembly.Gpu);
                if (assembly.RAM1 != null)
                    componentList.Add(assembly.RAM1);
                if (assembly.CoolingSystem != null)
                    componentList.Add(assembly.CoolingSystem);
                if (assembly.Gpu != null)
                    componentList.Add(assembly.PowerSupply);

                if (assembly.HDD1 != null)
                    componentList.Add(assembly.HDD1);

                if (assembly.SSD1 != null)
                    componentList.Add(assembly.SSD1);
                if (assembly.Case != null)
                    componentList.Add(assembly.Case);
                totalCost.Text = "Total Cost:" + assembly.CalculateCost(assembly).ToString() + " uah";
                assemblyList.ItemsSource = componentList;
            }
            else borderResult.Height = 500;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            menuFrame.NavigationService.GoBack();
        }
    }
}
