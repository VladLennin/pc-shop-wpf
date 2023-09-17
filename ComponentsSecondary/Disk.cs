using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КР2семестр
{
    abstract public class Disk:Component,ICloneable
    {
        int memoryCapacity;
        string formFactor;
        string portConnect;
        int speedRead;
        int speedWrite;

        public  Disk(int memoryCapacity, string formFactor, string producer, string portConnect, int speedRead, int speedWrite,string model,double cost,string image, ComponentType type,int count,int productCode) :base(model,count,cost,producer,image,type,productCode)
        {
            this.MemoryCapacity = memoryCapacity;
            this.FormFactor = formFactor;
            this.PortConnect = portConnect;
            this.SpeedRead = speedRead;
            this.SpeedWrite = speedWrite;
        }
        public Disk(int memoryCapacity, string formFactor, string producer, string portConnect, int speedRead, int speedWrite, string model, double cost, ComponentType type,int count,int productCode) : base(model,count, cost, producer, type,productCode)
        {
            this.MemoryCapacity = memoryCapacity;
            this.FormFactor = formFactor;
            this.PortConnect = portConnect;
            this.SpeedRead = speedRead;
            this.SpeedWrite = speedWrite;
        }

        protected Disk()
        {
        }

        public int MemoryCapacity { get => memoryCapacity; set => memoryCapacity = value; }
        public string FormFactor { get => formFactor; set => formFactor = value; }
        public int SpeedRead { get => speedRead; set => speedRead = value; }
        public int SpeedWrite { get => speedWrite; set => speedWrite = value; }
        public string PortConnect { get => portConnect; set => portConnect = value; }

       
    }
}
