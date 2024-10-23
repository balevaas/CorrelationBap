using BaseData.EntityFramework.Context;
using System.Collections.ObjectModel;
using System.Diagnostics;
using ViewModelBase;
using ViewModelBase.Commands.AsyncCommand;
using ViewModelBase.Commands.QuickCommand;

namespace BaseViewModel
{
    public class SelectDataViewModel : ViewModel
    {
        private readonly DataContext _model;
        public ObservableCollection<string> StationName { get; private set; }
        public static ObservableCollection<MonthItem> Months { get; private set; }
        public static ObservableCollection<YearItem> Years { get; private set; }
        public SelectDataViewModel(DataContext model)
        {
            _model = model;
            StationName = new ObservableCollection<string>(model.Stations.Select(s => s.Name));
            SelectStationID = new Command<string>(SelectStation);
            SelectPoint = new Command<int>(SelectPoints);
            Months = DateForPollutionViewModel.InsertMonth(Months);
            Years = DateForPollutionViewModel.InsertYear(Years);

            Test = new AsyncCommand(TestData);
        }
        public Command<string> SelectStationID { get; set; }
        public ObservableCollection<int> StationID { get; private set; }
        public string NameS;
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

        public int[] NumberYear { get; private set; }
        public int[] NumberMonth { get; private set; }
        public DateTime[] dateOneYear, dateSeasons;

        private void SelectDate()
        {
            NumberYear = new ObservableCollection<int>(Years.Where(m => m.IsSelected).Select(m => m.Year)).ToArray();
            NumberMonth = new ObservableCollection<int>(Months.Where(m => m.IsSelected).Select(m => m.NumberMonth)).ToArray();

            if (NumberYear[0] == 2021 && NumberYear[1] == 2022)
            {
               // SeasonPollut(dateSeasons);
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

        private void OneYearPollut(DateTime[] date)
        {
            date = new DateTime[NumberMonth.Length];
            for (int i = 0; i < date.Length; i++) date[i] = new DateTime(NumberYear[0], NumberMonth[i], 01);
            PollutionMas(date);
        }
        public ObservableCollection<int> PollutionsPost { get; private set; }
        public decimal[] Pollutions { get; private set; }
        
        public decimal[] PollutionMas(DateTime[] date)
        {
            if(PointID == null)
            {

            }
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
