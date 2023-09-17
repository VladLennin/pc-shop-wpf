using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
namespace КР2семестр
{
    public class CPU : Component, ICloneable,IWorkWithBD
    {

        Sockets socket;
        int cores;
        int threads;
        double frequency;
        bool integrated_graphics;
        int technical_process;
        int power;
        int passMark;
        string coreName;
        public CPU()
        {

        }
        public CPU(string _producer, Sockets _socket, int _cores, string _coreName, int _threads, double _frequency, bool _integrated_grafics, int _technical_process, int _passMark, string _model, double _cost, int _power, string image, ComponentType type, int _count, int productCode) : base(_model, _count, _cost, _producer, image, type, productCode)
        {
            socket = _socket; cores = _cores; threads = _threads; frequency = _frequency;
            integrated_graphics = _integrated_grafics; technical_process = _technical_process; power = _power; passMark = _passMark; coreName = _coreName;

        }
        public CPU(string _producer, Sockets _socket, int _cores, string _coreName, int _threads, double _frequency, bool _integrated_grafics, int _technical_process, int _passMark, string _model, double _cost, int _power, ComponentType type, int count, int productCode) : base(_model, count, _cost, _producer, type, productCode)
        {
            socket = _socket; cores = _cores; threads = _threads; frequency = _frequency;
            integrated_graphics = _integrated_grafics; technical_process = _technical_process; power = _power; passMark = _passMark; coreName = _coreName;
        }


        public Sockets Socket
        {
            get => socket;
            set => socket = value;
        }
        public int Cores
        {
            get => cores;
            set => cores = value;
        }
        public int Threads
        {
            get => threads;
            set => threads = value;
        }
        public double Frequency
        {
            get => frequency;
            set => frequency = value;
        }
        public bool Integrated_Grafics
        {
            get => integrated_graphics;
            set => integrated_graphics = value;
        }
        public int Technical_Process
        {
            get => technical_process;
            set => technical_process = value;
        }
        public int Power
        {
            get => power;
            set => power = value;
        }
        public int PassMark { get => passMark; set => passMark = value; }
        public string CoreName { get => coreName; set => coreName = value; }


        public  void FillComponentsFromBDtoBasket(List<Component> basket, OleDbConnection dbConnection,int userCode)
        {
            dbConnection.Open();


            List<int> temp1 = new List<int>();
            List<int> temp2 = new List<int>();


           string query = $"SELECT productCode,count FROM basket WHERE userCode = {userCode}";
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            OleDbDataReader dbReader = dbCommand.ExecuteReader();
            while (dbReader.Read())
            {
                temp1.Add((int)dbReader["productCode"]);
                temp2.Add((int)dbReader["count"]);
            }



            for (int i = 0; i < temp1.Count; i++)
            {
                string type = " ";
                query = $"SELECT [type] FROM products WHERE productCode = {temp1[i]}";
                dbCommand = new OleDbCommand(query, dbConnection);
                dbReader = dbCommand.ExecuteReader();
                while (dbReader.Read())
                {
                    type = (string)dbReader["type"];
                }


                if (type == "CPU")
                {
                    query = $"SELECT [productCode],[cores],[threads],[technical_process],[power],[passMark],[coreName],[model],[producer],[image],[integrated_grafics],[socket],[frequency],[cost]" +
                        $" FROM products WHERE productCode = {temp1[i]} and type = 'CPU' ";
                    dbCommand = new OleDbCommand(query, dbConnection);
                    dbReader = dbCommand.ExecuteReader();
                    while (dbReader.Read())
                    {
                        int cores = (int)dbReader["cores"];
                        int threads = (int)dbReader["threads"];
                        int technical_process = (int)dbReader["technical_process"];
                        int power = Convert.ToInt32((double)dbReader["power"]);
                        int passMark = (int)dbReader["passMark"];
                        int productCode = (int)dbReader["productCode"];
                        string coreName = (string)dbReader["coreName"];
                        string model = (string)dbReader["model"];
                        string producer = (string)dbReader["producer"];
                        string image = (string)dbReader["image"];
                        string integrated_gradicsTemp = (string)dbReader["integrated_grafics"];
                        string socketTemp = (string)dbReader["socket"];
                        double frequency = (double)dbReader["frequency"];
                        double cost = (double)dbReader["cost"];

                        Sockets socket = Sockets.Socket1150;

                        bool integrated_grafics = true;
                        for (int j = 0; j < 7; j++)
                        {
                            if (socketTemp == ((Sockets)j).ToString())
                            {
                                socket = (Sockets)j;
                                break;
                            }
                        }


                        if (integrated_gradicsTemp == "No")
                            integrated_grafics = false;

                        CPU cpu = new CPU(producer, socket, cores, coreName, threads, frequency, integrated_grafics, technical_process, passMark, model, cost, power, image, ComponentType.CPU, temp2[i], productCode);
                        cpu.Count = temp2[i];
                        basket.Add(cpu);


                    }
                }
            }
                        dbConnection.Close();
        }

