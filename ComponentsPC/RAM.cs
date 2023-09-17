using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КР2семестр
{
    public class RAM : Component, ICloneable,IWorkWithBD
    {
        double frequency;
        double power;
        RAMgen gen;
        int capacity;
        int countModules;
        public RAM(double _frequency, double _power, RAMgen _gen, int _capacity, double cost, string model, string producer, string image, ComponentType type, int _countModules, int count, int productCode) : base(model, count, cost, producer, image, type, productCode)
        {
            Frequency = _frequency;
            Power = _power;
            Gen = _gen;
            Capacity = _capacity;
            countModules = _countModules;
        }
        public RAM(double _frequency, double _power, RAMgen _gen, int _capacity, double cost, string model, string producer, ComponentType type, int count, int _countModules, int productCode) : base(model, count, cost, producer, type, productCode)
        {
            Frequency = _frequency;
            Power = _power;
            Gen = _gen;
            Capacity = _capacity;
            countModules = _countModules;
        }

        public RAM()
        {
        }

        public double Frequency { get => frequency; set => frequency = value; }
        public double Power { get => power; set => power = value; }
        public int Capacity { get => capacity; set => capacity = value; }
        public RAMgen Gen { get => gen; set => gen = value; }
        public int CountModules { get => countModules; set => countModules = value; }

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
                query = $"SELECT [model],[producer],[cost],[image],[frequency],[power],[RAMgen],[memory_capacity],[countModules] FROM products WHERE productCode = {productCodes[i]} and type = 'RAM'";
                dbCommand = new OleDbCommand(query, dbConnection);
                 dbReader = dbCommand.ExecuteReader();
                while(dbReader.Read())
                {
                    string model = (string)dbReader["model"];
                    string producer = (string)dbReader["producer"];
                    double cost = (double)dbReader["cost"];       
                    string image = (string)dbReader["image"];
                    double frequency = (double)dbReader["frequency"];
                    double power = (double)dbReader["power"];
                    string ramGEN = (string)dbReader["RAMgen"];
                    int memory_capacity = (int)dbReader["memory_capacity"];
                    int countModules = (int)dbReader["countModules"];

                    RAMgen ramGEN1 = RAMgen.DDR1;
                    for (int k = 0; k < 4; k++)
                    {
                        if(ramGEN==((RAMgen)k).ToString())
                        {
                            ramGEN1 = (RAMgen)k;
                        }
                    }
                    RAM ram = new RAM(frequency, power, ramGEN1, memory_capacity, cost, model, producer, image, ComponentType.RAM, countModules, counts[i], productCodes[i]);
                    ram.Count = counts[i];
                    basket.Add(ram);
                }


            }
            dbConnection.Close();

        }
        public  void FillComponentsFromBD(List<Component>components,OleDbConnection dbConnection)
        {
            dbConnection.Open();

            string query = $"SELECT COUNT(type) FROM products WHERE [type]='RAM' ";
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            if((int)dbCommand.ExecuteScalar()!=0)
            {
                query = $"SELECT [model],[producer],[cost],[count],[productCode],[image],[frequency],[power],[RAMgen],[memory_capacity],[countModules] FROM products WHERE type = 'RAM' ";
                dbCommand = new OleDbCommand(query, dbConnection);
                OleDbDataReader dbReader = dbCommand.ExecuteReader();
                while(dbReader.Read())
                {
                    string model = (string)dbReader["model"];
                    string producer = (string)dbReader["producer"];
                    double cost = (double)dbReader["cost"];
                    int count = (int)dbReader["count"];
                    int productCode = (int)dbReader["productCode"];
                    string image = (string)dbReader["image"];
                    double frequency = (double)dbReader["frequency"];
                    double power = (double)dbReader["power"];
                    string ramGEN = (string)dbReader["RAMgen"];
                    int memory_capacity = (int)dbReader["memory_capacity"];
                    int countModules = (int)dbReader["countModules"];

                    RAMgen ramGEn1 = RAMgen.DDR1;
                    for (int i = 0; i < 4; i++)
                    {
                        if(((RAMgen)i).ToString()==ramGEN)
                        {
                            ramGEn1 = (RAMgen)i;
                            break;
                        }
                    }
                    RAM ram = new RAM(frequency, power, ramGEn1, memory_capacity, cost, model, producer, image, ComponentType.RAM, countModules, count, productCode);
                    ram.Count = count;
                    components.Add(ram);


                }
            }
            dbConnection.Close();
               
        }
        public static bool AddRAMtoBD(string model,int count, double cost,string producer,string image,string type,double frequency,double power,string ramGEN,int capacity,int countModules,OleDbConnection dbConnection,List<Component>components)
        {
            bool check = true;
            dbConnection.Open();
            string query = $"SELECT COUNT(model) FROM products WHERE model = '{model}' and producer = '{producer}'";
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            if((int)dbCommand.ExecuteScalar()==0)
            {
                query = $"INSERT INTO products ([model],[producer],[count],[cost],[type],[image],[frequency],[power],[RAMgen],[memory_capacity],[countModules]) " +
                    $"VALUES('{model}','{producer}',{count},'{cost}','{type}','{image}','{frequency}','{power}','{ramGEN}',{capacity},{countModules})";
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
    }
}
