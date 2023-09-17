using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КР2семестр
{
    public class Motherboard : Component, ICloneable,IWorkWithBD
    {
        Sockets socket;
        CPUproducers forCPU;
        MotherboardSizes size;
        RAMgen ramGen;
        string chipset;

        int countRAM;
        int countChannelRAM;

        int maxCapacityRAM;
        int maxFrequencyRAM;

        List<Port> ports = new List<Port>();

        public Motherboard(Sockets socket, CPUproducers forCPU, string producer, MotherboardSizes size, string chipset, RAMgen ramGen, int countRAM, int countChannelRAM, int maxCapacityRAM, int maxFrequencyRAM, string model, double cost, string image, ComponentType type, int count, int productCode) : base(model, count, cost, producer, image, type, productCode)
        {
            this.Socket = socket;
            this.ForCPU = forCPU;
            this.Size = size;
            this.Chipset = chipset;
            this.RamGen = ramGen;
            this.CountRAM = countRAM;
            this.CountChannelRAM = countChannelRAM;
            this.MaxCapacityRAM = maxCapacityRAM;
            this.MaxFrequencyRAM = maxFrequencyRAM;
        }
        public Motherboard(Sockets socket, CPUproducers forCPU, string producer, MotherboardSizes size, string chipset, RAMgen ramGen, int countRAM, int countChannelRAM, int maxCapacityRAM, int maxFrequencyRAM, string model, double cost, ComponentType type, int count, int productCode) : base(model, count, cost, producer, type, productCode)
        {
            this.Socket = socket;
            this.ForCPU = forCPU;
            this.Size = size;
            this.Chipset = chipset;
            this.RamGen = ramGen;
            this.CountRAM = countRAM;
            this.CountChannelRAM = countChannelRAM;
            this.MaxCapacityRAM = maxCapacityRAM;
            this.MaxFrequencyRAM = maxFrequencyRAM;
        }

        public Motherboard()
        {
        }

        public int CountPorts { get => Ports.Count; }
        public string Chipset { get => chipset; set => chipset = value; }
        public int CountRAM { get => countRAM; set => countRAM = value; }
        public int CountChannelRAM { get => countChannelRAM; set => countChannelRAM = value; }
        public int MaxCapacityRAM { get => maxCapacityRAM; set => maxCapacityRAM = value; }
        public int MaxFrequencyRAM { get => maxFrequencyRAM; set => maxFrequencyRAM = value; }
        public Sockets Socket { get => socket; set => socket = value; }
        public CPUproducers ForCPU { get => forCPU; set => forCPU = value; }
        public MotherboardSizes Size { get => size; set => size = value; }
        public RAMgen RamGen { get => ramGen; set => ramGen = value; }
        internal List<Port> Ports { get => ports; set => ports = value; }

        public Port this[int index]
        {
            get => Ports[index];
            set => Ports[index] = value;
        }
        public void AddPort(Port port)
        {
            Ports.Add(port);
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
                string type = " ";
                query = $"SELECT [type] FROM products WHERE productCode = {productCodes[i]}";
                dbCommand = new OleDbCommand(query, dbConnection);
                dbReader = dbCommand.ExecuteReader();
                while (dbReader.Read())
                {
                    type = (string)dbReader["type"];
                }


                if (type == "Motherboard")
                {

                    query = $"SELECT [productCode],[model],[producer],[image],[socket],[cost],[forCPU],[size],[RAMgen],[chipset],[countRAM],[countRAMchannel],[maxCapacityRAM],[maxFrequencyRAM],[ports]" +
                        $" FROM products WHERE productCode = {productCodes[i]}";
                    dbCommand = new OleDbCommand(query, dbConnection);
                    dbReader = dbCommand.ExecuteReader();
                    while (dbReader.Read())
                    {


                        string model = (string)dbReader["model"];
                        string producer = (string)dbReader["producer"];
                        string image = (string)dbReader["image"];
                        double cost = (double)dbReader["cost"];

                        string socketTemp = (string)dbReader["socket"];
                        string sizeTemp = (string)dbReader["size"];
                        string RAMgenTemp = (string)dbReader["RAMgen"];
                        string forCPUtemp = (string)dbReader["forCPU"];
                        string chipset = (string)dbReader["chipset"];
                        int countRAM = (int)dbReader["countRAM"];
                        int countChannelRAM = (int)dbReader["countRAMchannel"];
                        int maxCapacityRAM = (int)dbReader["maxCapacityRAM"];
                        int maxFrequencyRAM = (int)dbReader["maxFrequencyRAM"];
                        string ports = (string)dbReader["ports"];

                        Sockets socket = Sockets.Socket1150;
                        for (int k = 0; k < 7; k++)
                        {
                            if (((Sockets)k).ToString() == socketTemp)
                            {
                                socket = (Sockets)k;
                                break;
                            }
                        }
                        CPUproducers forCPU = CPUproducers.Intel;
                        if (forCPUtemp == "AMD")
                            forCPU = CPUproducers.AMD;


                        MotherboardSizes size = MotherboardSizes.ATX;
                        for (int p = 0; p < 4; p++)
                        {
                            if (((MotherboardSizes)p).ToString() == sizeTemp)
                            {
                                size = (MotherboardSizes)p;
                                break;
                            }
                        }

                        RAMgen ramGen = RAMgen.DDR1;
                        for (int g = 0; g < 4; g++)
                        {
                            if (RAMgenTemp == ((RAMgen)g).ToString())
                            {
                                ramGen = (RAMgen)g;
                            }
                        }

                        Motherboard motherboard = new Motherboard(socket, forCPU, producer, size, chipset, ramGen, countRAM, countChannelRAM, maxCapacityRAM, maxFrequencyRAM, model, cost,image, ComponentType.Motherboard, counts[i], productCodes[i]);

                        string[] portsData = ports.Split(';');
                        for (int o = 0; o < portsData.Length; o++)
                        {
                            if (portsData[o] != "\n" && portsData[o] != "")
                            {
                                string[] tempPort = portsData[o].Split('-');
                                motherboard.AddPort(new Port(tempPort[0], Convert.ToInt32(tempPort[1].Substring(0, tempPort[1].Length - 1))));
                            }
                        }
                        motherboard.Count = counts[i];
                        basket.Add(motherboard);


                    }

                }
            }

            dbConnection.Close();
        }
        public  void FillComponentsFromBD(List<Component> components, OleDbConnection dbConnection)
        {

            dbConnection.Open();
            string query = @"SELECT [productCode],[model],[cost],[image],[producer],[count],[socket],[forCPU],[size],[RAMgen],[chipset],[countRAM],[countRAMchannel],[maxCapacityRAM],[maxFrequencyRAM],[ports] FROM products WHERE type = 'Motherboard'";
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            OleDbDataReader dbReader = dbCommand.ExecuteReader();
            while (dbReader.Read())
            {
                string model = (string)dbReader["model"];
                double cost = (double)dbReader["cost"];
                string image = (string)dbReader["image"];
                string producer = (string)dbReader["producer"];
                int count = (int)dbReader["count"];
                int productCode = (int)dbReader["productCode"];

                string socketTemp = (string)dbReader["socket"];
                string forCPUtemp = (string)dbReader["forCPU"];
                string sizeTemp = (string)dbReader["size"];
                string RAMgenTemp = (string)dbReader["RAMgen"];
                string chipset = (string)dbReader["chipset"];
                int countRAM = (int)dbReader["countRAM"];
                int countRAMchannel = (int)dbReader["countRAMchannel"];
                int maxCapacityRAM = (int)dbReader["maxCapacityRAM"];
                int maxFrequencyRAM = (int)dbReader["maxFrequencyRAM"];
                string ports = (string)dbReader["ports"];

                MotherboardSizes size = MotherboardSizes.ATX;
                for (int i = 0; i < 4; i++)
                {
                    if (sizeTemp == ((MotherboardSizes)i).ToString())
                        size = (MotherboardSizes)i;
                }

                Sockets socket = Sockets.Socket1150;
                for (int i = 0; i < 7; i++)
                {
                    if (socketTemp == ((Sockets)i).ToString())
                    {
                        socket = (Sockets)i;
                    }
                }
                CPUproducers forCPU = CPUproducers.AMD;
                if (forCPUtemp != "AMD")
                    forCPU = CPUproducers.Intel;

                RAMgen ramGEN = RAMgen.DDR1;
                for (int i = 0; i < 4; i++)
                {
                    if (RAMgenTemp == ((RAMgen)i).ToString())
                        ramGEN = (RAMgen)i;
                }


                Motherboard motherboard = new Motherboard(socket, forCPU, producer, size, chipset, ramGEN, countRAM, countRAMchannel, maxCapacityRAM, maxFrequencyRAM, model, cost, image, ComponentType.Motherboard, count, productCode);

                string[] portsData = ports.Split(';');
                for (int i = 0; i < portsData.Length; i++)
                {
                    if (portsData[i] != "\n" && portsData[i] != "")
                    {
                        string[] tempPort = portsData[i].Split('-');
                        motherboard.AddPort(new Port(tempPort[0], Convert.ToInt32(tempPort[1].Substring(0, tempPort[1].Length - 1))));
                    }
                }
                motherboard.Count = count;
                components.Add(motherboard);

            }
            dbConnection.Close();

        }
        public static int AddMotherboardToBD(double cost, string image, string producer, int count, string model, string socket, string forCPU, string size, string ramGEN, string chipset, int countRAM, int countRAMchannel, int maxCapacityRAM, int maxFrequencyRAM, string ports, OleDbConnection dbConnection, List<Component> components)
        {
            dbConnection.Open();
            string query = $"SELECT COUNT(model) FROM products  WHERE model='{model}'";
            OleDbCommand oleDbCommand = new OleDbCommand(query, dbConnection);

            if ((int)oleDbCommand.ExecuteScalar() == 0)
            {
                if (image != "")
                {
                    query = $"INSERT INTO [products] ([type],[cost],[image],[producer],[count],[model],[socket],[forCPU],[size],[RAMgen],[chipset],[countRAM],[countRAMchannel],[maxCapacityRAM],[maxFrequencyRAM],[ports])" +
                        $" VALUES('Motherboard','{cost}','{image}','{producer}',{count},'{model}','{socket}','{forCPU}','{size}','{ramGEN}','{chipset}','{countRAM}','{countRAMchannel}','{maxCapacityRAM}','{maxFrequencyRAM}','{ports}')";
                    OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
                    dbCommand.ExecuteNonQuery();
                    dbConnection.Close();

                }
                else
                {
                    query = $"INSERT INTO [products] ([cost],[producer],[count],[model],[socket],[forCPU],[size],[RAMgen],[chipset],[countRAM],[countRAMchannel],[maxCapacityRAM],[maxFrequencyRAM],[ports])" +
                        $" VALUES('Motherboard','{cost}','{producer}',{count},'{model}','{socket}','{forCPU}','{size}','{ramGEN}','{chipset}','{countRAM}','{countRAMchannel}','{maxCapacityRAM}','{maxFrequencyRAM}','{ports}')";

                    OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
                    dbCommand.ExecuteNonQuery();


                }
                MainMenu.SelectComponentsFromBD(dbConnection, components);
                return 1;
            }
               
              
            
           
            dbConnection.Close();
            return -1;
        }
    }
}
