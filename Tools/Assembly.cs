using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Data.OleDb;
namespace КР2семестр
{
    public class Assembly
    {
        Motherboard motherboard;
        CPU cpu;
        GPU gpu;
        PowerSupply powerSupply;
        Case _case;
        CoolingSystem coolingSystem;

        SSD SSD;
        HDD HDD;
        RAM RAM;

        string image;
        double cost;
        string authorLogin;
        string name;
        string time;
        public Assembly()
        {
        }

        public Assembly(Motherboard motherboard, CPU cpu, GPU gpu, PowerSupply powerSupply, Case @case, CoolingSystem coolingSystem, SSD ssd, HDD hdd, RAM ram)
        {
            this.Motherboard = motherboard;
            this.Cpu = cpu;
            this.Gpu = gpu;
            this.PowerSupply = powerSupply;
            Case = @case;
            this.CoolingSystem = coolingSystem;
            SSD = ssd;
            HDD = hdd;
            RAM = ram;


            Time = DateTime.Now.ToString();
            Image = Case.Image;

        }

        public Motherboard Motherboard { get => motherboard; set => motherboard = value; }
        public CPU Cpu { get => cpu; set => cpu = value; }
        public GPU Gpu { get => gpu; set => gpu = value; }
        public PowerSupply PowerSupply { get => powerSupply; set => powerSupply = value; }
        public Case Case { get => _case; set => _case = value; }
        public CoolingSystem CoolingSystem { get => coolingSystem; set => coolingSystem = value; }
        public string Image { get => image; set => image = value; }
        public double Cost { get => cost; set => cost = value; }
        public string AuthorLogin { get => authorLogin; set => authorLogin = value; }
        public string Name { get => name; set => name = value; }
        public SSD SSD1 { get => SSD; set => SSD = value; }
        public HDD HDD1 { get => HDD; set => HDD = value; }
        public RAM RAM1 { get => RAM; set => RAM = value; }
        public string Time { get => time; set => time = value; }

        public double CalculateCost(Assembly assembly)
        {
            double cost = 0;

            if (assembly.Cpu != null)
                cost += assembly.Cpu.Cost;

            if (assembly.Gpu != null)
                cost += assembly.Gpu.Cost;

            if (assembly.Motherboard != null)
                cost += assembly.Motherboard.Cost;

            if (assembly.PowerSupply != null)
                cost += assembly.PowerSupply.Cost;

            if (assembly.Case != null)
                cost += assembly.Case.Cost;

            if (assembly.CoolingSystem != null)
                cost += assembly.CoolingSystem.Cost;

            if (assembly.SSD != null)
                cost += assembly.SSD.Cost;

            if (assembly.HDD != null)
                cost += assembly.HDD.Cost;

            if (assembly.RAM != null)
                cost += assembly.RAM.Cost;


            return cost;
        }


        public List<Component> ConvertAssemblyToListComponents(Assembly assembly)
        {
            List<Component> components = new List<Component>();
            if (assembly.Cpu != null)
                components.Add(assembly.Cpu);

            if (assembly.Gpu != null)
                components.Add(assembly.Gpu);

            if (assembly.Motherboard != null)
                components.Add(assembly.Motherboard);

            if (assembly.PowerSupply != null)
                components.Add(assembly.PowerSupply);

            if (assembly.Case != null)
                components.Add(assembly.Case);

            if (assembly.CoolingSystem != null)
                components.Add(assembly.coolingSystem);

            if (assembly.SSD != null)
                components.Add(assembly.SSD);

            if (assembly.HDD != null)
                components.Add(assembly.HDD);

            if (assembly.RAM != null)
                components.Add(assembly.RAM);

            return components;
        }


        public void BuyAssemble(List<Component> components, Assembly assemble, User user, OleDbConnection dbConnection)
        {
            if (CheckComponentsOfAssembleOnStorage(components, assemble))
            {
                List<Component> componentsAssemble = assemble.ConvertAssemblyToListComponents(assemble);
                for (int i = 0; i < componentsAssemble.Count; i++)
                {
                    componentsAssemble[i].Count = 1;
                    user.Basket.Add(componentsAssemble[i]);

                    dbConnection.Open();
                    string query = $"SELECT COUNT(productCode) FROM basket WHERE productCode = {componentsAssemble[i].ProductCode} AND userCode = {user.UserCode} ";
                    OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
                    if ((int)dbCommand.ExecuteScalar() == 0)
                    {
                        query = $"INSERT INTO basket ([userCode],[productCode],[count]) VALUES('{user.UserCode}','{componentsAssemble[i].ProductCode}','1')";
                        dbCommand = new OleDbCommand(query, dbConnection);
                        dbCommand.ExecuteNonQuery();
                        for (int n = 0; n < components.Count; n++)
                        {
                            if (components[n].ProductCode == componentsAssemble[i].ProductCode)
                            {
                                int x = components[n].Count;
                                x--;
                                components[n].Count = x;
                                query = $"UPDATE products SET [count] = '{components[n].Count}' WHERE productCode = {componentsAssemble[i].ProductCode} ";
                                dbCommand = new OleDbCommand(query, dbConnection);
                                dbCommand.ExecuteNonQuery();
                                dbConnection.Close();
                            }
                        }
                    }
                    dbConnection.Close();

                }
                user.Basket = User.FillBasketFromBD(dbConnection, user);
                MessageBoxNew message2 = new MessageBoxNew("Assembly succesfull added to basket!");
                message2.Show();
            }

        }

