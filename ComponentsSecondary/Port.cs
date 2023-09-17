using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КР2семестр
{
    public class Port:ICloneable
    {
        string name;
        int count;
        public Port() { }
        public Port(string _name,int _count)
        {
            Name = _name;
            Count = _count;
        }

        public string Name { get => name; set => name = value; }
        public int Count { get => count; set => count = value; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"{Name} - {count}x";
        }
    }
}
