using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseView.DatasDTO
{
    public class SeasonItem
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return Name.ToString();
        }
        public bool IsSelected { get; set; }

        public static string[] SeasonNames()
        {
            string[] names =
                [
                "Зима",
                "Весна",
                "Лето",
                "Осень",
                "Весь год"
                ];
            return names;
        }
    }
}
