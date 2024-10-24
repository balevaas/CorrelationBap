using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelBase;

namespace BaseViewModel.DatasDTO
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

        //private SeasonItem _selectedSeason;
        ////public ObservableCollection<SeasonItem> Seasons { get; set; }
        //public SeasonItem SelectedSeason
        //{
        //    get => _selectedSeason;
        //    set
        //    {
        //        _selectedSeason = value;
        //        OnPropertyChanged();
        //        OnPropertyChanged(nameof(MonthNumbers));
        //    }
        //}
        //public ObservableCollection<int> MonthNumbers =>
        //    SelectedSeason != null ? new ObservableCollection<int>(SelectedSeason.MonthsNumber) : [];

        //public static ObservableCollection<SeasonItem> InsertSeason(ObservableCollection<SeasonItem> seasons)
        //{
        //    return seasons =
        //    [
        //        new("Зима", [12, 1, 2]),
        //        new("Весна", [3, 4, 5]),
        //        new("Лето", [6, 7, 8]),
        //        new("Осень", [9, 10, 11]),
        //        new("Весь год", [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12])
        //    ];
        //}
    }
}
