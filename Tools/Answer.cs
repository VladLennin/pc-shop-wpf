using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace КР2семестр
{
    public  class Answer
    {
        string text;
        string name;
        string time;
        string login;
        List<Answer> answers;

        public Answer(string text, string name,string login)
        {
            this.Text = text;
            this.Name = name;
            this.Login = login;
            this.Time = DateTime.Now.ToString();
            answers = new List<Answer>();
        }

        public string Text { get => text; set => text = value; }
        public string Name { get => name; set => name = value; }
        public string Time { get => time; set => time = value; }
        public string Login { get => login; set => login = value; }

        public void AddAnswer(Answer answer)
        {
            answers.Add(answer);
        }

    }
}
