using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelBase;

namespace BaseViewModel
{
    public class MonthItem : ViewModel
    {
        private bool isSelected;
        public string Name { get; set; }
        public int NumberMonth { get; set; }
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
    }
    public class YearItem : ViewModel
    {
        private bool isSelected;
        public int Year { get; set; }
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
    }

    public class Season 
    {
        public string Name { get; set; }
        public List<int> MonthsNumber { get; set; }
        public Season(string name, List<int> monthsNumber)
        {
            Name = name;
            MonthsNumber = monthsNumber;
        }

    }


    public class DateForPollutionViewModel : ViewModel
    {
        public static ObservableCollection<MonthItem>? Months { get; private set; }
        public static ObservableCollection<YearItem>? Years { get; private set; }

        public static ObservableCollection<MonthItem> InsertMonth(ObservableCollection<MonthItem> months)
        {
            return months =
            [
                new() { Name = "Январь", NumberMonth = 1 },
                new() { Name = "Февраль", NumberMonth = 2 },
                new() { Name = "Март", NumberMonth = 3},
                new() { Name = "Апрель", NumberMonth = 4},
                new() { Name = "Май", NumberMonth = 5},
                new() { Name = "Июнь", NumberMonth = 6 },
                new() { Name = "Июль", NumberMonth = 7 },
                new() { Name = "Август", NumberMonth = 8 },
                new() { Name = "Сентябрь", NumberMonth = 9 },
                new() { Name = "Октябрь", NumberMonth = 10 },
                new() { Name = "Ноябрь", NumberMonth = 11 },
                new() { Name = "Декабрь", NumberMonth = 12 }
            ];
        }
        public static ObservableCollection<YearItem> InsertYear(ObservableCollection<YearItem> years)
        {
            return years =
            [
                new() {Year = 2021},
                new() {Year = 2022}
            ];
        }


    }
}
