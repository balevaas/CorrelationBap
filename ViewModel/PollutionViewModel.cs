using BaseData.EntityFramework.Context;
using System.Collections.ObjectModel;
using ViewModelBase;
using ViewModelBase.Commands.AsyncCommand;
using ViewModelBase.Commands.QuickCommand;

namespace BaseViewModel
{
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

    public class PollutionViewModel : ViewModel
    {
        DateForPollutionViewModel datesForPol = new DateForPollutionViewModel();
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


        private readonly DataContext _model;

        public ObservableCollection<string> StationName { get; private set; }
        public static ObservableCollection<MonthItem> Months { get; private set; }
        public static ObservableCollection<YearItem> Years { get; private set; }

        public string NameS;

        public PollutionViewModel(DataContext model)
        {
            _model = model;
            StationName = new ObservableCollection<string>(model.Stations.Select(s => s.Name));
            SelectStationID = new Command<string>(SelectStation);
            SelectPoint = new Command<int>(SelectPoints);

            Months = DateForPollutionViewModel.InsertMonth(Months);
            Years = DateForPollutionViewModel.InsertYear(Years);

            Test = new AsyncCommand(TestData);

            Seasons =
            [
                new("Зима", [12, 1, 2]),
                new("Весна", [3, 4, 5]),
                new("Лето", [6, 7, 8]),
                new("Осень", [9, 10, 11]),
                new("Весь год", [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12])
            ];
        }

        public ObservableCollection<int> StationID { get; private set; }
        public Command<string> SelectStationID { get; set; }
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
        private void SelectPoints(int id)
        {
            IDPoint = new ObservableCollection<int>(_model.Points.Where(p => p.ID == id).Select(p => p.ID));
            PointID = IDPoint.ElementAt(0);
        }


        public ObservableCollection<int> NumberMonths { get; private set; }
        public ObservableCollection<int> NumberYears { get; private set; }

        public int[] NumberMonth = new int[11];
        public int[] NumberYear = new int[5];
        public DateTime[] dateOneYear, dateSeasons;

        public int[] months;

        public ObservableCollection<int> PollutionsPost { get; private set; }
        public ObservableCollection<decimal> Pollutions { get; private set; }
        private void SelectDate()
        {
            NumberMonths = new ObservableCollection<int>(Months.Where(m => m.IsSelected).Select(m => m.NumberMonth));
            NumberMonth = [.. NumberMonths];

            NumberYears = new ObservableCollection<int>(Years.Where(m => m.IsSelected).Select(m => m.Year));
            NumberYear = [.. NumberYears];

            if (NumberYear[0] == 2021 && NumberYear[1] == 2022)
            {
                SeasonPollut(dateSeasons);
            }

            else if (NumberYear[0] == 2022)
            {
                OneYearPollut(dateOneYear);
            }
            else if (NumberYear[0] == 2021)
            {
                OneYearPollut(dateOneYear);
            }
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
        private void OneYearPollut(DateTime[] date)
        {
            date = new DateTime[NumberMonth.Length];
            for (int i = 0; i < date.Length; i++) date[i] = new DateTime(NumberYear[0], NumberMonth[i], 01);
            PollutionMas(date);
        }
        public decimal[] PollutionMas(DateTime[] date)
        {
            PollutionsPost = new ObservableCollection<int>(_model.Pollutions.Where(m => date.Any(d => d.Date == m.Date)).Select(m => m.PointID));
            int post = PollutionsPost.FirstOrDefault();
            Pollutions = new ObservableCollection<decimal>(_model.Pollutions.Where(m => date.Any(d => d.Date == m.Date)).Where(m => m.PointID == post).Select(m => m.Concentration));
            decimal[] pol = [.. Pollutions];
            return pol;
        }

        public AsyncCommand Test { get; }
        private async Task TestData(CancellationToken _)
        {
            SelectDate();
        }
    }
}
