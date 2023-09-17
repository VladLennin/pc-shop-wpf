using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КР2семестр
{
    public class User
    {
        string login;
        string password;
        string name;
        string surname;
        string mail;
        string number;
        double money;
        double basketCost;
        int userCode;
        List<Component> basket;
        List<Order> orders;
        List<Assembly> assemblies;
        public User()
        {
            Basket = new List<Component>();
        }
        public User(string _login, string _password, int _userCode)
        {
            Login = _login;
            Password = _password;
            UserCode = _userCode;
            Basket = new List<Component>();
            orders = new List<Order>();
            Assemblies = new List<Assembly>();
        }

        public string Login { get => login; set => login = value; }
        public string Password { get => password; set => password = value; }
        public string Name { get => name; set => name = value; }
        public string Surname { get => surname; set => surname = value; }
        public string Mail { get => mail; set => mail = value; }
        public string Number { get => number; set => number = value; }
        public double Money { get => money; set => money = value; }
        public List<Component> Basket { get => basket; set => basket = value; }
        public double BasketCost { get => basketCost; set => basketCost = value; }
        public int UserCode { get => userCode; set => userCode = value; }
        internal List<Order> Orders { get => orders; set => orders = value; }
        public List<Assembly> Assemblies { get => assemblies; set => assemblies = value; }

        public void AddItemToBasket(Component component)
        {
            Basket.Add(component);
        }
        public void RemoveItemFromBasket(Component component)
        {
            Basket.Remove(component);
        }

        public override string ToString()
        {
            return $"{Name} {Surname}";
        }


        public void WriteAssemblyToBD(OleDbConnection dbConnection, Assembly assembly)
        {
            string query = $"INSERT INTO assemblies (userCode,assemblyName,timeCreation) VALUES({userCode},'{assembly.Name}','{assembly.Time}')";
            dbConnection.Open();
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            dbCommand.ExecuteNonQuery();

            query = $"SELECT assembliesCode FROM assemblies WHERE userCode = {userCode} and timeCreation = '{assembly.Time}'";
            dbCommand = new OleDbCommand(query, dbConnection);
            OleDbDataReader dbReader = dbCommand.ExecuteReader();
            int assemblyCode = 0;
            while (dbReader.Read())
            {
                assemblyCode = (int)dbReader["assembliesCode"];
            }
            List<Component> componetns = assembly.ConvertAssemblyToListComponents(assembly);
            for (int i = 0; i < componetns.Count; i++)
            {
                query = $"INSERT INTO assembliesComponents (assemblyCode,productCode) VALUES({assemblyCode},{componetns[i].ProductCode})";
                dbCommand = new OleDbCommand(query, dbConnection);
                dbCommand.ExecuteNonQuery();
            }
            dbConnection.Close();
        }
        public static void WriteOrderToBD(OleDbConnection dbConnection, Order order, User user)
        {

            dbConnection.Open();
            string query = $"INSERT INTO orders (userCode,orderCost,user_name,user_surname,orderTime,orderStatus,typeDelivery,deliveryAddres,typePayment) VALUES({user.UserCode},'{order.OrderCost}','{order.Name}','{order.Surname}','{order.DateOrder.ToString()}','{order.Status}','{order.TypeDelivery}','{order.Addres}','{order.TypePayment}')";
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            dbCommand.ExecuteNonQuery();

            query = $"SELECT orderCode FROM orders WHERE orderTime = '{order.DateOrder.ToString()}' and userCode = {user.UserCode}";
            dbCommand = new OleDbCommand(query, dbConnection);
            OleDbDataReader dbReader = dbCommand.ExecuteReader();
            int orderCode = 0;
            while (dbReader.Read())
            {
                orderCode = (int)dbReader["orderCode"];
            }
            order.OrderCode = orderCode;
            user.Orders.Add(order);
            for (int i = 0; i < order.Products.Count; i++)
            {

                query = $"INSERT INTO orderProducts (productCode,[count],orderCode) VALUES({order.Products[i].ProductCode},{order.Products[i].Count},{orderCode})";
                dbCommand = new OleDbCommand(query, dbConnection);
                dbCommand.ExecuteNonQuery();
            }
            query = $"DELETE FROM basket WHERE userCode = {user.UserCode}";
            dbCommand = new OleDbCommand(query, dbConnection);
            dbCommand.ExecuteNonQuery();

            dbConnection.Close();
            user.Basket.Clear();
            MessageBoxNew message = new MessageBoxNew("Order in procesing!");
            message.Show();
        }

        public static void ReadAllUsersFromBD(OleDbConnection dbConnection, List<User> users)
        {
            dbConnection.Open();
            string login = " ";
            string pass = " ";
            string mail = " ";
            string number = " ";
            string name = " ";
            string surname = " ";
            double purse = 0;
            int userCode = 0;




            string query = $"SELECT user_login,user_pass,user_mail,user_number,user_name,user_surname,purse,userCode,user_login,user_pass FROM users ";
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            OleDbDataReader dbReader = dbCommand.ExecuteReader();

            while (dbReader.Read())
            {
                mail = (string)dbReader["user_mail"];
                number = (string)dbReader["user_number"];
                name = (string)dbReader["user_name"];
                surname = (string)dbReader["user_surname"];
                userCode = (int)dbReader["userCode"];
                if ((int)dbReader["purse"] == 0)
                    purse = 0;
                login = (string)dbReader["user_login"];
                pass = (string)dbReader["user_pass"];
                users.Add(new User(login, pass, userCode) { Mail = mail, Number = number, Name = name, Surname = surname, Money = purse });
            }
            dbConnection.Close();


        }
        public void ReadAssembliesFromBD(User user, OleDbConnection dbConnection, List<Component> components)
        {
            string query = $"SELECT assembliesCode,assemblyName,timeCreation FROM assemblies WHERE userCode = {user.UserCode}";
            dbConnection.Open();
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            OleDbDataReader dbreader = dbCommand.ExecuteReader();
            List<int> assemblyCodes = new List<int>();
            List<string> assemblyNames = new List<string>();
            List<string> timeCreations = new List<string>();
            while (dbreader.Read())
            {
                assemblyCodes.Add((int)dbreader["assembliesCode"]);
                assemblyNames.Add((string)dbreader["assemblyName"]);
                timeCreations.Add((string)dbreader["timeCreation"]);
            }

            for (int i = 0; i < assemblyCodes.Count; i++)
            {
                List<int> productCodes = new List<int>();
                query = $"SELECT productCode FROM assembliesComponents WHERE assemblyCode={assemblyCodes[i]}";
                dbCommand = new OleDbCommand(query, dbConnection);
                dbreader = dbCommand.ExecuteReader();
                while (dbreader.Read())
                {
                    productCodes.Add((int)dbreader["productCode"]);
                }


                List<Component> componentsAssembly = new List<Component>();
                for (int j = 0; j < components.Count; j++)
                {
                    for (int h = 0; h < productCodes.Count; h++)
                    {
                        if (components[j].ProductCode == productCodes[h])
                        {
                            componentsAssembly.Add((Component)components[j].Clone());                          
                        }
                    }
                }
                Assembly assembly = new Assembly();
                assembly.Name = assemblyNames[i];
                assembly.Time = timeCreations[i];
                assembly.AuthorLogin = user.Login;
               
                for (int j = 0; j < componentsAssembly.Count; j++)
                {
                    if (componentsAssembly[j].Type == ComponentType.Case)
                    {
                        assembly.Case = (Case)componentsAssembly[j];
                        assembly.Image = componentsAssembly[j].Image;
                    }
                    else if (componentsAssembly[j].Type == ComponentType.CoolingSystem)
                    {
                        assembly.CoolingSystem = (CoolingSystem)componentsAssembly[j];
                    }
                    else if (componentsAssembly[j].Type == ComponentType.CPU)
                    {
                        assembly.Cpu = (CPU)componentsAssembly[j];
                    }
                    else if (componentsAssembly[j].Type == ComponentType.GPU)
                    {
                        assembly.Gpu = (GPU)componentsAssembly[j];
                    }
                    else if (componentsAssembly[j].Type == ComponentType.HDD)
                    {
                        assembly.HDD1 = (HDD)componentsAssembly[j];
                    }
                    else if (componentsAssembly[j].Type == ComponentType.Motherboard)
                    {
                        assembly.Motherboard = (Motherboard)componentsAssembly[j];
                    }
                    else if (componentsAssembly[j].Type == ComponentType.PowerSupply)
                    {
                        assembly.PowerSupply = (PowerSupply)componentsAssembly[j];
                    }
                    else if (componentsAssembly[j].Type == ComponentType.RAM)
                    {
                        assembly.RAM1 = (RAM)componentsAssembly[j];
                    }
                    else if (componentsAssembly[j].Type == ComponentType.SSD)
                    {
                        assembly.SSD1 = (SSD)componentsAssembly[j];
                    }


                }
                assembly.Cost = assembly.CalculateCost(assembly);
                assemblies.Add(assembly);

            }
            dbConnection.Close();

        }

        public virtual void ReadOrdersFromBD(User user, OleDbConnection dbConnection)
        {
            dbConnection.Open();
            user.Orders.Clear();
            string query = $"SELECT orderCode,orderCost,user_name,user_surname,orderTime,orderStatus,typeDelivery,deliveryAddres,typePayment FROM orders  WHERE userCode = {user.UserCode}";
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            OleDbDataReader dbReader = dbCommand.ExecuteReader();
            int orderCode = 0;
            string orderCost = " ";
            string user_name = " ";
            string user_surname = " ";
            string orderTime = " ";
            string orderStatus = " ";
            string typeDelivery = " ";
            string deliveryAddres = " ";
            string typePayment = " ";

            while (dbReader.Read())
            {
                orderCode = (int)dbReader["orderCode"];
                orderCost = (string)dbReader["orderCost"];
                user_name = (string)dbReader["user_name"];
                user_surname = (string)dbReader["user_surname"];
                orderTime = (string)dbReader["orderTime"];
                orderStatus = (string)dbReader["orderStatus"];
                typeDelivery = (string)dbReader["typeDelivery"];
                deliveryAddres = (string)dbReader["deliveryAddres"];
                typePayment = (string)dbReader["typePayment"];
                user.Orders.Add(new Order(user_name, user_surname, deliveryAddres, orderTime, typeDelivery, typePayment, orderCost, orderStatus, orderCode));
            }


            for (int i = 0; i < user.Orders.Count; i++)
            {
                query = $"SELECT [count],[productCode] FROM orderProducts WHERE orderCode = {user.Orders[i].OrderCode}";
                dbCommand = new OleDbCommand(query, dbConnection);
                dbReader = dbCommand.ExecuteReader();

                List<int> counts = new List<int>();
                List<int> productCodes = new List<int>();
                while (dbReader.Read())
                {
                    counts.Add((int)dbReader["count"]);
                    productCodes.Add((int)dbReader["productCode"]);
                }


                for (int k = 0; k < productCodes.Count; k++)
                {
                    query = $"SELECT  [model],[producer],[type],[image],[cost] FROM products WHERE productCode = {productCodes[k]}";
                    dbCommand = new OleDbCommand(query, dbConnection);
                    dbReader = dbCommand.ExecuteReader();
                    string model = " ";
                    string producer = " ";
                    string typeTemp = " ";
                    string image = " ";
                    double cost = 0;
                    while (dbReader.Read())
                    {
                        model = (string)dbReader["model"];
                        producer = (string)dbReader["producer"];
                        typeTemp = (string)dbReader["type"];
                        image = (string)dbReader["image"];
                        cost = (double)dbReader["cost"];

                    }
                    ComponentType type = ComponentType.Case;
                    for (int b = 0; b < 9; b++)
                    {
                        if (typeTemp == ((ComponentType)b).ToString())
                        {
                            type = (ComponentType)b;
                            break;
                        }
                    }
                    Component component = new Component(model, counts[k], cost, producer, image, type, productCodes[k]);
                    component.Count = counts[k];
                    user.Orders[i].Products.Add(component);

                }
            }
            dbConnection.Close();
        }

        public static User FillUserFromBD(string login, string pass, OleDbConnection dbConnection)
        {
            dbConnection.Open();
            string mail = " ";
            string number = " ";
            string name = " ";
            string surname = " ";
            double purse = 0;
            int userCode = 0;

            string query = $"SELECT COUNT(user_login) FROM users WHERE user_login='{login}' and user_pass='{pass}'";
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            if ((int)dbCommand.ExecuteScalar() != 0)
            {

                query = $"SELECT user_login,user_pass,user_mail,user_number,user_name,user_surname,purse,userCode FROM users WHERE user_login ='{login}' and user_pass='{pass}'";
                dbCommand = new OleDbCommand(query, dbConnection);
                OleDbDataReader dbReader = dbCommand.ExecuteReader();

                while (dbReader.Read())
                {
                    mail = (string)dbReader["user_mail"];
                    number = (string)dbReader["user_number"];
                    name = (string)dbReader["user_name"];
                    surname = (string)dbReader["user_surname"];
                    userCode = (int)dbReader["userCode"];
                    if ((int)dbReader["purse"] == 0)
                        purse = 0;
                }


                if (login != " ")
                {
                    User user = new User(login, pass, userCode) { Mail = mail, Number = number, Name = name, Surname = surname, Money = purse };
                    dbConnection.Close();
                    user.Basket = FillBasketFromBD(dbConnection, user);
                    return user;
                }


            }
            else
            {
                dbConnection.Close();
                return null;
            }
            dbConnection.Close();
            return null;
        }

        public static List<Component> FillBasketFromBD(OleDbConnection dbConnection, User user)
        {
            List<Component> basket = new List<Component>();
            dbConnection.Open();

            string query = $"SELECT COUNT(userCode) FROM basket WHERE userCode = {user.UserCode}";
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);

            if (((int)dbCommand.ExecuteScalar()) != 0)
            {
                dbConnection.Close();
                CPU cpu = new CPU(); cpu.FillComponentsFromBDtoBasket(basket, dbConnection, user.UserCode);
                GPU gpu = new GPU(); gpu.FillComponentsFromBDtoBasket(basket, dbConnection, user.UserCode);
                Motherboard motherboard = new Motherboard(); motherboard.FillComponentsFromBDtoBasket(basket, dbConnection, user.UserCode);
                RAM ram = new RAM(); ram.FillComponentsFromBDtoBasket(basket, dbConnection, user.UserCode);
                SSD ssd = new SSD(); ssd.FillComponentsFromBDtoBasket(basket, dbConnection, user.UserCode);
                HDD hdd = new HDD(); hdd.FillComponentsFromBDtoBasket(basket, dbConnection, user.UserCode);
                Case case1 = new Case(); case1.FillComponentsFromBDtoBasket(basket, dbConnection, user.UserCode);
                PowerSupply powerSupply = new PowerSupply(); powerSupply.FillComponentsFromBDtoBasket(basket, dbConnection, user.UserCode);
                CoolingSystem cs = new CoolingSystem(); cs.FillComponentsFromBDtoBasket(basket, dbConnection, user.UserCode);
            }

                for (int j = 0; j < basket.Count; j++)
                {
                    string path = Environment.CurrentDirectory;
                    string[] temp = path.Split('\\');
                    temp[temp.Length - 1] = "";
                    temp[temp.Length - 2] = "";
                    path = "";
                    for (int f = 0; f < temp.Length; f++)
                    {
                        if (temp[f] != "")
                            path += temp[f] + "\\";
                    }
                    path += $"Resources\\{basket[j].Image}";
                    basket[j].Image = path;
                }
            
            dbConnection.Close();
            return basket;

        }


    }
}