        public bool CheckComponentsOfAssembleOnStorage(List<Component> components, Assembly assembly)
        {
            List<Component> componentsOfAssembly = ConvertAssemblyToListComponents(assembly);
            for (int i = 0; i < components.Count; i++)
            {
                for (int j = 0; j < componentsOfAssembly.Count; j++)
                {
                    if (components[i].ProductCode == componentsOfAssembly[j].ProductCode && components[i].Count == 0)
                    {
                        MessageBoxNew message2 = new MessageBoxNew($"On storage not inklude\n {components[i].ToString()}");
                        message2.Show();
                        return false;
                    }
                }
            }
            return true;

        }
        public void CheckAssembly(Assembly assembly, Frame menuFrame, Frame assemblyFrame, User user, OleDbConnection dbConnection)
        {
            List<string> errors = new List<string>();

            if (assembly.Motherboard != null)
            {
                if (assembly.Cpu != null)
                {
                    if (assembly.Cpu.Socket != assembly.Motherboard.Socket)
                    {
                        errors.Add("-Socket of cpu not compatibility with motherboard!");
                    }
                    if (assembly.Gpu == null)
                    {
                        if (assembly.Cpu.Integrated_Grafics == false)
                        {
                            errors.Add("-Your assembly not inklude nothing graficks!");
                        }
                    }
                }
                else
                    errors.Add("-Your assembly not inklude CPU!");

                if (assembly.RAM != null)
                {
                    if (assembly.RAM.Frequency > assembly.Motherboard.MaxFrequencyRAM)
                    {
                        errors.Add("-Frequency of ram modules too much big for this motherboard!");
                    }
                }
                else
                {
                    errors.Add("-Your assembly not inklude RAM!");
                }
                if (assembly.PowerSupply == null)
                    errors.Add("-Your assembly  not inklude PowerSupply");

                if (assembly.Case == null)
                    errors.Add("-You assembly not inklude case!");

                if (assembly.CoolingSystem != null && assembly.Cpu != null)
                {
                    if (assembly.CoolingSystem.CompatibilityAMD == true && assembly.Cpu.Producer == "Intel")
                    {
                        if (assembly.CoolingSystem.CompatibilityIntel != true)
                        {
                            errors.Add("-Cooling system not compatibility with CPU!");
                        }

                    }
                    if (assembly.CoolingSystem.CompatibilityIntel == true && assembly.Cpu.Producer == "Amd")
                    {
                        if (assembly.CoolingSystem.CompatibilityAMD != true)
                        {
                            errors.Add("-Cooling system not compatibility with CPU!");
                        }
                    }
                }
                else
                    errors.Add("-Your assembly not inklude CoolingSystem!");

                if (assembly.HDD == null && assembly.SSD == null)
                    errors.Add("-Your assembly not inklude hard memory!");
            }
            else errors.Add("-Your assembly not inklude motherboard!");

            string query = $"SELECT Count(assemblyName) FROM assemblies WHERE assemblyName='{assembly.Name}'";
            dbConnection.Open();
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);

            if ((int)dbCommand.ExecuteScalar() == 0)
            {
                dbConnection.Close();
                if (errors.Count != 0)
                { assemblyFrame.Content = new AssemblyResultPage(errors, null, menuFrame); }
                else
                {
                    assembly.Cost = CalculateCost(assembly);
                    assembly.Image = assembly.Case.Image;
                    user.Assemblies.Add(assembly);
                    assembly.Time = DateTime.Now.ToString();
                    user.WriteAssemblyToBD(dbConnection, assembly);
                    menuFrame.Content = new AssemblyResultPage(errors, assembly, menuFrame);
                }
            }
            else
            {
                MessageBoxNew message = new MessageBoxNew("Change assembly name!");
                message.Show();
                dbConnection.Close();
            }





        }
    }
}
