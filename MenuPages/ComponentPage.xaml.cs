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
using System.Collections.ObjectModel;
using System.Data.OleDb;

namespace КР2семестр
{
    /// <summary>
    /// Логика взаимодействия для ComponentPage.xaml
    /// </summary>
    public partial class ComponentPage : Page
    {
        Frame menuFrame;
        Component component;
        User user;
        OleDbConnection dbConnection;

        int countStars;
        public ComponentPage(Component component,Frame menuFrame,User user,OleDbConnection dbConnection)
        {
            InitializeComponent();

            this.component = component;
            this.menuFrame = menuFrame;
            this.user = user;
            this.dbConnection = dbConnection;
            if(component.Count==0)
            {
                buttonBuy.Background = Brushes.Gray;
                buttonBuy.Content = "Not available";
                buttonBuy.MaxWidth = 270;
                buttonBuy.Click -= buttonBuy_Click;
            }


           
            feedbackList.ItemsSource = component.Feedbacks;

            //string path = Environment.CurrentDirectory;
            //string[] temp = path.Split('\\');
            //temp[temp.Length - 1] = "";
            //temp[temp.Length - 2] = "";
            //path = "";
            //for (int i = 0; i < temp.Length; i++)
            //{
            //    if(temp[i]!="")
            //    path += temp[i] + "\\";
            //}
            //path += $"Resources\\{component.Image}";


            ProductImage.Source = (ImageSource)new ImageSourceConverter().ConvertFromString(component.Image);
            ComponentString.Text = component.ToString();
            AddCharacteristics(this.component);
            //component.Feedbacks.Clear();
           
        



        }

        //public void FillFeedbacksFromDB()
        //{
        //    string text = " ";
        //    string user_login = " ";
        //    DateTime time = new DateTime();
        //    int countStars = 0;
        //    string name = " ";

        //        dbConnection.Open();
        //    string query = $"SELECT COUNT(productCode) FROM feedbacks WHERE productCode = {component.ProductCode}";
        //    OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
        //    if ((int)dbCommand.ExecuteScalar() != 0)
        //    {

        //        query = $"SELECT feedbackText,user_login,feedbackTime,countStars,user_name FROM feedbacks WHERE productCode={component.ProductCode}";
        //        dbCommand = new OleDbCommand(query, dbConnection);
        //        OleDbDataReader dbReader = dbCommand.ExecuteReader();
        //        while (dbReader.Read())
        //        {
        //            text = (string)dbReader["feedbackText"];
        //            user_login = (string)dbReader["user_login"];
        //            time = (DateTime)dbReader["feedbackTime"];
        //            countStars = (int)dbReader["countStars"];
        //            name = (string)dbReader["user_name"];
        //                component.AddFeedBack(new Feedback(text,name,user_login,countStars));
        //        }                   
                  
        //    }
        //        dbConnection.Close();
        //}






