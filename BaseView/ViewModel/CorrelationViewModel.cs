using BaseData.Context;
using BaseView.DatasDTO;
using BaseView.Utilities;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using MvvmHelpers.Commands;
using OxyPlot;
using System.Collections.ObjectModel;
using System.Data;
using WeatherDataParser;
using static ConnectionConfig.Strings;

namespace BaseView.ViewModel
{
    public class CorrelationViewModel : BaseViewModel
    {
        private readonly DataContext _model;

        public SelectMethods selectMethods = new();
        public ObservableCollection<string> StationName { get; private set; }
        public Parser parcer;
        public SaveDatas saveDatas = new();
        public int[] StationID { get; private set; }
               
        public CorrelationViewModel(DataContext model)
        {
            _model = model;
            parcer = new Parser(DateTime.Parse("01.01.2020"),GetConnectionStrings(Sqlite));
            StationID = new ObservableCollection<int>(_model.Stations.Select(i => i.ID)).ToArray();
            for(int i = 0; i < StationID.Length; i++)
            {
                parcer.UpdateStationData(StationID[i]);
            }

            StationName = SelectMethods.SelectStationName(_model);

            SelectStationID = new Command<string>(SelectStation);
            SelectPointID = new Command<int>(SelectPoint);
            SelectPollutionCommand = new RelayCommand(SelectPollution);
        }

        #region Выбор города, загрузка точек
        public Command<string> SelectStationID { get; set; }
        public List<int> Points { get; private set; }
        public int IdStation { get; set; }
        private void SelectStation(string name)
        {
            var select = selectMethods.SelectStation(name, _model);
            Points = select.Points;
            IdStation = select.IdStation;
            NameCity = select.cityName;
            OnPropertyChanged(nameof(Points));
        }
        #endregion

        #region Выбор точки, заполнение коллекций Года, месяца и сезоны
        public Command<int> SelectPointID { get; set; }
        public ObservableCollection<int> IDPoint { get; private set; }
        public int PointID;

        public ObservableCollection<YearItem> Years { get; private set; }

        private void SelectPoint(int pointID)
        {
            var resultSelect = selectMethods.SelectPoint(pointID, _model);
            Years = resultSelect.Item1;
            Months = resultSelect.Item2;
            Seasons = resultSelect.Item3;
            PointID = resultSelect.Item4;
            OnPropertyChanged(nameof(Years));
            OnPropertyChanged(nameof(Months));
            OnPropertyChanged(nameof(Seasons));
        }
        #endregion

        #region Активность кнопок месяцев или сезонов
        public ObservableCollection<MonthItem> Months { get; private set; }        
        public ObservableCollection<SeasonItem> Seasons { get; private set; }
        
        public bool IsMonthComboboxEnabled { get; set; }
        public bool IsSeasonRadioButtonEnabled { get; set; }
        public int SelectedCount { get; set; }
        public void UpdateSelection()
        {
            MonthItem months = new();
            SeasonItem season = new();
            SelectedCount = Years.Count(y => y.IsSelected);
            months.IsMonthComboboxEnabled = SelectedCount == 1;
            IsMonthComboboxEnabled = months.IsMonthComboboxEnabled;
            
            season.IsSeasonRadioButtonEnabled = SelectedCount > 1;
            IsSeasonRadioButtonEnabled = season.IsSeasonRadioButtonEnabled;
            OnPropertyChanged(nameof(IsMonthComboboxEnabled));
            OnPropertyChanged(nameof(IsSeasonRadioButtonEnabled));
        }
        #endregion

        #region Даты в пределах одного года
        public int[] NumberYear { get; private set; }
        public int[] NumberMonth { get; private set; }

        private void OneYearPollut(DateTime[] date)
        {            
            NumberMonth = new ObservableCollection<int>(Months.Where(m => m.IsSelected).Select(m => m.Number)).ToArray();
            if (NumberMonth[0] == 13 && NumberMonth.Length == 1)
            {
                int[] numbers = Months.Select(m => m.Number).ToArray();
                NumberMonth = numbers.Take(numbers.Length - 1).ToArray();
            }

            date = new DateTime[NumberMonth.Length];
            for (int i = 0; i < date.Length; i++) date[i] = new DateTime(NumberYear[0], NumberMonth[i], 01);
            saveDatas.Dates = date; // сохранили массив выбранных дат за год в SaveDatas
            saveDatas.Pollution = SelectMethods.GetPollution(date, _model, PointID);
            
            
        }
        #endregion

        #region Даты за сезон
        public string[] SelectSeason { get; private set; }
        private void SeasonYearPollut()
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
            List<DateTime> list = [];
            foreach (var year in NumberYear)
            {
                foreach (var month in mas)
                {
                    list.Add(new DateTime(year, month, 01));
                }
            }
            saveDatas.Dates = [.. list]; // сохранили выбранный массив дат за сезон в SaveDatas
            saveDatas.Pollution = SelectMethods.GetPollution(saveDatas.Dates, _model, PointID);
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

        public Calculation calc = new();
        public double Correlation, Slope, Intercept;
        public string NameCity { get; set; }    
        private void SelectDate()
        {            
            NumberYear = new ObservableCollection<int>(Years.Where(m => m.IsSelected).Select(m => m.Year)).ToArray();            
            
            if (NumberYear.Length == 1)
            {
                OneYearPollut(saveDatas.Dates);
                Informations = string.Format($"Город: {NameCity}, Пост - №{PointID}, {NumberYear[0]} год");   
                ResultCorrelations();
            }
            else if (NumberYear.Length > 1)
            {
                string years = "";
                SeasonYearPollut();
                int i = 0;
                while(i != NumberYear.Length)
                {
                    if (i < NumberYear.Length - 1) years += NumberYear[i].ToString() + ", ";
                    else if (i == NumberYear.Length - 1) years += NumberYear[i].ToString();
                    
                    i++;
                }
                years += " год.";
                Informations = string.Format($" Город: {NameCity}, Пост - №{PointID},\nСезон - {SelectSeason[0]} за {years}");
                ResultCorrelations();
            }
        }
        
        private void ResultCorrelations()
        {            
            var resultCalculate = calc.CalculateCorrelation(NumberYear, IdStation, NumberMonth, saveDatas.Pollution);
            Correlation = resultCalculate.Item1;
            Slope = resultCalculate.Item2;
            Intercept = resultCalculate.Item3;
            string CorrelationEquation = string.Format("y = {0:0.##}x + {1:0.##}", Slope, Intercept);           
            ResultCorrelation = string.Format($"Значение корреляции: {Math.Round(Correlation, 2)}, {CorrelationEquation}");
            DrawingCorr();
        }
        public PlotModel PlotModel { get; set; }
        private void DrawingCorr()
        {            
            PlotModel = calc.LoadData(NumberYear, saveDatas.Pollution, NameCity, PointID, Informations, ResultCorrelation);
            OnPropertyChanged(nameof(PlotModel));
        }
        #endregion
    }
}
