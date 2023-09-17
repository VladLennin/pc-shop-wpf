using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КР2семестр
{
   public class Order
    {
        string name;
        string surname;
        string addres;
        string dateOrder;
        List<Component> products;
        string typeDelivery;
        string typePayment;
        string orderCost;
        string status;
        int orderCode;
        public Order(string name, string surname, string addres, List<Component> products, string typeDelivery, string typePayment, string orderCost)
        {
            this.Name = name;
            this.Surname = surname;
            this.Addres = addres;
            this.DateOrder = DateTime.Now.ToString();
            this.Products = products;
            this.TypeDelivery = typeDelivery;
            this.TypePayment = typePayment;
            this.OrderCost = orderCost;
            this.Status = "In processing";
        }
        public Order(string name, string surname, string addres,string orderTime, string typeDelivery, string typePayment, string orderCost,string status,int orderCode)
        {
            this.Name = name;
            this.Surname = surname;
            this.Addres = addres;
            this.DateOrder = orderTime;
            this.Products = new List<Component>();
            this.TypeDelivery = typeDelivery;
            this.TypePayment = typePayment;
            this.OrderCost = orderCost;
            this.Status = status;
            this.orderCode = orderCode;
        }

        public string Name { get => name; set => name = value; }
        public string Surname { get => surname; set => surname = value; }
        public string Addres { get => addres; set => addres = value; }
        public string DateOrder { get => dateOrder; set => dateOrder = value; }
        public List<Component> Products { get => products; set => products = value; }
        public string TypeDelivery { get => typeDelivery; set => typeDelivery = value; }
        public string TypePayment { get => typePayment; set => typePayment = value; }
        public string OrderCost { get => orderCost; set => orderCost = value; }
        public string Status { get => status; set => status = value; }
        public int OrderCode { get => orderCode; set => orderCode = value; }

        
    }
}
