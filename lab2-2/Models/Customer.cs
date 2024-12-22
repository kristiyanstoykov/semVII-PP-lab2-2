using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2_2.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int NumberOfOrders { get; set; }

        public Customer(int id, string name, string address, int numberOfOrders)
        {
            Id = id;
            Name = name;
            Address = address;
            NumberOfOrders = numberOfOrders;
        }

        public override string ToString()
        {
            return $"{Id}: {Name}, {Address}, Orders: {NumberOfOrders}";
        }
    }
}
