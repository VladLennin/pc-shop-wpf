using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КР2семестр
{
    public class GPU : Component, ICloneable,IWorkWithBD
    {
        GPUfamily familyGPU;
        int memoryCapacity;
        int digitCapacity;
        GPUgen memoryGen;
        int passMark;

        List<Port> ports;

        public GPU(GPUfamily familyGPU, string producer, int memoryCapacity, int digitCapacity, GPUgen memoryGen, int passMark, string model, double cost, string image, ComponentType type, int count, int productCode) : base(model, count, cost, producer, image, type, productCode)
        {
            this.FamilyGPU = familyGPU;
            this.MemoryCapacity = memoryCapacity;
            this.DigitCapacity = digitCapacity;
            this.MemoryGen = memoryGen;
            this.PassMark = passMark;

            ports = new List<Port>();
        }
        public GPU(GPUfamily familyGPU, string producer, int memoryCapacity, int digitCapacity, GPUgen memoryGen, int passMark, string model, double cost, ComponentType type, int count, int productCode) : base(model, count, cost, producer, type, productCode)
        {
            this.FamilyGPU = familyGPU;
            this.MemoryCapacity = memoryCapacity;
            this.DigitCapacity = digitCapacity;
            this.MemoryGen = memoryGen;
            this.PassMark = passMark;

            ports = new List<Port>();
        }

        public GPU()
        {

        }

        public int MemoryCapacity { get => memoryCapacity; set => memoryCapacity = value; }
        public int DigitCapacity { get => digitCapacity; set => digitCapacity = value; }
        public int PassMark { get => passMark; set => passMark = value; }
        public GPUgen MemoryGen { get => memoryGen; set => memoryGen = value; }
        public GPUfamily FamilyGPU { get => familyGPU; set => familyGPU = value; }
        public int CountPorts { get => ports.Count; }
        public Port this[int index]
        {
            get => ports[index];
            set => ports[index] = value;
        }
        public void AddPort(Port port)
        {
            ports.Add(port);
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


                if (type == "GPU")
                {
                    query = $"SELECT [cost],[image],[producer],[count],[model],[gpu_family],[memory_capacity],[digit_capacity],[memory_gen],[passMark],[ports],[type],[productCode]" +
                        $" FROM products WHERE productCode = {productCodes[i]}";
                    dbCommand = new OleDbCommand(query, dbConnection);
                    dbReader = dbCommand.ExecuteReader();
                    while (dbReader.Read())
                    {

                        string gpu_family = (string)dbReader["gpu_family"];
                        int memory_capacity = (int)dbReader["memory_capacity"];
                        int digit_capacity = (int)dbReader["digit_capacity"];
                        string memory_gen = (string)dbReader["memory_gen"];
                        string ports = (string)dbReader["ports"];
                     


                       
                       
                        int passMark = (int)dbReader["passMark"];
                        int productCode = (int)dbReader["productCode"];    
                        string model = (string)dbReader["model"];
                        string producer = (string)dbReader["producer"];
                        string image = (string)dbReader["image"];
                        double cost = (double)dbReader["cost"];

                        GPUfamily gpuFamily = GPUfamily.AMD;
                        if (gpu_family == "NVIDIA")
                            gpuFamily = GPUfamily.NVIDIA;

                        GPUgen gpuGen = GPUgen.GDDR3;
                        for (int l = 0; l < 7; l++)
                        {
                            if(((GPUgen)l).ToString()==memory_gen)
                            {
                                gpuGen = (GPUgen)l;
                                break;
                            }
                        }




                        GPU gpu = new GPU(gpuFamily,producer,memory_capacity,digit_capacity,gpuGen,passMark,model,cost,image,ComponentType.GPU,counts[i],productCode);
                        gpu.Count = counts[i];
                        basket.Add(gpu);


                    }
                }
            }
            dbConnection.Close();
        }

        public  void FillComponentsFromBD(List<Component>components,OleDbConnection dbConnection)
        {
            dbConnection.Open();
            string query = $"SELECT COUNT(type) FROM products WHERE type = 'GPU'";
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            if ((int)dbCommand.ExecuteScalar() != 0)
            {
                query = $"SELECT productCode,gpu_family,memory_capacity,digit_capacity,memory_gen,passMark,ports,model,cost,image,producer,count" +
                 $" FROM products WHERE type = 'GPU' ";
                dbCommand = new OleDbCommand(query, dbConnection);
                OleDbDataReader dbReader = dbCommand.ExecuteReader();
                while (dbReader.Read())
                {
                    string gpu_familyTemp = (string)dbReader["gpu_family"];
                    int memory_capacity = (int)dbReader["memory_capacity"];
                    int digit_capacity = (int)dbReader["digit_capacity"];
                    string memory_genTemp = (string)dbReader["memory_gen"];
                    int passMark = (int)dbReader["passMark"];
                    string ports = (string)dbReader["ports"];


                    string model = (string)dbReader["model"];
                    double cost = (double)dbReader["cost"];
                    string image = (string)dbReader["image"];
                    string producer = (string)dbReader["producer"];
                    int count = (int)dbReader["count"];
                    int productCode = (int)dbReader["productCode"];

                    GPUfamily gpu_family = GPUfamily.AMD;
                    if (gpu_familyTemp != "AMD")
                        gpu_family = GPUfamily.AMD;
                    GPUgen memory_gen = GPUgen.GDDR3;
                    for (int i = 0; i < 7; i++)
                    {
                        if (memory_genTemp == ((GPUgen)i).ToString())
                            memory_gen = (GPUgen)i;
                    }

                    GPU gpu = new GPU(gpu_family, producer, memory_capacity, digit_capacity, memory_gen, passMark, model, cost, image, ComponentType.GPU, count, productCode);

                    string[] portsData = ports.Split(';');
                    for (int i = 0; i < portsData.Length; i++)
                    {
                        if (portsData[i] != "\n" && portsData[i] != "")
                        {
                            string[] tempPort = portsData[i].Split('-');
                            gpu.AddPort(new Port(tempPort[0], Convert.ToInt32(tempPort[1].Substring(0, tempPort[1].Length - 1))));
                        }
                    }
                    gpu.Count = count;
                    components.Add(gpu);
                }
            }
            dbConnection.Close();
        }

        public static int AddGPUtoBD(bool check, double cost, string image, string producer, int count, string model, string gpu_family, int memory_capacity, int digit_capacity, string memory_gen, int pass_mark, string ports, List<Component> components, OleDbConnection dbConnection)
        {
            dbConnection.Open();
            string query = $"SELECT COUNT(model) FROM products  WHERE model='{model}' and producer ='{producer}'";
            OleDbCommand oleDbCommand = new OleDbCommand(query, dbConnection);
            if (check)
            {
                if ((int)oleDbCommand.ExecuteScalar() == 0)
                {
                    if (image != "")
                    {
                        query = $"INSERT INTO [products] ([cost],[image],[producer],[count],[model],[gpu_family],[memory_capacity],[digit_capacity],[memory_gen],[passMark],[ports],[type])" +
                            $" VALUES('{cost}','{image}','{producer}',{count},'{model}','{gpu_family}',{memory_capacity},{digit_capacity},'{memory_gen}',{pass_mark},'{ports}','GPU')";
                        OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
                        dbCommand.ExecuteNonQuery();
                        dbConnection.Close();
                        return 1;
                    }
                    else
                    {
                        query = $"INSERT INTO [products] (cost,producer,[count],model,gpu_family,mmory_capacity,digit_capacity,memory_gen,pass_mark,ports,type)" +
                            $" VALUES('{cost}','{producer}','{count}','{model}','{gpu_family}','{memory_capacity}','{digit_capacity}','{memory_gen}','{pass_mark}','{ports}','GPU')";
                        OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
                        dbCommand.ExecuteNonQuery();
                        dbConnection.Close();
                        return 1;
                    }
                }
                dbConnection.Close();
                return -1;
                
            }
            dbConnection.Close();
            return -2;
        }
    }
}
