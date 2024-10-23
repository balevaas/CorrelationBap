using BaseData.EntityFramework.Context;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Diagnostics;
using ViewModelBase;
using ViewModelBase.Commands.AsyncCommand;
using ViewModelBase.Commands.QuickCommand;
using Xceed.Wpf.Toolkit;
using static BaseViewModel.SelectDataViewModel;

namespace BaseViewModel
{
    public class SelectDataViewModel : ViewModel
    {
        private readonly DataContext _model;
        public ObservableCollection<string> StationName { get; private set; }
        //public static ObservableCollection<MonthItem> Months { get; private set; }
        //public static ObservableCollection<YearItem> Years { get; private set; }
        public SelectDataViewModel(DataContext model)
        {
            _model = model;
            StationName = new ObservableCollection<string>(model.Stations.Select(s => s.Name));
            SelectStationID = new Command<string>(SelectStation);
            SelectPoint = new Command<int>(SelectPoints);
            //SelectYears = new Command<int>(SelectYear);
            //Months = DateForPollutionViewModel.InsertMonth(Months);
            //Years = DateForPollutionViewModel.InsertYear(Years);

            Seasons =
            [
               new("Зима", [12, 1, 2]),
                new("Весна", [3, 4, 5]),
                new("Лето", [6, 7, 8]),
                new("Осень", [9, 10, 11]),
                new("Весь год", [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12])
            ];

            Test = new AsyncCommand(TestData);
        }
        public Command<string> SelectStationID { get; set; }
        public ObservableCollection<int> StationID { get; private set; }
        public string NameS { get; private set; }
        public List<int> Points { get; private set; }
        private void SelectStation(string name)
        {
            StationID = new ObservableCollection<int>(_model.Stations.Where(p => p.Name == name).Select(p => p.ID));
            int id = StationID.ElementAt(0);
            NameS = name;
            Points = GetIDByStationID(id);
            OnPropertyChanged(nameof(Points));
        }
        public List<int> GetIDByStationID(int station)
        {
            var ID = _model.Points
                .Where(od => od.StationID == station)
                .Select(od => od.ID)
                .ToList();
            return ID; // Возвращаем найденный ID            
        }

        public Command<int> SelectPoint;
        public ObservableCollection<int> IDPoint { get; private set; }
        public int PointID;

        public class YearItem
        {
            public int Year { get; set; }
            public bool IsSelected { get; set; }
        }
        public ObservableCollection<YearItem> Years { get; private set; }
        public ObservableCollection<MonthItem> Months { get; private set; }
        private ObservableCollection<int> YearsInt { get; set; }
        private ObservableCollection<int> MonthInt { get; set; }
        private void SelectPoints(int id)
        {
            Years = new ObservableCollection<YearItem>();
            Months = new ObservableCollection<MonthItem>();
            IDPoint = new ObservableCollection<int>(_model.Points.Where(p => p.ID == id).Select(p => p.ID));
            YearsInt = new ObservableCollection<int>(_model.Pollutions.Where(p => p.PointID == id).Select(p => p.Date.Year).Distinct());
            MonthInt = new ObservableCollection<int>(_model.Pollutions.Where(p => p.PointID == id).Select(p => p.Date.Month).Distinct());
            for(int i = 0; i < YearsInt.Count; i++)
            {
                Years.Add(new YearItem { Year = YearsInt.ElementAt(i), IsSelected = false});
            }
            for(int i = 0; i < MonthInt.Count; i++)
            {
                Months.Add(new MonthItem { Number = MonthInt.ElementAt(i), Month = monthNames[MonthInt.ElementAt(i)], IsSelected = false });
            }
            PointID = IDPoint.ElementAt(0);
            OnPropertyChanged(nameof(Years));
        }

        public class MonthItem
        {
            public int Number { get; set; }
            public string Month { get; set; }
            public bool IsSelected { get; set; }
        }

        string[] monthNames = new string[]
        {
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
        };

        //public int[] testyear { get; private set; }
        //public ObservableCollection<int> testmonth { get; private set; }
        //public ObservableCollection<MonthItem> testmonths { get; private set; }
        //public Command<int> SelectYears {  get; private set; }
        //private void SelectYear(int id)
        //{
        //    testyear = new ObservableCollection<int>(Years.Where(m => m.IsSelected).Select(m => m.Year)).ToArray();
        //    testmonth = new ObservableCollection<int>();
        //    if(testyear.Length == 1)
        //    {
        //        testmonth = new ObservableCollection<int>(_model.Pollutions.Where(p => p.PointID == PointID).Select(p => p.Date.Month).Distinct());
        //        for(int i = 0; i < testmonth.Count; i++)
        //        {
        //            if (testmonth[i] >= 1 && testmonth[i] <= 12)
        //            {
        //                testmonths.Add(new MonthItem { Month = monthNames[testmonth[i - 1]], IsSelected = false });
        //            }
        //        }
        //    }
        //    OnPropertyChanged(nameof(testmonth));
        //}

        private Season _selectedSeason;
        public ObservableCollection<Season> Seasons { get; set; }
        public Season SelectedSeason
        {
            get => _selectedSeason;
            set
            {
                _selectedSeason = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(MonthNumbers));
            }
        }

        public ObservableCollection<int> MonthNumbers =>
            SelectedSeason != null ? new ObservableCollection<int>(SelectedSeason.MonthsNumber) : [];
        public int[] NumberYear { get; private set; }
        public int[] NumberMonth { get; private set; }
        public DateTime[] dateOneYear, dateSeasons;
        public int[] months;

        private void SelectDate()
        {
            NumberYear = new ObservableCollection<int>(Years.Where(m => m.IsSelected).Select(m => m.Year)).ToArray();
            

            if (NumberYear[0] == 2021 && NumberYear[1] == 2022)
            {
                SeasonPollut(dateSeasons);
            }

            else if (NumberYear[0] == 2022)
            {
                //NumberMonth = new ObservableCollection<int>(Months.Where(m => m.IsSelected).Select(m => m.NumberMonth)).ToArray();
                OneYearPollut(dateOneYear);
            }
            else if (NumberYear[0] == 2021)
            {
                //NumberMonth = new ObservableCollection<int>(Months.Where(m => m.IsSelected).Select(m => m.NumberMonth)).ToArray();
                OneYearPollut(dateOneYear);
            }
        }

        private void OneYearPollut(DateTime[] date)
        {
            date = new DateTime[NumberMonth.Length];
            for (int i = 0; i < date.Length; i++) date[i] = new DateTime(NumberYear[0], NumberMonth[i], 01);
            PollutionMas(date);
        }

        private void SeasonPollut(DateTime[] dates)
        {
            List<DateTime> dateSeason = [];
            months = [.. MonthNumbers];
            foreach (var year in NumberYear)
            {
                foreach (var month in months)
                {
                    dateSeason.Add(new DateTime(year, month, 01));
                }
            }
            DateTime[] datesArray = [.. dateSeason];
            PollutionMas(datesArray);
        }

        public ObservableCollection<int> PollutionsPost { get; private set; }
        public decimal[] Pollutions { get; private set; }
        public string MessageText;
        
        public decimal[] PollutionMas(DateTime[] date)
        {
            Pollutions = new ObservableCollection<decimal>(_model.Pollutions.Where(m => date.Any(d => d.Date == m.Date)).Where(m => m.PointID == PointID).Select(m => m.Concentration)).ToArray();
            return Pollutions;           
        }

        public AsyncCommand Test { get; }
        private async Task TestData(CancellationToken _)
        {
            SelectDate();
        }
    }
}
