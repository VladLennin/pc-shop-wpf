using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КР2семестр
{
 public   class CoolingSystem : Component,ICloneable,IWorkWithBD
    {

        bool compatibilityAMD;
        bool compatibilityIntel;

        public CoolingSystem(bool compatibilityAMD,bool compatibilityIntel,string _model, int _count, double _cost, string _producer, string _image, ComponentType _type, int _productCode) : base(_model, _count, _cost, _producer, _image, _type, _productCode)
        {
            this.compatibilityAMD = compatibilityAMD;
            this.compatibilityIntel = compatibilityIntel;
        }

        public CoolingSystem()
        {
        }

        public bool CompatibilityAMD { get => compatibilityAMD; set => compatibilityAMD = value; }
        public bool CompatibilityIntel { get => compatibilityIntel; set => compatibilityIntel = value; }

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
                query = $"SELECT [model],[producer],[cost],[image],[compatibilityAMD],[compatibilityIntel] FROM products WHERE productCode = {productCodes[i]} and type = 'CoolingSystem'";
                dbCommand = new OleDbCommand(query, dbConnection);
                dbReader = dbCommand.ExecuteReader();
                while (dbReader.Read())
                {
                    string model = (string)dbReader["model"];
                    string producer = (string)dbReader["producer"];
                    double cost = (double)dbReader["cost"];
                    string image = (string)dbReader["image"];
                    bool compatibilityAMD = (bool)dbReader["compatibilityAMD"];
                    bool compatibilityIntel = (bool)dbReader["compatibilityIntel"];


                    CoolingSystem cs = new CoolingSystem(compatibilityAMD, compatibilityIntel, model, counts[i], cost, producer, image, ComponentType.CoolingSystem, productCodes[i]);
                    cs.Count = counts[i];
                    basket.Add(cs);

                }


            }
            dbConnection.Close();
        }
        public  void FillComponentsFromBD(List<Component>components,OleDbConnection dbConnection)
        {
            dbConnection.Open();

            string query = $"SELECT COUNT(type) FROM products WHERE [type]='Case' ";
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            if ((int)dbCommand.ExecuteScalar() != 0)
            {
                query = $"SELECT [model],[count],[producer],[cost],[image],[compatibilityAMD],[compatibilityIntel],[productCode] FROM products WHERE type = 'CoolingSystem' ";
                dbCommand = new OleDbCommand(query, dbConnection);
                OleDbDataReader dbReader = dbCommand.ExecuteReader();
                while (dbReader.Read())
                {
                    string model = (string)dbReader["model"];
                    string producer = (string)dbReader["producer"];
                    double cost = (double)dbReader["cost"];
                    string image = (string)dbReader["image"];
                    int count = (int)dbReader["count"];
                    int productCode = (int)dbReader["productCode"];
                    bool compatibilityAMD = (bool)dbReader["compatibilityAMD"];
                    bool compatibilityIntel = (bool)dbReader["compatibilityIntel"];



                    CoolingSystem cs = new CoolingSystem(compatibilityAMD, compatibilityIntel, model, count, cost, producer, image, ComponentType.CoolingSystem, productCode);
                    cs.Count = count;
                       

                    components.Add(cs);


                }
            }
            dbConnection.Close();
        }

        public static bool AddCoolingSystemToBD(bool compatibilityAMD,bool compatibilityIntel,string model, string producer, double cost, string image, int count, string type,OleDbConnection dbConnection,List<Component>components)
        {
            bool check = true;
            dbConnection.Open();
            string query = $"SELECT COUNT(model) FROM products WHERE model = '{model}' and producer = '{producer}'";
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            if ((int)dbCommand.ExecuteScalar() == 0)
            {
                query = $"INSERT INTO products ([model],[producer],[count],[cost],[type],[image],[compatibilityAMD],[compatibilityIntel]) " +
                    $"VALUES('{model}','{producer}',{count},'{cost}','{type}','{image}',{compatibilityAMD},{compatibilityIntel})";
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
