using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConnectionHomeWork_1
{
    internal class Orders
    {
        public int Id { get; set; }
        public string Acceptor { get; set; }
        public string Repairer { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Defect { get; set; }  
        public string Fixed { get; set; }
    }
}
