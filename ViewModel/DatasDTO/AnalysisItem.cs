using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseViewModel.DatasDTO
{
    public class AnalysisItem
    {
        public string CityName { get; set; }
        public int NumberPoint { get; set; }
        public DateTime[] dateTimes { get; set; }
        public decimal[] Pollutions { get; set; }
    }
}
