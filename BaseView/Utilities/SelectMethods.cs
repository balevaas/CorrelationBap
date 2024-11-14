using BaseData.Context;
using BaseView.DatasDTO;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BaseView.Utilities
{
    public class SelectMethods : BaseViewModel
    {
        public SaveDatas saveDatas = new SaveDatas();
        public ObservableCollection<string> SelectStationName(DataContext _model)
        {
            ObservableCollection<string> names = new ObservableCollection<string>(_model.Stations.Select(p => p.Name));
            return names;
        }
        public List<int> GetIDByStationID(int station, DataContext _model)
        {
            var ID = _model.Points
                .Where(od => od.StationID == station)
                .Select(od => od.ID)
                .ToList();
            return ID; // Возвращаем найденный ID            
        }

        public List<YearItem> GetYears(int id, DataContext _model)
        {
            return _model.Pollutions.Where(p => p.PointID == id).Select(y => new YearItem { Year = y.Date.Year, IsSelected = false }).Distinct().ToList();
        }


        public List<MonthItem> GetMonthNames(DataContext _model)
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

        public decimal[] PollutionMas(DateTime[] date, DataContext _model, int PointID)
        {
            decimal[] Pollution = new ObservableCollection<decimal>(_model.Pollutions.Where(m => date.Any(d => d.Date == m.Date)).Where(m => m.PointID == PointID).Select(m => m.Concentration)).ToArray();
            return Pollution;
        }

        public ObservableCollection<int> StationID { get; private set; }
        public List<int> Points { get; private set; }
        public int IdStation { get; set; }
        public (List<int> Points, int IdStation, string cityName) SelectStation(string name, DataContext _model)
        {
            StationID = new ObservableCollection<int>(_model.Stations.Where(p => p.Name == name).Select(p => p.ID));
            IdStation = StationID.ElementAt(0);
            string cityName = name; // сохраняем выбранный город в класс SaveDatas
            Points = GetIDByStationID(IdStation, _model);
            return (Points, IdStation, cityName);
        }

        public ObservableCollection<int> IDPoint { get; private set; }
        public int PointID;

        public ObservableCollection<YearItem> Years { get; private set; }
        public ObservableCollection<MonthItem> Months { get; private set; }
        public ObservableCollection<SeasonItem> Seasons { get; private set; }
        public (ObservableCollection<YearItem>, ObservableCollection<MonthItem>, ObservableCollection<SeasonItem>, int) SelectPoint(int pointID, DataContext _model)
        {
            Years = new ObservableCollection<YearItem>();
            Months = new ObservableCollection<MonthItem>();
            Seasons = new ObservableCollection<SeasonItem>();
            IDPoint = new ObservableCollection<int>(_model.Points.Where(p => p.ID == pointID).Select(p => p.ID));
            PointID = IDPoint.ElementAt(0);
            saveDatas.PointNumber = PointID; // сохранили выбранный ПНЗ в класс SaveDatas
            foreach (var year in GetYears(PointID, _model))
            {
                Years.Add(year);
            }
            foreach (var month in GetMonthNames(_model))
            {
                Months.Add(month);
            }
            foreach (var seasons in GetSeasonItems())
            {
                Seasons.Add(seasons);
            }
            //OnPropertyChanged(nameof(Years));
            //OnPropertyChanged(nameof(Months));
            //OnPropertyChanged(nameof(Seasons));
            return (Years, Months, Seasons, PointID);
        }
    }
}
