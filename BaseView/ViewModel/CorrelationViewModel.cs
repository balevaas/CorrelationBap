using BaseData.Context;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
using BaseView.DatasDTO;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Windows.Input;
using BaseData.Entities;
using System.Windows.Media;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Annotations;
using OxyPlot.Series;
using System.Data;
using System.Linq;

namespace BaseView.ViewModel
{
    public class CorrelationViewModel : BaseViewModel
    {
        private readonly DataContext _model;

        public SelectMethods selectMethods = new SelectMethods();
        public ObservableCollection<string> StationName { get; private set; }
        
        #region Свойства для сохранения выбранных полей
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
        private decimal[] pollution;
        public decimal[] Pollution
        {
            get => pollution;
            set
            {
                pollution = value;
                OnPropertyChanged(nameof(Pollution));
            }
        }
        #endregion

        public CorrelationViewModel(DataContext model)
        {
            _model = model;

            StationName = selectMethods.SelectStationName(_model);

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
            Points = selectMethods.GetIDByStationID(id, _model);
            OnPropertyChanged(nameof(Points));
        }
        #endregion

        #region Выбор точки, извлечение только года
        public Command<int> SelectPointID { get; set; }
        public ObservableCollection<int> IDPoint { get; private set; }
        public int PointID;

        public ObservableCollection<YearItem> Years { get; private set; }

