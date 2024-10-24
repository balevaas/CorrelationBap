using BaseData.EntityFramework.Context;
using BaseViewModel.DatasDTO;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using ViewModelBase;
using ViewModelBase.Commands.AsyncCommand;
using ViewModelBase.Commands.QuickCommand;

namespace BaseViewModel
{
    public class SelectPointYearsViewModel : ViewModel
    {
        private readonly DataContext _model;
        public ObservableCollection<string> StationName { get; private set; }
        public int[] NumberYear { get; private set; }
        public int[] NumberMonth { get; private set; }
        public DateTime[] dateOneYear, dateSeason;
        public SelectPointYearsViewModel(DataContext model)
        {
            _model = model;
            StationName = new ObservableCollection<string>(model.Stations.Select(s => s.Name));
            SelectStationID = new Command<string>(SelectStation);
            SelectPointID = new Command<int>(SelectPoint);
            SelectPollutionCommand = new AsyncCommand(SelectPollution);
        }

        #region Выбор города, загрузка точек
        public Command<string> SelectStationID { get; set; }
        public ObservableCollection<int> StationID { get; private set; }
        private string cityName;
        public List<int> Points { get; private set; }
        private void SelectStation(string name)
        {
            StationID = new ObservableCollection<int>(_model.Stations.Where(p => p.Name == name).Select(p => p.ID));
            int id = StationID.ElementAt(0);
            cityName = name;
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
        #endregion

        #region Выбор точки, извлечение только года
        public Command<int> SelectPointID { get; set; }
        public ObservableCollection<int> IDPoint { get; private set; }
        public int PointID;

        public ObservableCollection<YearItem> Years { get; private set; }
        public List<YearItem> GetYears(int id)
        {
            return _model.Pollutions.Where(p => p.PointID == id).Select(y => new YearItem { Year = y.Date.Year, IsSelected = false }).Distinct().ToList();
        }
        private void SelectPoint(int pointID)
        {
            Years = new ObservableCollection<YearItem>();
            Months = new ObservableCollection<MonthItem>();
            Seasons = new ObservableCollection<SeasonItem>();
            IDPoint = new ObservableCollection<int>(_model.Points.Where(p => p.ID == pointID).Select(p => p.ID));
            PointID = IDPoint.ElementAt(0);
            foreach (var year in GetYears(PointID))
            {
                Years.Add(year);
            }
            foreach (var month in GetMonthNames())
            {
                Months.Add(month);
            }
            foreach (var seasons in GetSeasonItems())
            {
                Seasons.Add(seasons);
            }
            OnPropertyChanged(nameof(Years));
            OnPropertyChanged(nameof(Months));
            OnPropertyChanged(nameof(Seasons));
        }
        #endregion

        #region Месяцы
        public ObservableCollection<MonthItem> Months { get; private set; }
        private bool _isMonth;
        public bool IsMonthComboboxEnabled
        {
            get => _isMonth;
            set
            {
                _isMonth = value;
                OnPropertyChanged(nameof(IsMonthComboboxEnabled));
            }
        }
        public int[] numMonth { get; set; }
        public List<MonthItem> GetMonthNames()
        {
            int[] numMonth = new ObservableCollection<int>(_model.Pollutions.Select(p => p.Date.Month).Distinct()).ToArray();
            List<MonthItem> monthItems = new List<MonthItem>();
            string[] names = MonthItem.MonthNames();
            for (int i = 0; i < numMonth.Length; i++)
            {
                monthItems.Add(new MonthItem
                {
                    Number = i + 1,
                    Month = names[numMonth[i] - 1]
                });
            }
            return monthItems;
        }

        #endregion

        #region Сезон
        public ObservableCollection<SeasonItem> Seasons { get; private set; }
        private bool _isSeason;
        public bool IsSeasonRadioButtonEnabled
        {
            get => _isSeason;
            set
            {
                _isSeason = value;
                OnPropertyChanged(nameof(IsSeasonRadioButtonEnabled));
            }
        }
        public void UpdateSelection()
        {
            int selectedCount = Years.Count(y => y.IsSelected);
            IsMonthComboboxEnabled = selectedCount == 1;
            IsSeasonRadioButtonEnabled = selectedCount > 1;
        }
        public List<SeasonItem> GetSeasonItems()
        {
            List<SeasonItem> items = new List<SeasonItem>();
            string[] names = SeasonItem.SeasonNames();
            for (int i = 0; i < names.Length; i++)
            {
                items.Add(new SeasonItem
                {
                    Name = names[i]
                });
            }
            return items;
        }
        #endregion
        
        #region Даты в пределах одного года
        private void OneYearPollut(DateTime[] date)
        {
            NumberMonth = new ObservableCollection<int>(Months.Where(m => m.IsSelected).Select(m => m.Number)).ToArray();
            date = new DateTime[NumberMonth.Length];
            for (int i = 0; i <= date.Length; i++) date[i] = new DateTime(NumberYear[0], NumberMonth[i], 01); 
            PollutionMas(date);
        }
        #endregion

        #region Даты за сезон
        public string[] SelectSeason { get; private set; }
        private void SeasonYearPollut(DateTime[] date) 
        {
            SelectSeason = new ObservableCollection<string>(Seasons.Where(m => m.IsSelected).Select(m => m.Name)).ToArray();
            switch(SelectSeason[0])
            {
                case "Зима":
                    NumberMonth = [1, 2, 12];
                    InsertingDateSeason(NumberMonth);
                    break;
                case "Весна":
                    NumberMonth = [3, 4, 5];
                    InsertingDateSeason(NumberMonth);
                    break;
                case "Лето":
                    NumberMonth = [6, 7, 8];
                    InsertingDateSeason(NumberMonth);
                    break;
                case "Осень":
                    NumberMonth = [9, 10, 11];
                    InsertingDateSeason(NumberMonth);
                    break;
                case "Весь год":
                    NumberMonth = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
                    InsertingDateSeason(NumberMonth);
                    break;
            }
        }
        private void InsertingDateSeason(int[] mas)
        {
            List<DateTime> list = new List<DateTime>();
            foreach(var year in NumberYear)
            {
                foreach(var month in mas)
                {
                    list.Add(new DateTime(year, month, 01));
                }
            }
            DateTime[] dateSeason = list.ToArray();
            PollutionMas(dateSeason);
        }
        #endregion

        #region Загрузка загрязнений за выбранный период
        public decimal[] Pollutions { get; private set; }
        public decimal[] PollutionMas(DateTime[] date)
        {
            Pollutions = new ObservableCollection<decimal>(_model.Pollutions.Where(m => date.Any(d => d.Date == m.Date)).Where(m => m.PointID == PointID).Select(m => m.Concentration)).ToArray();
            return Pollutions;
        }
        #endregion

        #region Команда на кнопку
        public AsyncCommand SelectPollutionCommand { get; }
        private async Task SelectPollution(CancellationToken _)
        {
            SelectDate();
        }

        private void SelectDate()
        {
            NumberYear = new ObservableCollection<int>(Years.Where(m => m.IsSelected).Select(m => m.Year)).ToArray();
            IsControlActive = NumberYear.Length > 0;
            if(NumberYear.Length == 1)
            {
                OneYearPollut(dateOneYear);
            }
            else if(NumberYear.Length > 1) 
            {
                SeasonYearPollut(dateSeason);
            }    
        }
        #endregion

        #region Активатор для AnalysisUserControl
        private bool _isControlActive;
        public bool IsControlActive
        {
            get => _isControlActive;
            set
            {
                _isControlActive = value;
                OnPropertyChanged(nameof(IsControlActive));
            }
        }

        private string _firstValue;
        private string _secondValue;

        public string FirstValue
        {
            get => _firstValue;
            set
            {
                _firstValue = value;
                OnPropertyChanged(nameof(FirstValue));
            }
        }
        #endregion
    }
}