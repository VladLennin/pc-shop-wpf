using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КР2семестр
{
    public class PowerSupply : Component, ICloneable,IWorkWithBD
    {
        double power;
        PowerSupplyform formFactor;

        string gpuPower;
        string motherBoardPower;
        string cpuPower;

        int countGPUslots;
        int countSATA;
        int countMolex;

        public PowerSupply(string producer, double power, PowerSupplyform formFactor, string gpuPower, string motherBoardPower, string cpuPower, int countGPUslots, int countSATA, int countMolex, string model, double cost, string image, ComponentType type, int count, int productCode) : base(model, count, cost, producer, image, type, productCode)
        {

            this.power = power;
            this.formFactor = formFactor;
            this.gpuPower = gpuPower;
            this.motherBoardPower = motherBoardPower;
            this.cpuPower = cpuPower;
            this.countGPUslots = countGPUslots;
            this.countSATA = countSATA;
            this.countMolex = countMolex;
        }
        public PowerSupply(string producer, double power, PowerSupplyform formFactor, string gpuPower, string motherBoardPower, string cpuPower, int countGPUslots, int countSATA, int countMolex, string model, double cost, ComponentType type, int count, int productCode) : base(model, count, cost, producer, type, productCode)
        {

            this.power = power;
            this.formFactor = formFactor;
            this.gpuPower = gpuPower;
            this.motherBoardPower = motherBoardPower;
            this.cpuPower = cpuPower;
            this.countGPUslots = countGPUslots;
            this.countSATA = countSATA;
            this.countMolex = countMolex;
        }

        public PowerSupply()
        {
        }

        public double Power { get => power; set => power = value; }
        public string GpuPower { get => gpuPower; set => gpuPower = value; }
        public string MotherBoardPower { get => motherBoardPower; set => motherBoardPower = value; }
        public string CpuPower { get => cpuPower; set => cpuPower = value; }
        public int CountGPUslots { get => countGPUslots; set => countGPUslots = value; }
        public int CountSATA { get => countSATA; set => countSATA = value; }
        public int CountMolex { get => countMolex; set => countMolex = value; }
        public PowerSupplyform FormFactor { get => formFactor; set => formFactor = value; }

        public  void FillComponentsFromBDtoBasket(List<Component> basket, OleDbConnection dbConnection, int userCode)
        {
            dbConnection.Open();
            List<int> productCodes = new List<int>();
            List<int> counts = new List<int>();


            string query = $"SELECT productCode,count FROM basket WHERE userCode = {userCode}";
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            OleDbDataReader dbReader = dbCommand.ExecuteReader();
            while (dbReader.Read())
            {
                productCodes.Add((int)dbReader["productCode"]);
                counts.Add((int)dbReader["count"]);
            }

            for (int i = 0; i < productCodes.Count; i++)
            {
                query = $"SELECT  [model],[producer],[count],[cost],[productCode],[image],[power],[formFactor],[gpuPower],[motherboardPower],[cpuPower],[countGPUslots],[countSATA],[countMolex] FROM products WHERE productCode = {productCodes[i]} and type = 'PowerSupply'";
                dbCommand = new OleDbCommand(query, dbConnection);
                dbReader = dbCommand.ExecuteReader();
                while (dbReader.Read())
                {
                    string model = (string)dbReader["model"];
                    string producer = (string)dbReader["producer"];
                    double cost = (double)dbReader["cost"];
                    string image = (string)dbReader["image"];

                    double power = (double)dbReader["power"];
                    string formFactor = (string)dbReader["formFactor"];
                    string gpuPower = (string)dbReader["gpuPower"];
                    string motherBoardPower = (string)dbReader["motherboardPower"];
                    string cpuPower = (string)dbReader["cpuPower"];
                    int countGPUslots = (int)dbReader["countGPUslots"];
                    int countSATA = (int)dbReader["countSATA"];
                    int countMolex = (int)dbReader["countMolex"];

                    PowerSupplyform form = PowerSupplyform.ATX;
                    for (int l = 0; l < 3; l++)
                    {
                        if (((PowerSupplyform)l).ToString() == formFactor)
                        {
                            form = (PowerSupplyform)l;
                            break;
                        }
                    }


                    PowerSupply powerSupply = new PowerSupply(producer, power, form, gpuPower, motherBoardPower, cpuPower, countGPUslots, countSATA, countMolex, model, cost, image, ComponentType.PowerSupply, counts[i], productCodes[i]);
                    powerSupply.Count = counts[i];
                    basket.Add(powerSupply);
                }


            }
            dbConnection.Close();
        }
        public static bool AddPowerSupplyToBD(string model,double cost,string image, string producer,int count,string type,double power,string formFactor,string gpuPower,string motherboardPower,string cpuPower, int countGPU,int countSATA,int countMolex,OleDbConnection dbConnection,List<Component> components )
        {
            bool check = true;
            dbConnection.Open();
            string query = $"SELECT COUNT(model) FROM products WHERE model = '{model}' and producer = '{producer}'";
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            if ((int)dbCommand.ExecuteScalar() == 0)
            {
                query = $"INSERT INTO products ([model],[producer],[count],[cost],[type],[image],[power],[formFactor],[gpuPower],[motherboardPower],[cpuPower],[countGPUslots],[countSATA],[countMolex]) " +
                    $"VALUES('{model}','{producer}',{count},'{cost}','{type}','{image}','{power}','{formFactor}','{gpuPower}','{motherboardPower}','{cpuPower}',{countGPU},{countSATA},{countMolex})";
                dbCommand = new OleDbCommand(query, dbConnection);
                dbCommand.ExecuteNonQuery();
               
            }
            else
            {
                MessageBoxNew message = new MessageBoxNew("Product with this model already exist!");
                message.Show();
                check = false;
            }

            dbConnection.Close();
            MainMenu.SelectComponentsFromBD(dbConnection, components);
            return check;
        }
        public  void FillComponentsFromBD(List<Component> components, OleDbConnection dbConnection)
        {
            dbConnection.Open();

            string query = $"SELECT COUNT(type) FROM products WHERE [type]='PowerSupply' ";
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            if ((int)dbCommand.ExecuteScalar() != 0)
            {
                query = $"SELECT [model],[producer],[count],[cost],[productCode],[image],[power],[formFactor],[gpuPower],[motherboardPower],[cpuPower],[countGPUslots],[countSATA],[countMolex] FROM products WHERE type = 'PowerSupply' ";
                dbCommand = new OleDbCommand(query, dbConnection);
                OleDbDataReader dbReader = dbCommand.ExecuteReader();
                while (dbReader.Read())
                {
                    string model = (string)dbReader["model"];
                    string producer = (string)dbReader["producer"];
                    double cost = (double)dbReader["cost"];
                    int count = (int)dbReader["count"];
                    int productCode = (int)dbReader["productCode"];
                    string image = (string)dbReader["image"];
                    
                    double power = (double)dbReader["power"];
                    string formFactor = (string)dbReader["formFactor"];
                    string gpuPower = (string)dbReader["gpuPower"];
                    string motherBoardPower = (string)dbReader["motherboardPower"];
                    string cpuPower = (string)dbReader["cpuPower"];
                    int countGPUslots = (int)dbReader["countGPUslots"];
                    int countSATA = (int)dbReader["countSATA"];
                    int countMolex = (int)dbReader["countMolex"];

                    PowerSupplyform form = PowerSupplyform.ATX;
                    for (int i = 0; i < 3; i++)
                    {
                        if(((PowerSupplyform)i).ToString()==formFactor)
                        {
                            form = (PowerSupplyform)i;
                            break;
                        }
                    }


                    PowerSupply powerSupply = new PowerSupply(producer,power,form,gpuPower,motherBoardPower,cpuPower,countGPUslots,countSATA,countMolex,model,cost,image,ComponentType.PowerSupply,count,productCode);
                    powerSupply.Count = count;
                    components.Add(powerSupply);


                }
            }
            dbConnection.Close();
        }
    }
}
