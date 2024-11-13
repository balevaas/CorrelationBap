using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseView.DatasDTO
{
    public class MonthItem
    {
        public int Number { get; set; }
        public string Month { get; set; }
        public bool IsSelected { get; set; }
        public override string ToString()
        {
            return Month.ToString();
        }
        private bool _isMonth;
        public bool IsMonthComboboxEnabled
        {
            get => _isMonth;
            set
            {
                _isMonth = value;
                //OnPropertyChanged(nameof(IsMonthComboboxEnabled));
            }
        }
        public static string[] MonthNames()
        {
            string[] names =
        [
            "Январь",   // 0
            "Февраль",   // 1
            "Март",      // 2
            "Апрель",     // 3
            "Май",       // 4
            "Июнь",      // 5
            "Июль",      // 6
            "Август",    // 7
            "Сентябрь",  // 8
            "Октябрь",   // 9
            "Ноябрь",    // 10
            "Декабрь"    // 11
        ];
            return names;
        }
    }
}
