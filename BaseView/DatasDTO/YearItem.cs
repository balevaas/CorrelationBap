using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseView.DatasDTO
{
    public class YearItem
    {
        public int Year { get; set; }
        public bool IsSelected { get; set; }
        public override string ToString()
        {
            return Year.ToString();
        }
    }
}
