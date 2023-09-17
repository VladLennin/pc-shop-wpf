using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КР2семестр
{
    public class Feedback : Answer
    {
        int countStars;
        public Feedback(string text, string name,string login,int countStars,string time) : base(text, name,login)
        {
            this.CountStars = countStars;
            Time = time;
        }
        public Feedback(string text, string name, string login, int countStars) : base(text, name, login)
        {
            this.CountStars = countStars;
            
        }
        public int CountStars { get => countStars; set => countStars = value; }

        public override string ToString()
        {
            return $"{Text}_{Name}_{Login}_{CountStars}_{Time};";
        }
    }
}
