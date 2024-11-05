using BaseData.Context;
using BaseView.DatasDTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseView.ViewModel
{
    public class SelectMethods
    {
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
    }
}
