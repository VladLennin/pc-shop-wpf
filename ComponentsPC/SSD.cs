using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КР2семестр
{
    public class SSD : Disk, ICloneable,IWorkWithBD
    {
        TypeSSD typeSSD;

        public TypeSSD TypeSSD { get => typeSSD; set => typeSSD = value; }

        public SSD(string producer, int memoryCapacity, TypeSSD _typeSSD, string formFactor, string portConnect, int speedRead, int speedWrite, string model, double cost, string image, ComponentType type, int count, int productCode) : base(memoryCapacity, formFactor, producer, portConnect, speedRead, speedWrite, model, cost, image, type, count, productCode)
        {
            TypeSSD = _typeSSD;
        }
        public SSD(string producer, int memoryCapacity, TypeSSD _typeSSD, string formFactor, string portConnect, int speedRead, int speedWrite, string model, double cost, ComponentType type, int count, int productCode) : base(memoryCapacity, formFactor, producer, portConnect, speedRead, speedWrite, model, cost, type, count, productCode)
        {
            TypeSSD = _typeSSD;
        }

        public SSD()
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
                query = $"SELECT [model],[producer],[cost],[image],[memory_capacity],[formFactor],[portConnect],[speedWrite],[speedRead],[typeSSD] FROM products WHERE productCode = {productCodes[i]} and type = 'SSD'";
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
                    string typeSSD = (string)dbReader["typeSSD"];

                    TypeSSD typeSSD2 = TypeSSD.MLS;
                    for (int h = 0; h < 6; h++)
                    {
                        if(typeSSD==((TypeSSD)h).ToString())
                        {
                            typeSSD2 = (TypeSSD)h;
                            break;
                        }
                    }

                    SSD ssd = new SSD(producer,memory_capacity,typeSSD2,formFactor,portConnect,speedRead,speedWrite,model,cost,image,ComponentType.SSD,counts[i],productCodes[i]);
                    ssd.Count = counts[i];
                    basket.Add(ssd);
                }


            }
            dbConnection.Close();
        }
        public  void FillComponentsFromBD(List<Component> components, OleDbConnection dbConnection)
        {
            dbConnection.Open();

            string query = $"SELECT COUNT(type) FROM products WHERE [type]='SSD' ";
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            if ((int)dbCommand.ExecuteScalar() != 0)
            {
                query = $"SELECT [model],[producer],[cost],[count],[productCode],[image],[memory_capacity],[formFactor],[portConnect],[speedWrite],[speedRead],[typeSSD] FROM products WHERE type = 'SSD' ";
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
                    string typeSSD = (string)dbReader["typeSSD"];
                    int count = (int)dbReader["count"];
                    int productCode = (int)dbReader["productCode"];


                    TypeSSD typeSSD2 = TypeSSD.MLS;
                    for (int h = 0; h < 6; h++)
                    {
                        if (typeSSD == ((TypeSSD)h).ToString())
                        {
                            typeSSD2 = (TypeSSD)h;
                            break;
                        }
                    }

                    SSD ssd = new SSD(producer, memory_capacity, typeSSD2, formFactor, portConnect, speedRead, speedWrite, model, cost, image, ComponentType.SSD, count, productCode);
                    ssd.Count = count;
                    components.Add(ssd);


                }
            }
            dbConnection.Close();
        }
        public static bool AddSSDtoBD(string model, double cost, string image, string producer, int count, string type, int memory_capacity, string formFactor, string connectPort, int speedWrite, int speedRead, string typeSSD, List<Component> components, OleDbConnection dbConnection)
        {
            bool check = true;
            dbConnection.Open();
            string query = $"SELECT COUNT(model) FROM products WHERE model = '{model}' and producer = '{producer}'";
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            if ((int)dbCommand.ExecuteScalar() == 0)
            {
                query = $"INSERT INTO products ([model],[producer],[count],[cost],[type],[image],[memory_capacity],[formFactor],[portConnect],[speedWrite],[speedRead],[typeSSD]) " +
                    $"VALUES('{model}','{producer}',{count},'{cost}','{type}','{image}',{memory_capacity},'{formFactor}','{connectPort}',{speedWrite},{speedRead},'{typeSSD}')";
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