using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
namespace КР2семестр
{
    interface IWorkWithBD
    {
         void FillComponentsFromBD(List<Component> components, OleDbConnection dbConnection);

        void FillComponentsFromBDtoBasket(List<Component> basket, OleDbConnection dbConnection, int userCode);
    
            
    }
}
