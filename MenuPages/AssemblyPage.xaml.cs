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
    /// Логика взаимодействия для AssemblyPage.xaml
    /// </summary>
    public partial class AssemblyPage : Page
    {
        Frame menuFrame;
        User user;
        OleDbConnection dbConnection;
        ConstructPage constructPage;
        public AssemblyPage(List<Component> components, Frame menuFrame,User user,OleDbConnection dbConnection,ConstructPage constructPage)
        {
            InitializeComponent();
           
            this.menuFrame = menuFrame;
            this.user = user;
            this.dbConnection = dbConnection;
            this.constructPage = constructPage;
            componentsList.ItemsSource = components;

            


        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
                Border border = (Border)sender;
                Grid grid = (Grid)border.Child;
                 StackPanel stackPanel = (StackPanel)grid.Children[0];
                TextBlock textBlock1 = (TextBlock)stackPanel.Children[1];
                TextBlock textBlock2 = (TextBlock)stackPanel.Children[2];
                TextBlock textBlock3 = (TextBlock)stackPanel.Children[3];
                string text = textBlock1.Text + " " + textBlock2.Text + " " + textBlock3.Text;
                for (int i = 0; i < componentsList.ItemContainerGenerator.Items.Count; i++)
                {
                    Component component = (Component)componentsList.ItemContainerGenerator.Items[i];
                    string text2 = component.Type + " " + component.Producer + " " + component.Model;
                    if (text2 == text)
                    {
                        menuFrame.Content = new ComponentPage(component, menuFrame, user, dbConnection);
                        break;
                    }
                }
            
        }

      

        private void addToAssembly_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Grid grid = (Grid)button.Parent;           
            StackPanel stackPanel = (StackPanel)grid.Children[0];
            TextBlock textBlock1 = (TextBlock)stackPanel.Children[1];
            TextBlock textBlock2 = (TextBlock)stackPanel.Children[2];
            TextBlock textBlock3 = (TextBlock)stackPanel.Children[3];
            string text = textBlock1.Text + " " + textBlock2.Text + " " + textBlock3.Text;
            for (int i = 0; i < componentsList.ItemContainerGenerator.Items.Count; i++)
            {
                Component component = (Component)componentsList.ItemContainerGenerator.Items[i];
                string text2 = component.Type + " " + component.Producer + " " + component.Model;
                if (text2 == text)
                {
                    constructPage.FillComponentCell(component);
                    break;
                }
            }
            
        }
    }
}
