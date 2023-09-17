using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КР2семестр
{
  public  class Case : Component,ICloneable,IWorkWithBD
    {

        TypeCases type;
        int countSlots3_5;
        int countSlots2_5;
        List<string> sizesMotherboards;
        public Case(List<string> sizes ,int countSlots3_5, int countSlots2_5, TypeCases type, string _model, int _count, double _cost, string _producer, string _image, ComponentType _type, int _productCode) : base(_model, _count, _cost, _producer, _image, _type, _productCode)
        {
            this.TypeCase = type;
            this.CountSlots3_5 = countSlots3_5;
            this.CountSlots2_5 = countSlots2_5;
            SizesMotherboards = sizes;
        }

        public Case()
        {
        }

        public int CountSlots3_5 { get => countSlots3_5; set => countSlots3_5 = value; }
        public int CountSlots2_5 { get => countSlots2_5; set => countSlots2_5 = value; }
    
        public TypeCases TypeCase { get => type; set => type = value; }
        public List<string> SizesMotherboards { get => sizesMotherboards; set => sizesMotherboards = value; }

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
                query = $"SELECT [model],[producer],[cost],[image],[countSlots3_5],[countSlots2_5],[typeCase],[size] FROM products WHERE productCode = {productCodes[i]} and type = 'Case'";
                dbCommand = new OleDbCommand(query, dbConnection);
                dbReader = dbCommand.ExecuteReader();
                while (dbReader.Read())
                {
                    string model = (string)dbReader["model"];
                    string producer = (string)dbReader["producer"];
                    double cost = (double)dbReader["cost"];
                    string image = (string)dbReader["image"];
                    string typeCaseTemp = (string)dbReader["typeCase"];
                    string sizeTemp = (string)dbReader["size"];
                    int countSlots3_5 = (int)dbReader["countSlots3_5"];
                    int countSlots2_5 = (int)dbReader["countSlots2_5"];

                    List<string> sizes = sizeTemp.Split(',').ToList();
                    MotherboardSizes size = MotherboardSizes.ATX;
                    for (int k = 0; k < 4; k++)
                    {
                        if (sizeTemp == ((MotherboardSizes)k).ToString())
                        {
                            size = (MotherboardSizes)k;
                            break;
                        }
                    }
                    TypeCases type = TypeCases.Desktop;
                    for (int l = 0; l < 5; l++)
                    {
                        if(typeCaseTemp==((TypeCases)l).ToString())
                        {
                            type = (TypeCases)l;
                            break;
                        }
                    }

                    Case _case =  new Case(sizes,countSlots3_5, countSlots2_5, type, model, counts[i], cost, producer, image, ComponentType.Case, productCodes[i]);
                    _case.Count = counts[i];
                    basket.Add(_case);
                  
                }


            }
            dbConnection.Close();
        }
        public  void FillComponentsFromBD(List<Component> components, OleDbConnection dbConnection)
        {
            dbConnection.Open();

            string query = $"SELECT COUNT(type) FROM products WHERE [type]='Case' ";
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            if ((int)dbCommand.ExecuteScalar() != 0)
            {
                query = $"SELECT [model],[producer],[cost],[image],[countSlots3_5],[countSlots2_5],[typeCase],[size],[count],[productCode] FROM products WHERE type = 'Case' ";
                dbCommand = new OleDbCommand(query, dbConnection);
                OleDbDataReader dbReader = dbCommand.ExecuteReader();
                while (dbReader.Read())
                {
                    string model = (string)dbReader["model"];
                    string producer = (string)dbReader["producer"];
                    double cost = (double)dbReader["cost"];
                    string image = (string)dbReader["image"];
                    string typeCaseTemp = (string)dbReader["typeCase"];
                    string sizeTemp = (string)dbReader["size"];
                    int countSlots3_5 = (int)dbReader["countSlots3_5"];
                    int countSlots2_5 = (int)dbReader["countSlots2_5"];
                    int count = (int)dbReader["count"];
                    int productCode = (int)dbReader["productCode"];



                    List<string> sizes = sizeTemp.Split(',').ToList();
                  
                    MotherboardSizes size = MotherboardSizes.ATX;
                    for (int k = 0; k < 4; k++)
                    {
                        if (sizeTemp == ((MotherboardSizes)k).ToString())
                        {
                            size = (MotherboardSizes)k;
                            break;
                        }
                    }
                    TypeCases type = TypeCases.Desktop;
                    for (int l = 0; l < 5; l++)
                    {
                        if (typeCaseTemp == ((TypeCases)l).ToString())
                        {
                            type = (TypeCases)l;
                            break;
                        }
                    }
                    
                    Case _case = new Case(sizes,countSlots3_5, countSlots2_5, type, model, count, cost, producer, image, ComponentType.Case, productCode);
                    _case.Count = count;
                    components.Add(_case);


                }
            }
            dbConnection.Close();
        }
        public static bool AddCaseToBD(string model,string producer,double cost,string image,int count,string type,List<string> sizes,string typeCase,int count3_5,int count2_5,List<Component> components,OleDbConnection dbConnection)
        {
            string sizesTemp = " ";
            for (int i = 0; i < sizes.Count; i++)
            {
                sizesTemp += sizes[i] + ",";
            }
            bool check = true;
            dbConnection.Open();
            string query = $"SELECT COUNT(model) FROM products WHERE model = '{model}' and producer = '{producer}'";
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            if ((int)dbCommand.ExecuteScalar() == 0)
            {
                query = $"INSERT INTO products ([model],[producer],[count],[cost],[type],[image],[size],[typeCase],[countSlots3_5],[countSlots2_5]) " +
                    $"VALUES('{model}','{producer}',{count},'{cost}','{type}','{image}','{sizesTemp}','{typeCase}',{count3_5},{count2_5})";
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
