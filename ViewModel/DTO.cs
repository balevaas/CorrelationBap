using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseViewModel
{
    public class DTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Height { get; set; }
    }
}