        private void SelectPoint(int pointID)
        {
            Years = new ObservableCollection<YearItem>();
            Months = new ObservableCollection<MonthItem>();
            Seasons = new ObservableCollection<SeasonItem>();
            IDPoint = new ObservableCollection<int>(_model.Points.Where(p => p.ID == pointID).Select(p => p.ID));
            PointID = IDPoint.ElementAt(0);
            PointNumber = PointID; // сохранили выбранный ПНЗ в класс SaveDatas
            foreach (var year in selectMethods.GetYears(PointID, _model))
            {
                Years.Add(year);
            }
            foreach (var month in selectMethods.GetMonthNames(_model))
            {
                Months.Add(month);
            }
            foreach (var seasons in selectMethods.GetSeasonItems())
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

        #endregion

        #region Даты в пределах одного года
        ///public decimal[] Pollution;
        public int[] NumberYear { get; private set; }
        public int[] NumberMonth { get; private set; }

        private void OneYearPollut(DateTime[] date)
        {
            NumberMonth = new ObservableCollection<int>(Months.Where(m => m.IsSelected).Select(m => m.Number)).ToArray();
            date = new DateTime[NumberMonth.Length];
            for (int i = 0; i < date.Length; i++) date[i] = new DateTime(NumberYear[0], NumberMonth[i], 01);
            Dates = date; // сохранили массив выбранных дат за год в SaveDatas
            Pollution = selectMethods.PollutionMas(date, _model, PointID);
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
            Pollution = selectMethods.PollutionMas(Dates, _model, PointID);
        }
        #endregion

        #region Команда на кнопку
        public RelayCommand SelectPollutionCommand { get; }
        public void SelectPollution()
        {
            SelectDate();            
        }

        public string Informations { get; set; }
        public string ResultCorrelation { get; set; }
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
        private void UpdateData()
        {
            SaveDatas = new ObservableCollection<SaveDatas>();
            var items = new SaveDatas
            {
                CityName = CityName,
                PointNumber = PointNumber,
                Dates = Dates,
                Pollution = Pollution
            };
            SaveDatas.Add(items);
        }
        private void SelectDate()
        {        
            NumberYear = new ObservableCollection<int>(Years.Where(m => m.IsSelected).Select(m => m.Year)).ToArray();
            double res1, res2, res3;
            if (NumberYear.Length == 1)
            {
                OneYearPollut(Dates); 
                UpdateData();
                res1 = CalculateCorrelation().Item1;
                res2 = CalculateCorrelation().Item2;
                res3 = CalculateCorrelation().Item3;

                string correlationEquation = string.Format("y = {0:0.##}x + {1:0.##}", res2, res3);

                Informations = string.Format($"Город: {CityName}, ПНЗА №{PointNumber}");
                ResultCorrelation = string.Format($"Корреляция: {Math.Round(res1, 2)},\n{correlationEquation}");
                OnPropertyChanged(nameof(Informations));
                OnPropertyChanged(nameof(ResultCorrelation));
                DrawingCorr();
            }
            else if (NumberYear.Length > 1)
            {
                SeasonYearPollut(Dates);
                Informations = string.Format($"Город: {CityName}, ПНЗА №{PointNumber}");
                OnPropertyChanged(nameof(Informations));
                res1 = CalculateCorrelation().Item1;
                ResultCorrelation = string.Format($"Корреляция: {Math.Round(res1, 2)}");
                OnPropertyChanged(nameof(ResultCorrelation));
            }
        }

        public int[] wind = { 1, 2, 3 };
        public (double, double, double) CalculateCorrelation()
        {
            //if (Pollution == null) return 0;
            if (Pollution.Length != wind.Length) throw new ArgumentException("Arrays must be of equal length.");
            else
            {
                decimal count = wind.Length;
                int n = wind.Length;

                decimal sumWind = wind.Sum();
                decimal sumPol = Pollution.Sum();
                decimal sumWindPol = wind.Zip(Pollution, (x, y) => x * y).Sum();
                decimal sumWindSquare = wind.Sum(x => x * x);
                decimal sumPolSquare = Pollution.Sum(x => x * x);
                double correaltion = (double)(n * sumWindPol - sumWind * sumPol)
                    / Math.Sqrt((double)((n * sumWindSquare - sumWind * sumWind) * (n * sumPolSquare - sumPol * sumPol)));

                double slope = (double)(count * sumWindPol - sumWind * sumPol) / (double)(count * sumWindSquare - sumWind * sumWind);
                double intercept = (double)((double)sumPol - slope * (double)sumWind) / (double)count;
                return (correaltion, slope, intercept);
            }
        }

        private PlotModel _plotModel;
        public PlotModel PlotModel
        {
            get => _plotModel;
            set
            {
                _plotModel = value;
                OnPropertyChanged(nameof(PlotModel));
            }
        }

        public class DataPoints
        {
            public double Wind { get; set; }
            public double Pollution { get; set; }
        }
        public void DrawingCorr()
        {
            PlotModel = new PlotModel { Title = "График корреляции" };
            LoadData();
        }
        private void LoadData()
        {
            var series = new ScatterSeries { Title = $"Город - { CityName }, Пост - { PointNumber }, {NumberYear[0] } год\" ", MarkerType = MarkerType.Circle };
            List<DataPoints> dataPoints = new List<DataPoints>();
            
            for(int i = 0; i < Pollution.Length; i++)
            {
                dataPoints.Add(new DataPoints { Wind = wind[i], Pollution = (double)Pollution[i] });
            }
            foreach (var point in dataPoints)
            {
                series.Points.Add(new ScatterPoint(point.Wind, point.Pollution));
            }

            PlotModel.Series.Add(series);
            double minWind, maxWind, minPollution, maxPollution;
            if (dataPoints.Count > 0)
            {
                minWind = dataPoints.Min(p => p.Wind);
                maxWind = dataPoints.Max(p => p.Wind);
                minPollution = dataPoints.Min(p => p.Pollution);
                maxPollution = dataPoints.Max(p => p.Pollution);
                PlotModel.Axes.Clear(); // Очистка существующих осей

                PlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Ветер", Minimum = minWind - 1, Maximum = maxWind + 1 });
                PlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Концентрация нг / м^3", Minimum = minPollution - 1, Maximum = maxPollution + 1 });
            }

            double averageX = wind.Average();
            double averageY = (double)Pollution.Average();
            double b = (wind.Zip((Pollution), (x, y) => (x - averageX) * ((double)y - averageY)).Sum()) / (wind.Select(x => Math.Pow(x - averageX, 2)).Sum());
            double a = averageY - b * averageX;
            var lineSeries = new LineSeries
            {
                Title = "Линейная регрессия",
                StrokeThickness = 2,
                Color = OxyColors.Red
            };
            lineSeries.Points.Add(new DataPoint(wind.Min(), a + b * wind.Min()));
            lineSeries.Points.Add(new DataPoint(wind.Max(), a + b * wind.Max()));
            PlotModel.Series.Add(lineSeries);


            PlotModel.InvalidatePlot(true);
        }
        #endregion
    }
}
