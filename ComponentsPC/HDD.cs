using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КР2семестр
{
    public class HDD : Disk, ICloneable,IWorkWithBD
    {
        HDDspeedRotate speedRotate;

        public HDDspeedRotate SpeedRotate { get => speedRotate; set => speedRotate = value; }

        public HDD(int memoryCapacity, string formFactor, string producer, string portConnect, int speedRead, int speedWrite, HDDspeedRotate _speedRotate, string model, double cost, string image, ComponentType type, int count, int productCode) : base(memoryCapacity, formFactor, producer, portConnect, speedRead, speedWrite, model, cost, image, type, count, productCode)
        {
            SpeedRotate = _speedRotate;
        }
        public HDD(int memoryCapacity, string formFactor, string producer, string portConnect, int speedRead, int speedWrite, HDDspeedRotate _speedRotate, string model, double cost, ComponentType type, int count, int productCode) : base(memoryCapacity, formFactor, producer, portConnect, speedRead, speedWrite, model, cost, type, count, productCode)
        {
            SpeedRotate = _speedRotate;
        }

        public HDD()
        {
        }

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
                query = $"SELECT [model],[producer],[cost],[image],[memory_capacity],[formFactor],[portConnect],[speedWrite],[speedRead],[speedRotate] FROM products WHERE productCode = {productCodes[i]} and type = 'HDD'";
                dbCommand = new OleDbCommand(query, dbConnection);
                dbReader = dbCommand.ExecuteReader();
                while (dbReader.Read())
                {
                    string model = (string)dbReader["model"];
                    string producer = (string)dbReader["producer"];
                    double cost = (double)dbReader["cost"];
                    string image = (string)dbReader["image"];
                    int memory_capacity = (int)dbReader["memory_capacity"];
                    string formFactor = (string)dbReader["formFactor"];
                    string portConnect = (string)dbReader["portConnect"];
                    int speedWrite = (int)dbReader["speedWrite"];
                    int speedRead = (int)dbReader["speedRead"];
                    string speedRotate = (string)dbReader["speedRotate"];

                    HDDspeedRotate speed = HDDspeedRotate.speed_7200;
                    if (speedRotate == "speed_5400")
                        speed = HDDspeedRotate.speed_5400;

                    HDD hdd = new HDD(memory_capacity, formFactor, producer, portConnect, speedRead, speedWrite, speed, model, cost,image, ComponentType.HDD, counts[i], productCodes[i]);
                    hdd.Count = counts[i];
                    basket.Add(hdd);
                }


            }
            dbConnection.Close();
        }
        public  void FillComponentsFromBD(List<Component> components,OleDbConnection dbConnection)
        {
            dbConnection.Open();

            string query = $"SELECT COUNT(type) FROM products WHERE [type]='HDD' ";
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            if ((int)dbCommand.ExecuteScalar() != 0)
            {
                query = $"SELECT [model],[producer],[cost],[count],[productCode],[image],[memory_capacity],[formFactor],[portConnect],[speedWrite],[speedRead],[speedRotate] FROM products WHERE type = 'HDD' ";
                dbCommand = new OleDbCommand(query, dbConnection);
                OleDbDataReader dbReader = dbCommand.ExecuteReader();
                while (dbReader.Read())
                {
                    string model = (string)dbReader["model"];
                    string producer = (string)dbReader["producer"];
                    double cost = (double)dbReader["cost"];
                    string image = (string)dbReader["image"];
                    int memory_capacity = (int)dbReader["memory_capacity"];
                    string formFactor = (string)dbReader["formFactor"];
                    string portConnect = (string)dbReader["portConnect"];
                    int speedWrite = (int)dbReader["speedWrite"];
                    int speedRead = (int)dbReader["speedRead"];
                    string speedRotate = (string)dbReader["speedRotate"];
                    int count = (int)dbReader["count"];
                    int productCode = (int)dbReader["productCode"];


                    HDDspeedRotate speed = HDDspeedRotate.speed_7200;
                    if (speedRotate == "speed_5400")
                        speed = HDDspeedRotate.speed_5400;

                    HDD hdd = new HDD(memory_capacity, formFactor, producer, portConnect, speedRead, speedWrite, speed, model, cost, image, ComponentType.HDD, count, productCode);
                    hdd.Count = count;
                    components.Add(hdd);


                }
            }
            dbConnection.Close();
        }
        public static bool AddHDDtoBD(string model,double cost,string image,string producer,int count,string type,int memory_capacity,string formFactor,string connectPort,int speedWrite,int speedRead,string speedRotate,List<Component>components,OleDbConnection dbConnection)
        {
            bool check = true;
            dbConnection.Open();
            string query = $"SELECT COUNT(model) FROM products WHERE model = '{model}' and producer = '{producer}'";
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            if ((int)dbCommand.ExecuteScalar() == 0)
            {
                query = $"INSERT INTO products ([model],[producer],[count],[cost],[type],[image],[memory_capacity],[formFactor],[portConnect],[speedWrite],[speedRead],[speedRotate]) " +
                    $"VALUES('{model}','{producer}',{count},'{cost}','{type}','{image}',{memory_capacity},'{formFactor}','{connectPort}',{speedWrite},{speedRead},'{speedRotate}')";
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