        public  void FillComponentsFromBD(List<Component> components,OleDbConnection dbConnection)
        {
            dbConnection.Open();
            string query = $"SELECT productCode,cores,threads,technical_process,power,passMark,coreName,model,producer,image,count,integrated_grafics,socket,frequency,cost" +
               $" FROM products WHERE type = 'CPU'";
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            OleDbDataReader dbReader = dbCommand.ExecuteReader();
            while (dbReader.Read())
            {
                int cores = (int)dbReader["cores"];
                int threads = (int)dbReader["threads"];
                int technical_process = (int)dbReader["technical_process"];
                int power = Convert.ToInt32((double)dbReader["power"]);
                int passMark = (int)dbReader["passMark"];
                int count = (int)dbReader["count"];
                int productCode = (int)dbReader["productCode"];
                string coreName = (string)dbReader["coreName"];
                string model = (string)dbReader["model"];
                string producer = (string)dbReader["producer"];
                string image = (string)dbReader["image"];
                string integrated_gradicsTemp = (string)dbReader["integrated_grafics"];
                string socketTemp = (string)dbReader["socket"];
                double frequency = (double)dbReader["frequency"];
                double cost = (double)dbReader["cost"];

                Sockets socket = Sockets.Socket1150;

                bool integrated_grafics = true;
                for (int i = 0; i < 7; i++)
                {
                    if (socketTemp == ((Sockets)i).ToString())
                    {
                        socket = (Sockets)i;
                    }
                }


                if (integrated_gradicsTemp == "No")
                    integrated_grafics = false;

                CPU cpu = new CPU(producer, socket, cores, coreName, threads, frequency, integrated_grafics, technical_process, passMark, model, cost, power, image, ComponentType.CPU, count, productCode);
                cpu.Count = count;
                components.Add(cpu);
            }   dbConnection.Close();
        }

        public static int AddCPUtoBD(bool check,OleDbConnection dbConnection,int count,double cost,int cores,int threads,double frequency,int technical_process,int power,int passMark,string socket,string producer,string model,string coreName,string image,string integrated_grafics,List<Component>components)
        {
            dbConnection.Open();
            string query = $"SELECT COUNT(model) FROM products  WHERE model='{model}'";
            OleDbCommand oleDbCommand = new OleDbCommand(query, dbConnection);
            if ((int)oleDbCommand.ExecuteScalar() == 0)
            {
                if (check)
                {


                    if (image != "")
                    {
                        query = $"INSERT INTO [products] ([socket],[cores],[threads],[frequency],[integrated_grafics],[technical_process],[power],[passMark],[coreName],[model],[producer],[cost],[image],[count],[type])" +
                           $" VALUES ('{socket}',{cores},{threads},'{frequency}','{integrated_grafics}',{technical_process},{power},{passMark},'{coreName}','{model}','{producer}','{cost}','{image}',{count},'CPU')";
                        OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
                        dbCommand.ExecuteNonQuery();
                        
                       
                    }
                    else
                    {
                        query = $"INSERT INTO [products] ([socket],[cores],[threads],[frequency],[integrated_grafics],[technical_process],[power],[passMark],[coreName],[model],[producer],[cost],[image],[count],[type]) " +
                            $"VALUES ('{socket}',{cores},{threads},{frequency},'{integrated_grafics}',{technical_process},{power},{passMark},'{coreName}','{model}','{producer}',{cost},{count},'CPU')";
                        OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
                        dbCommand.ExecuteNonQuery();
                       
                       
                    }

                    dbConnection.Close();
                    MainMenu.SelectComponentsFromBD(dbConnection, components);
                    return 1;
                }
                return -1;
            }
            else return -2;
        }
    }
}