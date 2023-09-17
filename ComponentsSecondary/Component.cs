using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace КР2семестр
{
    public  class Component : ICloneable
    {
        int productCode;
        string model;
        double cost;
        string image;
        string producer;
        int count;
        ComponentType type;
        ObservableCollection<Feedback> feedbacks;
        public Component() { }
        public Component(string _model,int _count, double _cost, string _producer, string _image, ComponentType _type,int _productCode)
        {
            ProductCode = _productCode;
            model = _model;
            cost = _cost;
            image = _image;
            Producer = _producer;
            Type = _type;
            Count = count;
            Feedbacks = new ObservableCollection<Feedback>();
        }
        public Component(string _model,int _count, double _cost, string _producer, ComponentType _type,int _productCode)
        {
            ProductCode = _productCode;
            model = _model;
            cost = _cost;
            Producer = _producer;
            Type = _type;
            Count = _count;
            Feedbacks = new ObservableCollection<Feedback>();
        }
        public string Model
        {
            get => model;
            set => model = value;
        }
        public double Cost
        {
            get => cost;
            set => cost = value;
        }
        public double Rating
        {
            get
            {
                double countPoints = 0;
                for (int i = 0; i < feedbacks.Count; i++)
                {
                    countPoints += feedbacks[i].CountStars;
                }
                if (feedbacks.Count != 0)
                {
                    double rating = countPoints / feedbacks.Count;
                    rating = Math.Round(rating, 1);
                    return rating;
                }
                else return 0;
            }
        }
        public string Image { get => image; set => image = value; }
        public string Producer { get => producer; set => producer = value; }
        public ComponentType Type { get => type; set => type = value; }
        public int Count { get => count; set => count = value; }
        public int CountFeedBacks { get => Feedbacks.Count; }
        public ObservableCollection<Feedback> Feedbacks { get => feedbacks; set => feedbacks = value; }
        public int ProductCode { get => productCode; set => productCode = value; }

        public Feedback GetFeedBack(int index)
        {
            return Feedbacks[index];
        }
        public void AddFeedBack(Feedback feedback)
        {
            Feedbacks.Add(feedback);
        }
        public override string ToString()
        {
            return $"{Type} {Producer} {Model}";
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void FillFeedbacksFromDB(Component component, OleDbConnection dbConnection)
        {
            string text = " ";
            string user_login = " ";
            DateTime time = new DateTime();
            int countStars = 0;
            string name = " ";

            dbConnection.Open();
            string query = $"SELECT COUNT(productCode) FROM feedbacks WHERE productCode = {component.ProductCode}";
            OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);
            if ((int)dbCommand.ExecuteScalar() != 0)
            {

                query = $"SELECT feedbackText,user_login,feedbackTime,countStars,user_name FROM feedbacks WHERE productCode={component.ProductCode}";
                dbCommand = new OleDbCommand(query, dbConnection);
                OleDbDataReader dbReader = dbCommand.ExecuteReader();
                while (dbReader.Read())
                {
                    text = (string)dbReader["feedbackText"];
                    user_login = (string)dbReader["user_login"];
                    time = (DateTime)dbReader["feedbackTime"];
                    countStars = (int)dbReader["countStars"];
                    name = (string)dbReader["user_name"];
                    component.AddFeedBack(new Feedback(text, name, user_login, countStars));
                }

            }
            dbConnection.Close();
        }
    }
}