        private void buttonBuy_Click(object sender, RoutedEventArgs e)
        {
            dbConnection.Open();
            string query = $"SELECT COUNT(productCode) FROM basket WHERE productCode = {component.ProductCode} AND userCode = {user.UserCode} ";
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            if((int)dbCommand.ExecuteScalar()==0)
            {
                query = $"INSERT INTO basket ([userCode],[productCode],[count]) VALUES('{user.UserCode}','{component.ProductCode}','1')";
                dbCommand = new OleDbCommand(query, dbConnection);
                dbCommand.ExecuteNonQuery();
                MessageBoxNew message = new MessageBoxNew("Product Succesfull added to basket!");
                message.Show();
                component.Count--;
                query = $"UPDATE products SET [count] = '{component.Count}' WHERE productCode = {component.ProductCode} ";
                dbCommand = new OleDbCommand(query, dbConnection);
                dbCommand.ExecuteNonQuery();
                dbConnection.Close();
                user.Basket = User.FillBasketFromBD(dbConnection, user);
            }
            else
            {
                MessageBoxNew message = new MessageBoxNew("You already have this product\n          in your basket!");
                message.Show();
                dbConnection.Close();
            }
           
        }



        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            menuFrame.NavigationService.GoBack();
        }
        public void AddCharacteristics(Component component)
        {
            if (component is CPU)
            {
                CPU cpu = (CPU)component;

                List<TextBlock> characteristics = new List<TextBlock>()
                {   new TextBlock() { Text = "Socket:  " + cpu.Socket.ToString(),FontSize = 20 ,Margin = new Thickness(7)},
                    new TextBlock() { Text ="Count cores:  "+ cpu.Cores.ToString(),FontSize = 20,Margin = new Thickness(7) },
                    new TextBlock() {Text = "Count threads:  "+ cpu.Threads.ToString(),FontSize = 20,Margin = new Thickness(7) },
                    new TextBlock() {Text = "MAX frequency:  "+cpu.Frequency.ToString()+" GHz",FontSize = 20,Margin = new Thickness(7) },
                    new TextBlock() { Text = "Technical process:  " + cpu.Technical_Process.ToString()+" nm",FontSize = 20,Margin = new Thickness(7) },
                    new TextBlock() { Text = "Power:  " + cpu.Power.ToString()+" w",FontSize = 20,Margin = new Thickness(7) }, 
                    new TextBlock() { Text = "PassMark points:  " +cpu.PassMark.ToString()+" points",FontSize = 20,Margin = new Thickness(7)},
                    new TextBlock() { Text = "Core Name:  " + cpu.CoreName,FontSize = 20,Margin = new Thickness(7) }
                };

                if (cpu.Integrated_Grafics)
                 characteristics.Add(new TextBlock() { Text = "Integrated graphic:  Yes", FontSize = 20, Margin = new Thickness(7) });
                else if(cpu.Integrated_Grafics==false)
                 characteristics.Add(new TextBlock() { Text = "Integrated graphic:  No", FontSize = 20, Margin = new Thickness(7) });


                for (int i = 0; i < characteristics.Count; i++)
                {
                    CharacteristicsPanel.Children.Add(characteristics[i]);
                }
                ComponentCost.Text = cpu.Cost.ToString()+" uah";
                
            }
            else if(component is GPU)
            {
                GPU gpu = (GPU)component;

                List<TextBlock> characteristics = new List<TextBlock>()
                {   new TextBlock() { Text = "GPU family:  " + gpu.FamilyGPU.ToString(),FontSize = 20 ,Margin = new Thickness(7)},
                    new TextBlock() { Text ="Memory capacity:  "+ gpu.MemoryCapacity.ToString()+" Gb",FontSize = 20,Margin = new Thickness(7) },
                    new TextBlock() {Text = "Digit capacity:  "+ gpu.DigitCapacity.ToString()+" bit",FontSize = 20,Margin = new Thickness(7) },
                    new TextBlock() {Text = "Memory gen:  "+gpu.MemoryGen.ToString(),FontSize = 20,Margin = new Thickness(7) },                   
                    new TextBlock() {Text = "PassMark points:  " +gpu.PassMark.ToString()+" points",FontSize = 20,Margin = new Thickness(7)},  
                    new TextBlock() {Text = "Ports:  ",FontSize = 20,Margin = new Thickness(7)}
                };
                for (int i = 0; i < gpu.CountPorts; i++)
                {
                    if(i==0)
                    characteristics[5].Text += "\n" + gpu[i].ToString();
                    else
                    characteristics[5].Text +=gpu[i].ToString();

                }


                for (int i = 0; i < characteristics.Count; i++)
                {
                    CharacteristicsPanel.Children.Add(characteristics[i]);
                }
                ComponentCost.Text = gpu.Cost.ToString() + " uah";
            }
            else if(component is HDD)
            {
                HDD hdd = (HDD)component;

                List<TextBlock> characteristics = new List<TextBlock>()
                {   new TextBlock() { Text ="Memory capacity:  "+ hdd.MemoryCapacity.ToString()+" Gb",FontSize = 20,Margin = new Thickness(7) },
                    new TextBlock() {Text = "Form factor:  "+ hdd.FormFactor.ToString(),FontSize = 20,Margin = new Thickness(7) },
                    new TextBlock() {Text = "Speed Rotate:  "+ hdd.SpeedRotate.ToString()+" rps",FontSize = 20,Margin = new Thickness(7) },
                    new TextBlock() {Text = "Connect Port:  "+hdd.PortConnect,FontSize = 20,Margin = new Thickness(7) },
                    new TextBlock() {Text = "Speed Read:  " +hdd.SpeedRead.ToString()+" Mb/s",FontSize = 20,Margin = new Thickness(7)},
                    new TextBlock() {Text = "Speed Write:  "+hdd.SpeedWrite+" Mb/s",FontSize = 20,Margin = new Thickness(7)}
                };
                for (int i = 0; i < characteristics.Count; i++)
                {
                    CharacteristicsPanel.Children.Add(characteristics[i]);
                }
                ComponentCost.Text = hdd.Cost.ToString() + " uah";
            }
            else if(component is Motherboard)
            {
                Motherboard motherboard = (Motherboard)component;

                List<TextBlock> characteristics = new List<TextBlock>()
                {   new TextBlock() {Text ="Socket:  "+ motherboard.Socket.ToString(),FontSize = 20,Margin = new Thickness(7) },
                    new TextBlock() {Text = "For CPU:  "+ motherboard.ForCPU.ToString(),FontSize = 20,Margin = new Thickness(7) },
                    new TextBlock() {Text = "Size:  "+motherboard.Size.ToString(),FontSize = 20,Margin = new Thickness(7) },
                    new TextBlock() {Text = "RAM gen:  " +motherboard.RamGen.ToString(),FontSize = 20,Margin = new Thickness(7)},
                    new TextBlock() {Text = "Chipset:  "+motherboard.Chipset,FontSize = 20,Margin = new Thickness(7)},
                    new TextBlock() {Text = "Count RAM:  "+motherboard.CountRAM,FontSize = 20,Margin = new Thickness(7)},
                    new TextBlock() {Text = "Count Channels RAM:  "+motherboard.CountChannelRAM,FontSize = 20,Margin = new Thickness(7)},
                    new TextBlock() {Text = "MAX capacity RAM:  "+motherboard.MaxCapacityRAM+" Gb",FontSize = 20,Margin = new Thickness(7)},
                    new TextBlock() {Text = "MAX Frequency RAM:  "+motherboard.MaxFrequencyRAM+" Mhz",FontSize = 20,Margin = new Thickness(7)},
                    new TextBlock() {Text = "Ports:  ",FontSize = 20,Margin = new Thickness(7)}
                };
                for (int i = 0; i < motherboard.CountPorts; i++)
                {
                    if (i == 0)
                        characteristics[9].Text +="\n"+ motherboard[i].ToString();
                    else
                        characteristics[9].Text += motherboard[i].ToString();

                }
                for (int i = 0; i < characteristics.Count; i++)
                {
                    CharacteristicsPanel.Children.Add(characteristics[i]);
                }
                ComponentCost.Text = motherboard.Cost.ToString() + " uah";
            }
            else if(component is PowerSupply)
            {
                PowerSupply ps = (PowerSupply)component;

                List<TextBlock> characteristics = new List<TextBlock>()
                {   new TextBlock() {Text ="Power:  "+ ps.Power.ToString()+" w",FontSize = 20,Margin = new Thickness(7) },
                    new TextBlock() {Text = "Form Factor:  "+ ps.FormFactor.ToString(),FontSize = 20,Margin = new Thickness(7) },
                    new TextBlock() {Text = "GPU power:  "+ps.GpuPower.ToString(),FontSize = 20,Margin = new Thickness(7) },
                    new TextBlock() {Text = "MotherBoard power:  " +ps.MotherBoardPower.ToString(),FontSize = 20,Margin = new Thickness(7)},
                    new TextBlock() {Text = "CPU power:  "+ps.CpuPower,FontSize = 20,Margin = new Thickness(7)},
                    new TextBlock() {Text = "Count GPU slots:  "+ps.CountGPUslots,FontSize = 20,Margin = new Thickness(7)},
                    new TextBlock() {Text = "Count SATA:  "+ps.CountSATA,FontSize = 20,Margin = new Thickness(7)},
                    new TextBlock() {Text = "Count MOLEX:  "+ps.CountMolex,FontSize = 20,Margin = new Thickness(7)},                    
                };
                for (int i = 0; i < characteristics.Count; i++)
                {
                    CharacteristicsPanel.Children.Add(characteristics[i]);
                }
                ComponentCost.Text = ps.Cost.ToString() + " uah";
            }
            else if(component is RAM)
            {

                RAM ram = (RAM)component;

                List<TextBlock> characteristics = new List<TextBlock>()
                {   new TextBlock() {Text ="Frequency:  "+ ram.Frequency.ToString()+" MHz",FontSize = 20,Margin = new Thickness(7) },
                    new TextBlock() {Text = "Power:  "+ ram.Power.ToString()+" V",FontSize = 20,Margin = new Thickness(7) },
                    new TextBlock() {Text = "Memory gen:  "+ram.Gen.ToString(),FontSize = 20,Margin = new Thickness(7) },
                    new TextBlock() {Text = "Memory Capacity :  " +ram.Capacity.ToString()+" Gb",FontSize = 20,Margin = new Thickness(7)},
                     new TextBlock() {Text = "Count modules:  " +ram.CountModules.ToString(),FontSize = 20,Margin = new Thickness(7)},
                };

                for (int i = 0; i < characteristics.Count; i++)
                {
                    CharacteristicsPanel.Children.Add(characteristics[i]);
                }
                ComponentCost.Text = ram.Cost.ToString() + " uah";
            }
            else if(component is SSD)
            {
                SSD ssd = (SSD)component;

                List<TextBlock> characteristics = new List<TextBlock>()
                {   new TextBlock() { Text ="Memory capacity:  "+ ssd.MemoryCapacity.ToString()+" Gb",FontSize = 20,Margin = new Thickness(7) },
                    new TextBlock() {Text = "Form factor:  "+ ssd.FormFactor.ToString(),FontSize = 20,Margin = new Thickness(7) },
                    new TextBlock() {Text = "Memory type:  "+ ssd.TypeSSD.ToString(),FontSize = 20,Margin = new Thickness(7) },
                    new TextBlock() {Text = "Connect Port:  "+ssd.PortConnect,FontSize = 20,Margin = new Thickness(7) },
                    new TextBlock() {Text = "Speed Read:  " +ssd.SpeedRead.ToString()+" Mb/s",FontSize = 20,Margin = new Thickness(7)},
                    new TextBlock() {Text = "Speed Write:  "+ssd.SpeedWrite+" Mb/s",FontSize = 20,Margin = new Thickness(7)}
                };
                for (int i = 0; i < characteristics.Count; i++)
                {
                    CharacteristicsPanel.Children.Add(characteristics[i]);
                }
                ComponentCost.Text = ssd.Cost.ToString() + " uah";
            }
            else if (component is CoolingSystem)
            {
                CoolingSystem coolingSystem = (CoolingSystem)component;

                List<TextBlock> characteristics = new List<TextBlock>()
                {  
                    new TextBlock() { Text ="AMD compatibility:  ",FontSize = 20,Margin = new Thickness(7) },
                    new TextBlock() {Text = "Intel compatibility:  ",FontSize = 20,Margin = new Thickness(7) }, 
                };
                if (coolingSystem.CompatibilityAMD == true)
                    characteristics[0].Text += "Yes";
                else
                    characteristics[0].Text += "No";

                if (coolingSystem.CompatibilityIntel == true)
                    characteristics[1].Text += "Yes";
                else
                    characteristics[1].Text += "No";

                for (int i = 0; i < characteristics.Count; i++)
                {
                    CharacteristicsPanel.Children.Add(characteristics[i]);
                }
                ComponentCost.Text = coolingSystem.Cost.ToString() + " uah";
            }
            else if (component is Case)
            {
                Case case1 = (Case)component;

                List<TextBlock> characteristics = new List<TextBlock>()
                {   new TextBlock() { Text ="Compatibility sizes motherboards:  ",FontSize = 20,Margin = new Thickness(7) },
                    new TextBlock() {Text = "Type:  "+ case1.TypeCase.ToString(),FontSize = 20,Margin = new Thickness(7) },
                    new TextBlock() {Text = "Count 3.5 slots:  "+ case1.CountSlots3_5,FontSize = 20,Margin = new Thickness(7) },
                    new TextBlock() {Text = "Count 2.5 slots:  "+case1.CountSlots2_5,FontSize = 20,Margin = new Thickness(7) },
                    
                };

                for (int i = 0; i < case1.SizesMotherboards.Count-1; i++)
                {
                    characteristics[0].Text += $"\n -{case1.SizesMotherboards[i]}";
                }
                for (int i = 0; i < characteristics.Count; i++)
                {
                    CharacteristicsPanel.Children.Add(characteristics[i]);
                }
                ComponentCost.Text = case1.Cost.ToString() + " uah";
            }

        }
        public void AddFeedbacks(Component component)
        {
            feedbackList.ItemsSource = component.Feedbacks;

        }    
        private void addFeedBack_Click(object sender, RoutedEventArgs e)
        {
            bool check = true; ;
            for (int i = 0; i < component.Feedbacks.Count; i++)
            {
                if(component.Feedbacks[i].Login==user.Login)
                {
                    MaterialDesignThemes.Wpf.HintAssist.SetHint(feedBackText, "You wrote feedback early!");
                    feedBackText.Text = " ";
                    feedBackText.BorderBrush = Brushes.Red;
                   
                    star_empty();
                    check = false;
                    break;
                }
            }
            if (check)
            { 
                

                Feedback feedback = new Feedback(feedBackText.Text, user.Name + " " + user.Surname, user.Login, countStars);
                component.AddFeedBack(feedback);                            
               


                dbConnection.Open();
                string query = $"INSERT INTO  feedbacks  (productCode,feedbackText,feedbackTime,countStars,user_login,user_name) VALUES('{component.ProductCode}','{feedback.Text}','{feedback.Time}','{countStars}','{user.Login}','{feedback.Name}')";
                OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);             
                dbCommand.ExecuteNonQuery();
                dbConnection.Close();

                feedBackText.Text = " ";
                star_empty();
            }
        }
        private void feedBackText_KeyDown(object sender, KeyEventArgs e)
        {
            MaterialDesignThemes.Wpf.HintAssist.SetHint(feedBackText, "Enter your feedback!");
            feedBackText.BorderBrush = Brushes.LawnGreen;
        }
        #region StarsFiller
        private void star_empty()
        {
            star1.Foreground = Brushes.LightGray;
            star2.Foreground = Brushes.LightGray;
            star3.Foreground = Brushes.LightGray;
            star4.Foreground = Brushes.LightGray;
            star5.Foreground = Brushes.LightGray;
            countStars = 0;
        }
        private void star1_Click(object sender, RoutedEventArgs e)
        {
            star1.Foreground = Brushes.Yellow;
            star2.Foreground = Brushes.LightGray;
            star3.Foreground = Brushes.LightGray;
            star4.Foreground = Brushes.LightGray;
            star5.Foreground = Brushes.LightGray;
            countStars = 1;
        }
        private void star2_Click(object sender, RoutedEventArgs e)
        {
            star1.Foreground = Brushes.Yellow;
            star2.Foreground = Brushes.Yellow;
            star3.Foreground = Brushes.LightGray;
            star4.Foreground = Brushes.LightGray;
            star5.Foreground = Brushes.LightGray;
            countStars = 2;
        }

        private void star3_Click(object sender, RoutedEventArgs e)
        {
            star1.Foreground = Brushes.Yellow;
            star2.Foreground = Brushes.Yellow;
            star3.Foreground = Brushes.Yellow;
            star4.Foreground = Brushes.LightGray;
            star5.Foreground = Brushes.LightGray;
            countStars = 3;
        }

        private void star4_Click(object sender, RoutedEventArgs e)
        {
            star1.Foreground = Brushes.Yellow;
            star2.Foreground = Brushes.Yellow;
            star3.Foreground = Brushes.Yellow;
            star4.Foreground = Brushes.Yellow;
            star5.Foreground = Brushes.LightGray;
            countStars = 4;
        }

        private void star5_Click(object sender, RoutedEventArgs e)
        {
            star1.Foreground = Brushes.Yellow;
            star2.Foreground = Brushes.Yellow;
            star3.Foreground = Brushes.Yellow;
            star4.Foreground = Brushes.Yellow;
            star5.Foreground = Brushes.Yellow;
            countStars = 5;
        }
        #endregion /////////////////////////////////////////////////Stars Filler    //Stars Filler

    }
}
