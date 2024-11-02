using BaseData.Context;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
using BaseView.DatasDTO;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Windows.Input;
using BaseData.Entities;

namespace BaseView.ViewModel
{
    public class SelectViewModel : BaseViewModel
    {
        private readonly DataContext _model;
        
        public ObservableCollection<string> StationName { get; private set; }
        public int[] NumberYear { get; private set; }
        public int[] NumberMonth { get; private set; }
        public AnalysisViewModel Analysis { get; private set; }
        public SelectViewModel(DataContext model)
        {
            _model = model;
            //IsUserControl = false;
            StationName = new ObservableCollection<string>(model.Stations.Select(s => s.Name));
            SelectStationID = new Command<string>(SelectStation);
            SelectPointID = new Command<int>(SelectPoint);
            SelectPollutionCommand = new RelayCommand(SelectPollution);
        }

        #region Выбор города, загрузка точек
        public Command<string> SelectStationID { get; set; }
        public ObservableCollection<int> StationID { get; private set; }
        public List<int> Points { get; private set; }
        private void SelectStation(string name)
        {
            StationID = new ObservableCollection<int>(_model.Stations.Where(p => p.Name == name).Select(p => p.ID));
            int id = StationID.ElementAt(0);
            CityName = name; // сохраняем выбранный город в класс SaveDatas
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
            PointNumber = PointID; // сохранили выбранный ПНЗ в класс SaveDatas
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
        //private bool _isUserControl;
        //public bool IsUserControl
        //{
        //    get => _isUserControl;
        //    set
        //    {
        //        _isUserControl = value;
        //        OnPropertyChanged(nameof(IsUserControl));
        //    }
        //}
        private AnalysisViewModel analysis;
        //public void UpdateUserControl()
        //{
        //    int selectedYears = Years.Count(y => y.IsSelected);
        //    IsUserControl = selectedYears >= 1;
        //    UpdateData();
        //    //analysis = new AnalysisViewModel();
            
        //}
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
            for (int i = 0; i < date.Length; i++) date[i] = new DateTime(NumberYear[0], NumberMonth[i], 01);
            Dates = date; // сохранили массив выбранных дат за год в SaveDatas
            PollutionMas(date);
        }
        #endregion

        #region Даты за сезон
        public string[] SelectSeason { get; private set; }
        private void SeasonYearPollut(DateTime[] date)
        {
            SelectSeason = new ObservableCollection<string>(Seasons.Where(m => m.IsSelected).Select(m => m.Name)).ToArray();
            switch (SelectSeason[0])
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
            foreach (var year in NumberYear)
            {
                foreach (var month in mas)
                {
                    list.Add(new DateTime(year, month, 01));
                }
            }
            Dates = list.ToArray(); // сохранили выбранный массив дат за сезон в SaveDatas
            PollutionMas(Dates);
        }
        #endregion

        #region Загрузка загрязнений за выбранный период
        public decimal[] PollutionMas(DateTime[] date)
        {
            PollutionZaeb = new ObservableCollection<decimal>(_model.Pollutions.Where(m => date.Any(d => d.Date == m.Date)).Where(m => m.PointID == PointID).Select(m => m.Concentration)).ToArray();
            return PollutionZaeb;
        }
        #endregion

        #region Команда на кнопку
        public RelayCommand SelectPollutionCommand { get; }
        public void SelectPollution()
        {
            SelectDate();            
            //UpdateUserControl();
        }

        public string Informations { get; set; }
        public string Correaltion { get; set; }
        private ObservableCollection<SaveDatas> _saveDatas;
        public ObservableCollection<SaveDatas> SaveDatas
        {
            get => _saveDatas;
            set
            {
                _saveDatas = value;
                OnPropertyChanged(nameof(SaveDatas));
            }
        }
        private void SelectDate()
        {
            SaveDatas = new ObservableCollection<SaveDatas>();
            ZaebSave = new SaveDatas();
            NumberYear = new ObservableCollection<int>(Years.Where(m => m.IsSelected).Select(m => m.Year)).ToArray();
            double res;
            if (NumberYear.Length == 1)
            {
                OneYearPollut(Dates);
                UpdateData();
                res = CalculateCorrelation();
                Informations = string.Format($"Город: {CityName}, ПНЗА №{PointNumber}");
                Correaltion = string.Format($"Корреляция: {res}");
                OnPropertyChanged(nameof(Informations));
                OnPropertyChanged(nameof(Correaltion));
            }
            else if (NumberYear.Length > 1)
            {
                SeasonYearPollut(Dates);
                //Informations = string.Format($"Город: {CityName}, ПНЗА №{PointNumber}");
                //OnPropertyChanged(nameof(Informations));
            }
        }

        private SaveDatas ZaebSave;
        private string _cityName;
        public string CityName
        {
            get => _cityName;
            set
            {
                _cityName = value;
                OnPropertyChanged(nameof(CityName));
            }
        }
        private int _pointNumber;
        public int PointNumber
        {
            get => _pointNumber;
            set
            {
                _pointNumber = value;
                OnPropertyChanged(nameof(PointNumber));
            }
        }
        private DateTime[] _dates;
        public DateTime[] Dates
        {
            get => _dates;
            set
            {
                _dates = value;
                OnPropertyChanged(nameof(Dates));
            }
        }
        private decimal[] pollutionZaeb;
        public decimal[] PollutionZaeb
        {
            get => pollutionZaeb;
            set
            {
                pollutionZaeb = value;
                OnPropertyChanged(nameof(PollutionZaeb));
            }
        }

        public ObservableCollection<SaveDatas> Saves {  get; set; }
        private void UpdateData()
        {
            Saves = new ObservableCollection<SaveDatas>();
            var items = new SaveDatas
            {
                CityName = CityName,
                PointNumber = PointNumber,
                Dates = Dates,
                Pollution = PollutionZaeb
            };
            Saves.Add(items);
            Analysis = new AnalysisViewModel();
            Analysis.SaveDatas(Saves);
        }

        //public SelectViewModel() { }

        private int[] wind = { 1, 2, 3 };
        public double CalculateCorrelation()
        {
            if (PollutionZaeb == null) return 0;
            else if (PollutionZaeb.Length != wind.Length) throw new ArgumentException("Arrays must be of equal length.");
            else
            {
                decimal count = wind.Length;
                int n = wind.Length;

                decimal sumWind = wind.Sum();
                decimal sumPol = PollutionZaeb.Sum();
                decimal sumWindPol = wind.Zip(PollutionZaeb, (x, y) => x * y).Sum();
                decimal sumWindSquare = wind.Sum(x => x * x);
                decimal sumPolSquare = PollutionZaeb.Sum(x => x * x);
                double correaltion = (double)(n * sumWindSquare - sumWind * sumPol)
                    / Math.Sqrt((double)((n * sumWindSquare - sumWind * sumWind) * (n * sumPolSquare - sumPol * sumPol)));
                return correaltion;
            }
        }

        #endregion
    }
}
